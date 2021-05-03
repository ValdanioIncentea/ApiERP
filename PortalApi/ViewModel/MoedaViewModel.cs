using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class MoedaViewModel
    {
        public string Moeda { get; set; }
        public string Descricao { get; set; }
        public double Compra { get; set; }
        public double Venda { get; set; }
        public string ISO4217 { get; set; }
    }
}