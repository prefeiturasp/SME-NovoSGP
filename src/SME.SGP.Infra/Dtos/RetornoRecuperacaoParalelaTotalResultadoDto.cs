namespace SME.SGP.Infra
{
    public class RetornoRecuperacaoParalelaTotalResultadoDto : RetornoRecuperacaoParalelaTotalAlunosAnoDto
    {
        public string Eixo { get; set; }
        public string Objetivo { get; set; }
        public string Resposta { get; set; }
    }
}