using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWFAprovacaoParecerPorAlunoCommandHandler : AsyncRequestHandler<ExcluirWFAprovacaoParecerPorAlunoCommand>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo;
        private readonly IMediator mediator;

        public ExcluirWFAprovacaoParecerPorAlunoCommandHandler(IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo, IMediator mediator)
        {
            this.repositorioWFAprovacaoParecerConclusivo = repositorioWFAprovacaoParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoParecerConclusivo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ExcluirWFAprovacaoParecerPorAlunoCommand request, CancellationToken cancellationToken)
        {
            var wfAprovacaoParecer = await repositorioWFAprovacaoParecerConclusivo.ObterPorConselhoClasseAlunoId(request.ConselhoClasseAlunoId);
            if (wfAprovacaoParecer != null)
            {
                await repositorioWFAprovacaoParecerConclusivo.Excluir(wfAprovacaoParecer.Id);
                await mediator.Send(new ExcluirWorkflowCommand(wfAprovacaoParecer.WfAprovacaoId));
            }
        }
    }
}
