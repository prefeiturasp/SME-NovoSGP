using System;

namespace SME.SGP.Dto
{
    public class ObjetivoAprendizagemDto
    {
        public string Ano { get; set; }

        public DateTime AtualizadoEm { get; set; }
        public string Codigo { get; set; }

        public DateTime CriadoEm { get; set; }

        public string Descricao { get; set; }

        public long Id { get; set; }

        public long IdComponenteCurricular { get; set; }
    }
}