using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirConsolidacaoPap;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosAtivosPorAnoLetivo;
using SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesPapPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesPapPainelEducacionalUseCase
    {
        public ConsolidarInformacoesPapPainelEducacionalUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var ultimoAnoConsolidado = await mediator.Send(new ObterInformacoesPapUltimoAnoConsolidadoQuery());
            for (var ano = ultimoAnoConsolidado; ano <= DateTime.Now.Year; ano++)
            {
                await ConsolidarPorAno(ano);
            }

            return true;
        }

        private async Task ConsolidarPorAno(int anoLetivo)
        {
            var dadosAlunosTurmas = await mediator.Send(new ObterAlunosAtivosPorAnoLetivoQuery(anoLetivo));
            if (dadosAlunosTurmas is null || !dadosAlunosTurmas.Any())
                return;

            var alunosAbaixoDoLimite = await mediator.Send(new ObterFrequenciaPorLimitePercentualQuery(anoLetivo, PainelEducacionalConstants.PERCENTUAL_MAX_FREQUENCIA_INDICADORES_PAP));

            dadosAlunosTurmas = MarcarAlunosAbaixoDaFrequencia(dadosAlunosTurmas, alunosAbaixoDoLimite);

            var indicadoresPap = await mediator.Send(new ObterIndicadoresPapSgpConsolidadoQuery(anoLetivo));

            var (consolidacaoUe, consolidacaoDre, consolidacaoSme) = ConsolidarInformacoes(dadosAlunosTurmas, indicadoresPap);

            await mediator.Send(new ExcluirConsolidacaoPapCommand(anoLetivo));
            await mediator.Send(new SalvarConsolidacaoPapUeCommand(consolidacaoUe));
            await mediator.Send(new SalvarConsolidacaoPapDreCommand(consolidacaoDre));
            await mediator.Send(new SalvarConsolidacaoPapSmeCommand(consolidacaoSme));
        }

        private static (IList<PainelEducacionalConsolidacaoPapUe> consolidacaoUe,
                        IList<PainelEducacionalConsolidacaoPapDre> consolidacaoDre,
                        IList<PainelEducacionalConsolidacaoPapSme> consolidacaoSme)
            ConsolidarInformacoes(IList<DadosMatriculaAlunoTipoPapDto> dadosAlunosTurmas, IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap)
        {
            var tiposPap = PainelEducacionalConstants.COD_COMPONENTES_CURRICULARES_PARA_INDICADORES_PAP
                .Select(c => c.Key)
                .ToList();

            var consolidadoPorUe = new List<PainelEducacionalConsolidacaoPapUe>();
            var consolidadoPorDre = new List<PainelEducacionalConsolidacaoPapDre>();
            var consolidadoPorSme = new List<PainelEducacionalConsolidacaoPapSme>();
            foreach (var anoLetivo in dadosAlunosTurmas.Select(d => d.AnoLetivo).Distinct())
            {
                var indicaroesPapAnoLetivo = indicadoresPap.Where(i => i.AnoLetivo == anoLetivo);
                var topDificuldadesSme = indicaroesPapAnoLetivo?.Where(i => i.Abrangencia == "SME").OrderByDescending(i => i.Quantidade);
                foreach (var dre in dadosAlunosTurmas.Where(d => d.AnoLetivo == anoLetivo).Select(d => d.CodigoDre).Distinct())
                {
                    var indicadoresPapDre = indicaroesPapAnoLetivo?.Where(i => i.CodigoDre == dre);
                    var topDificuldadesDre = indicadoresPapDre?.Where(i => i.Abrangencia == "DRE").OrderByDescending(i => i.Quantidade);
                    foreach (var ue in dadosAlunosTurmas.Where(d => d.AnoLetivo == anoLetivo && d.CodigoDre == dre).Select(d => d.CodigoUe).Distinct())
                    {
                        var indicadoresPapUe = indicadoresPapDre?.Where(i => i.CodigoUe == ue);
                        var topDificuldadesUe = indicadoresPapUe?.Where(i => i.Abrangencia == "UE").OrderByDescending(i => i.Quantidade);
                        foreach (var tipoPap in tiposPap)
                        {
                            var alunosNaUeETipoPap = dadosAlunosTurmas
                                .Where(d => d.AnoLetivo == anoLetivo && d.CodigoDre == dre && d.CodigoUe == ue && d.TipoPap == tipoPap);
                            if (alunosNaUeETipoPap.Any())
                            {
                                var quantidadeAlunosUe = alunosNaUeETipoPap.Select(a => a.CodigoAluno).Distinct().Count();
                                var quantidadeAlunosAbaixoFrequenciaUe = alunosNaUeETipoPap.Count(a => a.AbaixoDoLimiteFrequencia);
                                var quantidadeTurmasUe = alunosNaUeETipoPap.Select(a => a.CodigoTurma).Distinct().Count();

                                consolidadoPorUe.Add(new PainelEducacionalConsolidacaoPapUe
                                {
                                    AnoLetivo = anoLetivo,
                                    CodigoDre = dre,
                                    CodigoUe = ue,
                                    TipoPap = tipoPap,
                                    TotalAlunos = quantidadeAlunosUe,
                                    TotalTurmas = quantidadeTurmasUe,
                                    TotalAlunosComFrequenciaInferiorLimite = quantidadeAlunosAbaixoFrequenciaUe,
                                    NomeDificuldadeTop1 = topDificuldadesUe?.ElementAtOrDefault(0)?.NomeDificuldade,
                                    NomeDificuldadeTop2 = topDificuldadesUe?.ElementAtOrDefault(1)?.NomeDificuldade,
                                    TotalAlunosDificuldadeTop1 = topDificuldadesUe?.ElementAtOrDefault(0)?.Quantidade ?? 0,
                                    TotalAlunosDificuldadeTop2 = topDificuldadesUe?.ElementAtOrDefault(1)?.Quantidade ?? 0,
                                    TotalAlunosDificuldadeOutras = topDificuldadesUe?.ElementAtOrDefault(2)?.Quantidade ?? 0
                                });
                            }
                        }
                    }

                    foreach (var tipoPap in tiposPap)
                    {
                        var alunosNaDreETipoPap = dadosAlunosTurmas
                            .Where(d => d.AnoLetivo == anoLetivo && d.CodigoDre == dre && d.TipoPap == tipoPap);
                        if (alunosNaDreETipoPap.Any())
                        {
                            var quantidadeAlunosDre = alunosNaDreETipoPap.Select(a => a.CodigoAluno).Distinct().Count();
                            var quantidadeAlunosAbaixoFrequenciaDre = alunosNaDreETipoPap.Count(a => a.AbaixoDoLimiteFrequencia);
                            var quantidadeTurmasDre = alunosNaDreETipoPap.Select(a => a.CodigoTurma).Distinct().Count();

                            consolidadoPorDre.Add(new PainelEducacionalConsolidacaoPapDre
                            {
                                AnoLetivo = anoLetivo,
                                CodigoDre = dre,
                                TipoPap = tipoPap,
                                TotalAlunos = quantidadeAlunosDre,
                                TotalTurmas = quantidadeTurmasDre,
                                TotalAlunosComFrequenciaInferiorLimite = quantidadeAlunosAbaixoFrequenciaDre,
                                NomeDificuldadeTop1 = topDificuldadesDre?.ElementAtOrDefault(0)?.NomeDificuldade,
                                NomeDificuldadeTop2 = topDificuldadesDre?.ElementAtOrDefault(1)?.NomeDificuldade,
                                TotalAlunosDificuldadeTop1 = topDificuldadesDre?.ElementAtOrDefault(0)?.Quantidade ?? 0,
                                TotalAlunosDificuldadeTop2 = topDificuldadesDre?.ElementAtOrDefault(1)?.Quantidade ?? 0,
                                TotalAlunosDificuldadeOutras = topDificuldadesDre?.ElementAtOrDefault(2)?.Quantidade ?? 0
                            });
                        }
                    }
                }

                foreach (var tipoPap in tiposPap)
                {
                    var alunosNaSmeETipoPap = dadosAlunosTurmas
                        .Where(d => d.AnoLetivo == anoLetivo && d.TipoPap == tipoPap);
                    if (alunosNaSmeETipoPap.Any())
                    {
                        var quantidadeAlunosSme = alunosNaSmeETipoPap.Select(a => a.CodigoAluno).Distinct().Count();
                        var quantidadeAlunosAbaixoFrequenciaSme = alunosNaSmeETipoPap.Count(a => a.AbaixoDoLimiteFrequencia);
                        var quantidadeTurmasSme = alunosNaSmeETipoPap.Select(a => a.CodigoTurma).Distinct().Count();

                        consolidadoPorSme.Add(new PainelEducacionalConsolidacaoPapSme
                        {
                            AnoLetivo = anoLetivo,
                            TipoPap = tipoPap,
                            TotalAlunos = quantidadeAlunosSme,
                            TotalTurmas = quantidadeTurmasSme,
                            TotalAlunosComFrequenciaInferiorLimite = quantidadeAlunosAbaixoFrequenciaSme,
                            NomeDificuldadeTop1 = topDificuldadesSme?.ElementAtOrDefault(0)?.NomeDificuldade,
                            NomeDificuldadeTop2 = topDificuldadesSme?.ElementAtOrDefault(1)?.NomeDificuldade,
                            TotalAlunosDificuldadeTop1 = topDificuldadesSme?.ElementAtOrDefault(0)?.Quantidade ?? 0,
                            TotalAlunosDificuldadeTop2 = topDificuldadesSme?.ElementAtOrDefault(1)?.Quantidade ?? 0,
                            TotalAlunosDificuldadeOutras = topDificuldadesSme?.ElementAtOrDefault(2)?.Quantidade ?? 0
                        });
                    }
                }
            }

            return (consolidadoPorUe, consolidadoPorDre, consolidadoPorSme);
        }

        //private static List<DificuldadeIndicadoresPapPorTipoDto> ObterMaioresDificuldadesDaUe(IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, int anoLetivo, string codigoDre, string codigoUe)
        //{
        //    return indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivo && i.CodigoDre == codigoDre && i.CodigoUe == codigoUe)
        //            .OrderByDescending(i => i.Quantidade)
        //            .Take(PainelEducacionalConstants.QTD_INDICADORES_PAP)
        //            .Select(i => new DificuldadeIndicadoresPapPorTipoDto
        //            {
        //                NomeDificuldade = i.NomeDificuldade,
        //                RespostaId = i.RespostaId,
        //                Quantidade = i.Quantidade,
        //            })
        //            .ToList();
        //}

        //private static List<DificuldadeIndicadoresPapPorTipoDto> ObterMaioresDificuldadesDaDre(IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, int anoLetivo, string codigoDre)
        //{
        //    return indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivo && i.CodigoDre == codigoDre)
        //            .GroupBy(i => new { i.RespostaId, i.NomeDificuldade })
        //            .Select(g => new DificuldadeIndicadoresPapPorTipoDto
        //            {
        //                RespostaId = g.Key.RespostaId,
        //                NomeDificuldade = g.Key.NomeDificuldade,
        //                Quantidade = g.Sum(x => x.Quantidade)
        //            })
        //            .OrderByDescending(i => i.Quantidade)
        //            .Take(PainelEducacionalConstants.QTD_INDICADORES_PAP)
        //            .ToList();
        //}

        //private static List<DificuldadeIndicadoresPapPorTipoDto> ObterMaioresDificuldadesDaSme(IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, int anoLetivo)
        //{
        //    return indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivo)
        //            .GroupBy(i => new { i.RespostaId, i.NomeDificuldade })
        //            .Select(g => new DificuldadeIndicadoresPapPorTipoDto
        //            {
        //                RespostaId = g.Key.RespostaId,
        //                NomeDificuldade = g.Key.NomeDificuldade,
        //                Quantidade = g.Sum(x => x.Quantidade)
        //            })
        //            .OrderByDescending(i => i.Quantidade)
        //            .Take(PainelEducacionalConstants.QTD_INDICADORES_PAP)
        //            .ToList();
        //}

        //private static int ObterQuantidadeOutrasDificuldadesUe
        //    (IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, List<DificuldadeIndicadoresPapPorTipoDto> maioresDificuldades, int anoLetivo, string codigoDre, string codigoUe)
        //{
        //    var totalDificuldades = indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivo && i.CodigoDre == codigoDre && i.CodigoUe == codigoUe)
        //            .Sum(i => i.Quantidade);
        //    var totalMaioresDificuldades = maioresDificuldades.Sum(i => i.Quantidade);
        //    return totalDificuldades - totalMaioresDificuldades;
        //}

        //private static int ObterQuantidadeOutrasDificuldadesDre
        //    (IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, List<DificuldadeIndicadoresPapPorTipoDto> maioresDificuldades, int anoLetivo, string codigoDre)
        //{
        //    var totalDificuldades = indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivo && i.CodigoDre == codigoDre)
        //            .Sum(i => i.Quantidade);
        //    var totalMaioresDificuldades = maioresDificuldades.Sum(i => i.Quantidade);
        //    return totalDificuldades - totalMaioresDificuldades;
        //}

        //private static int ObterQuantidadeOutrasDificuldadesDre
        //    (IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, List<DificuldadeIndicadoresPapPorTipoDto> maioresDificuldades, int anoLetivo)
        //{
        //    var totalDificuldades = indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivo)
        //            .Sum(i => i.Quantidade);
        //    var totalMaioresDificuldades = maioresDificuldades.Sum(i => i.Quantidade);
        //    return totalDificuldades - totalMaioresDificuldades;
        //}


        private static IList<DadosMatriculaAlunoTipoPapDto>
            MarcarAlunosAbaixoDaFrequencia(IList<DadosMatriculaAlunoTipoPapDto> dadosAlunosTurmas, IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto> frequenciaAlunos)
        {
            var lookupAlunosBaixaFrequencia = frequenciaAlunos
                .Select(f => (AlunoCodigo: f.AlunoCodigo.ToString(), f.TurmaId))
                .ToHashSet();

            if (!lookupAlunosBaixaFrequencia.Any())
                return dadosAlunosTurmas;

            foreach (var aluno in dadosAlunosTurmas)
            {
                aluno.AbaixoDoLimiteFrequencia = lookupAlunosBaixaFrequencia.Contains((aluno.CodigoAluno.ToString(), aluno.CodigoTurma));
            }

            return dadosAlunosTurmas;
        }
    }
}
