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
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public ObterAusenciasAlunosPorAlunosETurmaIdQueryHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> Handle(ObterAusenciasAlunosPorAlunosETurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(request.DataAula,request.TurmaId,request.Alunos);
        }
    }
}
