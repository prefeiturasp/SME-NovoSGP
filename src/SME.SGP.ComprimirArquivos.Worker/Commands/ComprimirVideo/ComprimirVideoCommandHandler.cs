using FFMpegCore;
using FFMpegCore.Enums;
using MediatR;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirVideoCommandHandler : IRequestHandler<ComprimirVideoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public ComprimirVideoCommandHandler(IMediator mediator, IServicoArmazenamento servicoArmazenamento, IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task<bool> Handle(ComprimirVideoCommand request, CancellationToken cancellationToken)
        {
            var nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(request.NomeArquivo);
            var extensao = Path.GetExtension(request.NomeArquivo);
            var inputTemp = Path.Combine(Path.GetTempPath(), $"sgp_input_{nomeArquivoSemExtensao}{extensao}");
            var outputTemp = Path.Combine(Path.GetTempPath(), $"sgp_output_{nomeArquivoSemExtensao}.mp4");

            try
            {
                if (!request.NomeArquivo.EhArquivoVideoParaOtimizar())
                    return false;

                var stream = await servicoArmazenamento.ObterStream(request.NomeArquivo, configuracaoArmazenamentoOptions.BucketArquivos);

                if (stream == null)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand(
                        $"O arquivo '{request.NomeArquivo}' não foi localizado no MinIO",
                        LogNivel.Critico, LogContexto.ComprimirArquivos), cancellationToken);
                    return false;
                }

                using (var fileStream = File.Create(inputTemp))
                    await stream.CopyToAsync(fileStream, cancellationToken);

                await FFMpegArguments
                    .FromFileInput(inputTemp)
                    .OutputToFile(outputTemp, overwrite: true, options => options
                        .WithVideoCodec(VideoCodec.LibX264)
                        .WithAudioCodec(AudioCodec.Aac)
                        .WithSpeedPreset(Speed.Medium)
                        .ForcePixelFormat("yuv420p")
                        .WithCustomArgument("-crf 28")
                        .WithAudioBitrate(128)
                        .WithFastStart())
                    .ProcessAsynchronously();

                var tamanhoOriginal = new FileInfo(inputTemp).Length;
                var tamanhoComprimido = new FileInfo(outputTemp).Length;

                if (tamanhoComprimido >= tamanhoOriginal) return true;
                using var outputStream = File.OpenRead(outputTemp);
                await servicoArmazenamento.ArmazenarSemOtimizar(request.NomeArquivo, outputStream, "video/mp4");

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(
                    $"Erro ao comprimir arquivo vídeo",
                    LogNivel.Critico, LogContexto.ComprimirArquivos,
                    ex.Message, rastreamento: ex.StackTrace, excecaoInterna: ex.InnerException?.ToString()), cancellationToken);
                return false;
            }
            finally
            {
                if (File.Exists(inputTemp)) File.Delete(inputTemp);
                if (File.Exists(outputTemp)) File.Delete(outputTemp);
            }
        }
    }
}
