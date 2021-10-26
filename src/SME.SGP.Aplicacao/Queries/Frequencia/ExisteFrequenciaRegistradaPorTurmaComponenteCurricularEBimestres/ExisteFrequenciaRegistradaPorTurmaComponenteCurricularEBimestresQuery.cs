using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQuery : IRequest<bool>
    {
        public ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQuery(string codigoTurma, string componenteCurricularId, long[] periodosEscolaresIds)
        {
            CodigoTurma = codigoTurma;
            ComponenteCurricularId = componenteCurricularId;
            PeriodosEscolaresIds = periodosEscolaresIds;
        }

        public string CodigoTurma { get; set; }
        public string ComponenteCurricularId { get; set; }
        public long[] PeriodosEscolaresIds { get; set; }
    }
}
