using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery : IRequest<IEnumerable<RegistroFrequenciaAlunoBimestreDto>>
    {
        public ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery(string codigoAluno, string[] codigosTurma, string[] componentesCurricularesId, long? periodoEscolarId)
        {
            CodigoAluno = codigoAluno;
            CodigosTurma = codigosTurma;
            ComponentesCurricularesId = componentesCurricularesId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public string CodigoAluno { get; set; }
        public string[] CodigosTurma { get; set; }
        public string[] ComponentesCurricularesId { get; set; }
        public long? PeriodoEscolarId { get; set; }
    }
}
