using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConceitoPorIdQueryHandler : IRequestHandler<ObterConceitoPorIdQuery, Conceito>
    {
        private readonly IRepositorioConceito repositorioConceito;

        public ObterConceitoPorIdQueryHandler(IRepositorioConceito repositorioConceito)
        {
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
        }
        public async Task<Conceito> Handle(ObterConceitoPorIdQuery request, CancellationToken cancellationToken)
         => await repositorioConceito.ObterPorIdAsync(request.Id);
    }
}
