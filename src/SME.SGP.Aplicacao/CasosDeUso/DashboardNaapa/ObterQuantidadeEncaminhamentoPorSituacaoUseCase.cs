using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoPorSituacaoUseCase : IObterQuantidadeEncaminhamentoPorSituacaoUseCase
    {
        private readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorio;

        public ObterQuantidadeEncaminhamentoPorSituacaoUseCase(IRepositorioConsolidadoEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<GraficoEncaminhamentoNAAPADto> Executar(FiltroGraficoEncaminhamentoPorSituacaoDto param)
        {
            var graficos = new GraficoEncaminhamentoNAAPADto();
            var consultaDados = await repositorio.ObterDadosGraficoSitaucaoPorUeAnoLetivo(param.AnoLetivo,param.UeId,param.DreId);
            graficos.DataUltimaConsolidacao = consultaDados.Select(x => x.DataUltimaConsolidacao).Max();
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