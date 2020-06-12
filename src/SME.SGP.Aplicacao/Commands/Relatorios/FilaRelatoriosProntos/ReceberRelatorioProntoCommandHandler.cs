using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoCommandHandler : IRequestHandler<ReceberRelatorioProntoCommand, bool>
    {
        private readonly IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio;
        private readonly IRepositorioCorrelacaoRelatorioJasper repositorioCorrelacaoRelatorioJasper;

        public ReceberRelatorioProntoCommandHandler(IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio,
                                                    IRepositorioCorrelacaoRelatorioJasper repositorioCorrelacaoRelatorioJasper)
        {
            this.repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio ?? throw new System.ArgumentNullException(nameof(repositorioCorrelacaoRelatorio));
            this.repositorioCorrelacaoRelatorioJasper = repositorioCorrelacaoRelatorioJasper ?? throw new System.ArgumentNullException(nameof(repositorioCorrelacaoRelatorioJasper));
        }
        public async Task<bool> Handle(ReceberRelatorioProntoCommand request, CancellationToken cancellationToken)
        {
            var correlacaoRelatorio = repositorioCorrelacaoRelatorio.ObterPorCodigoCorrelacao(request.CodigoCorrelacao);
            if (correlacaoRelatorio == null)
                throw new NegocioException("Correlação com relatório não encontrada.");

            repositorioCorrelacaoRelatorioJasper.Salvar(new RelatorioCorrelacaoJasper(correlacaoRelatorio, request.JSessionId, request.ExportacaoId, request.RequisicaoId));
            return await Task.FromResult(true);
        }
    }
}
