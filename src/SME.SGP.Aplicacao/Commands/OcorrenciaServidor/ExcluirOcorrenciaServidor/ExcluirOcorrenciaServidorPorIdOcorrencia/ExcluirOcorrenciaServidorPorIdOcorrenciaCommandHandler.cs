using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaServidorPorIdOcorrenciaCommandHandler : IRequestHandler<ExcluirOcorrenciaServidorPorIdOcorrenciaCommand,bool>
    {
        private readonly IRepositorioOcorrenciaServidor _repositorioOcorrenciaServidor;

        public ExcluirOcorrenciaServidorPorIdOcorrenciaCommandHandler(IRepositorioOcorrenciaServidor repositorioOcorrenciaServidor)
        {
            _repositorioOcorrenciaServidor = repositorioOcorrenciaServidor ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaServidor));
        }

        public async Task<bool> Handle(ExcluirOcorrenciaServidorPorIdOcorrenciaCommand request, CancellationToken cancellationToken)
        {
           await _repositorioOcorrenciaServidor.ExcluirPorOcorrenciaAsync(request.IdOcorrencia);
           return true;
        }
    }
}