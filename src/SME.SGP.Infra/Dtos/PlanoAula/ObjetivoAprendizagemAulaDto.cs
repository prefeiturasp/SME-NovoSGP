namespace SME.SGP.Infra
{
    public class ObjetivoAprendizagemAulaDto
    {
        public long Id { get; set; }
        public long PlanoAulaId { get; set; }
        public long ObjetivoAprendizagemId { get; set; }
        public long ComponenteCurricularId { get; set; }

        public ObjetivoAprendizagemAulaDto()
        {
        }
    }
}