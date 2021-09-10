using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoBimestreTurmaQueryHandler : IRequestHandler<ObterUltimoBimestreTurmaQuery, (int bimestre, bool possuiConselho)>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public ObterUltimoBimestreTurmaQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                                    IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public async Task<(int bimestre, bool possuiConselho)> Handle(ObterUltimoBimestreTurmaQuery request, CancellationToken cancellationToken)
        {
            var periodoEscolar = await repositorioPeriodoEscolar
                .ObterUltimoBimestreAsync(request.Turma.AnoLetivo, request.Turma.ObterModalidadeTipoCalendario(), DateTime.Today.Semestre());

            if (periodoEscolar == null)
                throw new NegocioException($"Não foi encontrado o ultimo periodo escolar para a turma {request.Turma.Nome}");

            var conselhoClasseUltimoBimestre = await repositorioConselhoClasse
                .ObterPorTurmaEPeriodoAsync(request.Turma.Id, periodoEscolar.Id);

            return (periodoEscolar.Bimestre, conselhoClasseUltimoBimestre != null);
        }
    }
}
