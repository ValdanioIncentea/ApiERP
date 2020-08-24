﻿using Portal.ApiLocal;
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

        public List<MovimentosViewModel> BuscarMovimentosDosArtigos()
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                DataTable Movimentos = new DataTable();

                string queryCabe = "select ID,Artigo, Armazem,Localizacao,Lote, StockLot_Actual, StockLot_Anterior, StockArm_Actual, StockArm_Anterior, StockLoc_Actual, StockLoc_Anterior, TipoMovimento, Quantidade, Data,Stock_Actual, Stock_Anterior from INV_Movimentos order by Data desc";

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
                conexao.Close();
                throw ex;
            }

        }
        
        public List<ArmazenViewModel> BuscarArmazens()
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                throw ex;
            }

        }

        public List<LocalizacaoViewModel> BuscarLocalizacoes()
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                throw ex;
            }

        }

        public List<LoteViewModel> BuscarLotes()
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                throw ex;
            }

        }

        public List<ArtigoViewModel> BuscarTodosArtigos()
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                DataTable Artigos = new DataTable();

                string queryCabe = "select Artigo,ArmazemSugestao,Descricao,Familia,Iva,MovStock,UnidadeBase,DataUltimaActualizacao FROM Artigo WHERE Familia is not null";

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
                throw ex;
            }

        }

        public List<CentrosDeCustosViewModel> BuscarCentrosDeCustos()
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                throw ex;
            }
        }

        internal List<ObrasViewModel> BuscarObras()
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                throw ex;
            }

        }

        internal List<EntidadesViewModel> BusarEntidades()
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                DataTable FamiliasDataTable = new DataTable();

                string queryCabe = "select e.Entidade,e.NomeFiscal,e.Nome,e.Localidade,e.Morada,e.NumContrib,p.Descricao,e.TipoEntidade, e.EnderecoWeb from v_entidades e, Paises p where p.Pais = e.Pais AND TipoEntidade in ('R','C','F')";

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
                throw ex;
            }

        }

        public List<FamiliaViewModel> BuscarFamilias()
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                throw ex;
            }

        }

        public List<DepartamentoViewModel> BuscarDepartamentos()
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                DataTable DepartamentoDataTable = new DataTable();

                string queryCabe = "select Departamento,Descricao from Departamentos";

                SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                reader.Fill(DepartamentoDataTable);

                List<DepartamentoViewModel> DepartamentoLista = new List<DepartamentoViewModel>();


                if (DepartamentoDataTable.Rows.Count > 0)
                {

                    foreach (DataRow Linha in DepartamentoDataTable.Rows)
                    {
                        DepartamentoViewModel familiaViewModel = new DepartamentoViewModel();
                        familiaViewModel.Codigo = Linha["Departamento"].ToString();
                        familiaViewModel.Descricao = Linha["Descricao"].ToString();
                        DepartamentoLista.Add(familiaViewModel);
                    };

                }

                conexao.Close();

                return DepartamentoLista;

            }
            catch (Exception ex)
            {
                conexao.Close();
                throw ex;
            }

        }

        public List<ArtigoMoedaViewModel> BuscarArtigoMoeda()
        {
            var conexao = Singleton.ConectarComOBancoBanco;
            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                DataTable Moedas = new DataTable();

                List<ArtigoMoedaViewModel> ArtigoMoedaViewModelLista = new List<ArtigoMoedaViewModel>();

                string queryCabe = "select Artigo,Moeda,PVP1,PVP2,PVP3,Unidade from ArtigoMoeda";

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
                throw ex;
            }
        }

        public List<DocumentosIntegradoViewModel> BuscarReferenciasDeDocumentosIntegrados()
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                throw ex;
            }

        }

        public void EliminarReferenciasDeDocumentosIntegrados(DocumentoAprovadosViewModel Documento)
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            SqlCommand Comando = new SqlCommand();

            try
            {
                if (conexao.State != ConnectionState.Open)
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
            }
            catch (Exception ex)
            {
                conexao.Close();
                throw ex;
            }


        }

        public List<DocumentosIntegradoViewModel> EliminarReferenciasDeDocumentosIntegrados()
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {
                if (conexao.State != ConnectionState.Open)
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
                throw ex;
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
                conexao.Close();
                throw ex;
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