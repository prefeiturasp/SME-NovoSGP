using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class WfAprovacaoNotaFechamentoTurmaDto
    {
        public WfAprovacaoNotaFechamento WfAprovacao { get; set; }
        public long TurmaId { get; set; }
        public long FechamentoTurmaDisciplinaId { get; set; }
        public string ComponenteCurricularDescricao { get; set; }
        public bool ComponenteCurricularEhRegencia { get; set; }
        public FechamentoNota FechamentoNota { get; set; }
        public string CodigoAluno { get; set; }
        public double? NotaAnterior { get; set; }
        public long? ConceitoAnteriorId { get; set; }
        public int? Bimestre { get; set; }
        public int AnoLetivo { get; set; }
        public bool LancaNota { get; set; }
    }
}
