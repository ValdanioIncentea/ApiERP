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
    
    public partial class Paises
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Paises()
        {
            this.Funcionarios = new HashSet<Funcionarios>();
            this.Nacionalidades = new HashSet<Nacionalidades>();
        }
    
        public string Pais { get; set; }
        public string Descricao { get; set; }
        public string IntrastatPais { get; set; }
        public string Idioma { get; set; }
        public Nullable<System.DateTime> DataUltimaActualizacao { get; set; }
        public byte[] VersaoUltAct { get; set; }
        public string SiglaIVA { get; set; }
        public string SiglaDeclIntracom { get; set; }
        public string SiglaTributacao { get; set; }
        public string Codigo { get; set; }
        public string Zona { get; set; }
        public string ISOA2 { get; set; }
        public string ISOA3 { get; set; }
        public string ISON { get; set; }
        public string Comunidade { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Funcionarios> Funcionarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Nacionalidades> Nacionalidades { get; set; }
    }
}
