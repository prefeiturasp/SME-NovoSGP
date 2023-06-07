using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoPorTurmaBimestreQueryHandler : IRequestHandler<ObterConselhoClasseConsolidadoPorTurmaBimestreQuery, IEnumerable<ConselhoClasseConsolidadoTurmaAluno>>
    {
        private readonly IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta;

        public ObterConselhoClasseConsolidadoPorTurmaBimestreQueryHandler(IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidado)
        {
            this.repositorioConselhoClasseConsolidadoConsulta = repositorioConselhoClasseConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
        }

        public async Task<IEnumerable<ConselhoClasseConsolidadoTurmaAluno>> Handle(ObterConselhoClasseConsolidadoPorTurmaBimestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseConsolidadoConsulta.ObterConselhosClasseConsolidadoPorTurmaBimestreAsync(request.TurmaId, request.SituacaoConselhoClasse, request.Bimestre);
        }
    }
}
