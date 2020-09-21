using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class UnidadeConversaoViewModel
    {
        public string UnidadeOrigem { get; set; }
        public string UnidadeDestino { get; set; }
        public string Formula { get; set; }
        public bool UtilizaFormula { get; set; }
        public double FactorConversao { get; set; }
    }
}