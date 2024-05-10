using MediatR;
using SME.SGP.Aplicacao;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterFrequenciasPorAlunosTurmaQueryHandlerFake : IRequestHandler<ObterFrequenciasPorAlunosTurmaQuery, IEnumerable<Dominio.FrequenciaAluno>>
    {
        public async Task<IEnumerable<Dominio.FrequenciaAluno>> Handle(ObterFrequenciasPorAlunosTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<Dominio.FrequenciaAluno>()
            {
                new Dominio.FrequenciaAluno()
                {
                    CodigoAluno = "1234",
                    DisciplinaId = "1"
                }
            });
        }
    }
}
