using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWFAprovacaoNotaFechamentoPorNotaCommandHandler : AsyncRequestHandler<ExcluirWFAprovacaoNotaFechamentoPorNotaCommand>
    {
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento;
        private readonly IMediator mediator;

        public ExcluirWFAprovacaoNotaFechamentoPorNotaCommandHandler(IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento, IMediator mediator)
        {
            this.repositorioWfAprovacaoNotaFechamento = repositorioWfAprovacaoNotaFechamento ?? throw new System.ArgumentNullException(nameof(repositorioWfAprovacaoNotaFechamento));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ExcluirWFAprovacaoNotaFechamentoPorNotaCommand request, CancellationToken cancellationToken)
        {
            var wfAprovacaoNotas = await repositorioWfAprovacaoNotaFechamento.ObterPorNotaId(request.FechamentoNotaId);
            foreach(var wfAprovacaoNota in wfAprovacaoNotas)
            {
                await repositorioWfAprovacaoNotaFechamento.Excluir(wfAprovacaoNota);
                await mediator.Send(new ExcluirWorkflowCommand(wfAprovacaoNota.WfAprovacaoId));
            }
        }
    }
}
