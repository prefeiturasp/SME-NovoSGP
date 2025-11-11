namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral
{
    public class DadosParaConsolidarEducacaoIntegralDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int ModalidadeTurma { get; set; }
        public string Ano { get; set; }
        public int QuantidadeAlunosIntegral { get; set; }
        public int QuantidadeAlunosParcial { get; set; }
    }
}
