using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AprovarWorkflowAlteracaoParecerConclusivoCommandHandler : AsyncRequestHandler<AprovarWorkflowAlteracaoParecerConclusivoCommand>
    {
        private readonly IMediator mediator;
        public readonly IUnitOfWork unitOfWork;

        public AprovarWorkflowAlteracaoParecerConclusivoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task Handle(AprovarWorkflowAlteracaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var pareceresEmAprovacao = await mediator.Send(new ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQuery(request.WorkflowId));
            if (pareceresEmAprovacao.NaoEhNulo() && pareceresEmAprovacao.ToArray().Any())
            {
                unitOfWork.IniciarTransacao();
                try
                {
                    foreach (var parecerEmAprovacao in pareceresEmAprovacao)
                        await AtualizarParecerConclusivo(parecerEmAprovacao, request.TurmaCodigo);

                    await mediator.Send(new NotificarAprovacaoParecerConclusivoCommand(pareceresEmAprovacao,
                                                                               request.TurmaCodigo,
                                                                               request.CriadorRf,
                                                                               request.CriadorNome,
                                                                               true)); ;

                    unitOfWork.PersistirTransacao();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task AtualizarParecerConclusivo(WFAprovacaoParecerConclusivoDto parecerEmAprovacao, string turmaCodigo)
        {
            var persistirParecerConclusivoDto = new PersistirParecerConclusivoDto()
            {
                ConselhoClasseAlunoId = parecerEmAprovacao.ConselhoClasseAlunoId,
                ConselhoClasseAlunoCodigo = parecerEmAprovacao.AlunoCodigo,
                ParecerConclusivoId = parecerEmAprovacao.ConselhoClasseParecerId,
                TurmaId = parecerEmAprovacao.TurmaId,
                TurmaCodigo = turmaCodigo,
                Bimestre = parecerEmAprovacao.Bimestre,
                AnoLetivo = parecerEmAprovacao.AnoLetivo
            };
            await mediator.Send(new PersistirParecerConclusivoCommand(persistirParecerConclusivoDto));
            await mediator.Send(new ExcluirWfAprovacaoParecerConclusivoCommand(parecerEmAprovacao.Id));
        }

    }
}
