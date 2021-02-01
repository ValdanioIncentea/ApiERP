using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErpBS100;
using CmpBE100;
using PortalApi.Models;
using PortalApi.ViewModel;
using System.Threading.Tasks;
using IntBE100;
using IIntBS100;
using PortalApi.App_Start;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace PortalApi.Repository
{
    public class DocumentosComprasRepository
    {

        HelperRepository _helperRepository = new HelperRepository();
        RasteabilidadeRepository _rasteabilidadeRepository = new RasteabilidadeRepository();
        public bool CriarDocumentoDeCompra(ErpBS BSO, List<DocumentoDeCompras> documentosDeCompras)
        {

            var DocumentoActual = new DocumentoDeCompras();

            var CodigoPortalDocumentosIntegrados = new List<int>();

            try
            {

                var DocumentosIntegrados = new List<DocumentosIntegradoViewModel>();

                var MapaDoFLuxo = new Dictionary<int, string>();

                if (documentosDeCompras.Count() > 0)
                {
                    var documentoDeCompras = documentosDeCompras.First();
                    MapaDoFLuxo.Add(1, documentoDeCompras.DocumentoPrincipal);
                    MapaDoFLuxo.Add(11, documentoDeCompras.DocumentoPrincipalAlternativo);
                    MapaDoFLuxo.Add(2, documentoDeCompras.SegundoDocumento);
                    MapaDoFLuxo.Add(21, documentoDeCompras.SegundoDocumentoAlternativo);
                    MapaDoFLuxo.Add(3, documentoDeCompras.TerceiroDocumento);
                    MapaDoFLuxo.Add(4, documentoDeCompras.QuartoDocumento);
                    MapaDoFLuxo.Add(5, documentoDeCompras.QuintoDocumento);
                    MapaDoFLuxo.Add(6, documentoDeCompras.SextoDocumento);
                }

                foreach (var documentoDeCompras in documentosDeCompras)
                {

                    if (!VeriricarSeNumeroDeProcessoExisteNoDocumentoDeCompras(documentoDeCompras.NumeroDeProcesso, documentoDeCompras.CodigoPortal, documentoDeCompras.Tipodoc, documentoDeCompras.Entidade))
                    {
                        CmpBEDocumentoCompra documento = new CmpBEDocumentoCompra();

                        DocumentoActual = documentoDeCompras;

                        documento.Tipodoc = documentoDeCompras.Tipodoc;
                        documento.TipoEntidade = documentoDeCompras.TipoEntidade;
                        documento.Entidade = documentoDeCompras.Entidade;
                        documento.NumDocExterno = documentoDeCompras.NumDocExterno;

                        BSO.Compras.Documentos.PreencheDadosRelacionados(documento);

                        documento.DataDoc = documentoDeCompras.DataDoc;

                        var rs = DateTime.Compare(documentoDeCompras.DataDoc.Date, DateTime.Now.Date);

                        if (rs < 0)
                        {

                            double CambioDoDia = BuscarCambio(documentoDeCompras.DataDoc, documentoDeCompras.Moeda);

                            if (BSO.Contexto.SentidoCambios == Constante.SentidoDireito)
                            {
                                documento.CambioMBase = CambioDoDia;
                                documento.CambioMAlt = 1;
                                documento.Cambio = CambioDoDia;
                            }
                            else
                            {
                                documento.CambioMBase = 1;
                                documento.CambioMAlt = CambioDoDia;
                                documento.Cambio = 1;
                            }

                        }

                        documento.Moeda = documentoDeCompras.Moeda;
                        documento.Serie = BSO.Base.Series.DaSerieDefeito("C", documento.Tipodoc, documento.DataDoc);
                        documento.CamposUtil["CDU_PROCESSO"].Valor = documentoDeCompras.NumeroDeProcesso;

                        int linhaNumero = 0;

                        if (documento.Tipodoc == MapaDoFLuxo[4])
                        {

                            string DocumentoAnterior = MapaDoFLuxo[3];

                            foreach (var linhaDocumento in documentoDeCompras.Linhas)
                            {

                                linhaNumero++;

                                var LinhaDoDocumento = _rasteabilidadeRepository.PesquisarLinhaDoDocumetoNoDocumentoDeOrgiem(linhaDocumento.NumeroDeProcesso, DocumentoAnterior, linhaDocumento.Artigo);

                                var Filial = LinhaDoDocumento.Rows[0]["Filial"].ToString();
                                var Serie = LinhaDoDocumento.Rows[0]["Serie"].ToString();
                                var NumDoc = Convert.ToInt32(LinhaDoDocumento.Rows[0]["NumDoc"]);
                                var NumLinha = Convert.ToInt32(LinhaDoDocumento.Rows[0]["NumLinha"]);

                                float TaxaIva = Convert.ToSingle(linhaDocumento.TaxaIva);

                                BSO.Compras.Documentos.AdicionaLinhaTransformada(documento, DocumentoAnterior, NumDoc, NumLinha, ref Filial, ref Serie);

                                documento.Linhas.GetEdita(linhaNumero).Descricao = linhaDocumento.Descricao;
                                documento.Linhas.GetEdita(linhaNumero).PrecUnit = linhaDocumento.Preco;
                                documento.Linhas.GetEdita(linhaNumero).Desconto1 = Convert.ToSingle(linhaDocumento.Desconto);
                                documento.Linhas.GetEdita(linhaNumero).TaxaIva = TaxaIva;
                                documento.Linhas.GetEdita(linhaNumero).CamposUtil["CDU_PROCESSO"].Valor = linhaDocumento.NumeroDeProcesso;
                                documento.Linhas.GetEdita(linhaNumero).CodIva = Convert.ToString(buscarCodigoDoImpostoIva(TaxaIva, documento.DataDoc));

                            }

                        }
                        else if (documento.Tipodoc == MapaDoFLuxo[3] || documento.Tipodoc == MapaDoFLuxo[5])
                        {

                            string DocumentoAnterior = documento.Tipodoc == MapaDoFLuxo[3] ? MapaDoFLuxo[2] : MapaDoFLuxo[4];

                            string Id = _rasteabilidadeRepository.BuscarOrigemDoDocumetoDeCompra(documentoDeCompras.NumeroDeProcesso, DocumentoAnterior, documentoDeCompras.Entidade);

                            if (Id == null)
                            {
                                _helperRepository.CriarLog("Integração", "Não foi encontrado o ID do documento pai do documento: " + documentoDeCompras.Tipodoc + "/" + documentoDeCompras.NumeroDeProcesso, "Erro");
                                return false;
                            }

                            var DocumentoDePedidoDeCotacao = _rasteabilidadeRepository.PesquisarDoDocumetoPorId(Id);

                            var dt_Linhas = _rasteabilidadeRepository.BuscarLinhasDoDocumetoDeCompra(Id);

                            foreach (var linhaDocumento in documentoDeCompras.Linhas)
                            {

                                linhaNumero++;

                                DataRow[] CotacoesDoFornecedor = dt_Linhas.Select($"Artigo='{linhaDocumento.Artigo}'");

                                var Linha = CotacoesDoFornecedor.First();

                                var Filial = DocumentoDePedidoDeCotacao["Filial"];
                                var Serie = DocumentoDePedidoDeCotacao["Serie"];
                                var NumDoc = Convert.ToInt32(DocumentoDePedidoDeCotacao["NumDoc"]);

                                float TaxaIva = Convert.ToSingle(linhaDocumento.TaxaIva);

                                BSO.Compras.Documentos.AdicionaLinhaTransformada(documento, DocumentoAnterior, NumDoc, Convert.ToInt32(Linha["NumLinha"]), ref Filial, ref Serie);

                                documento.Linhas.GetEdita(linhaNumero).Descricao = linhaDocumento.Descricao;
                                documento.Linhas.GetEdita(linhaNumero).PrecUnit = linhaDocumento.Preco;
                                documento.Linhas.GetEdita(linhaNumero).Desconto1 = Convert.ToSingle(linhaDocumento.Desconto);
                                documento.Linhas.GetEdita(linhaNumero).TaxaIva = TaxaIva;
                                documento.Linhas.GetEdita(linhaNumero).Quantidade = linhaDocumento.Quantidade;
                                documento.Linhas.GetEdita(linhaNumero).Armazem = linhaDocumento.Armazem;
                                documento.Linhas.GetEdita(linhaNumero).Localizacao = linhaDocumento.Localizacao;
                                documento.Linhas.GetEdita(linhaNumero).CamposUtil["CDU_PROCESSO"].Valor = linhaDocumento.NumeroDeProcesso;
                                documento.Linhas.GetEdita(linhaNumero).CodIva = Convert.ToString(buscarCodigoDoImpostoIva(TaxaIva, documento.DataDoc));

                            }

                        }
                        else if (documento.Tipodoc == MapaDoFLuxo[2])
                        {
                            foreach (var linhaDocumento in documentoDeCompras.Linhas)
                            {

                                linhaNumero++;

                                double Quantidade = linhaDocumento.Quantidade;

                                string Armazem = linhaDocumento.Armazem;

                                BSO.Compras.Documentos.AdicionaLinha(documento, linhaDocumento.Artigo, ref Quantidade, ref Armazem, ref Armazem, linhaDocumento.Preco, Convert.ToDouble(linhaDocumento.Desconto));
                                float TaxaIva = Convert.ToSingle(linhaDocumento.TaxaIva);
                                documento.Linhas.GetEdita(linhaNumero).Unidade = linhaDocumento.Unidade;
                                documento.Linhas.GetEdita(linhaNumero).Descricao = linhaDocumento.Descricao;
                                documento.Linhas.GetEdita(linhaNumero).DataEntrega = documento.DataDoc;
                                documento.Linhas.GetEdita(linhaNumero).CCustoCBL = linhaDocumento.CentroDeCusto;
                                documento.Linhas.GetEdita(linhaNumero).TaxaIva = TaxaIva;
                                documento.Linhas.GetEdita(linhaNumero).CamposUtil["CDU_PROCESSO"].Valor = linhaDocumento.NumeroDeProcesso;
                                documento.Linhas.GetEdita(linhaNumero).CodIva = Convert.ToString(buscarCodigoDoImpostoIva(TaxaIva, documento.DataDoc));

                            }
                        }

                        if (documento.Linhas.NumItens > 0)
                        {

                            BSO.Compras.Documentos.CalculaValoresTotais(documento);
                            if (!VeriricarSeNumeroDeProcessoExisteNoDocumentoDeCompras(documentoDeCompras.NumeroDeProcesso, documentoDeCompras.CodigoPortal, documentoDeCompras.Tipodoc, documentoDeCompras.Entidade))
                            {
                                BSO.Compras.Documentos.Actualiza(documento);
                                CodigoPortalDocumentosIntegrados.Add(documentoDeCompras.CodigoPortal);
                            }

                            var Referencia = $"{documento.Tipodoc} {documento.Serie}/{documento.NumDoc}";
                            if (!VerificarSeIdJáExiste(DocumentoActual.CodigoPortal))
                                ReferenciarDocumentosIntegrados(DocumentoActual.CodigoPortal, Referencia);
                            _helperRepository.CriarLog("Integração", $" O documento: { documento.Tipodoc}/{ documento.Serie}/ { documento.NumDoc}", "Sucesso");

                        }
                    }
                }

                return true;

            }
            catch (Exception e)
            {

                foreach (var CodigoDocumento in CodigoPortalDocumentosIntegrados)
                {
                    documentosDeCompras.RemoveAll(x => x.CodigoPortal == CodigoDocumento);
                }

                if (documentosDeCompras.Count > 0 && CodigoPortalDocumentosIntegrados.Count > 0)
                {
                    CriarDocumentoDeCompra(BSO, documentosDeCompras);
                }

                BSO.FechaEmpresaTrabalho();

                _helperRepository.CriarLog("Integração", "Integração de Documento de Compra: " + e.Message.ToString(), "Erro");

                return false;

            }

        }

        public string CriarDocumentoInterno(ErpBS BSO, List<DocumentoDeCompras> documentosDeCompras)
        {

            var DocumentoActual = new DocumentoDeCompras();

            var CodigoPortalDocumentosIntegrados = new List<int>();

            try
            {

                var DocumentosIntegrados = new List<DocumentosIntegradoViewModel>();

                var MapaDoFLuxo = new Dictionary<int, string>();

                if (documentosDeCompras.Count() > 0)
                {
                    var documentoDeCompras = documentosDeCompras.First();
                    MapaDoFLuxo.Add(1, documentoDeCompras.DocumentoPrincipal);
                    MapaDoFLuxo.Add(11, documentoDeCompras.DocumentoPrincipalAlternativo);
                    MapaDoFLuxo.Add(2, documentoDeCompras.SegundoDocumento);
                    MapaDoFLuxo.Add(21, documentoDeCompras.SegundoDocumentoAlternativo);
                    MapaDoFLuxo.Add(3, documentoDeCompras.TerceiroDocumento);
                    MapaDoFLuxo.Add(4, documentoDeCompras.QuartoDocumento);
                    MapaDoFLuxo.Add(5, documentoDeCompras.QuintoDocumento);
                    MapaDoFLuxo.Add(6, documentoDeCompras.SextoDocumento);
                }

                foreach (var documentoDeCompras in documentosDeCompras)
                {

                    if (!VeriricarSeNumeroDeProcessoExisteNoDocumentoInterno(documentoDeCompras.NumeroDeProcesso, documentoDeCompras.CodigoPortal, documentoDeCompras.Tipodoc, documentoDeCompras.Entidade))
                    {

                        IntBEDocumentoInterno documento = new IntBEDocumentoInterno();

                        DocumentoActual = documentoDeCompras;

                        documento.Tipodoc = documentoDeCompras.Tipodoc;
                        documento.TipoEntidade = documentoDeCompras.TipoEntidade;
                        documento.Entidade = documentoDeCompras.Entidade;
                        documento.Referencia = documentoDeCompras.NumDocExterno;

                        BSO.Internos.Documentos.PreencheDadosRelacionados(documento);
                        documento.Data = documentoDeCompras.DataDoc;
                        var rs = DateTime.Compare(documentoDeCompras.DataDoc.Date, DateTime.Now.Date);
                        if (rs < 0)
                        {
                            double CambioDoDia = BuscarCambio(documentoDeCompras.DataDoc, documentoDeCompras.Moeda);

                            if (BSO.Contexto.SentidoCambios == Constante.SentidoDireito)
                            {
                                documento.CambioMBase = CambioDoDia;
                                documento.CambioMAlt = 1;
                                documento.Cambio = CambioDoDia;
                            }
                            else
                            {
                                documento.CambioMBase = 1;
                                documento.CambioMAlt = CambioDoDia;
                                documento.Cambio = 1;
                            }

                        }

                        documento.Moeda = documentoDeCompras.Moeda;
                        documento.Serie = BSO.Base.Series.DaSerieDefeito("N", documento.Tipodoc, documento.Data);
                        documento.Destinatario = ConfigurationManager.AppSettings["Destinatario"].ToString();
                        documento.Requisitante = ConfigurationManager.AppSettings["Requisitante"].ToString();
                        documento.CamposUtil["CDU_PROCESSO"].Valor = documentoDeCompras.NumeroDeProcesso;

                        int linhaNumero = 0;

                        foreach (var linhaDocumento in documentoDeCompras.Linhas)
                        {

                            double Quantidade = linhaDocumento.Quantidade;

                            string Armazem = linhaDocumento.Armazem;

                            double Desconto = linhaDocumento.Desconto == null ? 0 : Convert.ToDouble(linhaDocumento.Desconto);

                            BSO.Internos.Documentos.AdicionaLinha(documento, linhaDocumento.Artigo, Armazem, linhaDocumento.Localizacao, linhaDocumento.Lote, linhaDocumento.Preco, Desconto, Quantidade);
                            linhaNumero++;
                            float TaxaIva = Convert.ToSingle(linhaDocumento.TaxaIva);
                            documento.Linhas.GetEdita(linhaNumero).DataEntrega = documento.Data;
                            documento.Linhas.GetEdita(linhaNumero).ObraID = linhaDocumento.IdObra.ToString();
                            documento.Linhas.GetEdita(linhaNumero).Descricao = linhaDocumento.Descricao;
                            documento.Linhas.GetEdita(linhaNumero).CCustoCBL = linhaDocumento.CentroDeCusto;

                            if (documento.Tipodoc == MapaDoFLuxo[21])
                            {

                                string Id = _rasteabilidadeRepository.LinhaOrigemDoDocumentoInternoDeOrigem(linhaDocumento.NumeroDeProcesso, linhaDocumento.Artigo, "('" + MapaDoFLuxo[1] + "','" + MapaDoFLuxo[11] + "')");

                                if (Id != null)
                                {

                                    _helperRepository.CriarLog("Rastreabilidade", $" Documento {documento.Tipodoc} | ID GERADO : { Id } -  Documento " + documento.Tipodoc, "Rastreabilidade");

                                    documento.Linhas.GetEdita(linhaNumero).IdLinhaOrigemCopia = Id;

                                }
                                else
                                {

                                    _helperRepository.CriarLog("Rastreabilidade", $"Documento {documento.Tipodoc} ID NÂO GERADO", "Rastreabilidade");

                                }

                            }

                        }

                        if (documento.Linhas.NumItens > 0)
                        {

                            BSO.Internos.Documentos.CalculaValoresTotais(documento);
                            if (!VeriricarSeNumeroDeProcessoExisteNoDocumentoInterno(documentoDeCompras.NumeroDeProcesso, documentoDeCompras.CodigoPortal, documentoDeCompras.Tipodoc, documentoDeCompras.Entidade))
                            {
                                BSO.Internos.Documentos.Actualiza(documento);
                                CodigoPortalDocumentosIntegrados.Add(documentoDeCompras.CodigoPortal);
                            }

                            var Referencia = $"{documento.Tipodoc} {documento.Serie}/{documento.NumDoc}";
                            if (!VerificarSeIdJáExiste(DocumentoActual.CodigoPortal))
                                ReferenciarDocumentosIntegrados(DocumentoActual.CodigoPortal, Referencia);
                            _helperRepository.CriarLog("Integração", $" O documento: { documento.Tipodoc}/{ documento.Serie}/ { documento.NumDoc}", "Sucesso");

                        }
                    }
                }
                return "OK";
            }
            catch (Exception e)
            {

                foreach (var CodigoDocumento in CodigoPortalDocumentosIntegrados)
                {
                    documentosDeCompras.RemoveAll(x => x.CodigoPortal == CodigoDocumento);
                }

                if (documentosDeCompras.Count > 0 && CodigoPortalDocumentosIntegrados.Count > 0)
                {
                    CriarDocumentoInterno(BSO, documentosDeCompras);
                }

                BSO.FechaEmpresaTrabalho();
                _helperRepository.CriarLog("Integração", "Integração de Documento Interno: " + e.Message.ToString(), "Erro");

                return e.Message;

            }

        }

        private float buscarCodigoDoImpostoIva(float Taxa, DateTime DataDoDocumento)
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable CodigosIva = new DataTable();

                    string queryCabe = "";

                    if (Taxa != 14)
                    {

                        if (DataDoDocumento >= Convert.ToDateTime("2019-10-01"))
                        {
                            queryCabe = $"select iva from iva where taxa = 0 and Descricao = 'Regime de Isenção'";
                        }
                        else
                        {
                            queryCabe = $"select iva from iva where taxa = 0 and Descricao = 'Isenta'";
                        }

                    }
                    else
                    {
                        queryCabe = $"select iva from iva where taxa = {Taxa}";
                    }

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(CodigosIva);

                    List<ArtigoViewModel> CambioLista = new List<ArtigoViewModel>();

                    if (CodigosIva.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in CodigosIva.Rows)
                        {
                            return Convert.ToSingle(Linha["iva"]);
                        }

                    }

                    return 90;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: buscarCodigoDoImpostoIva " + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public void ReferenciarDocumentosIntegrados(int ID, string Referencia)
        {

            SqlCommand Comando = new SqlCommand();

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    Comando.CommandText = $"insert into TDU_RefDocumentosIntegrados(CDU_ID,CDU_Referencia) values ({ID},'{Referencia}')";
                    Comando.CommandType = CommandType.Text;
                    Comando.Connection = conexao;
                    Comando.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: ReferenciarDocumentosIntegrados " + ex.Message.ToString(), "Erro");
                    throw ex;

                }

            }

        }

        public bool VerificarSeIdJáExiste(int ID)
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    string queryCabe = $"SELECT * FROM TDU_RefDocumentosIntegrados WHERE CDU_ID = {ID}";
                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);
                    DataTable dt_Check = new DataTable();

                    reader.Fill(dt_Check);

                    if (dt_Check.Rows.Count > 0)
                    {
                        return true;
                    }

                }
                catch (Exception ex)
                {

                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: VerificarSeIdJáExiste " + ex.Message.ToString(), "Erro");
                    throw ex;

                }
            }

            return false;

        }

        public double BuscarCambio(DateTime Data, string Moeda)
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    DataTable Cambios = new DataTable();

                    double Cambio = 0;

                    string queryCabe = $"SELECT TOP 1 Compra FROM MoedasHistorico WHERE DataCambio<='{Convert.ToDateTime(Data).ToString("yyyy-MM-dd")}' AND Moeda='{Moeda}'";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Cambios);

                    List<ArtigoViewModel> CambioLista = new List<ArtigoViewModel>();

                    if (Cambios.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Cambios.Rows)
                        {
                            Cambio = Convert.ToDouble(Linha["Compra"]);
                        }

                    }

                    conexao.Close();
                    return Cambio;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarCambio " + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public bool VeriricarSeNumeroDeProcessoExisteNoDocumentoInterno(string NumeroDeProcesso, int CodigoPortal, string TipoDeDocumento, string Entidade)
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    DataTable Registos = new DataTable();

                    string queryCabe = $"select id,Serie,Tipodoc,NumDoc from cabecInternos where cdu_processo = '{NumeroDeProcesso}' AND Tipodoc = '{TipoDeDocumento}' AND Entidade = '{Entidade}'";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Registos);

                    if (Registos.Rows.Count > 0)
                    {
                        foreach (DataRow Linha in Registos.Rows)
                        {
                            var Referencia = $"{Linha["Tipodoc"]} {Linha["Serie"]}/{Linha["NumDoc"]}";
                            if (!VerificarSeIdJáExiste(CodigoPortal))
                            {
                                ReferenciarDocumentosIntegrados(CodigoPortal, Referencia);
                            }
                        }
                        conexao.Close();
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: VeriricarSeNumeroDeProcessoExisteNoDocumentoInterno " + ex.Message.ToString(), "Erro");
                }
            }

            return false;

        }

        public bool VeriricarSeNumeroDeProcessoExisteNoDocumentoDeCompras(string NumeroDeProcesso, int CodigoPortal, string TipoDeDocumento, string Entidade)
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable Registos = new DataTable();

                    string queryCabe = $"select id,Serie,Tipodoc,NumDoc from CabecCompras where cdu_processo = '{NumeroDeProcesso}' AND Tipodoc = '{TipoDeDocumento}' AND Entidade = '{Entidade}'";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Registos);

                    if (Registos.Rows.Count > 0)
                    {
                        foreach (DataRow Linha in Registos.Rows)
                        {
                            var Referencia = $"{Linha["Tipodoc"]} {Linha["Serie"]}/{Linha["NumDoc"]}";
                            if (!VerificarSeIdJáExiste(CodigoPortal))
                                ReferenciarDocumentosIntegrados(CodigoPortal, Referencia);
                        }
                        conexao.Close();
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: VeriricarSeNumeroDeProcessoExisteNoDocumentoDeCompras " + ex.Message.ToString(), "Erro");
                }
            }

            return false;

        }

        public void CriaCDUEmFaltaNasTabelas()
        {

            string[] Tabelas = { "LinhasCompras", "LinhasInternos", "CabecInternos", "CabecCompras" };

            SqlCommand Comando = new SqlCommand();

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    for (int i = 0; i < Tabelas.Length; i++)
                    {

                        Comando.CommandText = $"IF NOT EXISTS(SELECT id FROM syscolumns WHERE id=OBJECT_ID('" + Tabelas[i] + "','u') and name='CDU_PROCESSO')"
                        + "BEGIN ALTER TABLE " + Tabelas[i] + " ADD CDU_PROCESSO varchar(100)"
                           + "Insert into StdCamposVar(Tabela, Campo, Descricao, Texto, Visivel, DadosSensiveis) Values('" + Tabelas[i] + "', 'CDU_PROCESSO', 'Campo de Utilizador', 'Campo_integracao', 0, 0)"
                        + "END";
                        Comando.CommandType = CommandType.Text;
                        Comando.Connection = conexao;
                        Comando.ExecuteNonQuery();

                    }

                    conexao.Close();

                }
                catch (Exception ex)
                {

                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo : CriaCDUEmFaltaNasTabelas(); " + ex.Message.ToString(), "Erro");
                    throw;

                }

            }

        }

        public void CriaTDUparaIntegracaoEmFalta()
        {

            string Tabela = "TDU_RefDocumentosIntegrados";

            SqlCommand Comando = new SqlCommand();

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    Comando.CommandText = $"IF NOT EXISTS(SELECT id FROM syscolumns WHERE id=OBJECT_ID('" + Tabela + "','u'))"
                    + "BEGIN CREATE TABLE " + Tabela + " (CDU_ID int identity primary key,CDU_Referencia varchar(100) not null)"
                    + "Insert into StdTabelasVar(Tabela,Apl) Values('" + Tabela + "','RDI')"
                       + "Insert into StdCamposVar(Tabela,Campo,Descricao,Texto,Visivel,DadosSensiveis) Values('" + Tabela + "','CDU_ID','campo da tabela','Para a integracao',0,0)"
                       + "Insert into StdCamposVar(Tabela,Campo,Descricao,Texto,Visivel,DadosSensiveis) Values('" + Tabela + "','CDU_Referencia','campo da tabela','Para a integracao',0,0)"
                    + "END";

                    Comando.CommandType = CommandType.Text;
                    Comando.Connection = conexao;
                    Comando.ExecuteNonQuery();

                    conexao.Close();

                }
                catch (Exception ex)
                {

                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo : CriaTDUparaIntegracaoEmFalta(); " + ex.Message.ToString(), "Erro");
                    throw;

                }

            }

        }


    }
}