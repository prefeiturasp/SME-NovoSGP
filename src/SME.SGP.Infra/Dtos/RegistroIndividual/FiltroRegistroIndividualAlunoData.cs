using System;

namespace SME.SGP.Infra
{
    public class FiltroRegistroIndividualAlunoData : FiltroRegistroIndividualBase
    {
        public FiltroRegistroIndividualAlunoData(long turmaId, long componenteCurricularId,
                                                 long alunoCodigo, DateTime data) : base(turmaId, componenteCurricularId)
        {
            AlunoCodigo = alunoCodigo;
            Data = data;
        }

        public long AlunoCodigo { get; set; }

        public DateTime Data { get; set; }
    }
}
