using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{

    public class SalvarConselhoClasseAlunoNotaCommand : IRequest<ConselhoClasseNotaRetornoDto>
    {
        public ConselhoClasseNotaDto ConselhoClasseNotaDto { get; set; }
        public string AlunoCodigo { get; set; }
        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public string CodigoTurma { get; set; }
        public int Bimestre { get; set; }
    }
}