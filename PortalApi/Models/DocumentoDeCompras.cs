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
        public string DocumentoPrincipal { get; set; }
        public string DocumentoPrincipalAlternativo { get; set; }
        public string SegundoDocumento { get; set; }
        public string SegundoDocumentoAlternativo { get; set; }
        public string TerceiroDocumento { get; set; }
        public string QuartoDocumento { get; set; }
        public string QuintoDocumento { get; set; }
        public string SextoDocumento { get; set; }
        public virtual List<LinhasCompras> Linhas { get; set; }
    }
}