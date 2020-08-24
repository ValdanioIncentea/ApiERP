using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Models
{
    public class FeriasAuxiliar
    {
      public int Ano { get; set; }
      public DateTime DataFeria { get; set; }
      public string Funcionario { get; set; }
      public int TipoMarcacao { get; set; }
    }
}