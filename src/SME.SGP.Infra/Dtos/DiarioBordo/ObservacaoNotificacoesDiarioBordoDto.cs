namespace SME.SGP.Infra
{
    public class ObservacaoNotificacoesDiarioBordoDto
    {
        public string Observacao { get; set; }

        public int QtdUsuariosNotificacao { get; set; }

        public AuditoriaDto Auditoria { get; set; }
    }
}
