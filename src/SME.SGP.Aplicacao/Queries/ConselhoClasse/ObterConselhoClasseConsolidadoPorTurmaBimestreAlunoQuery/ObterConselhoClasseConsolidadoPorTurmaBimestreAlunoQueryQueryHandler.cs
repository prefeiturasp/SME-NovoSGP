using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQueryHandler : IRequestHandler<ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQuery, ConselhoClasseConsolidadoTurmaAluno>
    {
        private readonly IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta;

        public ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQueryHandler(IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta)
        {
            this.repositorioConselhoClasseConsolidadoConsulta = repositorioConselhoClasseConsolidadoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoConsulta));
        }
        public Task<ConselhoClasseConsolidadoTurmaAluno> Handle(ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQuery request, CancellationToken cancellationToken)
        {
            return repositorioConselhoClasseConsolidadoConsulta.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(request.TurmaId, request.AlunoCodigo);
        }
    }
}