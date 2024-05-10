using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PersistirParecerConclusivoCommandHandler : IRequestHandler<PersistirParecerConclusivoCommand,bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public PersistirParecerConclusivoCommandHandler(IMediator mediator, IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<bool> Handle(PersistirParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorIdAsync(request.ConselhoClasseAlunoId);
            conselhoClasseAluno.ConselhoClasseParecerId = request.ParecerConclusivoId;
            conselhoClasseAluno.ParecerAlteradoManual = request.ParecerAlteradoManual;

            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            var alunoDaTurma = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(request.TurmaCodigo, conselhoClasseAluno.AlunoCodigo));

            var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(conselhoClasseAluno.AlunoCodigo, 
                                                                                                         request.TurmaId, 
                                                                                                         request.Bimestre,
                                                                                                         alunoDaTurma.Inativo, null, true);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemConsolidacaoConselhoClasseAluno, Guid.NewGuid(), null));

            return true;
        }
    }
}
