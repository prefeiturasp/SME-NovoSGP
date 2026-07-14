using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverConsolidacoesRegistrosPedagogicosCommandHandler : IRequestHandler<RemoverConsolidacoesRegistrosPedagogicosCommand>
    {
        private readonly IRepositorioConsolidacaoRegistrosPedagogicos repositorio;

        public RemoverConsolidacoesRegistrosPedagogicosCommandHandler(IRepositorioConsolidacaoRegistrosPedagogicos repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task Handle(RemoverConsolidacoesRegistrosPedagogicosCommand request, CancellationToken cancellationToken)
        {
            await repositorio.ExcluirPorAno(request.AnoLetivo);
        }
    }
}
