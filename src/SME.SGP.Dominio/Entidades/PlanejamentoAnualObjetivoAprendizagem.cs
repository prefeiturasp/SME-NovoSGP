namespace SME.SGP.Dominio
{
    public class PlanejamentoAnualObjetivoAprendizagem : EntidadeBase
    {
        public PlanejamentoAnualObjetivoAprendizagem()
        {

        }
        public PlanejamentoAnualObjetivoAprendizagem(long planejamentoAnualComponenteId, long objetivoAprendizagemId)
        {
            PlanejamentoAnualComponenteId= planejamentoAnualComponenteId;
            ObjetivoAprendizagemId = objetivoAprendizagemId;
        }
        public long PlanejamentoAnualComponenteId { get; set; }
        public long ObjetivoAprendizagemId { get; set; }
        public ObjetivoAprendizagem ObjetivoAprendizagem { get; set; }
        public bool Excluido { get; set; }
    }
}
