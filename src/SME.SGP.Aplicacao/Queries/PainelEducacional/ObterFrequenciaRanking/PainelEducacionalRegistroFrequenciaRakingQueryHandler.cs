using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaRanking
{
    public class PainelEducacionalRegistroFrequenciaRakingQueryHandler : IRequestHandler<PainelEducacionalRegistroFrequenciaRakingQuery, PainelEducacionalRegistroFrequenciaRankingDto>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola;
        public PainelEducacionalRegistroFrequenciaRakingQueryHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola)
        {
            this.repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola = repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola;
        }


        public async Task<PainelEducacionalRegistroFrequenciaRankingDto> Handle(PainelEducacionalRegistroFrequenciaRakingQuery request, CancellationToken cancellationToken)
        {
            var registrosFrequencia = await repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola.ObterFrequenciaGlobal(request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registrosFrequencia);
        }

        private PainelEducacionalRegistroFrequenciaRankingDto MapearParaDto(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola> registrosFrequencia)
        {
            var frequenciaRanking = new PainelEducacionalRegistroFrequenciaRankingDto();

            frequenciaRanking.EscolasEmSituacaoCritica = registrosFrequencia
                    .GroupBy(r => new { r.CodigoUe })
                    .Select(g => new PainelEducacionalRegistroFrequenciaRankingItemDto
                    {
                        Ue = g.Select(x => x.UE).FirstOrDefault(),
                        PercentualFrequencia = g.Average(x => x.PercentualFrequencia)
                    })
                    .Where(x => x.PercentualFrequencia < 85)
                    .OrderBy(x => x.Ue);

            frequenciaRanking.EscolasEmAtencao = registrosFrequencia
                   .GroupBy(r => new { r.CodigoUe })
                   .Select(g => new PainelEducacionalRegistroFrequenciaRankingItemDto
                   {
                       Ue = g.Select(x => x.UE).FirstOrDefault(),
                       PercentualFrequencia = g.Average(x => x.PercentualFrequencia)
                   })
                   .Where(x => x.PercentualFrequencia >= 85 && x.PercentualFrequencia <= 90)
                   .OrderBy(x => x.Ue);

            frequenciaRanking.EscolasRanqueadas = registrosFrequencia
                   .GroupBy(r => new { r.CodigoUe })
                   .Select(g => new PainelEducacionalRegistroFrequenciaRankingItemDto
                   {
                       Ue = g.Select(x => x.UE).FirstOrDefault(),
                       PercentualFrequencia = g.Average(x => x.PercentualFrequencia)
                   })
                   .Where(x => x.PercentualFrequencia > 90)
                   .OrderBy(x => x.Ue);

            return frequenciaRanking;
        }
    }
}
