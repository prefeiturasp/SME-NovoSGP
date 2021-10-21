using MediatR;
using Sentry;
using SME.Background.Core.Exceptions;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverArquivosExcluidosCommandHandler : IRequestHandler<RemoverArquivosExcluidosCommand, bool>
    {
        public async Task<bool> Handle(RemoverArquivosExcluidosCommand request, CancellationToken cancellationToken)
        {
            var expressao = @"\\[0-9]{4}\\[0-9]{2}\\[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            var regex = new Regex(expressao);
            var atual = regex.Matches(request.ArquivoAtual).Cast<Match>().Select(c => c.Value).ToList();
            var novo = regex.Matches(request.ArquivoNovo).Cast<Match>().Select(c => c.Value).ToList();
            var diferente = atual.Except(novo);
            DeletarArquivo(diferente, request.Caminho);
            return true;
        }

        private void DeletarArquivo(IEnumerable diferente, string caminho)
        {
            foreach (var item in diferente)
            {
                try
                {
                    var arquivo = $@"{UtilArquivo.ObterDiretorioBase()}\{caminho}{item.ToString()}";
                    var alterarBarras = arquivo.Replace(@"\", @"///");
                    if (File.Exists(alterarBarras))
                    {
                        File.SetAttributes(alterarBarras, FileAttributes.Normal);
                        File.Delete(alterarBarras);
                        Console.Write($"ERRODELETAR - Arquivo deletado do caminho {alterarBarras} ");
                        var mensagem = $"Arquivo deletado do caminho {alterarBarras} ";
                        SentrySdk.CaptureMessage(mensagem, Sentry.Protocol.SentryLevel.Error);
                    }
                    else
                    {
                        var mensagem = $"ERRODELETAR - Arquivo Informado para exclusão não existe no caminho {alterarBarras} ";
                        Console.Write(mensagem);
                        SentrySdk.CaptureMessage(mensagem, Sentry.Protocol.SentryLevel.Error);
                    }

                }
                catch (Exception ex)
                {
                    Console.Write($"ERRODELETAR - Falha ao deletar {ex.Message} ");
                    SentrySdk.CaptureMessage($"Falha ao deletar o arquivo {ex.Message} ");
                    SentrySdk.CaptureException(ex);
                }
            }

        }
    }
}
