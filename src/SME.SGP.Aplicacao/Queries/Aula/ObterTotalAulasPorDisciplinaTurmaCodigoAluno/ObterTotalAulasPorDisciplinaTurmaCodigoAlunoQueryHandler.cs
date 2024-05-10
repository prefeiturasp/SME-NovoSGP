using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQueryHandler : IRequestHandler<ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQuery, int>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioConsulta;

        public ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioConsulta)
        {
            this.repositorioConsulta = repositorioConsulta ?? throw new ArgumentNullException(nameof(repositorioConsulta));
        }

        public Task<int> Handle(ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQuery request, CancellationToken cancellationToken)
            => repositorioConsulta.ObterTotalAulasPorDisciplinaTurmaAluno(request.DataAula, request.CodigoAluno, request.DisciplinaId, request.TurmasId);
    }
}
