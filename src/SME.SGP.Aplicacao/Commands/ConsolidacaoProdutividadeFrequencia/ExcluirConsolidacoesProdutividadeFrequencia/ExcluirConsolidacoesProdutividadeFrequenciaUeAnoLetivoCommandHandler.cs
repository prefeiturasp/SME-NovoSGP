using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommandHandler : IRequestHandler<ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommand, bool>
    {
        private readonly IRepositorioConsolidacaoProdutividadeFrequencia repositorioConsolidacao;

        public ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommandHandler(IRepositorioConsolidacaoProdutividadeFrequencia repositorioConsolidacao)
        {
            this.repositorioConsolidacao = repositorioConsolidacao ?? throw new ArgumentNullException(nameof(repositorioConsolidacao));
        }

        public async Task<bool> Handle(ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacao.ExcluirConsolidacoes(request.UeCodigo, request.AnoLetivo);
            return true;
        }
    }
}
