using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommandHandler : IRequestHandler<PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommand, bool>
    {
        private readonly IMediator mediator;

        public PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommandHandler(
                                             IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommand request, CancellationToken cancellationToken)
        {
            var wfNotasPosConselho = await mediator.Send(new ObterIdsWorkflowPorWfAprovacaoIdQuery(request.WfAprovacaoId, request.TabelaVinculada));
            if (wfNotasPosConselho == null || !wfNotasPosConselho.Except(new long[] { request.WfIgnoradoId }).Any())
            {
                var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
                await PulicaFilaSgp(RotasRabbitSgp.WorkflowAprovacaoExcluir, request.WfAprovacaoId, usuarioLogado);
                return true;
            }
            return false;
        }

        private async Task PulicaFilaSgp(string fila, long id, Usuario usuario)
        {
            await mediator.Send(new PublicarFilaSgpCommand(fila, new FiltroIdDto(id), Guid.NewGuid(), usuario));
        }
    }
}
