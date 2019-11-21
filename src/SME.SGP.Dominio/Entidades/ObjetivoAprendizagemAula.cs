namespace SME.SGP.Dominio
{
    public class ObjetivoAprendizagemAula : EntidadeBase
    {
        public long PlanoAulaId { get; set; }
        public PlanoAula PlanoAula { get; set; }
        public long ObjetivoAprendizagemPlanoId { get; set; }
        public ObjetivoAprendizagemPlano ObjetivoAprendizagemPlano { get; set; }
    }
}
