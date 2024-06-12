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
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;

        public ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase(IMediator mediator, IRepositorioFrequenciaConsulta repositorioFrequencia) : base(mediator)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia)); 
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO>();
            var registrosFrequenciaProdutividade = await repositorioFrequencia.ObterInformacoesProdutividadeFrequencia(filtro.AnoLetivo, filtro.CodigoUe, filtro.Bimestre);
            foreach (var regFrequencia in registrosFrequenciaProdutividade)
                await mediator.Send(new SalvarConsolidacaoProdutividadeFrequenciaCommand(regFrequencia.ToEntity()));
            return true;
        }
    }
}
