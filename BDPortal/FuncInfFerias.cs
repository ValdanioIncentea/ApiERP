//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BDPortal
{
    using System;
    using System.Collections.Generic;
    
    public partial class FuncInfFerias
    {
        public short Ano { get; set; }
        public string Funcionario { get; set; }
        public Nullable<float> DiasDireito { get; set; }
        public Nullable<float> DiasAdicionais { get; set; }
        public Nullable<float> DiasAnoAnterior { get; set; }
        public Nullable<float> TotalDias { get; set; }
        public Nullable<float> DiasPorGozar { get; set; }
        public Nullable<float> DiasJaGozados { get; set; }
        public Nullable<float> DiasPorMarcar { get; set; }
        public Nullable<float> DiasFeriasJaPagas { get; set; }
        public string PeriodosFerias { get; set; }
        public bool FuncSemFerias { get; set; }
        public Nullable<float> DiasAdicionaisAntig { get; set; }
        public Nullable<float> DiasAdicionaisAssid { get; set; }
        public Nullable<float> DiasAdicionaisIdade { get; set; }
        public Nullable<float> DiasAntecipados { get; set; }
        public Nullable<System.Guid> IDGDOC { get; set; }
    
        public virtual Funcionarios Funcionarios { get; set; }
    }
}