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
    
    public partial class Familias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Familias()
        {
            this.Artigo = new HashSet<Artigo>();
        }
    
        public string Familia { get; set; }
        public string Descricao { get; set; }
        public Nullable<System.DateTime> DataUltimaActualizacao { get; set; }
        public string Etiqueta { get; set; }
        public string TipoDim1 { get; set; }
        public string TipoDim2 { get; set; }
        public string TipoDim3 { get; set; }
        public string Dim1 { get; set; }
        public string Dim2 { get; set; }
        public string Dim3 { get; set; }
        public byte[] VersaoUltAct { get; set; }
        public Nullable<bool> PermiteDevolucao { get; set; }
        public Nullable<int> NaturezaAnalitica { get; set; }
        public bool UtilizadoCCOP { get; set; }
        public Nullable<int> IdPastaCCOP { get; set; }
        public string GrupoCenariosCompras { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Artigo> Artigo { get; set; }
    }
}