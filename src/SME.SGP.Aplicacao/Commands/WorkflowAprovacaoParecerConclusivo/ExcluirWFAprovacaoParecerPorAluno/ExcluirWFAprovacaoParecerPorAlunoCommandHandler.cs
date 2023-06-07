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
            var wfAprovacaoPareceres = await repositorioWFAprovacaoParecerConclusivo.ObterPorConselhoClasseAlunoId(request.ConselhoClasseAlunoId);
            
            foreach (var wfAprovacaoParecer in wfAprovacaoPareceres)
            {
                await repositorioWFAprovacaoParecerConclusivo.Excluir(wfAprovacaoParecer.Id);
                await PublicarFilaExclusaoWfAprovacao(wfAprovacaoParecer.WfAprovacaoId, wfAprovacaoParecer.Id);
            }
        }

        private async Task PublicarFilaExclusaoWfAprovacao(long? wfAprovacaoId, long wfNotaConselho)
        {
            if (wfAprovacaoId.HasValue)
                await mediator.Send(new PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommand(wfAprovacaoId.Value, "wf_aprovacao_nota_conselho", wfNotaConselho));
        }
    }
}
