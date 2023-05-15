using System;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageProcessor;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirImagemCommandHandler : IRequestHandler<ComprimirImagemCommand, bool>
    {
        private readonly IMediator mediator;
        
        public ComprimirImagemCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<bool> Handle(ComprimirImagemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.NomeArquivo.EhArquivoImagemParaOtimizar())
                    return false;
            
                var input = Path.Combine(UtilArquivo.ObterDiretorioCompletoArquivos(), request.NomeArquivo); 
                
                if (!File.Exists(input))
                    await mediator.Send(new SalvarLogViaRabbitCommand($"O arquivo '{request.NomeArquivo}' não foi localizado no endereço '{input}'", LogNivel.Critico, LogContexto.ComprimirArquivos)); 

                var output = Path.Combine(UtilArquivo.ObterDiretorioCompletoTemporario(), request.NomeArquivo);

                using ( var imageFactory = new ImageFactory(preserveExifData: false))
                {
                    imageFactory.Load(input)
                        .Quality(50)
                        .Save(output);
                };

                await mediator.Send(new MoverExcluirArquivoFisicoCommand(input, output));

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao comprimir arquivo imagem", LogNivel.Critico, LogContexto.ComprimirArquivos, ex.Message,rastreamento:ex.StackTrace,excecaoInterna:ex.InnerException?.ToString()));
                return false;
            }
        }
    }
}
