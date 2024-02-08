using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaMensalPorTurmaMesAlunoQueryHandler : IRequestHandler<ObterFrequenciaMensalPorTurmaMesAlunoQuery, RegistroFrequenciaAlunoPorTurmaEMesDto>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta _repositorioRegistroFrequenciaAluno;

        public ObterFrequenciaMensalPorTurmaMesAlunoQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno)
        {
            _repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<RegistroFrequenciaAlunoPorTurmaEMesDto> Handle(ObterFrequenciaMensalPorTurmaMesAlunoQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioRegistroFrequenciaAluno.ObterRegistroFrequenciaAlunoPorTurmaMesDataRef(request.TurmaCodigo, request.AlunoCodigo, request.DataRef, request.Mes);
        }
    }
}
