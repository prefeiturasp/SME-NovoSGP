using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRestruturacoesPlanoAEEQuery : IRequest<IEnumerable<PlanoAEEReestruturacaoDto>>
    {
        public ObterRestruturacoesPlanoAEEQuery(long planoId)
        {
            PlanoId = planoId;
        }

        public long PlanoId { get; }
    }
}
