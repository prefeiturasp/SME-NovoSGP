using System;

namespace SME.SGP.Infra.Dtos
{
    public class WFAprovacaoParecerConclusivoDto
    {
        public long Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public long UsuarioSolicitanteId { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public long? ConselhoClasseParecerId { get; set; }
        public long WorkFlowAprovacaoId { get; set; }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public string NomeParecerAnterior { get; set; }
        public string NomeParecerNovo { get; set; }
        public string AlunoCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public bool ParecerAlteradoManual { get; set; }
    }
}
