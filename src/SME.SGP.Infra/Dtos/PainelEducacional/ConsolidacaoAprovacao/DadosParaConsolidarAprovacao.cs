namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao
{
    public class DadosParaConsolidarAprovacao
    {
        public long TurmaId { get; set; }
        public string CodigoAluno { get; set; }
        public int ParecerConclusivoId { get; set; }
        public string ParecerDescricao { get; set; }
    }
}
