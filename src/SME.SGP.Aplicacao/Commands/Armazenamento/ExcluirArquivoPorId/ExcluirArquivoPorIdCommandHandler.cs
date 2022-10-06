using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

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
            
            var extencao = Path.GetExtension(arquivo.Nome);

            var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = arquivo.Codigo.ToString() + extencao};
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
            
            return true;
        }
    }
}
