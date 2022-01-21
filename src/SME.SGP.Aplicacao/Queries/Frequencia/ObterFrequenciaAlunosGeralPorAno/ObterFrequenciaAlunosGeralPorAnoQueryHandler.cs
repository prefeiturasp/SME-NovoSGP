using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosGeralPorAnoQueryHandler : IRequestHandler<ObterFrequenciaAlunosGeralPorAnoQuery, IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno;

        public ObterFrequenciaAlunosGeralPorAnoQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        } 

        public Task<IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>> Handle(ObterFrequenciaAlunosGeralPorAnoQuery request, CancellationToken cancellationToken)
        {
            return repositorioRegistroFrequenciaAluno.ObterFrequenciaAlunosGeralPorAnoQuery(request.Ano);
        }
    }
}
