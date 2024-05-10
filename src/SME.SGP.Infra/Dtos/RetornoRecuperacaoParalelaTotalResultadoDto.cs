namespace SME.SGP.Infra
{
    public class RetornoRecuperacaoParalelaTotalResultadoDto : RetornoRecuperacaoParalelaTotalAlunosAnoDto
    {
        public string Eixo { get; set; }
        public int EixoId { get; set; }
        public string Objetivo { get; set; }
        public int ObjetivoEixoId { get; set; }
        public int ObjetivoId { get; set; }
        public string Resposta { get; set; }
        public int RespostaId { get; set; }
        public int Ordem { get; set; }
    }
}