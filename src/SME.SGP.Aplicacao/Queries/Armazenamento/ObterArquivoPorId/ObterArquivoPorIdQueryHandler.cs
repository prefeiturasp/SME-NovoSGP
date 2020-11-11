using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivoPorIdQueryHandler : IRequestHandler<ObterArquivoPorIdQuery, Arquivo>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ObterArquivoPorIdQueryHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<Arquivo> Handle(ObterArquivoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioArquivo.ObterPorIdAsync(request.ArquivoId);
    }
}
