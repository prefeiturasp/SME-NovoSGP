using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoAndamentoFechamentoCommandHandler : IRequestHandler<ExecutaNotificacaoAndamentoFechamentoCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoAndamentoFechamentoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoAndamentoFechamentoCommand request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery(request.PeriodoEncerrandoBimestre.PeriodoFechamento.UeId,
                                                                                                       DateTime.Now.Year,
                                                                                                       request.PeriodoEncerrandoBimestre.PeriodoEscolarId,
                                                                                                       request.ModalidadeTipoCalendario.ObterModalidades(),
                                                                                                       DateTime.Now.Semestre()));
            var componentes = await mediator.Send(ObterComponentesCurricularesQuery.Instance);

            if (turmas.NaoEhNulo() && turmas.Any())
                await EnviarNotificacaoTurmas(turmas, componentes, request.PeriodoEncerrandoBimestre.PeriodoEscolar, request.PeriodoEncerrandoBimestre.PeriodoFechamento.Ue);

            return true;
        }

        private async Task EnviarNotificacaoTurmas(IEnumerable<Turma> turmas, IEnumerable<ComponenteCurricularDto> componentes, PeriodoEscolar periodoEscolar, Ue ue)
        {
            var ues = new List<Ue>();

            if (ue.NaoEhNulo())
                ues.Add(ue);
            else
            {
                foreach(var ueId in turmas.Select(t => t.UeId).Distinct().ToList())
                {
                    var ueLocalizada = await mediator.Send(new ObterUePorIdQuery(ueId));
                    ues.Add(ueLocalizada);
                }
            }

            foreach (var ueAtual in ues)
            {
                var dto = new NotificacaoAndamentoFechamentoPorUeDto()
                {
                    UeId = ueAtual.Id,
                    TurmasIds = turmas.Where(t => t.UeId == ueAtual.Id).Select(t => t.Id).ToArray(),
                    Componentes = componentes.ToArray(),
                    PeriodoEscolarId = periodoEscolar.Id
                };

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaNotificacaoAndamentoFechamentoPorUe, dto, Guid.NewGuid(), null));
            }
        }
    }
}
