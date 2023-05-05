namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPAObservacoesDto
    {
        long IdObservacao { get; set; }
        string Observacao { get; set; }
        bool Proprietario { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
