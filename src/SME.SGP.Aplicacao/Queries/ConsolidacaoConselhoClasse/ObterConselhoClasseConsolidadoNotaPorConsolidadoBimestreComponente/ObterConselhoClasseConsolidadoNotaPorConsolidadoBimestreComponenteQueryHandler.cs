using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQueryHandler : IRequestHandler<ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQuery, ConselhoClasseConsolidadoTurmaAlunoNota>
    {
        private readonly IRepositorioConselhoClasseConsolidadoNota repositorioConselhoClasseConsolidadoNota;

        public ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQueryHandler(IRepositorioConselhoClasseConsolidadoNota repositorioConselhoClasseConsolidadoNota)
        {
            this.repositorioConselhoClasseConsolidadoNota = repositorioConselhoClasseConsolidadoNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoNota));
        }

        public Task<ConselhoClasseConsolidadoTurmaAlunoNota> Handle(ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQuery request, CancellationToken cancellationToken)
        {
            return repositorioConselhoClasseConsolidadoNota.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoNotaAsync(request.ConsolidadoTurmaAlunoId, request.Bimestre, request.ComponenteCurricularId);
        }
    }
}