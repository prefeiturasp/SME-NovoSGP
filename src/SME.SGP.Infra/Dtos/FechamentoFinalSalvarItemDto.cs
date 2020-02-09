namespace SME.SGP.Infra
{
    public class FechamentoFinalSalvarItemDto
    {
        public string AlunoRf { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public long? ConceitoId { get; set; }
        public double? Nota { get; set; }

        public bool EhNota()
        {
            return ConceitoId.HasValue;
        }
    }
}