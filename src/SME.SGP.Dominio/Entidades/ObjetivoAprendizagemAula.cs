using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ObjetivoAprendizagemAula : EntidadeBase
    {
        public ObjetivoAprendizagemAula() : base() { }

        public ObjetivoAprendizagemAula(long planoAulaId, long objetivoAprendizagemId, long componenteCurricularId) : base()
        {
            PlanoAulaId = planoAulaId;
            ObjetivoAprendizagemId = objetivoAprendizagemId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long PlanoAulaId { get; set; }
        public PlanoAula PlanoAula { get; set; }

        public long ComponenteCurricularId { get; set; }

        public long ObjetivoAprendizagemId { get; set; }
        public ObjetivoAprendizagem ObjetivoAprendizagem { get; set; }

        public bool Excluido { get; set; }
    }
}
