using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterTurmasPorCodigosQueryHandlerFake : IRequestHandler<ObterTurmasPorCodigosQuery, IEnumerable<Turma>>
    {
        private const string CODIGO_TURMA_1 = "1";
        private const string CODIGO_TURMA_2 = "2";

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return new List<Turma> { 
                new Turma {
                    CodigoTurma = CODIGO_TURMA_1,
                    Id = 1,
                    TipoTurma = Dominio.Enumerados.TipoTurma.Regular
                },
                new Turma {
                    CodigoTurma = CODIGO_TURMA_2,
                    Id = 2,
                    TipoTurma = Dominio.Enumerados.TipoTurma.Regular
                }
            };
        }
    }
}
