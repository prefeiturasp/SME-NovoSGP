using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComponenteCurricular
    {
        public long? AreaConhecimentoId { get; set; }
        public long ComponenteCurricularPaiId { get; set; }
        public string Descricao { get; set; }
        public bool EhBaseNacional { get; set; }
        public bool EhCompatilhado { get; set; }
        public bool EhRegenciaClasse { get; set; }
        public bool EhTerritorio { get; set; }
        public long? GrupoMatrizId { get; set; }
        public bool PermiteLancamentoNota { get; set; }
        public bool PermiteRegistroFrequencia { get; set; }
    }
}
