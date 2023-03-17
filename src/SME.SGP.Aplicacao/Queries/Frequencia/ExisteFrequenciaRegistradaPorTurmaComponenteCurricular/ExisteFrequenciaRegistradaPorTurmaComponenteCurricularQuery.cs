using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery : IRequest<bool>
    {
        public ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(string codigoTurma, string[] componentesCurricularesId, long periodoEscolarId, string professor = null)
        {
            CodigoTurma = codigoTurma;
            ComponentesCurricularesId = componentesCurricularesId;
            PeriodoEscolarId = periodoEscolarId;
            Professor = professor;
        }

        public string CodigoTurma { get; set; }
        public string[] ComponentesCurricularesId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public string Professor { get; set; }
    }
}
