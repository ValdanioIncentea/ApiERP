using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class DocumentoViewModel
    {
        public string Documento { get; set; }
        public string Descricao { get; set; }
        public string Modulo { get; set; }
        public bool HasPrecoDeCustoMedio { get; set; }
    }
}