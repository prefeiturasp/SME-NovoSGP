namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPAObservacoesDto
    {
        long IdObservacao { get; set; }
        string Observacao { get; set; }
        bool PodeEditarExcluir { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
