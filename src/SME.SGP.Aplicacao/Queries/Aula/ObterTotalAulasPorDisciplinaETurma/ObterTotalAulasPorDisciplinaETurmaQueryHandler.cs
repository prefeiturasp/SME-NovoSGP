using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorDisciplinaETurmaQueryHandler : IRequestHandler<ObterTotalAulasPorDisciplinaETurmaQuery, int>
    {
        private readonly IRepositorioRegistroAusenciaAlunoConsulta repositorioConsulta;

        public ObterTotalAulasPorDisciplinaETurmaQueryHandler(IRepositorioRegistroAusenciaAlunoConsulta repositorioConsulta)
        {
            this.repositorioConsulta = repositorioConsulta ?? throw new ArgumentNullException(nameof(repositorioConsulta));
        }

        public async Task<int> Handle(ObterTotalAulasPorDisciplinaETurmaQuery request, CancellationToken cancellationToken)
            => await repositorioConsulta.ObterTotalAulasPorDisciplinaETurma(request.DataAula, request.DisciplinaId, request.TurmaId);
    }
}
