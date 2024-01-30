namespace SME.SGP.Dominio
{
    public class SecaoEncaminhamentoNAAPAModalidade : EntidadeBase
    {
        public long SecaoEncaminhamentoNAAPAId { get; set; }
        public Modalidade Modalidade { get; set; }
        public bool Excluido { get; set; }
    }
}
