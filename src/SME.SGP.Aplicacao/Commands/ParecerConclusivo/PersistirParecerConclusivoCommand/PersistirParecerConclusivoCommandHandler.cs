using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
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
            var conselhoClasse = await mediator.Send(new ObterConselhoClasseAlunoPorIdQuery(request.ConselhoClasseAlunoId));

            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasse);            

            var aluno = (await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(new long[] { long.Parse(conselhoClasse.AlunoCodigo) }, request.AnoLetivo))).FirstOrDefault();

            var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(conselhoClasse.AlunoCodigo, 
                                                                                                         request.TurmaId, 
                                                                                                         request.Bimestre, 
                                                                                                         aluno.Inativo);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemConsolidacaoConselhoClasseAluno, Guid.NewGuid(), null));

            return true;
        }
    }
}
