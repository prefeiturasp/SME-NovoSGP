using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDeAtividadesAvaliativasPorIdsQueryHandler : IRequestHandler<ObterListaDeAtividadesAvaliativasPorIdsQuery,IEnumerable<AtividadeAvaliativa>>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ObterListaDeAtividadesAvaliativasPorIdsQueryHandler(IRepositorioAtividadeAvaliativa atividadeAvaliativa)
        {
            repositorioAtividadeAvaliativa =
                atividadeAvaliativa ?? throw new ArgumentNullException(nameof(atividadeAvaliativa));
        }

        public  async Task<IEnumerable<AtividadeAvaliativa>> Handle(ObterListaDeAtividadesAvaliativasPorIdsQuery request, CancellationToken cancellationToken)
        {
             return await repositorioAtividadeAvaliativa.ListarPorIds(request.AtividadesAvaliativasIds);
        }
    }
}