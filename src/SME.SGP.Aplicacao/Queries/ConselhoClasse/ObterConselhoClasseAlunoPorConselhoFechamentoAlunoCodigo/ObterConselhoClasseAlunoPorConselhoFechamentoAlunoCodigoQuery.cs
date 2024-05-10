using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQuery : IRequest<ConselhoClasseAluno>
    {
        public ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQuery(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            ConselhoClasseId = conselhoClasseId;
            FechamentoTurmaId = fechamentoTurmaId;
            AlunoCodigo = alunoCodigo;
        }

        public long FechamentoTurmaId { get; set; }
        public long ConselhoClasseId { get; set; }
        public string AlunoCodigo { get; set; }

    }
}
