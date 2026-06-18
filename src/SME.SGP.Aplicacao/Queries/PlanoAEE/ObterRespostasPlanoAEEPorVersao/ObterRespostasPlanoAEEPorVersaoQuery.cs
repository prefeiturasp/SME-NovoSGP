using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostasPlanoAEEPorVersaoQuery : IRequest<IEnumerable<RespostaQuestaoDto>>
    {
        public ObterRespostasPlanoAEEPorVersaoQuery(long versaoPlanoId)
        {
            VersaoPlanoId = versaoPlanoId;
        }

        public long VersaoPlanoId { get; }
    }
}
