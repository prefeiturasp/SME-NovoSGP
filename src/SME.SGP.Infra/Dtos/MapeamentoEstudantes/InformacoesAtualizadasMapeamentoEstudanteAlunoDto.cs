namespace SME.SGP.Infra.Dtos.MapeamentoEstudantes
{
    public class InformacoesAtualizadasMapeamentoEstudanteAlunoDto
    {
        public InformacoesAtualizadasMapeamentoEstudanteAlunoDto()
        {
        }
        public string TurmaAnoAnterior { get; set; }
        public long? IdParecerConclusivoAnoAnterior { get; set; }
        public string DescricaoParecerConclusivoAnoAnterior { get; set; }
        public int QdadeBuscasAtivasBimestre { get; set; }
        public int QdadePlanosAEEAtivos { get; set; }
        public int QdadeEncaminhamentosNAAPAAtivos { get; set; }
        public string AnotacoesPedagogicasBimestreAnterior { get; set; }
        public bool PossuiPlanoAEE() => QdadePlanosAEEAtivos > 0;
        public bool AcompanhadoPeloNAAPA() => QdadeEncaminhamentosNAAPAAtivos > 0;
    }
}
