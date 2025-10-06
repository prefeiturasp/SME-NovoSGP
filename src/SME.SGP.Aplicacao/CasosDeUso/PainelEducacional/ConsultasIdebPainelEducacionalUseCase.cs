using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdebPorAnoSerie;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasIdebPainelEducacionalUseCase : IConsultasIdebPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasIdebPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<PainelEducacionalIdebAgrupamentoDto> ObterIdeb(FiltroPainelEducacionalIdeb filtro)
        {
            ValidarParametros(filtro);

            var parametros = NormalizarParametrosFiltro(filtro);
            IEnumerable<PainelEducacionalIdebDto> idebPorAnoSerie = null;
            int anoUtilizado = parametros.AnoUtilizado;
            int anoMinimoConsulta = 2019;

            while (anoUtilizado >= anoMinimoConsulta)
            {
                idebPorAnoSerie = await mediator.Send(new ObterIdebPorAnoSerieQuery(anoUtilizado, int.Parse(parametros.Serie), parametros.CodigoDre, parametros.CodigoUe));
                if (idebPorAnoSerie != null && idebPorAnoSerie.Any())
                {
                    return MapearAgrupamentoIdeb(idebPorAnoSerie, parametros.AnoSolicitado, anoUtilizado, parametros.CodigoDre, parametros.CodigoUe);
                }

                anoUtilizado--;
            }

            int anoAtual = DateTime.Now.Year;
            return ObterIdebVazio(parametros.AnoSolicitado, anoAtual, parametros.CodigoDre, parametros.CodigoUe);
        }

        private static void ValidarParametros(FiltroPainelEducacionalIdeb filtro)
        {
            if (filtro == null)
                throw new NegocioException("Filtro não pode ser nulo");

            if (!Enum.IsDefined(typeof(PainelEducacionalIdebSerie), filtro.Serie))
                throw new NegocioException("Série/Ano inválida");

            if (filtro.AnoLetivo.HasValue)
                if (filtro.AnoLetivo.Value < 2019 || filtro.AnoLetivo.Value > DateTime.Now.Year + 1)
                    throw new NegocioException("Ano letivo deve estar entre 2019 e o próximo ano");
        }

        private static (int AnoSolicitado, int AnoUtilizado, string Serie, string CodigoDre, string CodigoUe) NormalizarParametrosFiltro(FiltroPainelEducacionalIdeb filtro)
        {
            var anoSolicitado = filtro.AnoLetivo ?? DateTime.Now.Year;
            var anoUtilizado = filtro.AnoLetivo ?? DateTime.Now.Year;

            return (
                AnoSolicitado: anoSolicitado,
                AnoUtilizado: anoUtilizado,
                Serie: filtro.Serie != 0 ? ((int)filtro.Serie).ToString() : ((int)PainelEducacionalIdebSerie.AnosIniciais).ToString(),
                CodigoDre: filtro.CodigoDre?.Trim(),
                CodigoUe: filtro.CodigoUe?.Trim()
            );
        }

        private static PainelEducacionalIdebAgrupamentoDto MapearAgrupamentoIdeb(IEnumerable<PainelEducacionalIdebDto> dados, int anoSolicitado, int anoUtilizado, string codigoDre, string codigoUe)
        {
            var registroConsolidado = dados
                  .GroupBy(r => new { r.Faixa })
                  .Select(g => new PainelEducacionalIdebDto
                  {
                      AnoLetivo = g.Select(x => x.AnoLetivo).FirstOrDefault(),
                      SerieAno = g.Select(x => x.SerieAno).FirstOrDefault(),
                      Nota = g.Select(x => x.Nota).FirstOrDefault(),
                      Faixa = g.Select(x => x.Faixa).FirstOrDefault(),
                      Quantidade = g.Sum(x => x.Quantidade),
                      CodigoDre = g.Select(x => x.CodigoDre).FirstOrDefault(),
                      CriadoEm = g.Select(x => x.CriadoEm).FirstOrDefault(),
                  })
                  .OrderBy(x => x.CodigoDre)
                  .ToList();

            var dadosAno = registroConsolidado.Where(d => d.AnoLetivo == anoUtilizado).ToList();
            var primeiroItem = dadosAno.FirstOrDefault();
            var serie = primeiroItem?.SerieAno.ToString() ?? string.Empty;
            var mediaGeral = (double)(primeiroItem?.Nota ?? 0);

            return new PainelEducacionalIdebAgrupamentoDto
            {
                AnoSolicitado = anoSolicitado,
                AnoUtilizado = anoUtilizado,
                AnoSolicitadoSemDados = anoUtilizado != anoSolicitado,
                Serie = serie,
                MediaGeral = mediaGeral,
                CodigoDre = dadosAno.Select(x => x.CodigoDre)?.FirstOrDefault(),
                CodigoUe = codigoUe,
                Distribuicao = dadosAno.Select(d => new FaixaQuantidadeIdeb
                {
                    Faixa = d.Faixa,
                    Quantidade = d.Quantidade
                }).ToList()
            };
        }

        private static PainelEducacionalIdebAgrupamentoDto ObterIdebVazio(int anoSolicitado, int anoUtilizado, string codigoDre, string codigoUe)
        {
            return new PainelEducacionalIdebAgrupamentoDto
            {
                AnoSolicitado = anoSolicitado,
                AnoUtilizado = anoUtilizado,
                AnoSolicitadoSemDados = anoUtilizado != anoSolicitado,
                Serie = string.Empty,
                MediaGeral = 0,
                CodigoDre = codigoDre,
                CodigoUe = codigoUe,
                Distribuicao = new List<FaixaQuantidadeIdeb>()
            };
        }
    }
}
