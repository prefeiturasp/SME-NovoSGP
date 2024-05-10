using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQuery(string alunoCodigo, string[] turmasCodigos, string[] componenteCurricularCodigos)
        {
            AlunoCodigo = alunoCodigo;
            TurmasCodigos = turmasCodigos;
            ComponenteCurricularCodigos = componenteCurricularCodigos;
        }

        public string AlunoCodigo { get; set; }
        public string[] TurmasCodigos { get; set; }
        public string[] ComponenteCurricularCodigos { get; set; }
    }
}
