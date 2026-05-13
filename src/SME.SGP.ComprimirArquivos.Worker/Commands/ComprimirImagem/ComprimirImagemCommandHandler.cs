using MediatR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirImagemCommandHandler : IRequestHandler<ComprimirImagemCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public ComprimirImagemCommandHandler(IMediator mediator, IServicoArmazenamento servicoArmazenamento, IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }
        
        public async Task<bool> Handle(ComprimirImagemCommand request, CancellationToken cancellationToken)
        {
            var inputTemp = Path.Combine(Path.GetTempPath(), $"input_{request.NomeArquivo}");
            var outputTemp = Path.Combine(Path.GetTempPath(), $"output_{request.NomeArquivo}");
            try
            {
                if (!request.NomeArquivo.EhArquivoImagemParaOtimizar())
                    return false;
            
                var stream = await servicoArmazenamento.ObterStream(request.NomeArquivo, configuracaoArmazenamentoOptions.BucketArquivos);
                if (stream == null)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand(
                        $"O arquivo '{request.NomeArquivo}' não foi localizado no MinIO",
                        LogNivel.Critico, LogContexto.ComprimirArquivos), cancellationToken);
                    return false;
                }

                using var outputStream = new MemoryStream();
                using (Image image = Image.Load(stream))
                {
                    IImageEncoder imageEncoder = image switch
                    {
                        Image<Rgba32> _ or Image<Bgra32> _ => new PngEncoder
                        {
                            CompressionLevel = PngCompressionLevel.BestCompression
                        },
                        Image<Argb32> _ => new GifEncoder(),
                        _ => new JpegEncoder { Quality = 50 }
                    };
                    await image.SaveAsync(outputStream, imageEncoder, cancellationToken);
                }

                outputStream.Position = 0;
                await servicoArmazenamento.ArmazenarSemOtimizar(request.NomeArquivo, outputStream, ObterContentType(request.NomeArquivo));


                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao comprimir arquivo imagem ComprimirImagemCommandHandler", LogNivel.Critico, LogContexto.ComprimirArquivos, ex.Message,rastreamento:ex.StackTrace,excecaoInterna:ex.InnerException?.ToString()), cancellationToken);
                return false;
            }
        }
        private static string ObterContentType(string nomeArquivo)
        {
            var ext = Path.GetExtension(nomeArquivo).ToLower();
            return ext switch
            {
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };
        }
    }
}
