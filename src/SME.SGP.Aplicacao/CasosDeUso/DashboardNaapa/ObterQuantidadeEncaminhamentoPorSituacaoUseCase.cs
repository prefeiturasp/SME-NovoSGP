using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> Executar(FiltroGraficoEncaminhamentoPorSituacaoDto param)
        {
            var graficos = new List<GraficoFrequenciaTurmaEvasaoDto>();
            var consultaDados = await repositorio.ObterDadosGraficoSitaucaoPorUeAnoLetivo(param.AnoLetivo,param.UeId);

            foreach (var grafico in consultaDados)
            {
                var item = new GraficoFrequenciaTurmaEvasaoDto
                {
                    Quantidade = grafico.Quantidade,
                    DataUltimaConsolidacao = grafico.DataUltimaConsolidacao,
                    Descricao = grafico.Situacao.Name()
                    
                };
                graficos.Add(item);
            }
            return graficos;
        }
    }
}