using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirConsolidacaoPap;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDadosBrutosParaConsolidacaoIndicadoresPap;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterInformacoesPapUltimoAnoConsolidado;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesPapPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesPapPainelEducacionalUseCase
    {
        private readonly IServicoPainelEducacionalConsolidacaoIndicadoresPap _servicoConsolidacaoIndicadoresPap;
        public ConsolidarInformacoesPapPainelEducacionalUseCase(
            IMediator mediator, IServicoPainelEducacionalConsolidacaoIndicadoresPap servicoIndicadoresPap)
            : base(mediator)
        {
            _servicoConsolidacaoIndicadoresPap = servicoIndicadoresPap ?? throw new ArgumentNullException(nameof(servicoIndicadoresPap));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoInicioConsolidacao = await DeterminarAnoInicioConsolidacao();
            var anoFimConsolidacao = DateTime.Now.Year;

            for (int anoLetivo = anoInicioConsolidacao; anoLetivo <= anoFimConsolidacao; anoLetivo++)
            {
                await ConsolidarAno(anoLetivo);
            }

            return true;
        }
        private async Task<int> DeterminarAnoInicioConsolidacao()
        {
            var ultimoAnoConsolidado = await mediator.Send(new ObterInformacoesPapUltimoAnoConsolidadoQuery());

            if (ultimoAnoConsolidado == 0)
                return PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE;

            if (ultimoAnoConsolidado == DateTime.Now.Year)
                return DateTime.Now.Year;

            return ultimoAnoConsolidado + 1;
        }

        private async Task ConsolidarAno(int anoLetivo)
        {
            var (consolidacaoSme, consolidacaoDre, consolidacaoUe) = await ObterDadosConsolidadosPorVisao(anoLetivo);

            if (consolidacaoSme == null || !consolidacaoSme.Any())
                return;

            await mediator.Send(new ExcluirConsolidacaoPapCommand(anoLetivo));
            await mediator.Send(new SalvarConsolidacaoPapSmeCommand(consolidacaoSme));
            await mediator.Send(new SalvarConsolidacaoPapDreCommand(consolidacaoDre));
            await mediator.Send(new SalvarConsolidacaoPapUeCommand(consolidacaoUe));
        }

        private async Task<(
                IList<PainelEducacionalConsolidacaoPapSme> Sme,
                IList<PainelEducacionalConsolidacaoPapDre> Dre,
                IList<PainelEducacionalConsolidacaoPapUe> Ue)> ObterDadosConsolidadosPorVisao(int anoLetivo)
        {
            var (dadosAlunosTurmasPap, indicadoresPap, dadosFrequencia) = await mediator
                .Send(new ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery(anoLetivo));

            if (dadosAlunosTurmasPap == null || !dadosAlunosTurmasPap.Any())
                return (null, null, null);

            // PROCESSAR E AGREGAR DADOS DE ALUNOS E FREQUÊNCIA
            var frequenciaPorTurmaLookup = dadosFrequencia
                                           .ToDictionary(f => f.CodigoTurma, f => f.QuantidadeAbaixoMinimoFrequencia);

            var dadosAgregadosAlunosFrequenciaUe = dadosAlunosTurmasPap
                .GroupBy(alunoTurma => new
                {
                    alunoTurma.AnoLetivo,
                    alunoTurma.CodigoDre,
                    alunoTurma.CodigoUe,
                    alunoTurma.TipoPap
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

            // PROCESSAR E PIVOTEAR DADOS DE DIFICULDADES
            var dificuldadesLookup = indicadoresPap
            .GroupBy(i => new { i.AnoLetivo, i.CodigoDre, i.CodigoUe, i.TipoPap })
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var outras = g.FirstOrDefault(EhOutrasDificuldades);
                    var topDificuldades = g.Where(d => !EhOutrasDificuldades(d))
                                           .OrderByDescending(d => d.Quantidade)
                                           .ToList();

                    var top1 = topDificuldades.FirstOrDefault();
                    var top2 = topDificuldades.Skip(1).FirstOrDefault();

                    return new DadosAgregadosDificuldade(
                        top1?.NomeDificuldade ?? string.Empty,
                        top1?.Quantidade ?? 0,
                        top2?.NomeDificuldade ?? string.Empty,
                        top2?.Quantidade ?? 0,
                        outras?.Quantidade ?? 0);
                }
            );

            var agregadosFrequenciaDreLookup = dadosAgregadosAlunosFrequenciaUe
            .GroupBy(g => new { g.Chave.AnoLetivo, g.Chave.CodigoDre, g.Chave.TipoPap })
            .ToDictionary(
                g => g.Key,
                g => new DadosAgregadosTurma(
                    g.Sum(item => item.TotalTurmas),
                    g.Sum(item => item.TotalAlunos),
                    g.Sum(item => item.TotalAlunosComFrequenciaInferiorLimite)));

            var agregadosFrequenciaSmeLookup = dadosAgregadosAlunosFrequenciaUe
            .GroupBy(g => new { g.Chave.AnoLetivo, g.Chave.TipoPap })
            .ToDictionary(
                g => g.Key,
                g => new DadosAgregadosTurma(
                    g.Sum(item => item.TotalTurmas),
                    g.Sum(item => item.TotalAlunos),
                    g.Sum(item => item.TotalAlunosComFrequenciaInferiorLimite)));

            var agregadosFrequenciaUeLookup = dadosAgregadosAlunosFrequenciaUe
                .ToDictionary(
                d => d.Chave,
                g => new DadosAgregadosTurma(
                    g.TotalTurmas,
                    g.TotalAlunos,
                    g.TotalAlunosComFrequenciaInferiorLimite));

            // MONTAR LISTAS FINAIS
            var consolidadosSme = new List<PainelEducacionalConsolidacaoPapSme>();
            var consolidadosDre = new List<PainelEducacionalConsolidacaoPapDre>();
            var consolidadosUe = new List<PainelEducacionalConsolidacaoPapUe>();

            foreach (var itemDificuldade in dificuldadesLookup)
            {
                var chave = itemDificuldade.Key;
                var dadosDificuldade = itemDificuldade.Value;

                // Visão SME
                if (string.IsNullOrEmpty(chave.CodigoDre) && string.IsNullOrEmpty(chave.CodigoUe))
                {
                    var chaveSme = new { chave.AnoLetivo, chave.TipoPap };
                    if (agregadosFrequenciaSmeLookup.TryGetValue(chaveSme, out var dadosAgregados))
                    {
                        var chaveAgregacaoSme = new ChaveAgregacao(chave.AnoLetivo, null, null, chave.TipoPap);
                        consolidadosSme.Add(MapearParaConsolidacaoSme(chaveAgregacaoSme, dadosAgregados, dadosDificuldade));
                    }
                }
                // Visão DRE
                else if (!string.IsNullOrEmpty(chave.CodigoDre) && string.IsNullOrEmpty(chave.CodigoUe))
                {
                    var chaveDre = new { chave.AnoLetivo, chave.CodigoDre, chave.TipoPap };
                    if (agregadosFrequenciaDreLookup.TryGetValue(chaveDre, out var dadosAgregados))
                    {
                        var chaveAgregacaoDre = new ChaveAgregacao(chave.AnoLetivo, chave.CodigoDre, null, chave.TipoPap);
                        consolidadosDre.Add(MapearParaConsolidacaoDre(chaveAgregacaoDre, dadosAgregados, dadosDificuldade));
                    }
                }
                // Visão UE
                else
                {
                    if (agregadosFrequenciaUeLookup.TryGetValue(chave, out var dadosAgregados))
                    {
                        var chaveAgregacaoUe = new ChaveAgregacao(chave.AnoLetivo, chave.CodigoDre, chave.CodigoUe, chave.TipoPap);
                        consolidadosUe.Add(MapearParaConsolidacaoUe(chaveAgregacaoUe, dadosAgregados, dadosDificuldade));
                    }
                }
            }

            return (consolidadosSme, consolidadosDre, consolidadosUe);
        }

        private bool EhOutrasDificuldades(ContagemDificuldadeIndicadoresPapPorTipoDto dificuldade) =>
            dificuldade.RespostaId == PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP &&
            dificuldade.NomeDificuldade.Equals(PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP);



        private static PainelEducacionalConsolidacaoPapSme
            MapearParaConsolidacaoSme(ChaveAgregacao chave, DadosAgregadosTurma turma, DadosAgregadosDificuldade dif)
        {
            var consolidado = new PainelEducacionalConsolidacaoPapSme();
            MapearDadosBase(consolidado, chave, turma, dif);
            return consolidado;
        }

        private static PainelEducacionalConsolidacaoPapDre
            MapearParaConsolidacaoDre(ChaveAgregacao chave, DadosAgregadosTurma turma, DadosAgregadosDificuldade dif)
        {
            var consolidado = new PainelEducacionalConsolidacaoPapDre
            {
                CodigoDre = chave.CodigoDre
            };
            MapearDadosBase(consolidado, chave, turma, dif);
            return consolidado;
        }

        private static PainelEducacionalConsolidacaoPapUe
            MapearParaConsolidacaoUe(ChaveAgregacao chave, DadosAgregadosTurma turma, DadosAgregadosDificuldade dif)
        {
            var consolidado = new PainelEducacionalConsolidacaoPapUe
            {
                CodigoDre = chave.CodigoDre,
                CodigoUe = chave.CodigoUe
            };
            MapearDadosBase(consolidado, chave, turma, dif);
            return consolidado;
        }

        private static void MapearDadosBase(PainelEducacionalConsolidacaoPapBase consolidado, ChaveAgregacao chave, DadosAgregadosTurma turma, DadosAgregadosDificuldade dif)
        {
            consolidado.AnoLetivo = chave.AnoLetivo;
            consolidado.TipoPap = chave.TipoPap;
            consolidado.TotalTurmas = turma.TotalTurmas;
            consolidado.TotalAlunos = turma.TotalAlunos;
            consolidado.TotalAlunosComFrequenciaInferiorLimite = turma.TotalAlunosComFrequenciaInferiorLimite;
            consolidado.NomeDificuldadeTop1 = dif.NomeDificuldadeTop1;
            consolidado.TotalAlunosDificuldadeTop1 = dif.TotalAlunosDificuldadeTop1;
            consolidado.NomeDificuldadeTop2 = dif.NomeDificuldadeTop2;
            consolidado.TotalAlunosDificuldadeTop2 = dif.TotalAlunosDificuldadeTop2;
            consolidado.TotalAlunosDificuldadeOutras = dif.TotalAlunosDificuldadeOutras;
        }

        private record ChaveAgregacao(int AnoLetivo, string CodigoDre, string CodigoUe, TipoPap TipoPap);
        private record DadosAgregadosTurma(int TotalTurmas, int TotalAlunos, int TotalAlunosComFrequenciaInferiorLimite);
        private record DadosAgregadosDificuldade(string NomeDificuldadeTop1, int TotalAlunosDificuldadeTop1, string NomeDificuldadeTop2, int TotalAlunosDificuldadeTop2, int TotalAlunosDificuldadeOutras);
    }
}