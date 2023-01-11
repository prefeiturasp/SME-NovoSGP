using MediatR;
using SME.SGP.Dados;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciasServidorPorIdOcorrenciaQueryHandler : IRequestHandler<ObterOcorrenciasServidorPorIdOcorrenciaQuery,IEnumerable<Dominio.OcorrenciaServidor>>
    {
        private readonly IRepositorioOcorrenciaServidor repositorioOcorrenciaServidor;

        public ObterOcorrenciasServidorPorIdOcorrenciaQueryHandler(IRepositorioOcorrenciaServidor repositorioOcorrenciaServidor)
        {
            this.repositorioOcorrenciaServidor = repositorioOcorrenciaServidor ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaServidor));
        }

        public async Task<IEnumerable<Dominio.OcorrenciaServidor>> Handle(ObterOcorrenciasServidorPorIdOcorrenciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioOcorrenciaServidor.ObterPorIdOcorrencia(request.IdOcorrencia);
        }
    }
}