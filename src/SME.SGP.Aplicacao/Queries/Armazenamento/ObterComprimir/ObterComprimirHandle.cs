using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Armazenamento.ObterComprimir
{
    public class ObterComprimirHandle : IRequestHandler<ObterComprimirQuery, IEnumerable<Arquivo>>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ObterComprimirHandle(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo;
        }

        public async Task<IEnumerable<Arquivo>> Handle(ObterComprimirQuery request, CancellationToken cancellationToken)
        {
            return await repositorioArquivo.ObterComprimir(request.DataInicio, request.DataFim);
        }
    }
}
