using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class LoteViewModel
    {
        public string Lote { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataFabrico { get; set; }
        public DateTime? Validade { get; set; }
        public bool? Activo { get; set; }
        public string Artigo { get; set; }
    }
}