namespace SME.SGP.Infra
{
    public class RecomendacaoConselhoClasseAlunoDTO
    {
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public string RecomendacoesAluno { get; set; }
        public string RecomendacoesFamilia { get; set; }
        public string AnotacoesPedagogicas { get; set; }
        public string MensagemAlerta { get; set; }
    }
}
