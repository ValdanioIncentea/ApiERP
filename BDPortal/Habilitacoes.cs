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
    
    public partial class Habilitacoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Habilitacoes()
        {
            this.Funcionarios = new HashSet<Funcionarios>();
        }
    
        public string Habilitacao { get; set; }
        public string Descricao { get; set; }
        public Nullable<short> PosBS { get; set; }
        public string CodigoQP { get; set; }
        public string NivelUE { get; set; }
        public byte TipoBalancoSocial { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Funcionarios> Funcionarios { get; set; }
    }
}
