using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class WfAprovacaoNotaFechamentoTurmaDto
    {
        public WfAprovacaoNotaFechamento WfAprovacao { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public long FechamentoTurmaDisciplinaId { get; set; }
        public string ComponenteCurricularDescricao { get; set; }
        public FechamentoNota FechamentoNota { get; set; }
        public string CodigoAluno { get; set; }
        public int? Bimestre { get; set; }
        public int AnoLetivo { get; set; }
        public bool LancaNota { get; set; }
        public string UsuarioSolicitanteRf { get; set; }
    }
}
