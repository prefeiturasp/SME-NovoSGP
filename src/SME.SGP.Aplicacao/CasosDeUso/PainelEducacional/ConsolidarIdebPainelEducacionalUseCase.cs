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

        private IEnumerable<PainelEducacionalIdebAgrupamento> ConverterParaIdebRaw(IEnumerable<PainelEducacionalIdebDto> dtos)
        {
            return dtos.Select(dto => new PainelEducacionalIdebAgrupamento
            {
                AnoLetivo = dto.AnoLetivo,
                Serie = Enum.GetName(typeof(PainelEducacionalIdebSerie), dto.SerieAno),
                Nota = dto.Nota,
                CriadoEm = dto.CriadoEm,
                CodigoDre = dto.CodigoDre,
                CodigoUe = dto.CodigoUe
            });
        }

        public IEnumerable<PainelEducacionalConsolidacaoIdeb> ProcessarIdeb(IEnumerable<PainelEducacionalIdebAgrupamento> dados)
        {
            var baseQuery = CriarBaseQuery(dados);
            var faixas = CalcularFaixas(baseQuery);
            var medias = CalcularMedias(baseQuery);
            var ultimaData = CalcularUltimaData(baseQuery);

            return MontarResultado(faixas, medias, ultimaData);
        }

        private IEnumerable<dynamic> CriarBaseQuery(IEnumerable<PainelEducacionalIdebAgrupamento> dados)
        {
            return dados.Select(x => new
            {
                x.AnoLetivo,
                x.Serie,
                x.Nota,
                x.CriadoEm,
                x.CodigoDre,
                x.CodigoUe,
                Faixa = GetFaixa(x.Nota)
            }).ToList();
        }

        private IEnumerable<dynamic> CalcularFaixas(IEnumerable<dynamic> baseQuery)
        {
            return baseQuery
                .GroupBy(x => new { x.AnoLetivo, x.Serie, x.CodigoDre, x.CodigoUe, x.Faixa })
                .Select(g => new
                {
                    g.Key.AnoLetivo,
                    Etapa = ParseEtapa(g.Key.Serie),
                    g.Key.CodigoDre,
                    g.Key.CodigoUe,
                    g.Key.Faixa,
                    Quantidade = g.Count()
                }).ToList();
        }

        private IEnumerable<dynamic> CalcularMedias(IEnumerable<dynamic> baseQuery)
        {
            return baseQuery
                .GroupBy(x => new { x.AnoLetivo, x.Serie, x.CodigoDre, x.CodigoUe })
                .Select(g => new
                {
                    g.Key.AnoLetivo,
                    Etapa = ParseEtapa(g.Key.Serie),
                    g.Key.CodigoDre,
                    g.Key.CodigoUe,
                    MediaGeral = Math.Round((decimal)g.Average(y => (double)y.Nota), 2)
                }).ToList();
        }

        private IEnumerable<dynamic> CalcularUltimaData(IEnumerable<dynamic> baseQuery)
        {
            return baseQuery
                .GroupBy(x => new { x.AnoLetivo, x.CodigoDre, x.CodigoUe })
                .Select(g => new
                {
                    g.Key.AnoLetivo,
                    g.Key.CodigoDre,
                    g.Key.CodigoUe,
                    UltimaAtualizacao = g.Max(y => y.CriadoEm)
                }).ToList();
        }

        private IEnumerable<PainelEducacionalConsolidacaoIdeb> MontarResultado(IEnumerable<dynamic> faixas, IEnumerable<dynamic> medias, IEnumerable<dynamic> ultimaData)
        {
            var resultado = from f in faixas
                            join m in medias
                                on new { f.AnoLetivo, Etapa = f.Etapa, f.CodigoDre, f.CodigoUe }
                                equals new { m.AnoLetivo, Etapa = m.Etapa, m.CodigoDre, m.CodigoUe }
                            join u in ultimaData
                                on new { f.AnoLetivo, f.CodigoDre, f.CodigoUe }
                                equals new { u.AnoLetivo, u.CodigoDre, u.CodigoUe }
                            orderby f.AnoLetivo, f.Etapa, f.CodigoDre, f.CodigoUe, f.Faixa
                            select new PainelEducacionalConsolidacaoIdeb
                            {
                                AnoLetivo = f.AnoLetivo,
                                Etapa = f.Etapa,
                                CodigoDre = f.CodigoDre.ToString(),
                                CodigoUe = f.CodigoUe.ToString(),
                                Faixa = f.Faixa,
                                Quantidade = f.Quantidade,
                                MediaGeral = m.MediaGeral
                            };

            return resultado.ToList();
        }

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
