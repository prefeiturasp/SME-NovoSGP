using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
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
            var registrosFrequencia = await repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola.ObterFrequenciaGlobal(request.AnoLetivo, request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registrosFrequencia, request.CodigoDre);
        }

        private PainelEducacionalRegistroFrequenciaRankingDto MapearParaDto(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola> registrosFrequencia, string codigoDre)
        {
            var frequenciaRanking = new PainelEducacionalRegistroFrequenciaRankingDto();

            var queryBase = registrosFrequencia
                .GroupBy(r => new { r.CodigoUe })
                .Select(g => new PainelEducacionalRegistroFrequenciaRankingItemDto
                {
                    Ue = g.Select(x => x.UE).FirstOrDefault(),
                    Dre = g.Select(x => x.DRE).FirstOrDefault(),
                    PercentualFrequencia = Math.Round(g.Average(x => x.PercentualFrequencia), 1)
                });

            Func<IEnumerable<PainelEducacionalRegistroFrequenciaRankingItemDto>, IEnumerable<PainelEducacionalRegistroFrequenciaRankingItemDto>> ordenar;

            if (string.IsNullOrWhiteSpace(codigoDre))
                ordenar = q => q.OrderBy(x => x.Dre).ThenBy(x => x.Ue);
            else
                ordenar = q => q.OrderBy(x => x.Ue);

            frequenciaRanking.EscolasEmSituacaoCritica = ordenar(
                queryBase.Where(x => x.PercentualFrequencia < 85));

            frequenciaRanking.EscolasEmAtencao = ordenar(
                queryBase.Where(x => x.PercentualFrequencia >= 85 && x.PercentualFrequencia <= 90));

            frequenciaRanking.EscolasRanqueadas = ordenar(
                queryBase.Where(x => x.PercentualFrequencia > 90));


            return frequenciaRanking;
        }
    }
}
