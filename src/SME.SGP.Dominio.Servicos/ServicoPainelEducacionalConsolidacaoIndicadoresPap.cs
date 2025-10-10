using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Servicos;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoPainelEducacionalConsolidacaoIndicadoresPap : IServicoPainelEducacionalConsolidacaoIndicadoresPap
    {
        public (IList<PainelEducacionalConsolidacaoPapSme> Sme,
                IList<PainelEducacionalConsolidacaoPapDre> Dre,
                IList<PainelEducacionalConsolidacaoPapUe> Ue)
            ConsolidarDados(IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosAlunosTurmasPap,
                            IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap,
                            IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> dadosFrequencia)
        {
            var dadosAgregadosAlunosFrequencia = AgregarAlunoTurmaFrequencia(dadosAlunosTurmasPap, dadosFrequencia);
            var dificuldadesLookup = AgregarDificuldadesPorChave(indicadoresPap);

            var consolidadosUe = ConsolidarPorVisaoUE(dadosAgregadosAlunosFrequencia, dificuldadesLookup);
            var consolidadosDre = ConsolidarPorVisaoDRE(dadosAgregadosAlunosFrequencia, dificuldadesLookup);
            var consolidadosSme = ConsolidarPorVisaoSME(dadosAgregadosAlunosFrequencia, dificuldadesLookup);
            return (consolidadosSme.ToList(), consolidadosDre.ToList(), consolidadosUe.ToList());
        }

        private bool EhOutrasDificuldades(ContagemDificuldadeIndicadoresPapPorTipoDto dificuldade) =>
            dificuldade.RespostaId == PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP &&
            dificuldade.NomeDificuldade.Equals(PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP);

        private static IEnumerable<PainelEducacionalConsolidacaoPapUe> ConsolidarPorVisaoUE(
            List<ConsolidadoAlunoTurmaFrequencia> dadosAgregadosAlunosFrequencia,
            IDictionary<ChaveAgregacao, DadosAgregadosDificuldade> dificuldades)
        {
            return dadosAgregadosAlunosFrequencia.Select(item =>
            {
                var chave = item.Chave;
                var dadosFrequencia = item.DadosAgregadosTurma;
                var dadosDificuldade = ObterDificuldadePorChave(chave, dificuldades);
                return MapearParaConsolidacaoUe(chave, dadosFrequencia, dadosDificuldade);
            });
        }

        private static IEnumerable<PainelEducacionalConsolidacaoPapDre> ConsolidarPorVisaoDRE(
            List<ConsolidadoAlunoTurmaFrequencia> dadosAgregadosAlunosFrequencia,
            IDictionary<ChaveAgregacao, DadosAgregadosDificuldade> dificuldades)
        {
            return dadosAgregadosAlunosFrequencia
                .GroupBy(g => new { g.Chave.AnoLetivo, g.Chave.CodigoDre, g.Chave.TipoPap })
                .Select(g =>
                {
                    var chave = new ChaveAgregacao(g.Key.AnoLetivo, g.Key.CodigoDre, null, g.Key.TipoPap);
                    var dadosFrequencia = new DadosAgregadosTurma(
                        g.Sum(item => item.DadosAgregadosTurma.TotalTurmas),
                        g.Sum(item => item.DadosAgregadosTurma.TotalAlunos),
                        g.Sum(item => item.DadosAgregadosTurma.TotalAlunosComFrequenciaInferiorLimite));

                    var dadosDificuldade = ObterDificuldadePorChave(chave, dificuldades);
                    return MapearParaConsolidacaoDre(chave, dadosFrequencia, dadosDificuldade);
                });
        }

        private static IEnumerable<PainelEducacionalConsolidacaoPapSme> ConsolidarPorVisaoSME(
            List<ConsolidadoAlunoTurmaFrequencia> dadosAgregadosAlunosFrequencia,
            IDictionary<ChaveAgregacao, DadosAgregadosDificuldade> dificuldades)
        {
            return dadosAgregadosAlunosFrequencia
                .GroupBy(g => new { g.Chave.AnoLetivo, g.Chave.TipoPap })
                .Select(g =>
                {
                    var chave = new ChaveAgregacao(g.Key.AnoLetivo, null, null, g.Key.TipoPap);
                    var dadosFrequencia = new DadosAgregadosTurma(
                        g.Sum(item => item.DadosAgregadosTurma.TotalTurmas),
                        g.Sum(item => item.DadosAgregadosTurma.TotalAlunos),
                        g.Sum(item => item.DadosAgregadosTurma.TotalAlunosComFrequenciaInferiorLimite));

                    var dadosDificuldade = ObterDificuldadePorChave(chave, dificuldades);
                    return MapearParaConsolidacaoSme(chave, dadosFrequencia, dadosDificuldade);
                });
        }

        private static PainelEducacionalConsolidacaoPapSme
            MapearParaConsolidacaoSme(ChaveAgregacao chave, DadosAgregadosTurma turma,
            DadosAgregadosDificuldade dificuldade)
        {
            var consolidado = new PainelEducacionalConsolidacaoPapSme();
            MapearDadosBase(consolidado, chave, turma, dificuldade);
            return consolidado;
        }

        private static PainelEducacionalConsolidacaoPapDre
            MapearParaConsolidacaoDre(ChaveAgregacao chave, DadosAgregadosTurma turma,
            DadosAgregadosDificuldade dificuldade)
        {
            var consolidado = new PainelEducacionalConsolidacaoPapDre
            {
                CodigoDre = chave.CodigoDre
            };
            MapearDadosBase(consolidado, chave, turma, dificuldade);
            return consolidado;
        }

        private static PainelEducacionalConsolidacaoPapUe
            MapearParaConsolidacaoUe(ChaveAgregacao chave, DadosAgregadosTurma turma,
            DadosAgregadosDificuldade dificuldade)
        {
            var consolidado = new PainelEducacionalConsolidacaoPapUe
            {
                CodigoDre = chave.CodigoDre,
                CodigoUe = chave.CodigoUe
            };
            MapearDadosBase(consolidado, chave, turma, dificuldade);
            return consolidado;
        }

        private static void MapearDadosBase(PainelEducacionalConsolidacaoPapBase consolidado, ChaveAgregacao chave,
            DadosAgregadosTurma turma, DadosAgregadosDificuldade dificuldade)
        {
            consolidado.AnoLetivo = chave.AnoLetivo;
            consolidado.TipoPap = chave.TipoPap;
            consolidado.TotalTurmas = turma.TotalTurmas;
            consolidado.TotalAlunos = turma.TotalAlunos;
            consolidado.TotalAlunosComFrequenciaInferiorLimite = turma.TotalAlunosComFrequenciaInferiorLimite;
            consolidado.NomeDificuldadeTop1 = dificuldade.NomeDificuldadeTop1;
            consolidado.TotalAlunosDificuldadeTop1 = dificuldade.TotalAlunosDificuldadeTop1;
            consolidado.NomeDificuldadeTop2 = dificuldade.NomeDificuldadeTop2;
            consolidado.TotalAlunosDificuldadeTop2 = dificuldade.TotalAlunosDificuldadeTop2;
            consolidado.TotalAlunosDificuldadeOutras = dificuldade.TotalAlunosDificuldadeOutras;
        }

        private static List<ConsolidadoAlunoTurmaFrequencia> AgregarAlunoTurmaFrequencia(
            IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosAlunoTurma,
            IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> frequenciaBaixa)
        {
            var frequenciaPorTurmaLookup = frequenciaBaixa
                                           .ToDictionary(f => f.CodigoTurma, f => f.QuantidadeAbaixoMinimoFrequencia);

            return dadosAlunoTurma
                .GroupBy(alunoTurma => new ChaveAgregacao(
                    alunoTurma.AnoLetivo,
                    alunoTurma.CodigoDre,
                    alunoTurma.CodigoUe,
                    alunoTurma.TipoPap))
                .Select(g =>
                {
                    var turmasUnicas = g.Select(a => a.CodigoTurma).Distinct().ToList();
                    return new ConsolidadoAlunoTurmaFrequencia(
                        g.Key,
                        new DadosAgregadosTurma(
                            turmasUnicas.Count(),
                            g.Select(a => a.CodigoAluno).Distinct().Count(),
                            turmasUnicas.Sum(t => frequenciaPorTurmaLookup.GetValueOrDefault(t))));
                })
                .ToList();
        }
        private Dictionary<ChaveAgregacao, DadosAgregadosDificuldade> AgregarDificuldadesPorChave(
            IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap)
        {
            return indicadoresPap
            .GroupBy(i => new ChaveAgregacao(i.AnoLetivo, i.CodigoDre, i.CodigoUe, i.TipoPap))
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
        }

        private static DadosAgregadosDificuldade ObterDificuldadePorChave(
            ChaveAgregacao chave,
            IDictionary<ChaveAgregacao, DadosAgregadosDificuldade> dificuldades)
        {
            if (dificuldades != null && dificuldades.TryGetValue(chave, out var dadosDificuldade))
                return dadosDificuldade;
            return new DadosAgregadosDificuldade(string.Empty, 0, string.Empty, 0, 0);
        }

        private record ChaveAgregacao(int AnoLetivo, string CodigoDre, string CodigoUe, TipoPap TipoPap);
        private record DadosAgregadosTurma(int TotalTurmas, int TotalAlunos, int TotalAlunosComFrequenciaInferiorLimite);
        private record ConsolidadoAlunoTurmaFrequencia(ChaveAgregacao Chave, DadosAgregadosTurma DadosAgregadosTurma);
        private record DadosAgregadosDificuldade(string NomeDificuldadeTop1, int TotalAlunosDificuldadeTop1, string NomeDificuldadeTop2, int TotalAlunosDificuldadeTop2, int TotalAlunosDificuldadeOutras);
    }
}
