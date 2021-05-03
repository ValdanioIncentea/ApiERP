using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Models
{
    public class EntidadesViewModel
    {
        public string Codigo { get; set; }
        public string NumContribuinte { get; set; }
        public string Pais { get; set; }
        public string AbrevicaoPais { get; set; }
        public string Nome { get; set; }
        public string Morada { get; set; }
        public string Localidade { get; set; }
        public string Abreviacao { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string Moeda { get; set; }
    }
}