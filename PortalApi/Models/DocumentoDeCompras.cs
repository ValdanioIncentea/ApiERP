using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Models
{
    public class DocumentoDeCompras
    {

        public int CodigoPortal { get; set; }
        public string Tipodoc { get; set; }
        public string TipoEntidade { get; set; }
        public string Moeda { get; set; }
        public string Entidade { get; set; }
        public string NumDocExterno { get; set; }
        public string NumeroDeProcesso { get; set; }
        public DateTime DataDoc { get; set; }
        public virtual List<LinhasCompras> Linhas { get; set; }
    }
}