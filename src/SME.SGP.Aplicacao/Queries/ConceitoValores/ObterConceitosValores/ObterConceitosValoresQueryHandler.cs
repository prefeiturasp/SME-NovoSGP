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
    public class ObterConceitosValoresQueryHandler : IRequestHandler<ObterConceitosValoresQuery, IEnumerable<Conceito>>
    {
        private readonly IRepositorioConceito repositorioConceito;

        public ObterConceitosValoresQueryHandler(IRepositorioConceito repositorioConceito)
        {
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
        }

        public async Task<IEnumerable<Conceito>> Handle(ObterConceitosValoresQuery request, CancellationToken cancellationToken)
            => await repositorioConceito.ListarAsync();
    }
}
