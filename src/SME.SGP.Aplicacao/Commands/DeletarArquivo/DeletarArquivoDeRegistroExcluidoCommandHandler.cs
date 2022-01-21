using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.DeletarArquivo
{
    public class DeletarArquivoDeRegistroExcluidoCommandHandler : IRequestHandler<DeletarArquivoDeRegistroExcluidoCommand, bool>
    {
        private readonly IMediator mediator;

        public DeletarArquivoDeRegistroExcluidoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(DeletarArquivoDeRegistroExcluidoCommand request, CancellationToken cancellationToken)
        {
            var arquivoAtual = request.ArquivoAtual.Replace(@"\", @"/");
            var expressao = @"\/[0-9]{4}\/[0-9]{2}\/[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            var regex = new Regex(expressao);
            var atual = regex.Matches(arquivoAtual).Cast<Match>().Select(c => c.Value).ToList();
            await DeletarArquivo(atual, request.Caminho);
            return true;
        }

        private async Task DeletarArquivo(IEnumerable arquivos, string caminho)
        {
            foreach (var item in arquivos)
            {
                try
                {
                    var arquivo = $@"{UtilArquivo.ObterDiretorioBase()}\{caminho}{item.ToString()}";
                    var alterarBarras = arquivo.Replace(@"\", @"///");
                    if (File.Exists(alterarBarras))
                    {
                        File.SetAttributes(alterarBarras, FileAttributes.Normal);
                        File.Delete(alterarBarras);
                    }
                    else
                    {
                        var mensagem = $"Arquivo Informado para exclusão não existe no caminho {alterarBarras}";
                        await mediator.Send(new SalvarLogViaRabbitCommand(mensagem,
                                                                          LogNivel.Informacao,
                                                                          LogContexto.Arquivos));
                    }
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Falha ao deletar o arquivo {ex.Message}",
                                                                      LogNivel.Critico,
                                                                      LogContexto.Arquivos));
                }
            }

        }
    }
}
