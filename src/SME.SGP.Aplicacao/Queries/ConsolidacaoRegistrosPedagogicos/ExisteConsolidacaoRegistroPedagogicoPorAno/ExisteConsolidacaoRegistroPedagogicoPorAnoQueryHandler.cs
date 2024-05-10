using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ExisteConsolidacaoRegistroPedagogicoPorAnoQueryHandler : IRequestHandler<ExisteConsolidacaoRegistroPedagogicoPorAnoQuery, bool>
    {
        private readonly IRepositorioConsolidacaoRegistrosPedagogicos repositorio;

        public ExisteConsolidacaoRegistroPedagogicoPorAnoQueryHandler(IRepositorioConsolidacaoRegistrosPedagogicos repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(ExisteConsolidacaoRegistroPedagogicoPorAnoQuery request, CancellationToken cancellationToken)
            => await repositorio.ExisteConsolidacaoRegistroPedagogicoPorAno(request.Ano);
    }
}
