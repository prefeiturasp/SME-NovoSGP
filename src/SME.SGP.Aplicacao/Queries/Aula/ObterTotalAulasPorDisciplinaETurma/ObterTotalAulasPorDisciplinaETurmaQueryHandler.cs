using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorDisciplinaETurmaQueryHandler : IRequestHandler<ObterTotalAulasPorDisciplinaETurmaQuery, int>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioConsulta;

        public ObterTotalAulasPorDisciplinaETurmaQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioConsulta)
        {
            this.repositorioConsulta = repositorioConsulta ?? throw new ArgumentNullException(nameof(repositorioConsulta));
        }

        public Task<int> Handle(ObterTotalAulasPorDisciplinaETurmaQuery request, CancellationToken cancellationToken)
            => repositorioConsulta.ObterTotalAulasPorDisciplinaETurma(request.DataAula, request.DisciplinaId, request.TurmasId);
    }
}
