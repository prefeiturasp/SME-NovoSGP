using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dados;

namespace SME.SGP.Aplicacao.Queries.Ocorrencia.OcorrenciaServidor.ObterOcorrenciasServidorPorIdOcorrencia
{
    public class ObterOcorrenciasServidorPorIdOcorrenciaQueryHandler : IRequestHandler<ObterOcorrenciasServidorPorIdOcorrenciaQuery,IEnumerable<Dominio.OcorrenciaServidor>>
    {
        private readonly RepositorioOcorrenciaServidor _repositorioOcorrenciaServidor;

        public ObterOcorrenciasServidorPorIdOcorrenciaQueryHandler(RepositorioOcorrenciaServidor repositorioOcorrenciaServidor)
        {
            _repositorioOcorrenciaServidor = repositorioOcorrenciaServidor ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaServidor));
        }

        public async Task<IEnumerable<Dominio.OcorrenciaServidor>> Handle(ObterOcorrenciasServidorPorIdOcorrenciaQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioOcorrenciaServidor.ObterPorIdOcorrencia(request.IdOcorrencia);
        }
    }
}