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
    public class RasteabilidadeRepository
    {

        HelperRepository _helperRepository = new HelperRepository();

        public string LinhaOrigemDoDocumentoInternoDeOrigem(string NumeroDeProcesso, string Artigo, string TipoDocumentos)
        {
            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                string queryCabe = $"SELECT l.Id FROM LinhasInternos l inner join CabecInternos c on l.IdCabecInternos = c.Id where c.TipoDoc IN {TipoDocumentos} AND l.Artigo = '{Artigo}' AND c.CDU_PROCESSO = '{NumeroDeProcesso}'";

                DataTable IDdoDocumentos = new DataTable();

                SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                _helperRepository.CriarLog("Rastreabilidade", $" QUERY:  " + queryCabe, "Rastreabilidade");

                reader.Fill(IDdoDocumentos);

                if (IDdoDocumentos.Rows.Count > 0)
                {

                    foreach (DataRow Linha in IDdoDocumentos.Rows)
                    {
                        return Linha["Id"].ToString();
                    }

                }

                return null;

            }
            catch (Exception ex)
            {
                _helperRepository.CriarLog("Integração", "Metodo: LinhaOrigemDoDocumentoInternoDeOrigem" + ex.Message.ToString(), "Erro");
                throw;
            }
            finally
            {
                conexao.Close();
            }
        }

        public string BuscarOrigemDoDocumetoDeCompra(string NumeroDeProcesso, string TipoDeDocumento)
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                string queryCabe = $"select Id from CabecCompras where TipoDoc = '{TipoDeDocumento}' and CDU_Processo = '{NumeroDeProcesso}'";

                DataTable IDdoDocumentos = new DataTable();

                SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                reader.Fill(IDdoDocumentos);

                if (IDdoDocumentos.Rows.Count > 0)
                {

                    foreach (DataRow Linha in IDdoDocumentos.Rows)
                    {
                        return Linha["Id"].ToString();
                    }

                }

                return null;

            }
            catch (Exception ex)
            {
                _helperRepository.CriarLog("Integração", "Metodo: BuscarOrigemDoDocumetoDeCompra" + ex.Message.ToString(), "Erro");
                throw;
            }
            finally
            {
                conexao.Close();
            }
        }

        public DataTable BuscarLinhasDoDocumetoDeCompra(string IdDocumentoCompras)
        {

            var conexao = Singleton.ConectarComOBancoBanco;

            try
            {

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                string queryCabe = $"select Artigo,NumLinha from LinhasCompras where IdCabecCompras = '{IdDocumentoCompras}'";

                DataTable LinhasDoDocumento = new DataTable();

                SqlDataAdapter reader = new SqlDataAdapter(queryCabe, conexao);

                reader.Fill(LinhasDoDocumento);

                return LinhasDoDocumento;

            }
            catch (Exception ex)
            {
                _helperRepository.CriarLog("Integração", "Metodo: BuscarOrigemDoDocumetoDeCompra" + ex.Message.ToString(), "Erro");
                throw;
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}