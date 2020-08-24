using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class ArtigoMoedaViewModel
    {
        public string Artigo { get; set; }
        public string Nome { get; set; }
        public Nullable<double> PVP1 { get; set; }
        public Nullable<double> PVP2 { get; set; }
        public Nullable<double> PVP3 { get; set; }
        public string Unidade { get; set; }
    }
}