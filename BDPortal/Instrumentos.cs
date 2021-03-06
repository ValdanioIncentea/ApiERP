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
    
    public partial class Instrumentos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instrumentos()
        {
            this.Funcionarios = new HashSet<Funcionarios>();
            this.Instrumentos1 = new HashSet<Instrumentos>();
        }
    
        public string Instrumento { get; set; }
        public string Descricao { get; set; }
        public Nullable<float> NumHoras { get; set; }
        public Nullable<System.DateTime> DataTabSal { get; set; }
        public string InstRegTrab { get; set; }
        public Nullable<System.DateTime> DataBoletim { get; set; }
        public string BoletimTrab { get; set; }
        public Nullable<short> DiasSubsAlimentacao { get; set; }
        public string InstrumentoAssociado { get; set; }
        public Nullable<short> MesesCalculoSalarioHora { get; set; }
        public string CodQuadroPessoal { get; set; }
        public Nullable<float> MaxDiasFerias { get; set; }
        public Nullable<float> DiasFeriasMes { get; set; }
        public Nullable<float> MaxDiasFeriasAnoAdmissao { get; set; }
        public Nullable<byte> MesesPGozoFerias { get; set; }
        public bool PagaPropSubsNatal { get; set; }
        public bool PagaPropSubsFerias { get; set; }
        public bool PagaPropMesFerias { get; set; }
        public bool PagaFeriasNGozadas { get; set; }
        public bool PagaFraccoes { get; set; }
        public Nullable<byte> MinDiasMes { get; set; }
        public Nullable<byte> MaxRenovacoes { get; set; }
        public Nullable<byte> MaxAnosContrato { get; set; }
        public string MotivSaidaFimCont { get; set; }
        public Nullable<byte> FormulaFeriasNGozadas { get; set; }
        public Nullable<byte> FormulaPropMesFerias { get; set; }
        public Nullable<byte> FormulaPropSubsFerias { get; set; }
        public Nullable<byte> FormulaPropSubsNatal { get; set; }
        public string RemAntiguidade { get; set; }
        public string RemComplemento { get; set; }
        public Nullable<bool> PagaPropExtraordinario { get; set; }
        public Nullable<byte> FormulaPropExtraordinario { get; set; }
        public bool AbateSubsidioAlimentacaoFaltasInjustificadas { get; set; }
        public string PercVencParaSubsFerias { get; set; }
        public string PercVencParaSubsNatal { get; set; }
        public Nullable<bool> CalculoSalarioHora { get; set; }
        public string FormulaSalarioHora { get; set; }
        public Nullable<float> MesesExtra { get; set; }
        public bool DistribuicaoDuodecimosAtefimContrato { get; set; }
        public bool FazAcertoUltMesSubsFerias { get; set; }
        public bool DistribProporcionalAdmisDemis { get; set; }
        public bool AltVencProcessamento { get; set; }
        public Nullable<byte> BaseRendimSubInsular { get; set; }
        public bool CalculaAbonoFamilia { get; set; }
        public Nullable<short> FormulaSubFer { get; set; }
        public Nullable<double> LimitesHExtraHorasAno { get; set; }
        public Nullable<double> LimitesHExtraHorasFimSemana { get; set; }
        public Nullable<double> LimitesHExtraHorasSemana { get; set; }
        public Nullable<double> LimitesHExtraPercentagem { get; set; }
        public short MaxDiasAntecipadosAno { get; set; }
        public Nullable<byte> MaxDiasSubFer { get; set; }
        public Nullable<byte> MesProcSubInsular { get; set; }
        public string RemunSubInsular { get; set; }
        public Nullable<bool> TemLimitesHorasExtras { get; set; }
        public byte TipoBalancoSocial { get; set; }
        public Nullable<byte> TipoCalculoSubInsular { get; set; }
        public Nullable<int> TipoProcLimitesHorasExtra { get; set; }
        public bool ConsideraAssiduidadeMesFerias { get; set; }
        public bool AnoCivilCompletoDiasFerias { get; set; }
        public bool CalculoSubsNatalDiasAno { get; set; }
        public string FormulaSalHoraExtra { get; set; }
        public bool CalculoSalHoraHextra { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Funcionarios> Funcionarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Instrumentos> Instrumentos1 { get; set; }
        public virtual Instrumentos Instrumentos2 { get; set; }
        public virtual MotivosSaida MotivosSaida { get; set; }
    }
}
