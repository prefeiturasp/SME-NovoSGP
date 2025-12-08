namespace SME.SGP.Infra
{
    public class AtendimentoNAAPAItineranciaDto
    {
       public long EncaminhamentoId { get; set; }
       public long? EncaminhamentoNAAPASecaoId { get; set; }
       public AtendimentoNAAPASecaoDto EncaminhamentoNAAPASecao { get; set; }
    }
}
