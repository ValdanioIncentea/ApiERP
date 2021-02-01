using Portal.ApiLocal;
using PortalApi.App_Start;
using PortalApi.Models;
using PortalApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PortalApi.Repository
{
    public class DadosMestreRepository
    {

        HelperRepository _helperRepository = new HelperRepository();

        public List<MovimentosViewModel> BuscarMovimentosDosArtigos()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {
                try
                {
                    conexao.Open();

                    DataTable Movimentos = new DataTable();

                    //string queryCabe = "select ID,Artigo, Armazem,Localizacao,Lote, StockLot_Actual, StockLot_Anterior, StockArm_Actual, StockArm_Anterior, StockLoc_Actual, StockLoc_Anterior, TipoMovimento, Quantidade, Data,Stock_Actual, Stock_Anterior from INV_Movimentos order by Data desc";
                    string queryCabe = "select ID,Artigo, Armazem,Localizacao,Lote, StockLot_Actual, StockLot_Anterior, StockArm_Actual, StockArm_Anterior, StockLoc_Actual, StockLoc_Anterior, TipoMovimento, Quantidade, Data,Stock_Actual, Stock_Anterior from INV_Movimentos"
    + " where INV_Movimentos.Id not in ("
    + " select m.Id from INV_movimentos m, INV_Origens o, INV_TiposOrigem Tpo, CabecInternos inte"
    + " where m.IdOrigem = o.id  and Tpo.Id = o.IdTipoOrigem and tpo.Modulo = 'N'"
    + " and inte.Id = o.IdChave1 and inte.CDU_PROCESSO is not null)"
    + " order by Data";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Movimentos);

                    List<MovimentosViewModel> MovimentosLista = new List<MovimentosViewModel>();

                    if (Movimentos.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Movimentos.Rows)
                        {

                            MovimentosViewModel _movimentoViewModel = new MovimentosViewModel();

                            _movimentoViewModel.Artigo = Linha["Artigo"].ToString();
                            _movimentoViewModel.Armazem = Linha["Armazem"].ToString();
                            _movimentoViewModel.Localizacao = Linha["Localizacao"].ToString();
                            _movimentoViewModel.Lote = Linha["Lote"].ToString();
                            _movimentoViewModel.StockLot_Actual = Convert.ToInt32(Linha["StockLot_Actual"]);
                            _movimentoViewModel.StockLot_Anterior = Convert.ToInt32(Linha["StockLot_Anterior"]);
                            _movimentoViewModel.StockArm_Actual = Convert.ToInt32(Linha["StockArm_Actual"]);
                            _movimentoViewModel.StockArm_Anterior = Convert.ToInt32(Linha["StockArm_Anterior"]);
                            _movimentoViewModel.StockLoc_Actual = Convert.ToInt32(Linha["StockLoc_Actual"]);
                            _movimentoViewModel.StockLoc_Anterior = Convert.ToInt32(Linha["StockLoc_Anterior"]);
                            _movimentoViewModel.TipoMovimento = Linha["TipoMovimento"].ToString();
                            _movimentoViewModel.Quantidade = Convert.ToInt32(Linha["Quantidade"]);
                            _movimentoViewModel.Data = Convert.ToDateTime(Linha["Data"]);
                            _movimentoViewModel.Id = Guid.Parse(Linha["ID"].ToString());
                            _movimentoViewModel.Stock_Actual = Convert.ToInt32(Linha["Stock_Actual"]);
                            _movimentoViewModel.Stock_Anterior = Convert.ToInt32(Linha["Stock_Anterior"]);

                            MovimentosLista.Add(_movimentoViewModel);

                        }

                    }

                    conexao.Close();
                    return MovimentosLista;

                }
                catch (Exception ex)
                {
                    _helperRepository.CriarLog("Integração", "Metodo : BuscarMovimentosDosArtigos" + ex.Message.ToString(), "Erro");
                    throw;
                }
                finally
                {
                    conexao.Close();
                }
            }

        }

        public List<ArmazenViewModel> BuscarArmazens()
        {
            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    DataTable Amazens = new DataTable();

                    string queryCabe = "select Armazem,Descricao,Morada,Localidade,DataUltimaActualizacao from Armazens";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Amazens);

                    List<ArmazenViewModel> ArmazensLista = new List<ArmazenViewModel>();

                    if (Amazens.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Amazens.Rows)
                        {

                            ArmazenViewModel armazenViewModel = new ArmazenViewModel();

                            armazenViewModel.Armazem = Linha["Armazem"].ToString();
                            armazenViewModel.Descricao = Linha["Descricao"].ToString();
                            armazenViewModel.Morada = Linha["Morada"].ToString();
                            armazenViewModel.Localidade = Linha["Localidade"].ToString();
                            armazenViewModel.DataUltimaActualizacao = Linha["DataUltimaActualizacao"].ToString();

                            ArmazensLista.Add(armazenViewModel);

                        }

                    }

                    conexao.Close();
                    return ArmazensLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo : BuscarArmazens" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }

        }

        public List<LocalizacaoViewModel> BuscarLocalizacoes()
        {
            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    DataTable Localizacoes = new DataTable();

                    string queryCabe = "select Armazem,Localizacao, Descricao from ArmazemLocalizacoes";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Localizacoes);

                    List<LocalizacaoViewModel> LocalizacaoLista = new List<LocalizacaoViewModel>();

                    if (Localizacoes.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Localizacoes.Rows)
                        {

                            LocalizacaoViewModel _localizacaoViewModel = new LocalizacaoViewModel();

                            _localizacaoViewModel.Armazem = Linha["Armazem"].ToString();
                            _localizacaoViewModel.Descricao = Linha["Descricao"].ToString();
                            _localizacaoViewModel.Localizacao = Linha["Localizacao"].ToString();

                            LocalizacaoLista.Add(_localizacaoViewModel);

                        }

                    }

                    conexao.Close();
                    return LocalizacaoLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo : BuscarLocalizacoes" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<LoteViewModel> BuscarLotes()
        {
            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    DataTable Lotes = new DataTable();

                    string queryCabe = "select Lote, Descricao,DataFabrico,Validade,Activo from ArtigoLote";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Lotes);

                    List<LoteViewModel> LoteLista = new List<LoteViewModel>();

                    if (Lotes.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Lotes.Rows)
                        {

                            LoteViewModel _loteViewModel = new LoteViewModel();

                            _loteViewModel.Lote = Linha["Armazem"].ToString();

                            _loteViewModel.Descricao = Linha["Descricao"].ToString();

                            if (Linha["Validade"] != null)
                                _loteViewModel.Validade = Convert.ToDateTime(Linha["Validade"]);
                            else
                                _loteViewModel.Validade = null;

                            if (Linha["DataFabrico"] != null)
                                _loteViewModel.DataFabrico = Convert.ToDateTime(Linha["DataFabrico"]);
                            else
                                _loteViewModel.DataFabrico = null;

                            _loteViewModel.Activo = Convert.ToBoolean(Linha["Activo"]);

                            LoteLista.Add(_loteViewModel);

                        }

                    }

                    conexao.Close();
                    return LoteLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarLotes" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<ArtigoViewModel> BuscarTodosArtigos()
        {
            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    DataTable Artigos = new DataTable();

                    string queryCabe = "select Artigo,ArmazemSugestao,Descricao,Familia,Iva,MovStock,UnidadeBase,DataUltimaActualizacao,UnidadeBase, UnidadeCompra, UnidadeEntrada,UnidadeSaida, UnidadeVenda FROM Artigo WHERE Familia is not null";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Artigos);

                    List<ArtigoViewModel> ArtigosLista = new List<ArtigoViewModel>();

                    if (Artigos.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Artigos.Rows)
                        {

                            ArtigoViewModel artigoViewModel = new ArtigoViewModel();

                            artigoViewModel.Codigo = Linha["Artigo"].ToString();
                            artigoViewModel.ArmazemSugestao = Linha["ArmazemSugestao"].ToString();
                            artigoViewModel.Descricao = Linha["Descricao"].ToString();
                            artigoViewModel.Familia = Linha["Familia"].ToString();
                            artigoViewModel.IVA = Linha["IVA"].ToString();
                            artigoViewModel.MovStock = Linha["MovStock"].ToString();
                            artigoViewModel.UnidadeBase = Linha["UnidadeBase"].ToString();
                            artigoViewModel.UnidadeCompra = Linha["UnidadeCompra"].ToString();
                            artigoViewModel.UnidadeEntrada = Linha["UnidadeEntrada"].ToString();
                            artigoViewModel.UnidadeSaida = Linha["UnidadeSaida"].ToString();
                            artigoViewModel.UnidadeVenda = Linha["UnidadeVenda"].ToString();
                            artigoViewModel.DataUltimaActualizacao = Linha["DataUltimaActualizacao"].ToString();

                            ArtigosLista.Add(artigoViewModel);

                        }

                    }

                    conexao.Close();
                    return ArtigosLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarTodosArtigos" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }

        }

        public List<UnidadeConversaoViewModel> BuscarUnidadesConversao()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {
                    conexao.Open();

                    DataTable UnidadesConversao = new DataTable();

                    string queryCabe = "select UnidadeOrigem, UnidadeDestino, UtilizaFormula, FactorConversao, Formula from UnidadesConversao";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(UnidadesConversao);

                    List<UnidadeConversaoViewModel> UnidadeConversaoLista = new List<UnidadeConversaoViewModel>();

                    if (UnidadesConversao.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in UnidadesConversao.Rows)
                        {

                            UnidadeConversaoViewModel UnidadeConvesao = new UnidadeConversaoViewModel();

                            UnidadeConvesao.UnidadeDestino = Linha["UnidadeDestino"].ToString();
                            UnidadeConvesao.UnidadeOrigem = Linha["UnidadeOrigem"].ToString();
                            UnidadeConvesao.Formula = Linha["Formula"].ToString();
                            UnidadeConvesao.FactorConversao = Convert.ToDouble(Linha["FactorConversao"]);
                            UnidadeConvesao.UtilizaFormula = Convert.ToBoolean(Linha["UtilizaFormula"]);

                            UnidadeConversaoLista.Add(UnidadeConvesao);

                        }

                    }
                    conexao.Close();
                    return UnidadeConversaoLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarUnidadesConversao" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }

        }

        public List<DocumentoViewModel> BuscarDocumentos()
        {
            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable DocCompras = new DataTable();

                    string queryCabe = "SELECT Documento,Descricao,Modulo='V' FrOM DocumentosVenda UNION SELECT Documento,Descricao,Modulo = 'C' FrOM DocumentosCompra UNION SELECT Documento,Descricao,Modulo = 'N' FROM DocumentosInternos";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(DocCompras);

                    List<DocumentoViewModel> ComprasLista = new List<DocumentoViewModel>();

                    if (DocCompras.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in DocCompras.Rows)
                        {

                            DocumentoViewModel DocCmp = new DocumentoViewModel();

                            DocCmp.Descricao = Linha["Descricao"].ToString();
                            DocCmp.Documento = Linha["Documento"].ToString();
                            DocCmp.Modulo = Linha["Modulo"].ToString();

                            ComprasLista.Add(DocCmp);

                        }

                    }

                    conexao.Close();
                    return ComprasLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarDocumentos" + ex.Message.ToString(), "Erro");
                    throw;
                }

            }
        }

        public List<CentrosDeCustosViewModel> BuscarCentrosDeCustos()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable Centros = new DataTable();

                    string queryCabe = "select Centro, Descricao, TipoConta, Ano from PlanoCentros where TipoConta = 'M'";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Centros);

                    List<CentrosDeCustosViewModel> CentrosLista = new List<CentrosDeCustosViewModel>();

                    if (Centros.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Centros.Rows)
                        {

                            CentrosDeCustosViewModel CentroViewModel = new CentrosDeCustosViewModel();

                            CentroViewModel.Centro = Linha["Centro"].ToString();
                            CentroViewModel.Descricao = Linha["Descricao"].ToString();
                            CentroViewModel.TipoConta = Linha["TipoConta"].ToString();
                            CentroViewModel.Ano = Convert.ToInt32(Linha["Ano"]);

                            CentrosLista.Add(CentroViewModel);

                        }

                    }
                    conexao.Close();
                    return CentrosLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarCentrosDeCustos" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<ObrasViewModel> BuscarObras()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable ObrasDataTable = new DataTable();

                    string queryCabe = "select ID,Descricao,Codigo,ERPTipoEntidadeA, ERPEntidadeA from COP_Obras";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(ObrasDataTable);

                    List<ObrasViewModel> Obras = new List<ObrasViewModel>();


                    if (ObrasDataTable.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in ObrasDataTable.Rows)
                        {

                            ObrasViewModel ObrasViewModel = new ObrasViewModel()
                            {
                                Codigo = Linha["Codigo"].ToString(),
                                Descricao = Linha["Descricao"].ToString(),
                                Id = Guid.Parse(Linha["ID"].ToString()),
                                Entidade = Linha["ERPEntidadeA"].ToString(),
                                TipoEntidade = Linha["ERPTipoEntidadeA"].ToString()
                            };

                            Obras.Add(ObrasViewModel);

                        };

                    }

                    conexao.Close();

                    return Obras;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarObras" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<EntidadesViewModel> BusarEntidades()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable FamiliasDataTable = new DataTable();

                    string queryCabe = "select e.Entidade,e.NomeFiscal,e.Nome,e.Localidade,e.Morada,e.NumContrib,p.Descricao,e.TipoEntidade, e.EnderecoWeb from v_entidades e, Paises p where p.Pais = e.Pais AND TipoEntidade in ('R','C','F','D')";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(FamiliasDataTable);

                    List<EntidadesViewModel> Entidades = new List<EntidadesViewModel>();


                    if (FamiliasDataTable.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in FamiliasDataTable.Rows)
                        {

                            EntidadesViewModel entidadeViewModel = new EntidadesViewModel()
                            {
                                Codigo = Linha["Entidade"].ToString(),
                                Abreviacao = Helper.Abreviar(Linha["NomeFiscal"].ToString()),
                                Nome = Linha["Nome"].ToString(),
                                Localidade = Linha["Localidade"].ToString(),
                                Morada = Linha["Morada"].ToString(),
                                NumContribuinte = Linha["NumContrib"].ToString(),
                                Pais = Linha["Descricao"].ToString(),
                                Tipo = Linha["TipoEntidade"].ToString(),
                                Email = Linha["EnderecoWeb"].ToString()
                            };

                            Entidades.Add(entidadeViewModel);

                        }

                    }

                    conexao.Close();
                    return Entidades;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BusarEntidades" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<FamiliaViewModel> BuscarFamilias()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable Familias = new DataTable();

                    string queryCabe = "select Familia,Descricao,DataUltimaActualizacao from Familias";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Familias);

                    List<FamiliaViewModel> FamiliaLista = new List<FamiliaViewModel>();


                    if (Familias.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Familias.Rows)
                        {

                            FamiliaViewModel familiaViewModel = new FamiliaViewModel();
                            familiaViewModel.ID = Linha["Familia"].ToString();
                            familiaViewModel.Nome = Linha["Descricao"].ToString();
                            familiaViewModel.DataActualizacao = Linha["DataUltimaActualizacao"].ToString();
                            FamiliaLista.Add(familiaViewModel);
                        };

                    }

                    conexao.Close();
                    return FamiliaLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarFamilias" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<DepartamentoViewModel> BuscarDepartamentos()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable DepartamentoDataTable = new DataTable();

                    string queryCabe = "select Entidade, Nome from V_Entidades where TipoEntidade = 'D'";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(DepartamentoDataTable);

                    List<DepartamentoViewModel> DepartamentoLista = new List<DepartamentoViewModel>();


                    if (DepartamentoDataTable.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in DepartamentoDataTable.Rows)
                        {
                            DepartamentoViewModel _departamentoViewModel = new DepartamentoViewModel();
                            _departamentoViewModel.Codigo = Linha["Entidade"].ToString();
                            _departamentoViewModel.Descricao = Linha["Nome"].ToString();
                            DepartamentoLista.Add(_departamentoViewModel);
                        };

                    }

                    conexao.Close();
                    return DepartamentoLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarDepartamentos" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<ArtigoMoedaViewModel> BuscarArtigoMoeda()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable Moedas = new DataTable();

                    List<ArtigoMoedaViewModel> ArtigoMoedaViewModelLista = new List<ArtigoMoedaViewModel>();

                    string queryCabe = "select am.Artigo,am.Moeda,am.PVP1,am.PVP2,am.PVP3,am.Unidade from ArtigoMoeda am, Artigo a, Familias f where am.Artigo = a.Artigo and f.familia = a.Familia";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Moedas);

                    if (Moedas.Rows.Count > 0)
                    {

                        foreach (DataRow moeda in Moedas.Rows)
                        {
                            ArtigoMoedaViewModel artigoMoedaViewModel = new ArtigoMoedaViewModel();

                            artigoMoedaViewModel.Artigo = moeda["Artigo"].ToString();
                            artigoMoedaViewModel.Nome = moeda["Moeda"].ToString();
                            artigoMoedaViewModel.PVP1 = Convert.ToDouble(moeda["PVP1"]);
                            artigoMoedaViewModel.PVP2 = Convert.ToDouble(moeda["PVP2"]);
                            artigoMoedaViewModel.PVP3 = Convert.ToDouble(moeda["PVP3"]);
                            artigoMoedaViewModel.Unidade = moeda["Unidade"].ToString();

                            ArtigoMoedaViewModelLista.Add(artigoMoedaViewModel);

                        }

                    }
                    conexao.Close();

                    return ArtigoMoedaViewModelLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();

                    _helperRepository.CriarLog("Integração", "Metodo: BuscarArtigoMoeda" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<DocumentosIntegradoViewModel> BuscarReferenciasDeDocumentosIntegrados()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable Referencias = new DataTable();

                    string queryCabe = "select CDU_ID,CDU_Referencia from TDU_RefDocumentosIntegrados";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Referencias);

                    List<DocumentosIntegradoViewModel> ReferenciasLista = new List<DocumentosIntegradoViewModel>();

                    if (Referencias.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Referencias.Rows)
                        {

                            DocumentosIntegradoViewModel Documento = new DocumentosIntegradoViewModel();

                            Documento.ID = Convert.ToInt16(Linha["CDU_ID"]);
                            Documento.Referencia = Linha["CDU_Referencia"].ToString();

                            ReferenciasLista.Add(Documento);

                        }

                    }

                    conexao.Close();

                    return ReferenciasLista;
                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo: BuscarReferenciasDeDocumentosIntegrados" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public void EliminarReferenciasDeDocumentosIntegrados(DocumentoAprovadosViewModel Documento)
        {

            SqlCommand Comando = new SqlCommand();

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    var referencias = Documento.Referencia.Split(',');
                    int contador = 0;
                    for (int i = 0; i < referencias.Length; i++)
                    {
                        Comando.CommandText = $"DELETE FROM TDU_RefDocumentosIntegrados WHERE CDU_ID = { Convert.ToInt32(referencias[i])}";
                        Comando.CommandType = CommandType.Text;
                        Comando.Connection = conexao;
                        Comando.ExecuteNonQuery();
                        contador++;
                    }
                    conexao.Close();
                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo : <List>EliminarReferenciasDeDocumentosIntegrados" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public List<DocumentosIntegradoViewModel> EliminarReferenciasDeDocumentosIntegrados()
        {

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    DataTable Referencias = new DataTable();

                    string queryCabe = "select CDU_ID,CDU_Referencia from TDU_RefDocumentosIntegrados";

                    SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                    reader.Fill(Referencias);

                    List<DocumentosIntegradoViewModel> ReferenciasLista = new List<DocumentosIntegradoViewModel>();

                    if (Referencias.Rows.Count > 0)
                    {

                        foreach (DataRow Linha in Referencias.Rows)
                        {

                            DocumentosIntegradoViewModel Documento = new DocumentosIntegradoViewModel();

                            Documento.ID = Convert.ToInt16(Linha["CDU_ID"]);
                            Documento.Referencia = Linha["CDU_Referencia"].ToString();

                            ReferenciasLista.Add(Documento);

                        }

                    }

                    conexao.Close();
                    return ReferenciasLista;

                }
                catch (Exception ex)
                {
                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo : EliminarReferenciasDeDocumentosIntegrados" + ex.Message.ToString(), "Erro");
                    throw;
                }
            }
        }

        public void AlterarEstadoDocumentoInterno(DocumentoAprovadosViewModel documentoAprovados)
        {

            SqlConnection conexao = Singleton.ConectarComOBancoBanco;
            SqlTransaction Trasancao = null;
            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();
                Trasancao = conexao.BeginTransaction();

                var referencias = documentoAprovados.Referencia.Split(',');
                int Contador = 0;
                for (int i = 0; i < referencias.Length; i++)
                {

                    SqlCommand Comando = new SqlCommand();

                    AlterarEstadoDoCabecalhoDoDocumentoInterno(Trasancao, Comando, documentoAprovados.Estado, referencias[i], conexao);

                    AlterarEstadoDaLinhaDoDocumentoInterno(Trasancao, Comando, documentoAprovados.Estado, referencias[i], conexao);
                    Contador++;
                }

                if (Contador > 0)
                {
                    Trasancao.Commit();
                }
            }
            catch (Exception ex)
            {
                Trasancao.Rollback();
                _helperRepository.CriarLog("Integração", "Metodo : AlterarEstadoDocumentoInterno" + ex.Message.ToString(), "Erro");
                throw ex;
            }
            finally
            {
                conexao.Close();
            }
        }

        public void AlterarEstadoDoCabecalhoDoDocumentoInterno(SqlTransaction Trasancao, SqlCommand Comando, string Estado, string NumeroDeProcesso, SqlConnection conexao)
        {
            try
            {

                Comando.CommandText = @"UPDATE CabecInternos set Estado = @Estado WHERE CDU_PROCESSO = @NumeroDeProcesso AND Estado <> @Estado";
                Comando.CommandTimeout = 0;
                Comando.Parameters.AddWithValue("@Estado", Estado);
                Comando.Parameters.AddWithValue("@NumeroDeProcesso", NumeroDeProcesso);
                Comando.CommandType = CommandType.Text;
                Comando.Connection = conexao;
                Comando.Transaction = Trasancao;
                Comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AlterarEstadoDaLinhaDoDocumentoInterno(SqlTransaction Trasancao, SqlCommand Comando, string Estado, string NumeroDeProcesso, SqlConnection conexao)
        {
            try
            {

                Comando.CommandText = @"UPDATE l set l.Estado = @Estado from  LinhasInternos l, CabecInternos c WHERE  c.Id = l.IdCabecInternos AND c.CDU_PROCESSO = @NumeroDeProcesso AND l.Estado <> @Estado";
                Comando.CommandType = CommandType.Text;
                Comando.Connection = conexao;
                Comando.Transaction = Trasancao;
                Comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}