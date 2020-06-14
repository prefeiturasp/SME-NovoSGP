using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoCommandHandler : IRequestHandler<ReceberRelatorioProntoCommand, RelatorioCorrelacaoJasper>
    {
        private readonly IRepositorioCorrelacaoRelatorioJasper repositorioCorrelacaoRelatorioJasper;

        public ReceberRelatorioProntoCommandHandler(IRepositorioCorrelacaoRelatorioJasper repositorioCorrelacaoRelatorioJasper)
        {
            this.repositorioCorrelacaoRelatorioJasper = repositorioCorrelacaoRelatorioJasper ?? throw new System.ArgumentNullException(nameof(repositorioCorrelacaoRelatorioJasper));
        }
        public Task<RelatorioCorrelacaoJasper> Handle(ReceberRelatorioProntoCommand request, CancellationToken cancellationToken)
        {
            var correlacao = new RelatorioCorrelacaoJasper(request.RelatorioCorrelacao, request.JSessionId, request.ExportacaoId, request.RequisicaoId);

            repositorioCorrelacaoRelatorioJasper.Salvar(correlacao);

            return Task.FromResult(correlacao);
        }
    }
}
