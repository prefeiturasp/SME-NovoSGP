using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivosPorCodigosQueryHandler : IRequestHandler<ObterArquivosPorCodigosQuery, IEnumerable<Arquivo>>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ObterArquivosPorCodigosQueryHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<IEnumerable<Arquivo>> Handle(ObterArquivosPorCodigosQuery request, CancellationToken cancellationToken)
            => await repositorioArquivo.ObterPorCodigos(request.Codigos);
    }
}
