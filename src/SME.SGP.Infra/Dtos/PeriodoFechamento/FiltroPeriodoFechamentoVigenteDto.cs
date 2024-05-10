using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroPeriodoFechamentoVigenteDto
    {
        public int AnoLetivo { get; set; }
        public ModalidadeTipoCalendario? ModalidadeTipoCalendario { get; set; }
    }
}
