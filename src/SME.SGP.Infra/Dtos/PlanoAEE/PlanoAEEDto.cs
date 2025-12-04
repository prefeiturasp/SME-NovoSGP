using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;

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
        public ResponsavelDto Responsavel { get; set; }
        public bool PodeDevolverPlanoAEE { get; set; }
        public bool PermitirExcluir { get; set; }
        public bool RegistroCadastradoEmOutraUE { get; set; }
        public bool PermitirEncerramentoManual { get; set; }
    }

}
