namespace SME.SGP.Dominio
{
    public class RecuperacaoParalelaPeriodoObjetivoResposta : EntidadeBase
    {
        public bool Excluido { get; set; }
        public long ObjetivoId { get; set; }
        public long PeriodoRecuperacaoParalelaId { get; set; }
        public long RecuperacaoParalelaId { get; set; }
        public long RespostaId { get; set; }
    }
}