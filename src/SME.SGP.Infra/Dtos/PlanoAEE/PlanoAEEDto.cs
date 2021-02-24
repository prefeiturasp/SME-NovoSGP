using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAEEDto
    {
        public long Id { get; set; }
        public long QuestionarioId { get; set; }
        public PlanoAEEVersaoDto UltimaVersao { get; set; }
        public AlunoReduzidoDto Aluno { get; set; }
        public TurmaAnoDto Turma { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public string SituacaoDescricao { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public IEnumerable<QuestaoDto> Questoes { get; set; }
        public IEnumerable<PlanoAEEVersaoDto> Versoes { get; set; }
    }

}
