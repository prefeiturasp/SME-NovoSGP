using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandlerFake : IRequestHandler<ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery, IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>>
    {
        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> Handle(ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<RegistroFrequenciaPorDisciplinaAlunoDto>()
            {
                new RegistroFrequenciaPorDisciplinaAlunoDto() 
                {
                    Bimestre = 1,
                    AlunoCodigo = "1",
                    ComponenteCurricularId = "1"
                }
            });
        }
    }
}
