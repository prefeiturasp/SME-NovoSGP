using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb
{
    public class ComponenteCurricularIdebResumidoDto
    {
        public ComponenteCurricularEnum? ComponenteCurricular { get; set; }
        public decimal? Percentual { get; set; }
    }
}