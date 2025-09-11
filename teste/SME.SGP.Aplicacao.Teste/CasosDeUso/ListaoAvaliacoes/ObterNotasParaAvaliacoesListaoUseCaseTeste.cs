using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ListaoAvaliacoes
{
    public class ObterNotasParaAvaliacoesListaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IConsultasDisciplina> consultasDisciplina;
        private readonly Mock<IConsultasPeriodoFechamento> consultasPeriodoFechamento;
        private readonly ObterNotasParaAvaliacoesUseCase useCase;

        public ObterNotasParaAvaliacoesListaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            consultasDisciplina = new Mock<IConsultasDisciplina>();
            consultasPeriodoFechamento = new Mock<IConsultasPeriodoFechamento>();
            useCase = new ObterNotasParaAvaliacoesUseCase(mediator.Object, consultasDisciplina.Object, consultasPeriodoFechamento.Object);
        }

        [Fact(DisplayName = "ObterNotasParaAvaliacoesListaoUseCase - Não permite editar nota de avaliação com data inferior a data de matrícula do aluno")]
        public async Task NaoPermiteEditarNotaAvaliacaoComDataInferiorDataMatriculaAluno()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 01);
            var periodoFim = new DateTime(anoAtual, 04, 30);

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1", AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            consultasDisciplina.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma("1", true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            mediator.Setup(x => x.Send(It.Is<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(y => y.ComponentesCurriculares.Single() == "1" &&
                                                                                                    y.TurmaCodigo == "1" &&
                                                                                                    y.PeriodoInicio == periodoInicio &&
                                                                                                    y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>() {
                    new() { Id = 1, DataAvaliacao = new DateTime(anoAtual, 03, 07) },
                    new() { Id = 2, DataAvaliacao = new DateTime(anoAtual, 04, 24) } });

            mediator.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>()
                {
                    new() { CodigoAluno = "1", DataMatricula = new DateTime(anoAtual, 03, 15), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo }
                });

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(y => y.Ids.Single() == 1 &&
                                                                                                        y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao());

            mediator.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y => y.Tipo == TipoParametroSistema.MediaBimestre && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5");

            mediator.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y => y.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("50");

            mediator.Setup(x => x.Send(It.Is<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(y => y.DataAvaliacao == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            mediator.Setup(x => x.Send(It.Is<ObterNotasPorAlunosAtividadesAvaliativasQuery>(y => y.AtividadesAvaliativasId.Intersect(new long[] { 1, 2 }).Count() == 2 &&
                                                                                                 y.AlunosId.Single() == "1" &&
                                                                                                 y.ComponenteCurricularId == "1" &&
                                                                                                 y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            mediator.Setup(x => x.Send(It.Is<ObterAusenciasDaAtividadesAvaliativasQuery>(y => y.TurmaCodigo == "1" && y.ComponenteCurricularCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            var fechamentos = new List<FechamentoPorTurmaPeriodoCCDto>() { new() };
            fechamentos[0].FechamentoAlunos.Add(new() { AlunoCodigo = "1" });
            fechamentos[0].FechamentoAlunos[0].FechamentoNotas.Add(new FechamentoNotaPorTurmaPeriodoCCDto() { Id = 1 });

            mediator.Setup(x => x.Send(It.Is<ObterFechamentosPorTurmaPeriodoCCQuery>(y => y.PeriodoEscolarId == 1 &&
                                                                                          y.TurmaId == 1 &&
                                                                                          y.ComponenteCurricularId == 1 &&
                                                                                          !y.EhRegencia), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentos);

            mediator.Setup(x => x.Send(It.Is<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(y => y.IdsFechamentoNota.Single() == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            mediator.Setup(x => x.Send(It.Is<ObterFrequenciasPorAlunosTurmaCCDataQuery>(y => y.AlunosCodigo.Single() == "1" &&
                                                                                             y.DataReferencia == periodoFim &&
                                                                                             y.TipoFrequencia == TipoFrequenciaAluno.PorDisciplina &&
                                                                                             y.TurmaCodigo == "1" &&
                                                                                             y.ComponenteCurriularId == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>() { new() { CodigoAluno = "1" } });

            consultasPeriodoFechamento.Setup(x => x.TurmaEmPeriodoDeFechamentoVigente(It.IsAny<Turma>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new PeriodoFechamentoVigenteDto());

            mediator.Setup(x => x.Send(It.Is<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(y => y.CodigoEstudante.Single() == "1" && y.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            mediator.Setup(x => x.Send(It.Is<ObterAlunosAtivosTurmaProgramaPapEolQuery>(y => y.AnoLetivo == anoAtual && y.AlunosCodigos.Single() == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AlunosTurmaProgramaPapDto>());

            var resultado = await useCase.Executar(new ListaNotasConceitosDto()
            {
                AnoLetivo = anoAtual,
                Bimestre = 1,
                DisciplinaCodigo = 1,
                Modalidade = Modalidade.Fundamental,
                TurmaCodigo = "1",
                TurmaId = 1,
                PeriodoInicioTicks = periodoInicio.Ticks,
                PeriodoFimTicks = periodoFim.Ticks,
                PeriodoEscolarId = 1
            });

            Assert.NotNull(resultado);
            Assert.Single(resultado.Bimestres);
            Assert.Single(resultado.Bimestres.Single().Alunos);
            Assert.Equal(2, resultado.Bimestres.Single().Alunos.Single().NotasAvaliacoes.Count);
            Assert.False(resultado.Bimestres.Single().Alunos.Single().NotasAvaliacoes.Single(na => na.AtividadeAvaliativaId == 1).PodeEditar);
            Assert.True(resultado.Bimestres.Single().Alunos.Single().NotasAvaliacoes.Single(na => na.AtividadeAvaliativaId == 2).PodeEditar);
        }

        [Fact(DisplayName = "ObterNotasParaAvaliacoesListaoUseCaseTeste - Deve desconsiderar alunos com vinculo indevido")]
        public async Task DeveDesconsiderarAlunosComVinculoIndevido()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 01);
            var periodoFim = new DateTime(anoAtual, 04, 30);

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1", AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            consultasDisciplina.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma("1", true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            mediator.Setup(x => x.Send(It.Is<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(y => y.ComponentesCurriculares.Single() == "1" &&
                                                                                                    y.TurmaCodigo == "1" &&
                                                                                                    y.PeriodoInicio == periodoInicio &&
                                                                                                    y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>() {
                    new() { Id = 1, DataAvaliacao = new DateTime(anoAtual, 03, 07) },
                    new() { Id = 2, DataAvaliacao = new DateTime(anoAtual, 04, 24) } });

            mediator.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>()
                {
                    new() { CodigoAluno = "1", DataMatricula = new DateTime(anoAtual, 03, 15), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 1 },
                    new() { CodigoAluno = "2", DataMatricula = new DateTime(anoAtual, 01, 10), CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido, NumeroAlunoChamada = 2 },
                    new() { CodigoAluno = "3", DataMatricula = new DateTime(anoAtual - 1, 11, 01), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 3 }
                });

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(y => y.Ids.Single() == 1 &&
                                                                                                        y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao());

            mediator.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y => y.Tipo == TipoParametroSistema.MediaBimestre && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5");

            mediator.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y => y.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("50");

            mediator.Setup(x => x.Send(It.Is<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(y => y.DataAvaliacao == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            mediator.Setup(x => x.Send(It.Is<ObterNotasPorAlunosAtividadesAvaliativasQuery>(y => y.AtividadesAvaliativasId.Intersect(new long[] { 1, 2 }).Count() == 2 &&
                                                                                                 y.AlunosId.Intersect(new string[] { "1", "3" }).Count() == 2 &&
                                                                                                 y.ComponenteCurricularId == "1" &&
                                                                                                 y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            mediator.Setup(x => x.Send(It.Is<ObterAusenciasDaAtividadesAvaliativasQuery>(y => y.TurmaCodigo == "1" && y.ComponenteCurricularCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            var fechamentos = new List<FechamentoPorTurmaPeriodoCCDto>() { new() };
            fechamentos[0].FechamentoAlunos.Add(new() { AlunoCodigo = "1" });
            fechamentos[0].FechamentoAlunos[0].FechamentoNotas.Add(new FechamentoNotaPorTurmaPeriodoCCDto() { Id = 1 });

            mediator.Setup(x => x.Send(It.Is<ObterFechamentosPorTurmaPeriodoCCQuery>(y => y.PeriodoEscolarId == 1 &&
                                                                                          y.TurmaId == 1 &&
                                                                                          y.ComponenteCurricularId == 1 &&
                                                                                          !y.EhRegencia), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentos);

            mediator.Setup(x => x.Send(It.Is<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(y => y.IdsFechamentoNota.Single() == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            mediator.Setup(x => x.Send(It.Is<ObterFrequenciasPorAlunosTurmaCCDataQuery>(y => y.AlunosCodigo.Intersect(new string[] { "1", "3" }).Count() == 1 &&
                                                                                             y.DataReferencia == periodoFim &&
                                                                                             y.TipoFrequencia == TipoFrequenciaAluno.PorDisciplina &&
                                                                                             y.TurmaCodigo == "1" &&
                                                                                             y.ComponenteCurriularId == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>() { new() { CodigoAluno = "1" } });

            consultasPeriodoFechamento.Setup(x => x.TurmaEmPeriodoDeFechamentoVigente(It.IsAny<Turma>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new PeriodoFechamentoVigenteDto());

            mediator.Setup(x => x.Send(It.Is<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(y => y.CodigoEstudante.Intersect(new string[] { "1", "3" }).Count() == 2 && y.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            mediator.Setup(x => x.Send(It.Is<ObterAlunosAtivosTurmaProgramaPapEolQuery>(y => y.AnoLetivo == anoAtual && y.AlunosCodigos.Intersect(new string[] { "1", "3" }).Count() == 2), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AlunosTurmaProgramaPapDto>());

            var resultado = await useCase.Executar(new ListaNotasConceitosDto()
            {
                AnoLetivo = anoAtual,
                Bimestre = 1,
                DisciplinaCodigo = 1,
                Modalidade = Modalidade.Fundamental,
                TurmaCodigo = "1",
                TurmaId = 1,
                PeriodoInicioTicks = periodoInicio.Ticks,
                PeriodoFimTicks = periodoFim.Ticks,
                PeriodoEscolarId = 1
            });

            Assert.NotNull(resultado);
            Assert.Single(resultado.Bimestres);
            Assert.Equal(2, resultado.Bimestres.Single().Alunos.Count);
            Assert.Single(resultado.Bimestres.Single().Alunos.Where(a => a.NumeroChamada == 1));
            Assert.Empty(resultado.Bimestres.Single().Alunos.Where(a => a.NumeroChamada == 2));
            Assert.Single(resultado.Bimestres.Single().Alunos.Where(a => a.NumeroChamada == 3));
        }
    }
}
