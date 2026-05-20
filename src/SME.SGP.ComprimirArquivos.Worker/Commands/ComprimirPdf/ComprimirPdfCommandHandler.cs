using MediatR;
using Microsoft.Extensions.Options;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.OtimizarArquivos.Worker.Commands.ComprimirPdf
{
    public class ComprimirPdfCommandHandler : IRequestHandler<ComprimirPdfCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;
        private readonly CaminhoGhostscriptUtil caminhoGhostscriptUtil;

        public ComprimirPdfCommandHandler(IMediator mediator, IServicoArmazenamento servicoArmazenamento, IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptionsAccessor, CaminhoGhostscriptUtil caminhoGhostscriptUtil)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptionsAccessor));
            this.caminhoGhostscriptUtil = caminhoGhostscriptUtil ?? throw new ArgumentNullException(nameof(caminhoGhostscriptUtil));
        }

        public async Task<bool> Handle(ComprimirPdfCommand request, CancellationToken cancellationToken)
        {
            var nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(request.NomeArquivo);
            var inputTemp = Path.Combine(Path.GetTempPath(), $"sgp_input_{nomeArquivoSemExtensao}.pdf");
            var outputTemp = Path.Combine(Path.GetTempPath(), $"sgp_output_{nomeArquivoSemExtensao}.pdf");

            try
            {
                if (!request.NomeArquivo.EhArquivoPdfParaOtimizar())
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

                var ghostscriptPath = caminhoGhostscriptUtil.ObterCaminhoGhostscript();
                if (string.IsNullOrEmpty(ghostscriptPath))
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand(
                        $"Ghostscript não encontrado no sistema. Não foi possível comprimir o PDF '{request.NomeArquivo}'.",
                        LogNivel.Critico, LogContexto.ComprimirArquivos), cancellationToken);
                    return false;
                }

                var arguments = $"-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 " +
                                $"-dDownsampleColorImages=true -dColorImageResolution=72 -dColorImageDownsampleType=/Bicubic -dColorImageCompression=/JPEG -dJPEGQ=50 " +
                                $"-dDownsampleGrayImages=true -dGrayImageResolution=72 -dGrayImageDownsampleType=/Bicubic -dGrayImageCompression=/JPEG -dJPEGQ=50 " +
                                $"-dDownsampleMonoImages=true -dMonoImageResolution=72 " +
                                $"-dEmbedAllFonts=false -dSubsetFonts=false " + 
                                $"-dNOPAUSE -dBATCH -sOutputFile=\"{outputTemp}\" \"{inputTemp}\"";

                var startInfo = new ProcessStartInfo
                {
                    FileName = ghostscriptPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    await process.WaitForExitAsync(cancellationToken);

                    if (process.ExitCode != 0)
                    {
                        var error = await process.StandardError.ReadToEndAsync();
                        await mediator.Send(new SalvarLogViaRabbitCommand(
                            $"Erro ao comprimir PDF com Ghostscript: {error}",
                            LogNivel.Critico, LogContexto.ComprimirArquivos,
                            excecaoInterna: $"Ghostscript Exit Code: {process.ExitCode}"), cancellationToken);
                        return false;
                    }
                }

                if (!File.Exists(outputTemp))
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand(
                        $"Ghostscript não gerou arquivo de saída para '{request.NomeArquivo}'.",
                        LogNivel.Critico, LogContexto.ComprimirArquivos), cancellationToken);
                    return false;
                }


                var tamanhoOriginal = new FileInfo(inputTemp).Length;
                var tamanhoComprimido = new FileInfo(outputTemp).Length;

                if (tamanhoComprimido < tamanhoOriginal)
                {
                    using var outputStream = File.OpenRead(outputTemp);
                    await servicoArmazenamento.ArmazenarSemOtimizar(request.NomeArquivo, outputStream, "application/pdf");
                }
                else
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand(
                        $"PDF '{request.NomeArquivo}' não foi reduzido de tamanho. Original: {tamanhoOriginal} bytes, Comprimido: {tamanhoComprimido} bytes.",
                        LogNivel.Informacao, LogContexto.ComprimirArquivos), cancellationToken);
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(
                            $"Erro ao comprimir arquivo PDF",
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
