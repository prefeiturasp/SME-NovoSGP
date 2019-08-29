using System;

namespace SME.SGP.Dto
{
    public class ObjetivoAprendizagemDto
    {
        public int Ano { get; set; }
                
        public string Codigo { get; set; }
        
        public string Descricao { get; set; }

        public long Id { get; set; }

        public long IdComponenteCurricular { get; set; }
    }
}