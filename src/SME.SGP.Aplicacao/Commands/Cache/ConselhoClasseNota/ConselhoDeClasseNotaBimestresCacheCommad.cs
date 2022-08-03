using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConselhoDeClasseNotaBimestresCacheCommad : ConselhoDeClasseNotaBimestresCache, IRequest<bool>
    {
        public ConselhoClasseNotaDto ConselhoClasseNotaDto { get; set; }

        public ConselhoDeClasseNotaBimestresCacheCommad(long conselhoClasseId, string codigoAluno, int? bimestre, ConselhoClasseNotaDto dto) 
            : base(conselhoClasseId, codigoAluno, bimestre)
        {
            ConselhoClasseNotaDto = dto;
        }
    }
}
