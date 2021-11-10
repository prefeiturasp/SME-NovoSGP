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
    public class ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandler : IRequestHandler<ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery, IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> Handle(ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(request.DataAula,request.TurmaId,request.Alunos);
        }
    }
}
