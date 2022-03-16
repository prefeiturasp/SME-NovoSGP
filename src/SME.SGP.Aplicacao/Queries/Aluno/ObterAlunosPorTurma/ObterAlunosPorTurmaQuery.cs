using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public string TurmaCodigo { get; set; }
        public bool ConsideraInativos { get; set; }

        public ObterAlunosPorTurmaQuery(string turmaCodigo, bool consideraInativos = false)
        {
            TurmaCodigo = turmaCodigo;
            ConsideraInativos = consideraInativos;
        }
    }
}
