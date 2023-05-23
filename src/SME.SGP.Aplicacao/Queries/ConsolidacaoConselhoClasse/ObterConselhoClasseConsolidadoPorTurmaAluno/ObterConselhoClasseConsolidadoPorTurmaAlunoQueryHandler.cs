using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoPorTurmaAlunoQueryHandler : IRequestHandler<ObterConselhoClasseConsolidadoPorTurmaAlunoQuery, ConselhoClasseConsolidadoTurmaAluno>
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;

        public ObterConselhoClasseConsolidadoPorTurmaAlunoQueryHandler(IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado)
        {
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
        }

        public Task<ConselhoClasseConsolidadoTurmaAluno> Handle(ObterConselhoClasseConsolidadoPorTurmaAlunoQuery request, CancellationToken cancellationToken)
        {
            return repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(request.TurmaId, request.AlunoCodigo);
        }
    }
}
