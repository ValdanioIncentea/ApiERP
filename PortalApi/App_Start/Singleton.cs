using ErpBS100;
using PortalApi.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PortalApi.App_Start
{
    public sealed class Singleton
    {
        private static ErpBS bso = null;
        private static readonly object padlock = new object();
        private static readonly object padlockBD = new object();
        private static SqlConnection Conexao = null;
        private static HelperRepository _helperRepository = new HelperRepository();

        public static string ConnctionString = $"Data Source={ConfigurationManager.AppSettings["Instancia"]};Initial Catalog={ConfigurationManager.AppSettings["BancoDeDados"]};Persist Security Info=True;User ID={ConfigurationManager.AppSettings["User"]};Password={ConfigurationManager.AppSettings["Senha"]};MultipleActiveResultSets=True";

        Singleton()
        {
        }

        public static ErpBS AbrirEmpresaERPV10
        {
            get
            {
                lock (padlock)
                {
                    if (bso == null)
                    {
                        try
                        {
                            ErpBS bso = new ErpBS();
                            string plataforma = ConfigurationManager.AppSettings["plataforma"].ToString();
                            if (plataforma.Equals("0"))
                            {
                                bso.AbreEmpresaTrabalho(StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial, ConfigurationManager.AppSettings["empresaprimavera"].ToString(), ConfigurationManager.AppSettings["usuarioprimavera"].ToString(), ConfigurationManager.AppSettings["senhaprimavera"].ToString(), null, "DEFAULT");
                            }
                            else
                            {
                                bso.AbreEmpresaTrabalho(StdBE100.StdBETipos.EnumTipoPlataforma.tpProfissional, ConfigurationManager.AppSettings["empresaprimavera"].ToString(), ConfigurationManager.AppSettings["usuarioprimavera"].ToString(), ConfigurationManager.AppSettings["senhaprimavera"].ToString(), null, "DEFAULT");
                            }
                            //bso.AbreEmpresaTrabalho(StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial,ConfigurationManager.AppSettings["EmpresaErp"].ToString(), ConfigurationManager.AppSettings["UserErp"].ToString(), ConfigurationManager.AppSettings["SenhaErp"].ToString(), null, "DEFAULT");
                            return bso;
                        }catch(Exception ex)
                        {
                            _helperRepository.CriarLog("Integração", "Metodo : AbrirEmpresaERPV10 " + ex.Message.ToString(), "Erro");
                        }
                    }
                    return bso;
                }
            }
        }

        public static SqlConnection ConectarComOBancoBanco
        {
            get
            {
                lock (padlockBD)
                {
                    if (Conexao == null)
                    {
                        Conexao = new SqlConnection($"Data Source={ConfigurationManager.AppSettings["Instancia"].ToString()};Initial Catalog={ConfigurationManager.AppSettings["BancoDeDados"].ToString()};Persist Security Info=True;User ID={ConfigurationManager.AppSettings["User"].ToString()};Password={ConfigurationManager.AppSettings["Senha"].ToString()};MultipleActiveResultSets=True");
                        return Conexao;
                    }
                    return Conexao;
                }
            }
        }
    }
}