using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPlanoAEE;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterConsolidacaoPlanosAEE;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarPlanoAEEPainelEducacionalUseCase : AbstractUseCase, IConsolidarPlanoAEEPainelEducacionalUseCase
    {
        public ConsolidarPlanoAEEPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var indicadores = new List<DadosParaConsolidarPlanosAEEDto>();

           var indicadoresTemp = await ObterConsolidacaoPlanosAEE();
           indicadores.AddRange(indicadoresTemp);

            if (indicadores == null || !indicadores.Any())
                return false;

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoPlanoAEECommand(indicadores));

            return true;
        }

        private async Task<IEnumerable<DadosParaConsolidarPlanosAEEDto>> ObterConsolidacaoPlanosAEE()
        {
            var anoLetivo = DateTime.Now.Year;

            var planos = await mediator.Send(new ObterDadosParaConsolidarPlanosAEEQuery(anoLetivo));

            var retorno = planos
                .GroupBy(p => new { p.CodigoDre, p.CodigoUe, p.AnoLetivo, p.SituacaoPlano })
                .Select(g => new DadosParaConsolidarPlanosAEEDto
                {
                    CodigoDre = g.Key.CodigoDre,
                    CodigoUe = g.Key.CodigoUe,
                    AnoLetivo = g.Key.AnoLetivo,
                    SituacaoPlano = g.Key.SituacaoPlano, 
                    QuantidadeSituacaoPlano = g.Count(),
                })
                .ToList();

            return retorno;
        }
    }
}
