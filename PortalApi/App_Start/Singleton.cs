using ErpBS100;
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
                        ErpBS bso = new ErpBS(); 
                        bso.AbreEmpresaTrabalho(StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial,ConfigurationManager.AppSettings["EmpresaErp"].ToString(), ConfigurationManager.AppSettings["UserErp"].ToString(), ConfigurationManager.AppSettings["SenhaErp"].ToString(), null, "DEFAULT");
                        return bso;
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