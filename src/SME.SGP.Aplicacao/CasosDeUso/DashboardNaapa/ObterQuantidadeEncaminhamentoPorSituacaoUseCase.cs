using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

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

        public async Task<GraficoEncaminhamentoNAAPADto> Executar(FiltroGraficoEncaminhamentoPorSituacaoDto param)
        {
            var graficos = new GraficoEncaminhamentoNAAPADto();
            var consultaDados = await repositorio.ObterDadosGraficoSitaucaoPorUeAnoLetivo(param.AnoLetivo,param.UeId,param.DreId);

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