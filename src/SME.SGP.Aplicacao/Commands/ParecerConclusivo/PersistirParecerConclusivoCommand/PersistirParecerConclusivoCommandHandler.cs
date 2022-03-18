using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
            var conselhoClasse = request.ConselhoClasseAluno;

            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasse);

            var periodoEscolarId = conselhoClasse.ConselhoClasse.FechamentoTurma.PeriodoEscolarId;

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(periodo)

            int bimestre = conselhoClasse.ConselhoClasse.FechamentoTurma.PeriodoEscolar != null ? conselhoClasse.ConselhoClasse.FechamentoTurma.PeriodoEscolar.Bimestre : 0;

            var aluno = (await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(new long[] { long.Parse(conselhoClasse.AlunoCodigo) }, conselhoClasse.ConselhoClasse.FechamentoTurma.Turma.AnoLetivo))).FirstOrDefault();

            var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(conselhoClasse.AlunoCodigo, conselhoClasse.ConselhoClasse.FechamentoTurma.Turma.Id, bimestre, aluno.Inativo);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemConsolidacaoConselhoClasseAluno, Guid.NewGuid(), null));

            return true;
        }
    }
}
