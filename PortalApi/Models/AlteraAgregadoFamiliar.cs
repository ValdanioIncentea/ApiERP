using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Models
{
    public class AlteraAgregadoFamiliar
    {
        public string Funcionario { get; set; }
        public string NomeAgregado { get; set; }
        public DateTime DataNasc { get; set; }
        public int Afinidade { get; set; }
        public int Estudante { get; set; }
        public string NumBI { get; set; }
        public DateTime DataBI { get; set; }
        public string NIF { get; set; }
    }
}