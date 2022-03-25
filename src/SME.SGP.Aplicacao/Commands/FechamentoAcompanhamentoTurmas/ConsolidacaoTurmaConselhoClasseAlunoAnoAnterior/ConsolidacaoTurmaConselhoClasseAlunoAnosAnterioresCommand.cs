using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand : IRequest<bool>
    {
        public long ConsolidacaoId { get; set; }
        public ConsolidacaoConselhoClasseAlunoMigracaoDto AlunoNota { get; set; }        
    }
}
