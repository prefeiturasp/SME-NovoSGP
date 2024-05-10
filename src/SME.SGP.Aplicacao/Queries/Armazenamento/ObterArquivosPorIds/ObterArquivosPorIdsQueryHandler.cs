using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivosPorIdsQueryHandler : IRequestHandler<ObterArquivosPorIdsQuery, IEnumerable<Arquivo>>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ObterArquivosPorIdsQueryHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<IEnumerable<Arquivo>> Handle(ObterArquivosPorIdsQuery request,
            CancellationToken cancellationToken)
        {
            return await repositorioArquivo.ObterPorIds(request.Ids);
        }
    }
}