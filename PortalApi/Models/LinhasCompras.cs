using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Models
{
    public class LinhasCompras
    {
        public string Artigo { get; set; }
        public string Armazem { get; set; }
        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double TaxaIva { get; set; }
        public Nullable<double> Desconto { get; set; }
        public Nullable<Guid> IdObra { get; set; }
        public string CentroDeCusto { get; set; }
        public string Localizacao { get; set; }
        public string Lote { get; set; }
    }
}