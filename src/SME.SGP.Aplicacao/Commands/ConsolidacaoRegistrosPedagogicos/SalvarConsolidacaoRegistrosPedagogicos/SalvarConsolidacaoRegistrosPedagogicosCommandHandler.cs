using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoRegistrosPedagogicosCommandHandler : AsyncRequestHandler<SalvarConsolidacaoRegistrosPedagogicosCommand>
    {
        private readonly IRepositorioConsolidacaoRegistrosPedagogicos repositorio;

        public SalvarConsolidacaoRegistrosPedagogicosCommandHandler(IRepositorioConsolidacaoRegistrosPedagogicos repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        protected override async Task Handle(SalvarConsolidacaoRegistrosPedagogicosCommand request, CancellationToken cancellationToken)
        {
            await repositorio.Inserir(request.ConsolidacaoRegistrosPedagogicos);                        
        }        
    }
}
