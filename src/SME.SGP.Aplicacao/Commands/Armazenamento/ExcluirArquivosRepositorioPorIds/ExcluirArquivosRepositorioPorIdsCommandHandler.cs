using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivosRepositorioPorIdsCommandHandler : IRequestHandler<ExcluirArquivosRepositorioPorIdsCommand,bool>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ExcluirArquivosRepositorioPorIdsCommandHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<bool> Handle(ExcluirArquivosRepositorioPorIdsCommand request, CancellationToken cancellationToken)
              => await repositorioArquivo.ExcluirArquivosPorIds(request.Ids.ToArray());
        
    }
}