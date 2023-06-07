namespace SME.SGP.Infra
{
    public class ConselhoClasseRecomendacaoDto
    {
        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string CodigoTurma { get; set; }
        public int Bimestre { get; set; }
        public bool ConsideraHistorico { get; set; }
    }
}