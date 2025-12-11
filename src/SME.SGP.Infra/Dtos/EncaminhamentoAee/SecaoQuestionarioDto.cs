using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class SecaoQuestionarioDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool Concluido { get; set; }
        public long QuestionarioId { get; set; }
        public int Etapa { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public string? NomeComponente { get; set; }
        public int Ordem { get; set; }
        public TipoQuestionario TipoQuestionario { get; set; }
        public int[] ModalidadesCodigo { get; set; }
        public long? EncaminhamentoEscolarId { get; set; }
        public IEnumerable<QuestaoDto>? EncaminhamentoEscolar { get; set; } = null;
    }
}
