using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarAlunosDaTurmaPorComponenteQuery : IRequest<IEnumerable<AlunoDadosBasicosDto>>
    {
        public ListarAlunosDaTurmaPorComponenteQuery(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }

        public long ComponenteCurricularId { get; set; }
    }
}
