using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestresQuery : IRequest<IEnumerable<TotalFrequenciaEAulasAlunoDto>>
    {
        public ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestresQuery(string alunoCodigo, long tipoCalendarioId, string[] componentesCurricularesIds, string[] turmasCodigo, int bimestre)
        {
            AlunoCodigo = alunoCodigo;
            TipoCalendarioId = tipoCalendarioId;
            ComponentesCurricularesIds = componentesCurricularesIds;
            TurmasCodigo = turmasCodigo;
            Bimestre = bimestre;
        }

        public string AlunoCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public string[] ComponentesCurricularesIds { get; set; }
        public string[] TurmasCodigo { get; set; }
        public int Bimestre { get; set; }
    }
}
