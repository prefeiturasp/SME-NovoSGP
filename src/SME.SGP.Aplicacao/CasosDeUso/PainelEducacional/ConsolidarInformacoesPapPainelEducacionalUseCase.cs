using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap;
using SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Entidades.MapeamentoPap;
using SME.SGP.Infra;
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
            await ConsolidarAnoAtual();
            //var ultimoAnoConsolidado = await mediator.Send(new ObterInformacoesPapUltimoAnoConsolidadoQuery());
            //for (var ano = ultimoAnoConsolidado; ano <= DateTime.Now.Year; ano++)
            //{
            //    await ConsolidarPorAno(ano);
            //}

            return true;
        }

        private async Task ConsolidarAnoAtual()
        {
            var anoLetivoAtual = DateTime.Now.Year;

            // FASE 1: BUSCAR TODOS OS DADOS EM PARALELO
            var obterAlunosTask = mediator.Send(new ObterAlunosTurmaPapQuery());
            var obterIndicadoresTask = mediator.Send(new ObterIndicadoresPapSgpConsolidadoQuery(anoLetivoAtual));

            //await Task.WhenAll(obterAlunosTask, obterIndicadoresTask);

            //var dadosAlunosTurmasPap = obterAlunosTask.Result;
            var dadosAlunosTurmasPap = await obterAlunosTask;
            var dadosFrequencia = await mediator.Send(new ObterQuantidadeAbaixoFrequenciaPorTurmaQuery(anoLetivoAtual));
            var indicadoresPap = await obterIndicadoresTask;

            if (dadosAlunosTurmasPap == null || !dadosAlunosTurmasPap.Any())
                return;

            // FASE 2: PROCESSAR E AGREGAR DADOS DE ALUNOS E FREQUÊNCIA
            var frequenciaPorTurmaLookup = dadosFrequencia
                                          .ToDictionary(f => f.CodigoTurma, f => f.QuantidadeAbaixoMinimoFrequencia);

            var dadosAgregadosAlunosFrequencia = dadosAlunosTurmasPap
                .GroupBy(aluno => new
                {
                    aluno.AnoLetivo,
                    aluno.CodigoDre,
                    aluno.CodigoUe,
                    TipoPap = PapComponenteCurricularMap.ObterTipoPapPorComponente(aluno.ComponenteCurricularId)
                })
                .Select(g =>
                {
                    var turmasUnicas = g.Select(a => a.CodigoTurma).Distinct().ToList();
                    return new
                    {
                        Chave = g.Key,
                        TotalTurmas = turmasUnicas.Count(),
                        TotalAlunos = g.Select(a => a.CodigoAluno).Distinct().Count(),
                        TotalAlunosComFrequenciaInferiorLimite = turmasUnicas.Sum(t => frequenciaPorTurmaLookup.GetValueOrDefault(t))
                    };
                })
                .ToList();

            // FASE 3: PROCESSAR E PIVOTEAR DADOS DE DIFICULDADES
            var dificuldadesLookup = indicadoresPap
            .GroupBy(i => new { i.AnoLetivo, i.CodigoDre, i.CodigoUe, i.TipoPap })
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var outras = g.FirstOrDefault(d => d.RespostaId == 0 && d.NomeDificuldade == "Outras");
                    var topDificuldades = g.Where(d => d.RespostaId != 0).OrderByDescending(d => d.Quantidade).ToList();

                    var top1 = topDificuldades.FirstOrDefault();
                    var top2 = topDificuldades.Skip(1).FirstOrDefault();

                    return new
                    {
                        NomeDificuldadeTop1 = top1?.NomeDificuldade ?? string.Empty,
                        TotalAlunosDificuldadeTop1 = top1?.Quantidade ?? 0,
                        NomeDificuldadeTop2 = top2?.NomeDificuldade ?? string.Empty,
                        TotalAlunosDificuldadeTop2 = top2?.Quantidade ?? 0,
                        TotalAlunosDificuldadeOutras = outras?.Quantidade ?? 0
                    };
                }
            );

            // FASE 4: UNIR OS DADOS E CONSTRUIR A ENTIDADE FINAL
            var consolidacaoFinal = new List<PainelEducacionalConsolidacaoPapUe>();

            foreach (var dado in dadosAgregadosAlunosFrequencia)
            {
                var chaveDificuldade = new { dado.Chave.AnoLetivo, dado.Chave.CodigoDre, dado.Chave.CodigoUe, dado.Chave.TipoPap };

                dificuldadesLookup.TryGetValue(chaveDificuldade, out var dadosDificuldade);

                consolidacaoFinal.Add(new PainelEducacionalConsolidacaoPapUe
                {
                    AnoLetivo = dado.Chave.AnoLetivo,
                    CodigoDre = dado.Chave.CodigoDre,
                    CodigoUe = dado.Chave.CodigoUe,
                    TipoPap = dado.Chave.TipoPap,
                    TotalTurmas = dado.TotalTurmas,
                    TotalAlunos = dado.TotalAlunos,
                    TotalAlunosComFrequenciaInferiorLimite = dado.TotalAlunosComFrequenciaInferiorLimite,
                    // Dados vindos da consulta de dificuldades
                    NomeDificuldadeTop1 = dadosDificuldade?.NomeDificuldadeTop1 ?? string.Empty,
                    TotalAlunosDificuldadeTop1 = dadosDificuldade?.TotalAlunosDificuldadeTop1 ?? 0,
                    NomeDificuldadeTop2 = dadosDificuldade?.NomeDificuldadeTop2 ?? string.Empty,
                    TotalAlunosDificuldadeTop2 = dadosDificuldade?.TotalAlunosDificuldadeTop2 ?? 0,
                    TotalAlunosDificuldadeOutras = dadosDificuldade?.TotalAlunosDificuldadeOutras ?? 0,
                    // CriadoEm é definido na entidade
                });
            }
        }

        //private async Task ConsolidarPorAno(int anoLetivo)
        //{
        //    var dadosAlunosTurmas = await mediator.Send(new ObterAlunosAtivosPorAnoLetivoQuery(anoLetivo));
        //    if (dadosAlunosTurmas is null || !dadosAlunosTurmas.Any())
        //        return;

        //    var alunosAbaixoDoLimite = await mediator.Send(new ObterFrequenciaPorLimitePercentualQuery(anoLetivo, PainelEducacionalConstants.PERCENTUAL_MAX_FREQUENCIA_INDICADORES_PAP));

        //    dadosAlunosTurmas = MarcarAlunosAbaixoDaFrequencia(dadosAlunosTurmas, alunosAbaixoDoLimite);

        //    var indicadoresPap = await mediator.Send(new ObterIndicadoresPapSgpConsolidadoQuery(anoLetivo));

        //    var (consolidacaoUe, consolidacaoDre, consolidacaoSme) = ConsolidarInformacoes(dadosAlunosTurmas, indicadoresPap);

        //    await mediator.Send(new ExcluirConsolidacaoPapCommand(anoLetivo));
        //    await mediator.Send(new SalvarConsolidacaoPapUeCommand(consolidacaoUe));
        //    await mediator.Send(new SalvarConsolidacaoPapDreCommand(consolidacaoDre));
        //    await mediator.Send(new SalvarConsolidacaoPapSmeCommand(consolidacaoSme));
        //}

        //private static (IList<PainelEducacionalConsolidacaoPapUe> consolidacaoUe,
        //                IList<PainelEducacionalConsolidacaoPapDre> consolidacaoDre,
        //                IList<PainelEducacionalConsolidacaoPapSme> consolidacaoSme)
        //    ConsolidarInformacoes(IList<DadosMatriculaAlunoTipoPapDto> dadosAlunosTurmas, IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap)
        //{
        //    var tiposPap = PainelEducacionalConstants.COD_COMPONENTES_CURRICULARES_PARA_INDICADORES_PAP
        //        .Select(c => c.Key)
        //        .ToList();

        //    var consolidadoPorUe = new List<PainelEducacionalConsolidacaoPapUe>();
        //    var consolidadoPorDre = new List<PainelEducacionalConsolidacaoPapDre>();
        //    var consolidadoPorSme = new List<PainelEducacionalConsolidacaoPapSme>();
        //    foreach (var anoLetivo in dadosAlunosTurmas.Select(d => d.AnoLetivo).Distinct())
        //    {
        //        var indicaroesPapAnoLetivo = indicadoresPap.Where(i => i.AnoLetivo == anoLetivo);
        //        var topDificuldadesSme = indicaroesPapAnoLetivo?.Where(i => i.Abrangencia == "SME").OrderByDescending(i => i.Quantidade);
        //        foreach (var dre in dadosAlunosTurmas.Where(d => d.AnoLetivo == anoLetivo).Select(d => d.CodigoDre).Distinct())
        //        {
        //            var indicadoresPapDre = indicaroesPapAnoLetivo?.Where(i => i.CodigoDre == dre);
        //            var topDificuldadesDre = indicadoresPapDre?.Where(i => i.Abrangencia == "DRE").OrderByDescending(i => i.Quantidade);
        //            foreach (var ue in dadosAlunosTurmas.Where(d => d.AnoLetivo == anoLetivo && d.CodigoDre == dre).Select(d => d.CodigoUe).Distinct())
        //            {
        //                var indicadoresPapUe = indicadoresPapDre?.Where(i => i.CodigoUe == ue);
        //                var topDificuldadesUe = indicadoresPapUe?.Where(i => i.Abrangencia == "UE").OrderByDescending(i => i.Quantidade);
        //                foreach (var tipoPap in tiposPap)
        //                {
        //                    var alunosNaUeETipoPap = dadosAlunosTurmas
        //                        .Where(d => d.AnoLetivo == anoLetivo && d.CodigoDre == dre && d.CodigoUe == ue && d.TipoPap == tipoPap);
        //                    if (alunosNaUeETipoPap.Any())
        //                    {
        //                        var quantidadeAlunosUe = alunosNaUeETipoPap.Select(a => a.CodigoAluno).Distinct().Count();
        //                        var quantidadeAlunosAbaixoFrequenciaUe = alunosNaUeETipoPap.Count(a => a.AbaixoDoLimiteFrequencia);
        //                        var quantidadeTurmasUe = alunosNaUeETipoPap.Select(a => a.CodigoTurma).Distinct().Count();

        //                        consolidadoPorUe.Add(new PainelEducacionalConsolidacaoPapUe
        //                        {
        //                            AnoLetivo = anoLetivo,
        //                            CodigoDre = dre,
        //                            CodigoUe = ue,
        //                            TipoPap = tipoPap,
        //                            TotalAlunos = quantidadeAlunosUe,
        //                            TotalTurmas = quantidadeTurmasUe,
        //                            TotalAlunosComFrequenciaInferiorLimite = quantidadeAlunosAbaixoFrequenciaUe,
        //                            NomeDificuldadeTop1 = topDificuldadesUe?.ElementAtOrDefault(0)?.NomeDificuldade,
        //                            NomeDificuldadeTop2 = topDificuldadesUe?.ElementAtOrDefault(1)?.NomeDificuldade,
        //                            TotalAlunosDificuldadeTop1 = topDificuldadesUe?.ElementAtOrDefault(0)?.Quantidade ?? 0,
        //                            TotalAlunosDificuldadeTop2 = topDificuldadesUe?.ElementAtOrDefault(1)?.Quantidade ?? 0,
        //                            TotalAlunosDificuldadeOutras = topDificuldadesUe?.ElementAtOrDefault(2)?.Quantidade ?? 0
        //                        });
        //                    }
        //                }
        //            }

        //            foreach (var tipoPap in tiposPap)
        //            {
        //                var alunosNaDreETipoPap = dadosAlunosTurmas
        //                    .Where(d => d.AnoLetivo == anoLetivo && d.CodigoDre == dre && d.TipoPap == tipoPap);
        //                if (alunosNaDreETipoPap.Any())
        //                {
        //                    var quantidadeAlunosDre = alunosNaDreETipoPap.Select(a => a.CodigoAluno).Distinct().Count();
        //                    var quantidadeAlunosAbaixoFrequenciaDre = alunosNaDreETipoPap.Count(a => a.AbaixoDoLimiteFrequencia);
        //                    var quantidadeTurmasDre = alunosNaDreETipoPap.Select(a => a.CodigoTurma).Distinct().Count();

        //                    consolidadoPorDre.Add(new PainelEducacionalConsolidacaoPapDre
        //                    {
        //                        AnoLetivo = anoLetivo,
        //                        CodigoDre = dre,
        //                        TipoPap = tipoPap,
        //                        TotalAlunos = quantidadeAlunosDre,
        //                        TotalTurmas = quantidadeTurmasDre,
        //                        TotalAlunosComFrequenciaInferiorLimite = quantidadeAlunosAbaixoFrequenciaDre,
        //                        NomeDificuldadeTop1 = topDificuldadesDre?.ElementAtOrDefault(0)?.NomeDificuldade,
        //                        NomeDificuldadeTop2 = topDificuldadesDre?.ElementAtOrDefault(1)?.NomeDificuldade,
        //                        TotalAlunosDificuldadeTop1 = topDificuldadesDre?.ElementAtOrDefault(0)?.Quantidade ?? 0,
        //                        TotalAlunosDificuldadeTop2 = topDificuldadesDre?.ElementAtOrDefault(1)?.Quantidade ?? 0,
        //                        TotalAlunosDificuldadeOutras = topDificuldadesDre?.ElementAtOrDefault(2)?.Quantidade ?? 0
        //                    });
        //                }
        //            }
        //        }

        //        foreach (var tipoPap in tiposPap)
        //        {
        //            var alunosNaSmeETipoPap = dadosAlunosTurmas
        //                .Where(d => d.AnoLetivo == anoLetivo && d.TipoPap == tipoPap);
        //            if (alunosNaSmeETipoPap.Any())
        //            {
        //                var quantidadeAlunosSme = alunosNaSmeETipoPap.Select(a => a.CodigoAluno).Distinct().Count();
        //                var quantidadeAlunosAbaixoFrequenciaSme = alunosNaSmeETipoPap.Count(a => a.AbaixoDoLimiteFrequencia);
        //                var quantidadeTurmasSme = alunosNaSmeETipoPap.Select(a => a.CodigoTurma).Distinct().Count();

        //                consolidadoPorSme.Add(new PainelEducacionalConsolidacaoPapSme
        //                {
        //                    AnoLetivo = anoLetivo,
        //                    TipoPap = tipoPap,
        //                    TotalAlunos = quantidadeAlunosSme,
        //                    TotalTurmas = quantidadeTurmasSme,
        //                    TotalAlunosComFrequenciaInferiorLimite = quantidadeAlunosAbaixoFrequenciaSme,
        //                    NomeDificuldadeTop1 = topDificuldadesSme?.ElementAtOrDefault(0)?.NomeDificuldade,
        //                    NomeDificuldadeTop2 = topDificuldadesSme?.ElementAtOrDefault(1)?.NomeDificuldade,
        //                    TotalAlunosDificuldadeTop1 = topDificuldadesSme?.ElementAtOrDefault(0)?.Quantidade ?? 0,
        //                    TotalAlunosDificuldadeTop2 = topDificuldadesSme?.ElementAtOrDefault(1)?.Quantidade ?? 0,
        //                    TotalAlunosDificuldadeOutras = topDificuldadesSme?.ElementAtOrDefault(2)?.Quantidade ?? 0
        //                });
        //            }
        //        }
        //    }

        //    return (consolidadoPorUe, consolidadoPorDre, consolidadoPorSme);
        //}

        //private static List<DificuldadeIndicadoresPapPorTipoDto> ObterMaioresDificuldadesDaUe(IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, int anoLetivoAtual, string codigoDre, string codigoUe)
        //{
        //    return indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivoAtual && i.CodigoDre == codigoDre && i.CodigoUe == codigoUe)
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

        //private static List<DificuldadeIndicadoresPapPorTipoDto> ObterMaioresDificuldadesDaDre(IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, int anoLetivoAtual, string codigoDre)
        //{
        //    return indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivoAtual && i.CodigoDre == codigoDre)
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

        //private static List<DificuldadeIndicadoresPapPorTipoDto> ObterMaioresDificuldadesDaSme(IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, int anoLetivoAtual)
        //{
        //    return indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivoAtual)
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
        //    (IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, List<DificuldadeIndicadoresPapPorTipoDto> maioresDificuldades, int anoLetivoAtual, string codigoDre, string codigoUe)
        //{
        //    var totalDificuldades = indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivoAtual && i.CodigoDre == codigoDre && i.CodigoUe == codigoUe)
        //            .Sum(i => i.Quantidade);
        //    var totalMaioresDificuldades = maioresDificuldades.Sum(i => i.Quantidade);
        //    return totalDificuldades - totalMaioresDificuldades;
        //}

        //private static int ObterQuantidadeOutrasDificuldadesDre
        //    (IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, List<DificuldadeIndicadoresPapPorTipoDto> maioresDificuldades, int anoLetivoAtual, string codigoDre)
        //{
        //    var totalDificuldades = indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivoAtual && i.CodigoDre == codigoDre)
        //            .Sum(i => i.Quantidade);
        //    var totalMaioresDificuldades = maioresDificuldades.Sum(i => i.Quantidade);
        //    return totalDificuldades - totalMaioresDificuldades;
        //}

        //private static int ObterQuantidadeOutrasDificuldadesDre
        //    (IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap, List<DificuldadeIndicadoresPapPorTipoDto> maioresDificuldades, int anoLetivoAtual)
        //{
        //    var totalDificuldades = indicadoresPap
        //            .Where(i => i.AnoLetivo == anoLetivoAtual)
        //            .Sum(i => i.Quantidade);
        //    var totalMaioresDificuldades = maioresDificuldades.Sum(i => i.Quantidade);
        //    return totalDificuldades - totalMaioresDificuldades;
        //}


        //private static IList<DadosMatriculaAlunoTipoPapDto>
        //    MarcarAlunosAbaixoDaFrequencia(IList<DadosMatriculaAlunoTipoPapDto> dadosAlunosTurmas, IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto> frequenciaAlunos)
        //{
        //    var lookupAlunosBaixaFrequencia = frequenciaAlunos
        //        .Select(f => (AlunoCodigo: f.AlunoCodigo.ToString(), f.TurmaId))
        //        .ToHashSet();

        //    if (!lookupAlunosBaixaFrequencia.Any())
        //        return dadosAlunosTurmas;

        //    foreach (var aluno in dadosAlunosTurmas)
        //    {
        //        aluno.AbaixoDoLimiteFrequencia = lookupAlunosBaixaFrequencia.Contains((aluno.CodigoAluno.ToString(), aluno.CodigoTurma));
        //    }

        //    return dadosAlunosTurmas;
        //}
    }
}
