namespace SME.SGP.Dominio
{
    public class ObjetivoAprendizagemPlano : EntidadeBase
    {
        public long ComponenteCurricularId { get; set; }
        public long ObjetivoAprendizagemJuremaId { get; set; }
        public long PlanoId { get; set; }
    }
}