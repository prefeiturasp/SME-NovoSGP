using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoPorTurmaBimestreQueryHandler : IRequestHandler<ObterConselhoClasseConsolidadoPorTurmaBimestreQuery, IEnumerable<ConselhoClasseConsolidadoTurmaAluno>>
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;

        public ObterConselhoClasseConsolidadoPorTurmaBimestreQueryHandler(IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado)
        {
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
        }

        public async Task<IEnumerable<ConselhoClasseConsolidadoTurmaAluno>> Handle(ObterConselhoClasseConsolidadoPorTurmaBimestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseConsolidado.ObterConselhosClasseConsolidadoPorTurmaBimestreAsync(request.TurmaId, request.Bimestre, request.SituacaoConselhoClasse);
        }
    }
}
