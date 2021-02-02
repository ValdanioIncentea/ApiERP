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

        ErpBS BSO;

        public void IntegrarFornecedor(List<DocumentoDeCompras> documentosDeCompras)
        {

            try
            {



            }
            catch (Exception e)
            {
                _helperRepository.CriarLog("Integração", "Integração de Documento de Compra: " + e.Message.ToString(), "Erro");
            }

        }


        public void CriaContaCBL(string Entidade, string Nome, string TipoEntidade, string ContaCBL, short Ano, string SegmentoTerceiro = "")
        {

            StdBELista PrefixoConta = new StdBELista();
            var strSQL = "";
            string CBL;
            int col;
            object r;

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
                PrefixoConta = BSO.Consulta(strSQL);
                col = 0;


                for (int i = 1; i < PrefixoConta.NumColunas() + 1; i++)
                {

                    CblBEConta Conta = new CblBEConta();

                    CBL = PrefixoConta.Valor(col);

                    if (CBL != "" && SegmentoTerceiro == "001")
                    {
                        CBL = CBL.Substring(0, 4) + "1";
                        CBL = CBL + ContaCBL;
                    }
                    else if (CBL != "" && SegmentoTerceiro == "002")
                    {
                        CBL = CBL.Substring(0, 4) + "2";
                        CBL = CBL + ContaCBL;
                    }
                    else
                    {
                        CBL = PrefixoConta.Valor(col) + ContaCBL;
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
                        BSO.Contabilidade.PlanoContas.Actualiza(Conta);

                    }

                    col = col + 1;

                }

                if (TipoEntidade == "C" || TipoEntidade == "F")
                    AssociarEntidadeComAContabilidae((TipoEntidade == "C" ? 1 : 2), Ano, Entidade, ContaCBL, "001", "1");

            }
            catch (Exception ex)
            {
                _helperRepository.CriarLog("Integração", "Metodo : Erro no CriaContaCBL(); Com a Entidade " + Entidade + " Detalhes: " + ex.Message.ToString(), "Erro");
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