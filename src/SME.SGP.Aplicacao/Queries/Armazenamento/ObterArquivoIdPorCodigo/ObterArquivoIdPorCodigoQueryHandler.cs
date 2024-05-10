using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivoIdPorCodigoQueryHandler : IRequestHandler<ObterArquivoIdPorCodigoQuery, long>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ObterArquivoIdPorCodigoQueryHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<long> Handle(ObterArquivoIdPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioArquivo.ObterIdPorCodigo(request.ArquivoCodigo);
    }
}
