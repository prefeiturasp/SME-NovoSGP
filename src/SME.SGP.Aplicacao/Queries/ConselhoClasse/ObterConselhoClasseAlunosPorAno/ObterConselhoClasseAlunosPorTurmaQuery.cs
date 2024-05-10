using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunosPorTurmaQuery : IRequest<IEnumerable<ConselhoClasseFechamentoAlunoDto>>
    {
        public ObterConselhoClasseAlunosPorTurmaQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; }
    }
}
