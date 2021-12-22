using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciasAlunosPorAlunosETurmaIdQueryHandler : IRequestHandler<ObterAusenciasAlunosPorAlunosETurmaIdQuery, IEnumerable<AusenciaPorDisciplinaAlunoDto>>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno;

        public ObterAusenciasAlunosPorAlunosETurmaIdQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> Handle(ObterAusenciasAlunosPorAlunosETurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(request.DataAula, request.Alunos, request.TurmasId);
        }
    }
}
