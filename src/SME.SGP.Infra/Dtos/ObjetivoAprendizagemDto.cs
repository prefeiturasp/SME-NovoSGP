using System;

namespace SME.SGP.Infra
{
    public class ObjetivoAprendizagemDto
    {
        public string Ano { get; set; }
                
        public string Codigo { get; set; }
        
        public string Descricao { get; set; }

        public long Id { get; set; }

        public long IdComponenteCurricular { get; set; }
        public long ComponenteCurricularEolId { get; set; }
    }
}