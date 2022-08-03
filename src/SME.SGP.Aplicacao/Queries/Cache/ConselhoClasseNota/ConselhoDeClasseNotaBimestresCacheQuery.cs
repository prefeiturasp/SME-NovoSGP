using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConselhoDeClasseNotaBimestresCacheQuery : ConselhoDeClasseNotaBimestresCache, IRequest<ValorCache<ConselhoClasseAlunoNotasConceitosRetornoDto>>
    {
        public ConselhoDeClasseNotaBimestresCacheQuery(long conselhoClasseId, string codigoAluno, int? bimestre)
                : base(conselhoClasseId, codigoAluno, bimestre)
        {
        }
    }
}
