using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoRegistrosPedagogicosQueryHandler : IRequestHandler<ObterConsolidacaoRegistrosPedagogicosQuery, IEnumerable<ConsolidacaoRegistrosPedagogicosDto>>
    {
        private readonly IRepositorioConsolidacaoRegistrosPedagogicos repositorio;

        public ObterConsolidacaoRegistrosPedagogicosQueryHandler(IRepositorioConsolidacaoRegistrosPedagogicos repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>> Handle(ObterConsolidacaoRegistrosPedagogicosQuery request, CancellationToken cancellationToken)
            => await repositorio.GerarRegistrosPedagogicos(request.UeId, request.AnoLetivo);
    }
}
