namespace SME.SGP.Infra
{
    public class AtendimentoNAAPAObservacaoSalvarDto
    {
        public long Id { get; set; }
        public string Observacao { get; set; }
        public long EncaminhamentoNAAPAId { get; set; }
    }
}