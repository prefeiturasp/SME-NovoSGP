using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoPorSituacaoUseCase : IObterQuantidadeEncaminhamentoPorSituacaoUseCase
    {
        private readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorio;
        private readonly IMediator mediator;

        public ObterQuantidadeEncaminhamentoPorSituacaoUseCase(IRepositorioConsolidadoEncaminhamentoNAAPA repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GraficoAtendimentoNAAPADto> Executar(FiltroGraficoEncaminhamentoPorSituacaoDto param)
        {
            var graficos = new GraficoAtendimentoNAAPADto();
            var consultaDados = await repositorio.ObterDadosGraficoSituacaoPorUeAnoLetivo(param.AnoLetivo, param.UeId, param.DreId, (int?)param.Modalidade);

            graficos.DataUltimaConsolidacao = await mediator.Send(new ObterDataUltimaConsolicacaoDashboardNaapaQuery(TipoParametroSistema.GerarConsolidadoEncaminhamentoNAAPA, param.AnoLetivo));
            
            foreach (var grafico in consultaDados)
            {
                var item = new GraficoBaseDto
                {
                    Quantidade = grafico.Quantidade,
                    Descricao = grafico.Situacao.Name()
                    
                };
                graficos.Graficos.Add(item);
            }

            return graficos;
        }
    }
}