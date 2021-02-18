namespace SME.SGP.Infra
{
    public class ObservacaoNotificacoesDiarioBordoDto
    {
        public long Id { get; set; }
        public string Observacao { get; set; }

        public bool Proprietario { get; set; }

        public int QtdUsuariosNotificacao { get; set; }

        public AuditoriaDto Auditoria { get; set; }
    }
}
