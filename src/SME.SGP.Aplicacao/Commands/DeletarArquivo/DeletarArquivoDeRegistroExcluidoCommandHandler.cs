using MediatR;
using SME.SGP.Infra;
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
        public async Task<bool> Handle(DeletarArquivoDeRegistroExcluidoCommand request, CancellationToken cancellationToken)
        {
            var arquivoAtual = request.ArquivoAtual.Replace(@"\", @"/");
            var expressao = @"\/[0-9]{4}\/[0-9]{2}\/[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            var regex = new Regex(expressao);
            var atual = regex.Matches(arquivoAtual).Cast<Match>().Select(c => c.Value).ToList();
            DeletarArquivo(atual, request.Caminho);
            return true;
        }

        private void DeletarArquivo(IEnumerable arquivos, string caminho)
        {
            foreach (var item in arquivos)
            {
                var arquivo = $@"{UtilArquivo.ObterDiretorioBase()}\{caminho}{item.ToString()}";
                if (File.Exists(arquivo))
                    File.Delete(arquivo);
            }

        }
    }
}
