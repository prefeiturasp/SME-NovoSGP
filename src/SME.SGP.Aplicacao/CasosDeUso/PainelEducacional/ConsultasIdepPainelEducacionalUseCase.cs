using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
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

        //public async Task<PainelEducacionalIdepAgrupamentoDto> ObterIdepPorAnoEtapa(int anoLetivo, string etapa, string codigoDre)
        //{
        //    IEnumerable<PainelEducacionalIdepDto> idepPorAnoEtapa = await mediator.Send(new ObterIdepPorAnoEtapaQuery(anoLetivo, etapa, codigoDre));

        //    PainelEducacionalIdepAgrupamentoDto result =  MapearIdep(idepPorAnoEtapa, anoLetivo);

        //    return result;
        //}

        public async Task<PainelEducacionalIdepAgrupamentoDto> ObterIdepPorAnoEtapa(int anoLetivo, string etapa, string codigoDre)
        {
            IEnumerable<PainelEducacionalIdepDto> idepPorAnoEtapa = null;
            int anoUtilizado = anoLetivo;

            while (anoUtilizado >= 2019)
            {

                idepPorAnoEtapa = await mediator.Send(new ObterIdepPorAnoEtapaQuery(anoUtilizado, etapa, codigoDre));
                if (idepPorAnoEtapa != null && idepPorAnoEtapa.Any())
                {
                    return MapearIdep(idepPorAnoEtapa, anoLetivo, anoUtilizado);
                }
                anoUtilizado--;

            }

            return ObterIdepVazio(idepPorAnoEtapa, anoLetivo, anoUtilizado);
        }

        private static PainelEducacionalIdepAgrupamentoDto MapearIdep(IEnumerable<PainelEducacionalIdepDto> dados, int anoSolicitado, int anoUtilizado)
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
                Distribuicao = dadosAno.Select(d => new FaixaQuantidade
                {
                    Faixa = d.Faixa,
                    Quantidade = d.Quantidade
                }).ToList()
            };
        }

        private static PainelEducacionalIdepAgrupamentoDto ObterIdepVazio(IEnumerable<PainelEducacionalIdepDto> dados, int anoSolicitado, int anoUtilizado)
        {
            return new PainelEducacionalIdepAgrupamentoDto
            {
                AnoSolicitado = anoSolicitado,
                AnoUtilizado = anoUtilizado,
                AnoSolicitadoSemDados = true,
                Etapa = string.Empty,
                MediaGeral = 0,
                Distribuicao = new List<FaixaQuantidade>()
            };
        }
    }
}
