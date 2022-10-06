using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQueryHandler : IRequestHandler<ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery, IEnumerable<FrequenciaAlunoTurmaDto>>
    {
        public readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno;

        public ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<FrequenciaAlunoTurmaDto>> Handle(ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery request, CancellationToken cancellationToken)
         => await repositorioRegistroFrequenciaAluno.ObterRegistroFrequenciaAlunosNaTurma(request.TurmaCodigo, request.AlunoCodigo);
    }
}
