using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoDeClasseNotaBimestresCacheCommand : IRequest<bool>
    {
        public SalvarConselhoDeClasseNotaBimestresCacheCommand(long conselhoClasseId, string codigoAluno, int? bimestre,
            ConselhoClasseNotaDto conselhoClasseNotaDto)
        {
            ConselhoClasseId = conselhoClasseId;
            CodigoAluno = codigoAluno;
            Bimestre = bimestre;
            ConselhoClasseNotaDto = conselhoClasseNotaDto;
        }

        public long ConselhoClasseId { get; }
        public string CodigoAluno { get; }
        public int? Bimestre { get; }    
        public ConselhoClasseNotaDto ConselhoClasseNotaDto { get; }
    }
}
