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
    
    public partial class TiposRendimento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TiposRendimento()
        {
            this.Funcionarios = new HashSet<Funcionarios>();
        }
    
        public string TipoRendimento { get; set; }
        public string Descricao { get; set; }
        public string TipoEntidadeRec { get; set; }
        public string EntidadeRec { get; set; }
        public double Percentagem { get; set; }
        public string PendenteAGerarRec { get; set; }
        public string PendenteAGerarPag { get; set; }
        public string TipoEntidadePag { get; set; }
        public string EntidadePag { get; set; }
        public string Categoria { get; set; }
        public byte[] VersaoUltAct { get; set; }
        public Nullable<double> PercentagemIVA { get; set; }
        public string CodigoRendimento { get; set; }
        public double LimiteIsencao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Funcionarios> Funcionarios { get; set; }
    }
}
