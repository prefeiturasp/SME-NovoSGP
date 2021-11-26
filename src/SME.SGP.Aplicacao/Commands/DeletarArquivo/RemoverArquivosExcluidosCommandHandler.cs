using MediatR;
using SME.Background.Core.Exceptions;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
            var arquivoAtual = request.ArquivoAtual.Replace(@"\", @"/");
            var arquivoNovo = request.ArquivoNovo.Replace(@"\", @"/");
            var expressao = @"\/[0-9]{4}\/[0-9]{2}\/[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            var regex = new Regex(expressao);
            var atual = regex.Matches(arquivoAtual).Cast<Match>().Select(c => c.Value).ToList();
            var novo = regex.Matches(arquivoNovo).Cast<Match>().Select(c => c.Value).ToList();
            var diferente = atual.Except(novo);
            DeletarArquivo(diferente, request.Caminho);
            return true;
        }

        private void DeletarArquivo(IEnumerable diferente, string caminho)
        {
            foreach (var item in diferente)
            {
                var arquivo = $@"{UtilArquivo.ObterDiretorioBase()}/{caminho}{item.ToString()}";
                var alterarBarras = arquivo.Replace(@"\", @"/");
                if (File.Exists(alterarBarras))
                    ExcluirArquivo(alterarBarras);
                else if (caminho.Equals(TipoArquivo.AcompanhamentoAluno.Name()))
                {
                    var caminhoAntigoAcompanhamentoAluno = "aluno/acompanhamento";
                    arquivo = $@"{UtilArquivo.ObterDiretorioBase()}/{caminhoAntigoAcompanhamentoAluno}{item.ToString()}";
                    alterarBarras = arquivo.Replace(@"\", @"/");
                    ExcluirArquivo(alterarBarras);
                }
                else
                    throw new Background.Core.Exceptions.ErroInternoException($"Arquivo Informado para exclusão não existe. {alterarBarras}");
            }
        }

        private void ExcluirArquivo(string caminhoArquivo)
        {
            if (File.Exists(caminhoArquivo))
                File.Delete(caminhoArquivo);
            else
                throw new Background.Core.Exceptions.ErroInternoException($"Arquivo Informado para exclusão não existe. {caminhoArquivo}");
        }
    }
}
