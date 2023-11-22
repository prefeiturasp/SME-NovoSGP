using MediatR;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    internal class ObterTamanhoCaracteresJustificativaNotaQueryHandler : IRequestHandler<ObterTamanhoCaracteresJustificativaNotaQuery, int>
    {
        public Task<int> Handle(ObterTamanhoCaracteresJustificativaNotaQuery request, CancellationToken cancellationToken)
        {
            string justificativaFormatada = Regex.Replace(request.Justificativa, "<.*?>", string.Empty);

            justificativaFormatada = justificativaFormatada.Replace("&nbsp;", string.Empty);

            return Task.FromResult(justificativaFormatada.Length);
        }
    }

}
