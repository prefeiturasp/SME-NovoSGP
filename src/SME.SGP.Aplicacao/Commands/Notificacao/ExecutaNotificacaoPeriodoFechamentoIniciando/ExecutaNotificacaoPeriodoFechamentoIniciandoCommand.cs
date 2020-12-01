using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoPeriodoFechamentoIniciandoCommand : IRequest<bool>
    {
        public ExecutaNotificacaoPeriodoFechamentoIniciandoCommand(PeriodoFechamentoBimestre periodoFechamentoBimestre, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            PeriodoFechamentoBimestre = periodoFechamentoBimestre;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public PeriodoFechamentoBimestre PeriodoFechamentoBimestre { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
    }
}
