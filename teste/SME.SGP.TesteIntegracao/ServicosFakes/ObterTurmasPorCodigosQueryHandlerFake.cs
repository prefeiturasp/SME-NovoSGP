using MediatR;
using SME.SGP.Aplicacao;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterTurmasPorCodigosQueryHandlerFake : IRequestHandler<ObterTurmasPorCodigosQuery, IEnumerable<Dominio.Turma>>
    {
        private const string CODIGO_TURMA_1 = "1";
        private const string CODIGO_TURMA_2 = "2";

        public async Task<IEnumerable<Dominio.Turma>> Handle(ObterTurmasPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<Dominio.Turma> {
                new Dominio.Turma {
                    CodigoTurma = CODIGO_TURMA_1,
                    Id = 1,
                    TipoTurma = Dominio.Enumerados.TipoTurma.Regular
                },
                new Dominio.Turma {
                    CodigoTurma = CODIGO_TURMA_2,
                    Id = 2,
                    TipoTurma = Dominio.Enumerados.TipoTurma.Regular
                }
            });
        }
    }
}
