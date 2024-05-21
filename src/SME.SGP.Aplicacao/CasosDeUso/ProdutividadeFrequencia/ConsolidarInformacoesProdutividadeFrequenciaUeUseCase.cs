using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarInformacoesProdutividadeFrequenciaUeUseCase : AbstractUseCase, IConsolidarInformacoesProdutividadeFrequenciaUeUseCase
    {
        private readonly IRepositorioRegistroAcaoBuscaAtiva repositorioBuscaAtiva;

        public ConsolidarInformacoesProdutividadeFrequenciaUeUseCase(IMediator mediator, IRepositorioRegistroAcaoBuscaAtiva repositorioBuscaAtiva) : base(mediator)
        {
            this.repositorioBuscaAtiva = repositorioBuscaAtiva ?? throw new System.ArgumentNullException(nameof(repositorioBuscaAtiva)); 
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();
            var ue = await mediator.Send(new ObterCodigoUEDREPorIdQuery(filtro.Id));
            await mediator.Send(new ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommand(ue.UeCodigo, filtro.Data.Year));
            foreach (int bimestre in new int[] { 1, 2, 3, 4 })
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaBimestre, 
                                                               new FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO()
                                                               {
                                                                   AnoLetivo = filtro.Data.Year,
                                                                   Bimestre = bimestre,
                                                                   CodigoUe = ue.UeCodigo
                                                               }, Guid.NewGuid()));
                

            return true;
        }
    }
}
