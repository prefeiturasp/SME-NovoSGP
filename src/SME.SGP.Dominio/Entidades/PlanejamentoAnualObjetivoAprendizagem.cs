namespace SME.SGP.Dominio
{
    public class PlanejamentoAnualObjetivoAprendizagem : EntidadeBase
    {
        public long PlanejamentoAnualComponenteId { get; set; }
        public long ObjetivoAprendizagemId { get; set; }
        public ObjetivoAprendizagem ObjetivoAprendizagem { get; set; }
    }
}
