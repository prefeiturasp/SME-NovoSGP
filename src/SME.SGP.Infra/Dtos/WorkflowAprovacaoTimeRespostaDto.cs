namespace SME.SGP.Infra
{
    public class WorkflowAprovacaoTimeRespostaDto
    {
        public string AlteracaoData { get; set; }
        public string AlteracaoUsuario { get; set; }
        public string AlteracaoUsuarioRf { get; set; }
        public long Nivel { get; set; }
        public string NivelDescricao { get; set; }
        public long NivelId { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
    }
}