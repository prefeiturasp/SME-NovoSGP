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
    public class ExcluirWfAprovacaoNotaConselhoPorNotaIdCommandHandler : AsyncRequestHandler<ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho;
        private readonly IMediator mediator;

        public ExcluirWfAprovacaoNotaConselhoPorNotaIdCommandHandler(IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho, IMediator mediator)
        {
            this.repositorioWFAprovacaoNotaConselho = repositorioWFAprovacaoNotaConselho ?? throw new System.ArgumentNullException(nameof(repositorioWFAprovacaoNotaConselho));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand request, CancellationToken cancellationToken)
        {
            var wfAprovacaoNotas = await repositorioWFAprovacaoNotaConselho.ObterWorkflowAprovacaoNota(request.ConselhoClasseNotaId);
            foreach (var wfAprovacaoNota in wfAprovacaoNotas)
            {
                await repositorioWFAprovacaoNotaConselho.RemoverAsync(wfAprovacaoNota);
                await PublicarFilaExclusaoWfAprovacao(wfAprovacaoNota.WfAprovacaoId, wfAprovacaoNota.Id);
            }
        }

        private async Task PublicarFilaExclusaoWfAprovacao(long? wfAprovacaoId, long wfNotaConselho)
        {
            if (wfAprovacaoId.HasValue)
            {
                var wfNotasPosConselho = await mediator.Send(new ObterIdsWorkflowPorWfAprovacaoIdQuery(wfAprovacaoId.Value, "wf_aprovacao_nota_conselho"));
                if (wfNotasPosConselho == null || !wfNotasPosConselho.Except(new long[] { wfNotaConselho } ).Any())
                {
                    var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
                    await PulicaFilaSgp(RotasRabbitSgp.WorkflowAprovacaoExcluir, wfAprovacaoId.Value, usuarioLogado);
                }
            }
        }

        private async Task PulicaFilaSgp(string fila, long id, Usuario usuario)
        {
            await mediator.Send(new PublicarFilaSgpCommand(fila, new FiltroIdDto(id), Guid.NewGuid(), usuario));
        }
    }
}
