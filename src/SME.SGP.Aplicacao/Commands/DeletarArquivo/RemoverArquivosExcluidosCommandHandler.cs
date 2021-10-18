using MediatR;
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
            var expressao = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
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
                var path = Path.Combine(UtilArquivo.ObterDiretorioBase(), caminho, item.ToString());
                if (File.Exists(path))
                    File.Delete(path);
            }

        }
    }
}
