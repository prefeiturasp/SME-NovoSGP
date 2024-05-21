using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase : AbstractUseCase, IConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase
    {
        private readonly IRepositorioConsolidacaoProdutividadeFrequencia repositorioConsolidacao;

        public ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase(IMediator mediator, IRepositorioConsolidacaoProdutividadeFrequencia repositorioConsolidacao) : base(mediator)
        {
            this.repositorioConsolidacao = repositorioConsolidacao ?? throw new System.ArgumentNullException(nameof(repositorioConsolidacao)); 
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO>();
            

            return true;
        }
    }
}
