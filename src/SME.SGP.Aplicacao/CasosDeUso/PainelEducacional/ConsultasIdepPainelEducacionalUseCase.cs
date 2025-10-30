using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasIdepPainelEducacionalUseCase : IConsultasIdepPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasIdepPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<PainelEducacionalIdepAgrupamentoDto> ObterIdepPorAnoEtapa(int anoLetivo, int etapa, string codigoDre)
        {
            IEnumerable<PainelEducacionalIdepDto> idepPorAnoEtapa = null;
            var parametros = NormalizarParametros(anoLetivo, etapa, codigoDre);
            int anoUtilizado = parametros.AnoLetivo;
            int anoMinimoConsulta = 2019;

            while (anoUtilizado >= anoMinimoConsulta)
            {
                idepPorAnoEtapa = await mediator.Send(new ObterIdepPorAnoEtapaQuery(anoUtilizado, parametros.Etapa, parametros.CodigoDre));
                if (idepPorAnoEtapa != null && idepPorAnoEtapa.Any())
                {
                    return MapearAgrupamentoIdep(idepPorAnoEtapa, parametros.AnoLetivo, anoUtilizado, parametros.CodigoDre);
                }

                anoUtilizado--;
            }

            int anoAtual = DateTime.Now.Year;
            return ObterIdepVazio(parametros.AnoLetivo, anoAtual, parametros.CodigoDre);
        }

        private static (int AnoLetivo, int Etapa, string CodigoDre) NormalizarParametros(int anoLetivo, int etapa, string codigoDre)
        {
            return (
                AnoLetivo: anoLetivo == 0 ? DateTime.Now.Year : anoLetivo,
                Etapa: etapa == 0 ? ((int)PainelEducacionalIdepEtapa.AnosIniciais) : etapa,
                CodigoDre: codigoDre?.Trim()
            );
        }

        private static PainelEducacionalIdepAgrupamentoDto MapearAgrupamentoIdep(IEnumerable<PainelEducacionalIdepDto> dados, int anoSolicitado, int anoUtilizado, string codigoDre)
        {
            var dadosAno = dados.Where(d => d.AnoLetivo == anoUtilizado).ToList();
            var etapa = dadosAno.FirstOrDefault().Etapa.ToString();
            var mediaGeral = (double)dadosAno.FirstOrDefault().MediaGeral;

            return new PainelEducacionalIdepAgrupamentoDto
            {
                AnoSolicitado = anoSolicitado,
                AnoUtilizado = anoUtilizado,
                AnoSolicitadoSemDados = anoUtilizado != anoSolicitado,
                Etapa = etapa,
                MediaGeral = mediaGeral,
                CodigoDre = codigoDre,
                Distribuicao = dadosAno.Select(d => new FaixaQuantidade
                {
                    Faixa = d.Faixa,
                    Quantidade = d.Quantidade
                }).ToList()
            };
        }

        private static PainelEducacionalIdepAgrupamentoDto ObterIdepVazio(int anoSolicitado, int anoUtilizado, string codigoDre)
        {
            return new PainelEducacionalIdepAgrupamentoDto
            {
                AnoSolicitado = anoSolicitado,
                AnoUtilizado = anoUtilizado,
                AnoSolicitadoSemDados = true,
                Etapa = string.Empty,
                MediaGeral = 0,
                CodigoDre = codigoDre,
                Distribuicao = new List<FaixaQuantidade>()
            };
        }
    }
}
