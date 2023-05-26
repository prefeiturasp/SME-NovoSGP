using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoPorSituacaoUseCase : IObterQuantidadeEncaminhamentoPorSituacaoUseCase
    {
        private readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorio;
        public async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> Executar(FiltroGraficoEncaminhamentoPorSituacaoDto param)
        {
            var graficos = new List<GraficoFrequenciaTurmaEvasaoDto>();
            var consultaDados = await repositorio.ObterDadosGraficoSitaucaoPorUeAnoLetivo(param.AnoLetivo,param.UeId);

            foreach (var grafico in consultaDados)
            {
                var item = new GraficoFrequenciaTurmaEvasaoDto
                {
                    Quantidade = int.Parse(grafico.Quantiddade.ToString()),
                    DataUltimaConsolidacao = grafico.DataUltimaConsolidacao,
                    Descricao = grafico.Situacao.Name()
                    
                };
                graficos.Add(item);
            }
            return graficos;
        }
    }
}