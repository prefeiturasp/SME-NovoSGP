using System;

namespace SME.SGP.Infra.Dtos
{
    public class PeriodoEscolarLetivoTurmaDto
    {
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
