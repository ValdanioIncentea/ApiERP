using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class MovimentosViewModel
    {
        public Guid Id { get; set; }
        public string Artigo { get; set; }
        public string Armazem { get; set; }
        public string Localizacao { get; set; }
        public string Lote { get; set; }
        public int StockLot_Actual { get; set; }
        public int StockLot_Anterior { get; set; }
        public int StockArm_Actual { get; set; }
        public int StockArm_Anterior { get; set; }
        public int StockLoc_Actual { get; set; }
        public int StockLoc_Anterior { get; set; }
        public string TipoMovimento { get; set; }
        public int Quantidade { get; set; }
        public int Stock_Actual { get; set; }
        public int Stock_Anterior { get; set; }
        public DateTime Data { get; set; }
    }
}