using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Models
{
    public class RecibosVencimento
    {
        public string Codigo { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Id { get; set; }
        public decimal TotalRemuneracao { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal TotalLiquido { get; set; }
        public string MesExtenso { get; set; }
    }
}