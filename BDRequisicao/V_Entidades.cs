//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BDRequisicao
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_Entidades
    {
        public string TipoEntidade { get; set; }
        public string Entidade { get; set; }
        public string Nome { get; set; }
        public string NomeFiscal { get; set; }
        public string Morada { get; set; }
        public string Morada2 { get; set; }
        public string Localidade { get; set; }
        public string Cp { get; set; }
        public string CpLoc { get; set; }
        public string Distrito { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string NumContrib { get; set; }
        public string TextoExcepcaoRetencao { get; set; }
        public Nullable<bool> EfectuaRetencao { get; set; }
        public string Pais { get; set; }
        public string TipoMercado { get; set; }
        public Nullable<bool> GestaoDiasPag { get; set; }
        public Nullable<byte> DiaPagamento1 { get; set; }
        public Nullable<byte> DiaPagamento2 { get; set; }
        public Nullable<byte> DiaPagamento3 { get; set; }
        public Nullable<byte> NumDiasRetrocesso { get; set; }
        public string ContribuinteNaoResidente { get; set; }
        public string EnderecoWeb { get; set; }
        public Nullable<System.DateTime> DataCriacao { get; set; }
        public int ActividadeEmpresarial { get; set; }
        public int TrataIvaCaixa { get; set; }
        public Nullable<bool> ExcluirRecap { get; set; }
        public string NaturezaEntidade { get; set; }
        public string TipoTerceiro { get; set; }
    }
}