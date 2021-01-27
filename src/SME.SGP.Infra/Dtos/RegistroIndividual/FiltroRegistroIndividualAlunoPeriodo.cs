using System;

namespace SME.SGP.Infra
{
    public class FiltroRegistroIndividualAlunoPeriodo : FiltroRegistroIndividualBase
    {
        public FiltroRegistroIndividualAlunoPeriodo(long turmaId, long componenteCurricularId, 
                                                    long alunoCodigo, DateTime dataInicio, DateTime dataFim) : base(turmaId, componenteCurricularId)
        {
            AlunoCodigo = alunoCodigo;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public long AlunoCodigo { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }
    }
}
