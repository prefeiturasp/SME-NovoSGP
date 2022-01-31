using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacaoCargosNiveisCommandHandler : IRequestHandler<TrataNotificacaoCargosNiveisCommand, bool>
    {
        private readonly IMediator mediator;

        public TrataNotificacaoCargosNiveisCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(TrataNotificacaoCargosNiveisCommand request, CancellationToken cancellationToken)
        {

            foreach (var notificacaoParaTratarAgrupada in request.Notificacoes.GroupBy(a => a.UECodigo))
            {

                var cargosIdsDaUe = notificacaoParaTratarAgrupada.Select(a => a.Cargo).Distinct().ToList();
                var listaCargoDosNiveis = EnumExtensao.ListarDto<Cargo>();
                cargosIdsDaUe.AddRange(listaCargoDosNiveis.Select(a => a.Id));
                cargosIdsDaUe = cargosIdsDaUe.Distinct().ToList();
                var dreCodigo = notificacaoParaTratarAgrupada.FirstOrDefault(a => !string.IsNullOrEmpty(a.DRECodigo)).DRECodigo;

                var funcionariosCargosDaUe = await mediator.Send(new ObterFuncionariosCargosPorUeCargosQuery(notificacaoParaTratarAgrupada.Key, cargosIdsDaUe, dreCodigo));

                var workflowsIdsParaTratar = notificacaoParaTratarAgrupada.Select(a => a.WorkflowId).Distinct().ToList();

                foreach (var workflowsIdParaTratar in workflowsIdsParaTratar)
                {

                    var notificacaoParaTratar = notificacaoParaTratarAgrupada.FirstOrDefault(a => a.WorkflowId == workflowsIdParaTratar);

                    var funcionariosNoCargo = funcionariosCargosDaUe.Where(a => a.CargoId == (Cargo)notificacaoParaTratar.Cargo).ToList();

                    if (!funcionariosNoCargo.Any())
                        await mediator.Send(new AlteraWorkflowAprovacaoNivelNotificacaoCargoCommand(notificacaoParaTratar.WorkflowId, notificacaoParaTratar.NotificacaoId, funcionariosCargosDaUe.ToList()));
                    else await mediator.Send(new AlteraWorkflowAprovacaoNotificacaoCargoCommand(notificacaoParaTratar.WorkflowId, notificacaoParaTratar.NotificacaoId, funcionariosNoCargo));

                }
            }

            return true;

        }

    }
}
