using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.ViewModel
{
    public class ObrasViewModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public string TipoEntidade { get; set; }
        public string Entidade { get; set; }
    }
}