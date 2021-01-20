using System;

namespace SME.SGP.Infra
{
    public class FiltroListagemDiarioBordoDto
    {
        public FiltroListagemDiarioBordoDto(long turmaId, long componenteCurricularId, DateTime? dataInicio, DateTime? dataFim)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public long TurmaId { get; set; }

        public long ComponenteCurricularId { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}
