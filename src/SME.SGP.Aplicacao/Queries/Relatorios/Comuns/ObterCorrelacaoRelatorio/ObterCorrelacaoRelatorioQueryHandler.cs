using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCorrelacaoRelatorioQueryHandler : IRequestHandler<ObterCorrelacaoRelatorioQuery, RelatorioCorrelacao>
    {
        private readonly IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio;

        public ObterCorrelacaoRelatorioQueryHandler(IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio)
        {
            this.repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio;
        }
        public Task<RelatorioCorrelacao> Handle(ObterCorrelacaoRelatorioQuery request, CancellationToken cancellationToken)
        {
            var correlacao = repositorioCorrelacaoRelatorio.ObterCorrelacaoJasperPorCodigo(request.CodigoCorrelacao);

            return Task.FromResult(correlacao);
        }
    }

}
