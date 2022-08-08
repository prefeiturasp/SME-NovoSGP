using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoDeClasseNotaBimestresCacheQuery : IRequest<ValorCache<ConselhoClasseAlunoNotasConceitosRetornoDto>>
    {
        public ObterConselhoDeClasseNotaBimestresCacheQuery(long conselhoClasseId, string codigoAluno, int? bimestre)
        {
            ConselhoClasseId = conselhoClasseId;
            CodigoAluno = codigoAluno;
            Bimestre = bimestre;
        }

        public long ConselhoClasseId { get; }

        public string CodigoAluno { get; }

        public int? Bimestre { get; }     
    }
}
