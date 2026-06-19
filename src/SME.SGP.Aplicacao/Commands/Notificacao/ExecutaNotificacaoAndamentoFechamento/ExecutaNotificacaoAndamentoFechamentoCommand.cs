using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoAndamentoFechamentoCommand : IRequest<bool>
    {
        public ExecutaNotificacaoAndamentoFechamentoCommand(PeriodoFechamentoBimestre periodoEncerrandoBimestre, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            PeriodoEncerrandoBimestre = periodoEncerrandoBimestre;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public PeriodoFechamentoBimestre PeriodoEncerrandoBimestre { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
    }
}
