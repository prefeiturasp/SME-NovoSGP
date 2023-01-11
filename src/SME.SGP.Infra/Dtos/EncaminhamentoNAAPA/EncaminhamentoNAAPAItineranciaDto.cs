namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPAItineranciaDto
    {
       public long EncaminhamentoId { get; set; }
       public long? EncaminhamentoNAAPASecaoId { get; set; }
       public EncaminhamentoNAAPASecaoDto EncaminhamentoNAAPASecao { get; set; }
    }
}
