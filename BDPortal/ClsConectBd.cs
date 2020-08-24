using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace BDPortal
{
   public class ClsConectBd
    {
        private SqlConnection con = new SqlConnection("Server='" + ConfigurationManager.AppSettings["instancia"].ToString() + "';Database='" + ConfigurationManager.AppSettings["bd"].ToString() + "';User Id='" + ConfigurationManager.AppSettings["usuario"].ToString() + "';Password='" + ConfigurationManager.AppSettings["senhabd"].ToString() + "'");
        
        public SqlConnection getConectionString()
        {
            return con;
        }

        public void abreconexao()
        {
            con.Open();
        }

        public string comand(string query)
        {
            string message = string.Empty;
            try
            {
                abreconexao();
                SqlCommand cmd = new SqlCommand(query, con);
                int message1 = cmd.ExecuteNonQuery();
                if (message1 == 1)
                    message = "1";
                else
                    message = "0";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                fecharconexao();
            }
            return message;
        }

        public DataTable select(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                abreconexao();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter dt2 = new SqlDataAdapter(cmd);
                dt2.Fill(dt);
                if (dt.Rows.Count == 0)
                    dt = null;
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                fecharconexao();
            }
            return dt;
        }

        public void fecharconexao()
        {
            con.Close();
        }
    }
}
