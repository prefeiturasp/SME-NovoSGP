using FFMpegCore;
using FFMpegCore.Enums;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirVideoCommandHandler : IRequestHandler<ComprimirVideoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly CaminhoArmazenamentoOptions caminhoArmazenamentoOptions;

        public ComprimirVideoCommandHandler(IMediator mediator, CaminhoArmazenamentoOptions caminhoArmazenamentoOptions)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.caminhoArmazenamentoOptions = caminhoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(caminhoArmazenamentoOptions));
        }

        public async Task<bool> Handle(ComprimirVideoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.NomeArquivo.EhArquivoVideoParaOtimizar())
                    return false;

                var input = Path.Combine(caminhoArmazenamentoOptions.CaminhoFisico, request.NomeArquivo);

                var output = Path.Combine(caminhoArmazenamentoOptions.CaminhoTemporario, request.NomeArquivo);

                FFMpeg.Convert(input, output, VideoType.Mp4, Speed.UltraFast, VideoSize.Hd, AudioQuality.Normal, true);

                await mediator.Send(new MoverExcluirArquivoFisicoCommand(input, output));

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao comprimir arquivo vídeo", LogNivel.Critico, LogContexto.ComprimirArquivos, ex.Message, rastreamento: ex.StackTrace, excecaoInterna: ex.InnerException?.ToString()));
                return false;
            }

        }
    }
}
