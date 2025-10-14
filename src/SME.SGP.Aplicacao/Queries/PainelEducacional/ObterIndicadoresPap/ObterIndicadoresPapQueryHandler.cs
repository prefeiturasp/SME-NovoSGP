using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapQueryHandler : IRequestHandler<ObterIndicadoresPapQuery, IndicadoresPapDto>
    {
        private readonly IRepositorioPainelEducacionalConsultaIndicadoresPap repositorioConsultaIndicadoresPap;
        public ObterIndicadoresPapQueryHandler(IRepositorioPainelEducacionalConsultaIndicadoresPap repositorioConsultaIndicadoresPap)
        {
            this.repositorioConsultaIndicadoresPap = repositorioConsultaIndicadoresPap;
        }
        public async Task<IndicadoresPapDto> Handle(ObterIndicadoresPapQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<PainelEducacionalConsolidacaoPapBase> consolidacoes;

            if (!string.IsNullOrEmpty(request.CodigoUe))
                consolidacoes = await repositorioConsultaIndicadoresPap.ObterConsolidacoesUePorAno(request.AnoLetivo, request.CodigoDre, request.CodigoUe);
            else if (!string.IsNullOrEmpty(request.CodigoDre))
                consolidacoes = await repositorioConsultaIndicadoresPap.ObterConsolidacoesDrePorAno(request.AnoLetivo, request.CodigoDre);
            else
                consolidacoes = await repositorioConsultaIndicadoresPap.ObterConsolidacoesSmePorAno(request.AnoLetivo);

            return MapearRetorno(consolidacoes);
        }

        private static IndicadoresPapDto MapearRetorno(IEnumerable<PainelEducacionalConsolidacaoPapBase> consolidacoes)
        {
            return new IndicadoresPapDto
            {
                NomeDificuldadeTop1 = consolidacoes.FirstOrDefault()?.NomeDificuldadeTop1,
                NomeDificuldadeTop2 = consolidacoes.FirstOrDefault()?.NomeDificuldadeTop2,
                QuantidadesPorTipoPap = MapearQuantidadesPorTipo(consolidacoes)
            };
        }

        private static List<IndicadoresPapQuantidadesPorTipoDto> MapearQuantidadesPorTipo(IEnumerable<PainelEducacionalConsolidacaoPapBase> consolidacoes)
        {
            return consolidacoes.Select(c => new IndicadoresPapQuantidadesPorTipoDto
            {
                TipoPap = c.TipoPap,
                TotalAlunos = c.TotalAlunos,
                TotalAlunosComFrequenciaInferiorLimite = c.TotalAlunosComFrequenciaInferiorLimite,
                TotalAlunosDificuldadeOutras = c.TotalAlunosDificuldadeOutras,
                TotalAlunosDificuldadeTop1 = c.TotalAlunosDificuldadeTop1,
                TotalAlunosDificuldadeTop2 = c.TotalAlunosDificuldadeTop2,
                TotalTurmas = c.TotalTurmas
            }).OrderBy(i => i.TipoPap).ToList();
        }
    }
}