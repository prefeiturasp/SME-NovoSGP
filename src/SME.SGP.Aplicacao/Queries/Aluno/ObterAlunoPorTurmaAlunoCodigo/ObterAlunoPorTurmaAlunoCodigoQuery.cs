using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorTurmaAlunoCodigoQuery : IRequest<AlunoPorTurmaResposta>
    {
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public bool ConsideraInativos { get; set; }

        public ObterAlunoPorTurmaAlunoCodigoQuery(string turmaCodigo, string alunoCodigo, bool consideraInativos = false)
        {
            TurmaCodigo = turmaCodigo;
            ConsideraInativos = consideraInativos;
            AlunoCodigo = alunoCodigo;
        }
    }
}
