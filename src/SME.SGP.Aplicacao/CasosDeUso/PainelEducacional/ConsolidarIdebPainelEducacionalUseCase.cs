using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoIdeb;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdeb;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarIdebPainelEducacionalUseCase : AbstractUseCase, IConsolidarIdebPainelEducacionalUseCase
    {
        public ConsolidarIdebPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var registrosIdebDto = await mediator.Send(new PainelEducacionalIdebQuery());

            var indicadores = registrosIdebDto
                 .GroupBy(x => new { x.AnoLetivo, x.CodigoDre, x.CodigoUe, x.SerieAno, Faixa = ObterFaixa(x.Nota) })
                 .Select(g => new PainelEducacionalConsolidacaoIdeb
                 {
                     AnoLetivo = g.Key.AnoLetivo,
                     Etapa = ObterEtapa(g.Key.SerieAno),
                     CodigoDre = g.Key.CodigoDre,
                     CodigoUe = g.Key.CodigoUe,
                     Faixa = g.Key.Faixa,
                     Quantidade = g.Count(),
                     MediaGeral = Math.Round((decimal)g.Average(y => (double)y.Nota), 2)
                 }).ToList();

            await mediator.Send(new PainelEducacionalSalvarConsolidacaoIdebCommand(indicadores));

            return true;
        }

        private static string ObterFaixa(decimal nota)
        {
            if (nota >= 0m && nota < 1m) return "0-1";
            if (nota >= 1m && nota < 2m) return "1-2";
            if (nota >= 2m && nota < 3m) return "2-3";
            if (nota >= 3m && nota < 4m) return "3-4";
            if (nota >= 4m && nota < 5m) return "4-5";
            if (nota >= 5m && nota < 6m) return "5-6";
            if (nota >= 6m && nota < 7m) return "6-7";
            if (nota >= 7m && nota < 8m) return "7-8";
            if (nota >= 8m && nota < 9m) return "8-9";
            if (nota >= 9m && nota <= 10m) return "9-10";
            return "Desconhecida";
        }

        private static PainelEducacionalIdebSerie ObterEtapa(int serieAno)
        {

            return serieAno switch
            {
                1  => PainelEducacionalIdebSerie.AnosIniciais,
                2 => PainelEducacionalIdebSerie.AnosFinais,
                3 => PainelEducacionalIdebSerie.EnsinoMedio,
                _ => PainelEducacionalIdebSerie.EnsinoMedio
            };
        }
    }
}
