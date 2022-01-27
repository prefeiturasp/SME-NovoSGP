using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoPorIdCommandHandler : IRequestHandler<ExcluirArquivoPorIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioArquivo repositorioArquivo { get; }

        public ExcluirArquivoPorIdCommandHandler(IMediator mediator, IRepositorioArquivo repositorioArquivo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<bool> Handle(ExcluirArquivoPorIdCommand request, CancellationToken cancellationToken)
        {
            var arquivo = await repositorioArquivo.ObterPorIdAsync(request.ArquivoId);

            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(arquivo.Id));
            await mediator.Send(new ExcluirArquivoFisicoCommand(arquivo.Codigo, arquivo.Tipo, arquivo.Nome));

            return true;
        }
    }
}
