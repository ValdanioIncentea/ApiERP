using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class ValidarDocumentoCompraViewModel
    {
        public string Filial { get; set; }
        public string TipoDoc { get; set; }
        //public string strSerie { get; set; }
        //public int NumDoc { get; set; }
        public string TipoEntidade { get; set; }
        public string Entidade { get; set; }
        public string NumDocExterno { get; set; }
        public int IDPortal { get; set; }
    }
}