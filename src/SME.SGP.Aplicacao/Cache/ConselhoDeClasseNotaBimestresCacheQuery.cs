using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Cache
{
    public class ConselhoDeClasseNotaBimestresCacheQuery : ConselhoDeClasseNotaBimestresCache, IRequest<ValorCache<ConselhoClasseAlunoNotasConceitosRetornoDto>>
    {
        public ConselhoDeClasseNotaBimestresCacheQuery(long conselhoClasseId, string codigoAluno, int? bimestre)
                : base(conselhoClasseId, codigoAluno, bimestre)
        {
        }
    }
}
