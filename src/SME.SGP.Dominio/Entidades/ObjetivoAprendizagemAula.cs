namespace SME.SGP.Dominio
{
    public class ObjetivoAprendizagemAula : EntidadeBase
    {
        public ObjetivoAprendizagemAula() : base() { }

        public ObjetivoAprendizagemAula(long planoAulaId, long objetivoAprendizagemPlanoId) : base()
        {
            PlanoAulaId = planoAulaId;
            ObjetivoAprendizagemPlanoId = objetivoAprendizagemPlanoId;
        }

        public long PlanoAulaId { get; set; }
        public PlanoAula PlanoAula { get; set; }
        public long ObjetivoAprendizagemPlanoId { get; set; }
        public ObjetivoAprendizagemPlano ObjetivoAprendizagemPlano { get; set; }

        public long ObjetivoAprendizagemId { get; set; }
        public ObjetivoAprendizagem ObjetivoAprendizagem { get; set; }

        public bool Excluido { get; set; }
    }
}
