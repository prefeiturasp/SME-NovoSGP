namespace SME.SGP.Infra.Dtos
{
    public class NotificacoesParaTratamentoCargosNiveisDto
    {
        public int Cargo { get; set; }
        public long NotificacaoId { get; set; }        
        public string UECodigo { get; set; }
        public string DRECodigo { get; set; }
        public long WorkflowId { get; set; }

    }
}
