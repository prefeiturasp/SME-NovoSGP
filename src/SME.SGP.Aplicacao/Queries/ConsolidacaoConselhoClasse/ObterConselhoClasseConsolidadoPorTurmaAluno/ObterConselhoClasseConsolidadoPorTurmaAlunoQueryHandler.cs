using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoPorTurmaAlunoQueryHandler : IRequestHandler<ObterConselhoClasseConsolidadoPorTurmaAlunoQuery, ConselhoClasseConsolidadoTurmaAluno>
    {
        private readonly IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta;

        public ObterConselhoClasseConsolidadoPorTurmaAlunoQueryHandler(IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta)
        {
            this.repositorioConselhoClasseConsolidadoConsulta = repositorioConselhoClasseConsolidadoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoConsulta));
        }

        public Task<ConselhoClasseConsolidadoTurmaAluno> Handle(ObterConselhoClasseConsolidadoPorTurmaAlunoQuery request, CancellationToken cancellationToken)
        {
            return repositorioConselhoClasseConsolidadoConsulta.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(request.TurmaId, request.AlunoCodigo);
        }
    }
}
