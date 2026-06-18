using MediatR;
using Microsoft.Extensions.Options;
using SkiaSharp;
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

                // Lê os bytes da stream
                var imagemBytes = new byte[stream.Length];
                await stream.ReadAsync(imagemBytes, 0, (int)stream.Length, cancellationToken);

                // Decodifica a imagem com SkiaSharp
                using (var skBitmap = SKBitmap.Decode(imagemBytes))
                {
                    if (skBitmap == null)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand(
                            $"Não foi possível decodificar a imagem '{request.NomeArquivo}'",
                            LogNivel.Critico, LogContexto.ComprimirArquivos), cancellationToken);
                        return false;
                    }

                    // Determina o formato baseado na extensão do arquivo
                    var formato = DeterminarFormato(request.NomeArquivo);

                    // Codifica a imagem com compressão
                    using (var image = SKImage.FromBitmap(skBitmap))
                    {
                        SKData encodedData = formato switch
                        {
                            SKEncodedImageFormat.Png => image.Encode(SKEncodedImageFormat.Png, 9), // Máxima compressão
                            SKEncodedImageFormat.Gif => image.Encode(SKEncodedImageFormat.Gif, 100),
                            _ => image.Encode(SKEncodedImageFormat.Jpeg, 50) // JPEG com qualidade 50
                        };

                        await outputStream.WriteAsync(encodedData.ToArray(), 0, encodedData.ToArray().Length, cancellationToken);
                    }
                }

                outputStream.Position = 0;
                await servicoArmazenamento.ArmazenarSemOtimizar(
                    request.NomeArquivo,
                    outputStream,
                    ObterContentType(request.NomeArquivo));

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(
                    $"Erro ao comprimir arquivo imagem ComprimirImagemCommandHandler",
                    LogNivel.Critico,
                    LogContexto.ComprimirArquivos,
                    ex.Message,
                    rastreamento: ex.StackTrace,
                    excecaoInterna: ex.InnerException?.ToString()), cancellationToken);
                return false;
            }
        }

        /// <summary>
        /// Determina o formato de imagem baseado na extensão do arquivo
        /// </summary>
        private static SKEncodedImageFormat DeterminarFormato(string nomeArquivo)
        {
            var extensao = Path.GetExtension(nomeArquivo).ToLower();

            return extensao switch
            {
                ".png" => SKEncodedImageFormat.Png,
                ".gif" => SKEncodedImageFormat.Gif,
                ".jpg" or ".jpeg" => SKEncodedImageFormat.Jpeg,
                ".webp" => SKEncodedImageFormat.Webp,
                ".bmp" => SKEncodedImageFormat.Bmp,
                _ => SKEncodedImageFormat.Jpeg // Padrão
            };
        }

        private static string ObterContentType(string nomeArquivo)
        {
            var ext = Path.GetExtension(nomeArquivo).ToLower();
            return ext switch
            {
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };
        }
    }
}