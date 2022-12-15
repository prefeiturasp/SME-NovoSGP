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
    public class AtualizaSituacaoConselhoClasseCommandHandler : IRequestHandler<AtualizaSituacaoConselhoClasseCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public AtualizaSituacaoConselhoClasseCommandHandler(IMediator mediator, IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public async Task<bool> Handle(AtualizaSituacaoConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CodigoTurma))
                request.CodigoTurma = await mediator.Send(new ObterTurmaPorConselhoClasseIdQuery(request.ConselhoClasseId), cancellationToken);

            var alunosComNotaLancada = await mediator.Send(new ObterAlunosComNotaLancadaPorConselhoClasseIdQuery(request.ConselhoClasseId), cancellationToken);

            var alunosTurma = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(request.CodigoTurma), cancellationToken);

            var situacaoConselhoClasse = ExisteAlunoSemNota(alunosTurma, alunosComNotaLancada) ?
                SituacaoConselhoClasse.EmAndamento :
                SituacaoConselhoClasse.Concluido;

            return await repositorioConselhoClasse.AtualizarSituacao(request.ConselhoClasseId, situacaoConselhoClasse);
        }

        private bool ExisteAlunoSemNota(IEnumerable<AlunoPorTurmaResposta> alunosTurma, IEnumerable<string> alunosComNotaLancada)
            => alunosTurma.Any(a => !alunosComNotaLancada.Contains(a.CodigoAluno));
    }
}
