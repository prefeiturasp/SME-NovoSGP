using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorTurmaEMesQueryHandler : IRequestHandler<ObterFrequenciaAlunosPorTurmaEMesQuery, IEnumerable<RegistroFrequenciaAlunoPorTurmaEMesDto>>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta _repositorioRegistroFrequenciaAluno;

        public ObterFrequenciaAlunosPorTurmaEMesQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno)
        {
            _repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<RegistroFrequenciaAlunoPorTurmaEMesDto>> Handle(ObterFrequenciaAlunosPorTurmaEMesQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioRegistroFrequenciaAluno.ObterRegistroFrequenciaAlunosPorTurmaEMes(request.TurmaCodigo, request.Mes);
        }
    }
}
