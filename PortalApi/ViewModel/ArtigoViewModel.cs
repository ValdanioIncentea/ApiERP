using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class ArtigoViewModel
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string IVA { get; set; }
        public string ArmazemSugestao { get; set; }
        public string MovStock { get; set; }
        public string Familia { get; set; }
        public string DataUltimaActualizacao { get; set; }
        public string UnidadeBase { get; set; }
        public string UnidadeCompra { get; set; }
        public string UnidadeEntrada { get; set; }
        public string UnidadeSaida { get; set; }
        public string UnidadeVenda { get; set; }

    }
}