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
                 .GroupBy(x => new { x.AnoLetivo, x.CodigoDre, x.CodigoUe, x.SerieAno, x.Nota })
                 .Select(g => new PainelEducacionalConsolidacaoIdeb
                 {
                     AnoLetivo = g.Key.AnoLetivo,
                     Etapa = ObterEtapa(g.Key.SerieAno),
                     CodigoDre = g.Key.CodigoDre,
                     CodigoUe = g.Key.CodigoUe,
                     Faixa = ObterFaixa(g.Key.Nota),
                     Quantidade = g.Count(),
                     MediaGeral = Math.Round((decimal)g.Average(y => (double)y.Nota), 2)
                 });

            await mediator.Send(new PainelEducacionalSalvarConsolidacaoIdebCommand(indicadores));

            return true;
        }

        private static readonly (decimal Min, decimal Max, string Label)[] FaixasIdeb =
        {
            (0m, 0.99m, "0-1"),
            (1m, 1.99m, "1-2"),
            (2m, 2.99m, "2-3"),
            (3m, 3.99m, "3-4"),
            (4m, 4.99m, "4-5"),
            (5m, 5.99m, "5-6"),
            (6m, 6.99m, "6-7"),
            (7m, 7.99m, "7-8"),
            (8m, 8.99m, "8-9"),
            (9m, 10m, "9-10")
        };

        private static string ObterFaixa(decimal nota)
            => FaixasIdeb.FirstOrDefault(f => nota >= f.Min && nota <= f.Max).Label ?? "Desconhecida";

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
