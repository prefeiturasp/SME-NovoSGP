using System;

namespace SME.SGP.Infra
{
    public class FiltroObterAulasPorTurmaComponenteDataDto
    {
        public FiltroObterAulasPorTurmaComponenteDataDto(string turmaCodigo, long componenteCurricular, DateTime dataAula)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricular = componenteCurricular;
            DataAula = dataAula;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricular { get; set; }
        public DateTime DataAula { get; set; }
    }
}
