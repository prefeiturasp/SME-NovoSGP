using System;
using System.IO;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirVideoCommandHandler : IRequestHandler<ComprimirVideoCommand, bool>
    {
        private readonly IMediator mediator;
        
        public ComprimirVideoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<bool> Handle(ComprimirVideoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.NomeArquivo.EhArquivoVideoParaOtimizar())
                    return false;

                var input = Path.Combine(UtilArquivo.ObterDiretorioCompletoArquivos(), request.NomeArquivo);               

                var output = Path.Combine(UtilArquivo.ObterDiretorioCompletoTemporario(), request.NomeArquivo);                

                FFMpeg.Convert(input, output, VideoType.Mp4, Speed.UltraFast, VideoSize.Hd, AudioQuality.Normal, true);

                await mediator.Send(new MoverExcluirArquivoFisicoCommand(input, output));
            
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao comprimir arquivo vídeo", LogNivel.Critico, LogContexto.ComprimirArquivos, ex.Message,rastreamento:ex.StackTrace,excecaoInterna:ex.InnerException?.ToString()));
                return false;
            }
            
        }
    }
}
