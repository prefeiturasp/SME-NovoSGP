using MediatR;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DeletarArquivoPastaTempCommandHandler : IRequestHandler<DeletarArquivoPastaTempCommand, bool>
    {
        public async Task<bool> Handle(DeletarArquivoPastaTempCommand request, CancellationToken cancellationToken)
        {
            var expressao = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
            var regex = new Regex(expressao);
            var atual = regex.Matches(request.ArquivoAtual).Cast<Match>().Select(c => c.Value).ToList();
            var novo = regex.Matches(request.ArquivoNovo).Cast<Match>().Select(c => c.Value).ToList();
            var diferente = atual.Except(novo);
            return true;
        }

        private void DeletarArquivo(IEnumerable diferente)
        {

        }
    }
}
