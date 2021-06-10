using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery : IRequest<ConselhoClasseAluno>
    {
        public ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(long conselhoClasseId, string alunoCodigo)
        {
            ConselhoClasseId = conselhoClasseId;
            AlunoCodigo = alunoCodigo;
        }

        public long ConselhoClasseId { get; set; }
        public string AlunoCodigo { get; set; }

    }
}
