using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Models
{
    public class FornecedorViewModel
    {
        public string Codigo { get; set; }
        public string NumContribuinte { get; set; }
        public string Pais { get; set; }
        public string Nome { get; set; }
        public string Morada { get; set; }
        public string Localidade { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string NomeFiscal { get; set; }
        public string CodigoPostal { get; set; }
        public string LocalidadeCodigoPostal { get; set; }
        public string Telefone { get; set; }
        public string Fax { get; set; }
        public string SegmentoTerceiro { get; set; }
        public string Moeda { get; set; }
        public string condPag { get; set; }
        public string ModoPag { get; set; }
        public bool PessoaSingular { get; set; }
        public bool HasContabilidade { get; set; }
        public bool Retencao { get; set; }

    }
}