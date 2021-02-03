using BasBE100;
using CblBE100;
using ErpBS100;
using PortalApi.App_Start;
using PortalApi.Models;
using PortalApi.ViewModel;
using StdBE100;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PortalApi.Repository
{
    public class EntidadeRepository
    {
        HelperRepository _helperRepository = new HelperRepository();

        public void IntegrarFornecedor(ErpBS BSO, List<FornecedorViewModel> Fornecedores)
        {

            try
            {

                foreach (var _Fornecedor in Fornecedores)
                {

                    if (!BSO.Base.Fornecedores.Existe(_Fornecedor.Codigo))
                    {

                        BasBEFornecedor Fornecedor = new BasBEFornecedor();
                        Fornecedor.Fornecedor = _Fornecedor.Codigo;
                        Fornecedor.Nome = _Fornecedor.Nome;
                        Fornecedor.NomeFiscal = _Fornecedor.NomeFiscal;
                        Fornecedor.Morada = _Fornecedor.Morada;
                        Fornecedor.Localidade = _Fornecedor.Localidade;
                        Fornecedor.CodigoPostal = _Fornecedor.CodigoPostal;
                        Fornecedor.NumContribuinte = _Fornecedor.NumContribuinte;
                        Fornecedor.LocalidadeCodigoPostal = _Fornecedor.LocalidadeCodigoPostal;
                        Fornecedor.Telefone = _Fornecedor.Telefone;
                        Fornecedor.Fax = _Fornecedor.Fax;
                        Fornecedor.SegmentoTerceiro = _Fornecedor.SegmentoTerceiro;
                        Fornecedor.Moeda = _Fornecedor.Moeda;
                        Fornecedor.Pais = _Fornecedor.Pais;
                        Fornecedor.UtilizaIdioma = true;
                        Fornecedor.PessoaSingular = _Fornecedor.PessoaSingular;
                        Fornecedor.B2BEnderecoMail = _Fornecedor.Email;
                        Fornecedor.Idioma = BSO.Base.Paises.DaValorAtributo(Fornecedor.Pais, "Idioma");

                        if (Fornecedor.Pais == "AN")
                        {
                            Fornecedor.LocalOperacao = "0";
                            Fornecedor.TipoMercado = "1";
                        }
                        else
                        {
                            Fornecedor.LocalOperacao = "3";
                            Fornecedor.TipoMercado = "2";
                            Fornecedor.RegimeIvaReembolsos = 5;
                        }

                        Fornecedor.CondPag = _Fornecedor.condPag;
                        Fornecedor.ModoPag = "AMORT";
                        Fornecedor.EnderecoWeb = _Fornecedor.Email;

                        BSO.Base.Fornecedores.Actualiza(Fornecedor);

                        int anoCBL = BSO.Contabilidade.ExerciciosCBL.DaUltimoAno();

                        CriaContaCBL(BSO, _Fornecedor.Codigo, _Fornecedor.Nome, "F", _Fornecedor.Codigo, anoCBL, _Fornecedor.SegmentoTerceiro);

                    }
                }
            }
            catch (Exception e)
            {
                _helperRepository.CriarLog("Integração", "Integração de Fornecedor: " + e.Message.ToString(), "Erro");
            }

        }

        public void CriaContaCBL(ErpBS BSO, string Entidade, string Nome, string TipoEntidade, string ContaCBL, int Ano, string SegmentoTerceiro)
        {

            var strSQL = "";
            string CBL;

            if (TipoEntidade == "C")
            {
                strSQL = "SELECT ContaCli01,ContaCli02,ContaCli03 FROM ExerciciosCBL WHERE Ano='" + Ano + "'";
            }

            if (TipoEntidade == "F")
            {
                strSQL = "SELECT ContaFor01,ContaFor02,ContaFor03 FROM ExerciciosCBL WHERE Ano='" + Ano + "'";
            }

            try
            {

                StdBELista PrefixoConta = new StdBELista();

                PrefixoConta = BSO.Consulta(strSQL);

                var TotalColunas = PrefixoConta.NumColunas();

                for (int i = 0; i < TotalColunas; i++)
                {

                    CblBEConta Conta = new CblBEConta();

                    CBL = Convert.ToString(PrefixoConta.Valor(i));

                    if (!String.IsNullOrEmpty(CBL))
                    {

                        CBL = PrefixoConta.Valor(i);

                        if (SegmentoTerceiro == "001")
                        {
                            CBL = CBL.Substring(0, 4) + "1" + ContaCBL;
                        }
                        else if (SegmentoTerceiro == "002")
                        {
                            CBL = CBL.Substring(0, 4) + "2" + ContaCBL;
                        }
                        else
                        {
                            CBL = CBL + ContaCBL;
                        }

                        if (CBL != "")
                        {

                            Conta.Conta = CBL.Replace("?", "");
                            Conta.Descricao = Nome;
                            Conta.TipoConta = "M";
                            Conta.Grupo = "TERC";
                            Conta.Entidade = Entidade;
                            Conta.Ano = Ano;
                            Conta.TipoEntidade = TipoEntidade;

                            if (!BSO.Contabilidade.PlanoContas.Existe(Ano, Conta.Conta))
                            {
                                BSO.Contabilidade.PlanoContas.Actualiza(Conta);
                            }

                        }
                    }

                }

                if (TipoEntidade == "C" || TipoEntidade == "F")
                    AssociarEntidadeComAContabilidae((TipoEntidade == "C" ? 1 : 2), Ano, Entidade, ContaCBL, "001", "1");

            }
            catch (Exception ex)
            {
                _helperRepository.CriarLog("Integração", "Metodo : Erro no CriaContaCBL(); Com a Entidade " + Entidade + " Detalhes: " + ex.Message.ToString(), "Erro");
            }

        }

        public void CriaRetencao(ErpBS BSO, string TipoEntidade, string Entidade, string TipoRendimento, double Valor)
        {
            BasBEOutraRetencao RetEntidade = new BasBEOutraRetencao();

            string EntidadeRetencao = "";

            if (BSO.Consulta("SELECT EntidadeRec FROM TiposRendimento WHERE TipoRendimento ='" + TipoRendimento + "'").Vazia() == false)
            {

                EntidadeRetencao = BSO.Consulta("SELECT EntidadeRec FROM TiposRendimento WHERE TipoRendimento ='" + TipoRendimento + "'").Valor(0);

            }

            string PendenteAGerar = BSO.PagamentosRecebimentos.TiposConta.DaValorAtributo(TipoRendimento, "PendenteAGerarRec");
            string PendenteAGerarEstorno = BSO.PagamentosRecebimentos.TiposConta.DaValorAtributo(TipoRendimento, "PendenteAGerarPag");
            double Percentagem = BSO.PagamentosRecebimentos.TiposConta.DaValorAtributo(TipoRendimento, "Percentagem");

            RetEntidade.TipoEntidade = TipoEntidade;
            RetEntidade.Entidade = Entidade;
            RetEntidade.TipoEntidadeRetencao = "E";
            RetEntidade.TipoRendimento = TipoRendimento;
            RetEntidade.EntidadeRetencao =EntidadeRetencao;

            if (Valor != 0)
            {
                RetEntidade.Valor = Valor;
            }
            else
            {
                RetEntidade.Valor = Percentagem;
            }

            RetEntidade.PendenteAGerar = PendenteAGerar;
            RetEntidade.PendenteAGerarEstorno = PendenteAGerarEstorno;

            if (BSO.Consulta("SELECT Entidade FROM OutrasRetencoes WHERE TipoEntidade='" + TipoEntidade + "' AND Entidade='" + Entidade + "' AND TipoRendimento='" + TipoRendimento + "'").Vazia())
            {
                BSO.PagamentosRecebimentos.OutrasRetencoes.Actualiza(RetEntidade);
            }

        }

        public void AssociarEntidadeComAContabilidae(int Tabela, int Ano, string Entidade, string ContaCBL, string Plano = "001", string Coluna = "1")
        {

            SqlCommand Comando = new SqlCommand();

            using (SqlConnection conexao = new SqlConnection(Singleton.ConnctionString))
            {

                try
                {

                    conexao.Open();

                    Comando.CommandText = $"INSERT INTO CnfTabLigCBL(Id,Tabela,Ano,Plano,Entidade,Coluna,Conta)VALUES" +
                     $"(NEWID(),{Tabela}, {Ano},'{Plano}','{Entidade}',{Coluna},'{ContaCBL}')";

                    Comando.CommandType = CommandType.Text;
                    Comando.Connection = conexao;
                    Comando.ExecuteNonQuery();

                    conexao.Close();

                }
                catch (Exception ex)
                {

                    conexao.Close();
                    _helperRepository.CriarLog("Integração", "Metodo : AssociarEntidadeComAContabilidae(); " + ex.Message.ToString(), "Erro");
                    throw;

                }

            }

        }

    }
}