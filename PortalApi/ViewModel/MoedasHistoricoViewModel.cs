using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class MoedasHistoricoViewModel
    {
        public string Moeda { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataCambio { get; set; }
        public double Compra { get; set; }
        public double Venda { get; set; }
    }
}