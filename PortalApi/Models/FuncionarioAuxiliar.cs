using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Models
{
    public class FuncionarioAuxiliar
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int Telefone { get; set; }
        public string DataNascimento { get; set; }
        public string DataAdmissao { get; set; }
    }
}