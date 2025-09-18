using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoIdeb;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdeb;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
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

            var registrosIdeb = ConverterParaIdepRaw(registrosIdebDto);

            var ideb = ProcessarIdep(registrosIdeb);

            await mediator.Send(new PainelEducacionalSalvarConsolidacaoIdebCommand(ideb));

            return true;
        }

        private IEnumerable<PainelEducacionalIdepAgrupamento> ConverterParaIdepRaw(IEnumerable<PainelEducacionalIdebDto> dtos)
        {
            return dtos.Select(dto => new PainelEducacionalIdepAgrupamento
            {
                AnoLetivo = dto.AnoLetivo,
                Etapa = dto.SerieAno >= 1 && dto.SerieAno <= 5 ? PainelEducacionalIdepEtapa.AnosIniciais.ToString()
                : PainelEducacionalIdepEtapa.AnosFinais.ToString(),
                Nota = dto.Nota,
                CriadoEm = dto.CriadoEm,
                CodigoDre = dto.CodigoDre
            });
        }

        public IEnumerable<PainelEducacionalConsolidacaoIdeb> ProcessarIdep(IEnumerable<PainelEducacionalIdepAgrupamento> dados)
        {
            var baseQuery = CriarBaseQuery(dados);
            var faixas = CalcularFaixas(baseQuery);
            var medias = CalcularMedias(baseQuery);
            var ultimaData = CalcularUltimaData(baseQuery);

            return MontarResultado(faixas, medias, ultimaData);
        }

        private IEnumerable<dynamic> CriarBaseQuery(IEnumerable<PainelEducacionalIdepAgrupamento> dados)
        {
            return dados.Select(x => new
            {
                x.AnoLetivo,
                x.Etapa,
                x.Nota,
                x.CriadoEm,
                x.CodigoDre,
                Faixa = GetFaixa(x.Nota)
            }).ToList();
        }

        private IEnumerable<dynamic> CalcularFaixas(IEnumerable<dynamic> baseQuery)
        {
            return baseQuery
                .GroupBy(x => new { x.AnoLetivo, x.Etapa, x.CodigoDre, x.Faixa })
                .Select(g => new
                {
                    g.Key.AnoLetivo,
                    Etapa = ParseEtapa(g.Key.Etapa),
                    g.Key.CodigoDre,
                    g.Key.Faixa,
                    Quantidade = g.Count()
                }).ToList();
        }

        private IEnumerable<dynamic> CalcularMedias(IEnumerable<dynamic> baseQuery)
        {
            return baseQuery
                .GroupBy(x => new { x.AnoLetivo, x.Etapa, x.CodigoDre })
                .Select(g => new
                {
                    g.Key.AnoLetivo,
                    Etapa = ParseEtapa(g.Key.Etapa),
                    g.Key.CodigoDre,
                    MediaGeral = Math.Round(g.Average(y => y.Nota), 2)
                }).ToList();
        }

        private IEnumerable<dynamic> CalcularUltimaData(IEnumerable<dynamic> baseQuery)
        {
            return baseQuery
                .GroupBy(x => new { x.AnoLetivo, x.CodigoDre })
                .Select(g => new
                {
                    g.Key.AnoLetivo,
                    g.Key.CodigoDre,
                    UltimaAtualizacao = g.Max(y => y.CriadoEm)
                }).ToList();
        }

        private IEnumerable<PainelEducacionalConsolidacaoIdeb> MontarResultado(IEnumerable<dynamic> faixas, IEnumerable<dynamic> medias, IEnumerable<dynamic> ultimaData)
        {
            var resultado = from f in faixas
                            join m in medias
                                on new { f.AnoLetivo, Etapa = f.Etapa, f.CodigoDre }
                                equals new { m.AnoLetivo, Etapa = m.Etapa, m.CodigoDre }
                            join u in ultimaData
                                on new { f.AnoLetivo, f.CodigoDre }
                                equals new { u.AnoLetivo, u.CodigoDre }
                            orderby f.AnoLetivo, f.Etapa, f.CodigoDre, f.Faixa
                            select new PainelEducacionalConsolidacaoIdeb
                            {
                                AnoLetivo = f.AnoLetivo,
                                Etapa = f.Etapa,
                                CodigoDre = f.CodigoDre.ToString(),
                                Faixa = f.Faixa,
                                Quantidade = f.Quantidade,
                                MediaGeral = m.MediaGeral,
                                UltimaAtualizacao = u.UltimaAtualizacao
                            };

            return resultado.ToList();
        }

        private static string GetFaixa(decimal nota)
        {
            if (nota >= 0 && nota < 1) return "0-1";
            if (nota >= 1 && nota < 2) return "1-2";
            if (nota >= 2 && nota < 3) return "2-3";
            if (nota >= 3 && nota < 4) return "3-4";
            if (nota >= 4 && nota < 5) return "4-5";
            if (nota >= 5 && nota < 6) return "5-6";
            if (nota >= 6 && nota < 7) return "6-7";
            if (nota >= 7 && nota < 8) return "7-8";
            if (nota >= 8 && nota < 9) return "8-9";
            if (nota >= 9 && nota <= 10) return "9-10";
            return null;
        }

        private static PainelEducacionalIdepEtapa ParseEtapa(string etapa)
        {
            return etapa switch
            {
                "AnosIniciais" => PainelEducacionalIdepEtapa.AnosIniciais,
                "AnosFinais" => PainelEducacionalIdepEtapa.AnosFinais,
                _ => throw new ArgumentException($"Etapa inválida: {etapa}")
            };
        }
    }
}
