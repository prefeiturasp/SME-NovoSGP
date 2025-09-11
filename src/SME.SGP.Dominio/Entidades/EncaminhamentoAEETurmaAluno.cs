namespace SME.SGP.Dominio
{
    public class EncaminhamentoAEETurmaAluno : EntidadeBase
    {
        public long EncaminhamentoAEEId { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
