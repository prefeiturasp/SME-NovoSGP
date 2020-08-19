using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaIntentocesPorIdQueryHandler : IRequestHandler<ObterCartaIntentocesPorIdQuery, CartaIntencoes>
    {
        private readonly IRepositorioCartaIntencoes repositorioCartaIntencoes;

        public ObterCartaIntentocesPorIdQueryHandler(IRepositorioCartaIntencoes repositorioCartaIntencoes)
        {
            this.repositorioCartaIntencoes = repositorioCartaIntencoes ?? throw new ArgumentNullException(nameof(repositorioCartaIntencoes));
        }

        public async Task<CartaIntencoes> Handle(ObterCartaIntentocesPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCartaIntencoes.ObterPorIdAsync(request.Id);
        }
    }
}
