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
using BDPortal;
using PortalApi.App_Start;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace PortalApi.Repository
{
    public class DocumentosComprasRepository
    {

        HelperRepository _helperRepository = new HelperRepository();
        public bool CriarDocumentoDeCompra(ErpBS BSO, List<DocumentoDeCompras> documentosDeCompras)
        {
            try
            {

                var DocumentosIntegrados = new List<DocumentosIntegradoViewModel>();

                foreach (var documentoDeCompras in documentosDeCompras)
                {
                    if (!VeriricarSeNumeroDeProcessoExisteNoDocumentoDeCompras(documentoDeCompras.NumeroDeProcesso, documentoDeCompras.CodigoPortal, documentoDeCompras.Tipodoc, documentoDeCompras.Entidade))
                    {
                        CmpBEDocumentoCompra documento = new CmpBEDocumentoCompra();

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
                        foreach (var linhaDocumento in documentoDeCompras.Linhas)
                        {

                            double Quantidade = linhaDocumento.Quantidade;

                            string Armazem = linhaDocumento.Armazem;

                            BSO.Compras.Documentos.AdicionaLinha(documento, linhaDocumento.Artigo, ref Quantidade, ref Armazem, ref Armazem, linhaDocumento.Preco, Convert.ToDouble(linhaDocumento.Desconto));
                            linhaNumero++;
                            float TaxaIva = Convert.ToSingle(linhaDocumento.TaxaIva);
                            documento.Linhas.GetEdita(linhaNumero).Unidade = linhaDocumento.Unidade;
                            documento.Linhas.GetEdita(linhaNumero).Descricao = linhaDocumento.Descricao;
                            documento.Linhas.GetEdita(linhaNumero).DataEntrega = documento.DataDoc;
                            documento.Linhas.GetEdita(linhaNumero).CCustoCBL = linhaDocumento.CentroDeCusto;
                            documento.Linhas.GetEdita(linhaNumero).TaxaIva = TaxaIva;
                            documento.Linhas.GetEdita(linhaNumero).CodIva = Convert.ToString(buscarCodigoDoImpostoIva(TaxaIva, documento.DataDoc));

                        }

                        if (documento.Linhas.NumItens > 0)
                        {
                            BSO.Compras.Documentos.CalculaValoresTotais(documento);
                            if (!VeriricarSeNumeroDeProcessoExisteNoDocumentoDeCompras(documentoDeCompras.NumeroDeProcesso, documentoDeCompras.CodigoPortal, documentoDeCompras.Tipodoc, documentoDeCompras.Entidade))
                            {
                                BSO.Compras.Documentos.Actualiza(documento);
                            }

                            var Referencia = $"{documento.Tipodoc} {documento.Serie}/{documento.NumDoc}";
                            if (!VerificarSeIdJáExiste(documentoDeCompras.CodigoPortal))
                                ReferenciarDocumentosIntegrados(documentoDeCompras.CodigoPortal, Referencia);
                            _helperRepository.CriarLog("Integração", $" O documento: { documento.Tipodoc}/{ documento.Serie}/ { documento.NumDoc}", "Sucesso");
                        }
                    }
                    else
                    {

                    }
                }
                return true;

            }
            catch (Exception e)
            {

                BSO.FechaEmpresaTrabalho();

                _helperRepository.CriarLog("Integração", "Integração de Documento de Compra: " + e.Message.ToString(), "Erro");

                return false;

            }

        }

        public string CriarDocumentoInterno(ErpBS BSO, List<DocumentoDeCompras> documentosDeCompras)
        {
            try
            {

                var DocumentosIntegrados = new List<DocumentosIntegradoViewModel>();

                foreach (var documentoDeCompras in documentosDeCompras)
                {
                    if (!VeriricarSeNumeroDeProcessoExisteNoDocumentoInterno(documentoDeCompras.NumeroDeProcesso, documentoDeCompras.CodigoPortal, documentoDeCompras.Tipodoc, documentoDeCompras.Entidade))
                    {
                        IntBEDocumentoInterno documento = new IntBEDocumentoInterno();

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
                            documento.Linhas.GetEdita(linhaNumero).CCustoCBL = linhaDocumento.CentroDeCusto;

                        }

                        if (documento.Linhas.NumItens > 0)
                        {

                            BSO.Internos.Documentos.CalculaValoresTotais(documento);
                            if (!VeriricarSeNumeroDeProcessoExisteNoDocumentoInterno(documentoDeCompras.NumeroDeProcesso, documentoDeCompras.CodigoPortal, documentoDeCompras.Tipodoc, documentoDeCompras.Entidade))
                            {
                                BSO.Internos.Documentos.Actualiza(documento);
                            }

                            var Referencia = $"{documento.Tipodoc} {documento.Serie}/{documento.NumDoc}";
                            if (!VerificarSeIdJáExiste(documentoDeCompras.CodigoPortal))
                                ReferenciarDocumentosIntegrados(documentoDeCompras.CodigoPortal, Referencia);
                            _helperRepository.CriarLog("Integração", $" O documento: { documento.Tipodoc}/{ documento.Serie}/ { documento.NumDoc}", "Sucesso");

                        }
                    }
                }
                return "OK";
            }
            catch (Exception e)
            {

                BSO.FechaEmpresaTrabalho();
                _helperRepository.CriarLog("Integração", "Integração de Documento Interno: " + e.Message.ToString(), "Erro");

                return e.Message;

            }

        }

        private float buscarCodigoDoImpostoIva(float Taxa, DateTime DataDoDocumento)
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                DataTable CodigosIva = new DataTable();
                string queryCabe = "";
                //double CodIva = 0;

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
                _helperRepository.CriarLog("Integração", "Metodo: buscarCodigoDoImpostoIva " + ex.Message.ToString(), "Erro");
                throw;
            }
            finally
            {
                conexao.Close();
            }
        }

        public void ReferenciarDocumentosIntegrados(int ID, string Referencia)
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            SqlCommand Comando = new SqlCommand();

            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                Comando.CommandText = $"insert into TDU_RefDocumentosIntegrados(CDU_ID,CDU_Referencia) values ({ID},'{Referencia}')";
                Comando.CommandType = CommandType.Text;
                Comando.Connection = conexao;
                Comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _helperRepository.CriarLog("Integração", "Metodo: ReferenciarDocumentosIntegrados " + ex.Message.ToString(), "Erro");
                throw ex;
            }
            finally
            {
                conexao.Close();
            }
        }

        public bool VerificarSeIdJáExiste(int ID)
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {

                if (conexao.State != ConnectionState.Open)
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
                _helperRepository.CriarLog("Integração", "Metodo: VerificarSeIdJáExiste " + ex.Message.ToString(), "Erro");
                throw ex;
            }
            finally
            {
                conexao.Close();
            }

            return false;

        }

        public double BuscarCambio(DateTime Data, string Moeda)
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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

                return Cambio;

            }
            catch (Exception ex)
            {
                _helperRepository.CriarLog("Integração", "Metodo: BuscarCambio " + ex.Message.ToString(), "Erro");
                throw;
            }
            finally
            {
                conexao.Close();
            }
        }

        public bool VeriricarSeNumeroDeProcessoExisteNoDocumentoInterno(string NumeroDeProcesso, int CodigoPortal, string TipoDeDocumento, string Entidade)
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                _helperRepository.CriarLog("Integração", "Metodo: VeriricarSeNumeroDeProcessoExisteNoDocumentoInterno " + ex.Message.ToString(), "Erro");
            }
            finally
            {
                conexao.Close();
            }

            return false;

        }

        public bool VeriricarSeNumeroDeProcessoExisteNoDocumentoDeCompras(string NumeroDeProcesso, int CodigoPortal, string TipoDeDocumento, string Entidade)
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {

                if (conexao.State != ConnectionState.Open)
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
                _helperRepository.CriarLog("Integração", "Metodo: VeriricarSeNumeroDeProcessoExisteNoDocumentoDeCompras " + ex.Message.ToString(), "Erro");
            }
            finally
            {
                conexao.Close();
            }

            return false;

        }

    }
}