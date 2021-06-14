using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery : IRequest<bool>
    {
        public ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(string codigoTurma, string componenteCurricularId, long periodoEscolarId)
        {
            CodigoTurma = codigoTurma;
            ComponenteCurricularId = componenteCurricularId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public string CodigoTurma { get; set; }
        public string ComponenteCurricularId { get; set; }
        public long PeriodoEscolarId { get; set; }
    }
}
