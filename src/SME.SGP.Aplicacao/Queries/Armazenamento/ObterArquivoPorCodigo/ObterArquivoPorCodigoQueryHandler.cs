using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivoPorCodigoQueryHandler : IRequestHandler<ObterArquivoPorCodigoQuery, Arquivo>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ObterArquivoPorCodigoQueryHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<Arquivo> Handle(ObterArquivoPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioArquivo.ObterPorCodigo(request.Codigo);
    }
}
