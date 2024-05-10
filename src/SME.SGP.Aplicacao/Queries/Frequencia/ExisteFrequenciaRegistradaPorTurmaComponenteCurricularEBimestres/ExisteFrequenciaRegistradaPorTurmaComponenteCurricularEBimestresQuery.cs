using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQuery : IRequest<bool>
    {
        public ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQuery(string codigoTurma, string[] componentesCurricularesId, long[] periodosEscolaresIds, string professor = null)
        {
            CodigoTurma = codigoTurma;
            ComponentesCurricularesId = componentesCurricularesId;
            PeriodosEscolaresIds = periodosEscolaresIds;
            Professor = professor;
        }

        public string CodigoTurma { get; set; }
        public string[] ComponentesCurricularesId { get; set; }
        public long[] PeriodosEscolaresIds { get; set; }
        public string Professor { get; set; }
    }
}
