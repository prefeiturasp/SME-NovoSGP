using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoProdutividadeFrequenciaCommandHandler : IRequestHandler<SalvarConsolidacaoProdutividadeFrequenciaCommand, long>
    {
        private readonly IRepositorioConsolidacaoProdutividadeFrequencia repositorioConsolidacao;

        public SalvarConsolidacaoProdutividadeFrequenciaCommandHandler(IRepositorioConsolidacaoProdutividadeFrequencia repositorioConsolidacao)
        {
            this.repositorioConsolidacao = repositorioConsolidacao ?? throw new ArgumentNullException(nameof(repositorioConsolidacao));
        }

        public async Task<long> Handle(SalvarConsolidacaoProdutividadeFrequenciaCommand request, CancellationToken cancellationToken)
        {               
            return await repositorioConsolidacao.SalvarAsync(request.Consolidacao);           
        }
    }
}
