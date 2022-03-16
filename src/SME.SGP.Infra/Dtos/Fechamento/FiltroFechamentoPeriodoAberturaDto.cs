using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroFechamentoPeriodoAberturaDto
    {
        public FiltroFechamentoPeriodoAberturaDto(PeriodoFechamentoBimestre periodoFechamentoBimestre, ModalidadeTipoCalendario modalidadeTipoCalendarioo)
        {
            PeriodoFechamentoBimestre = periodoFechamentoBimestre;
            ModalidadeTipoCalendario = modalidadeTipoCalendarioo; 
        }

        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
        public PeriodoFechamentoBimestre PeriodoFechamentoBimestre { get; set; }

    }
}
