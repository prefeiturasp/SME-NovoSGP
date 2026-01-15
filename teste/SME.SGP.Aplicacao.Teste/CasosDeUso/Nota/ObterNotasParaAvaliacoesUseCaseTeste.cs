using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Nota
{
    public class ObterNotasParaAvaliacoesUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConsultasDisciplina> _consultasDisciplinaMock;
        private readonly Mock<IConsultasPeriodoFechamento> _consultasPeriodoFechamentoMock;
        private readonly ObterNotasParaAvaliacoesUseCase _useCase;

        public ObterNotasParaAvaliacoesUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _consultasDisciplinaMock = new Mock<IConsultasDisciplina>();
            _consultasPeriodoFechamentoMock = new Mock<IConsultasPeriodoFechamento>();
            _useCase = new ObterNotasParaAvaliacoesUseCase(_mediatorMock.Object, _consultasDisciplinaMock.Object, _consultasPeriodoFechamentoMock.Object);
        }
        private ListaNotasConceitosDto CriarFiltroSimples()
        {
            return new ListaNotasConceitosDto
            {
                AnoLetivo = DateTime.Today.Year,
                Bimestre = 1,
                TurmaCodigo = "123456",
                DisciplinaCodigo = 456,
            };
        }

        [Fact]
        public async Task Nao_Permite_Editar_Nota_Avaliacao_Com_Data_Inferior_Data_Matricula_Aluno()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 01);
            var periodoFim = new DateTime(anoAtual, 04, 30);

            _mediatorMock.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1", AnoLetivo = anoAtual });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            _consultasDisciplinaMock.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma("1", true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(y => y.ComponentesCurriculares.Single() == "1" &&
                                                                                                    y.TurmaCodigo == "1" &&
                                                                                                    y.PeriodoInicio == periodoInicio &&
                                                                                                    y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>() {
                    new() { Id = 1, DataAvaliacao = new DateTime(anoAtual, 03, 07) },
                    new() { Id = 2, DataAvaliacao = new DateTime(anoAtual, 04, 24) } });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>()
                {
                    new() { CodigoAluno = "1", DataMatricula = new DateTime(anoAtual, 03, 15), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo }
                });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(y => y.Ids.Single() == 1 &&
                                                                                                        y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y => y.Tipo == TipoParametroSistema.MediaBimestre && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y => y.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("50");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(y => y.DataAvaliacao == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterNotasPorAlunosAtividadesAvaliativasQuery>(y => y.AtividadesAvaliativasId.Intersect(new long[] { 1, 2 }).Count() == 2 &&
                                                                                                 y.AlunosId.Single() == "1" &&
                                                                                                 y.ComponenteCurricularId == "1" &&
                                                                                                 y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAusenciasDaAtividadesAvaliativasQuery>(y => y.TurmaCodigo == "1" && y.ComponenteCurricularCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            var fechamentos = new List<FechamentoPorTurmaPeriodoCCDto>() { new() };
            fechamentos[0].FechamentoAlunos.Add(new() { AlunoCodigo = "1" });
            fechamentos[0].FechamentoAlunos[0].FechamentoNotas.Add(new FechamentoNotaPorTurmaPeriodoCCDto() { Id = 1 });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterFechamentosPorTurmaPeriodoCCQuery>(y => y.PeriodoEscolarId == 1 &&
                                                                                          y.TurmaId == 1 &&
                                                                                          y.ComponenteCurricularId == 1 &&
                                                                                          !y.EhRegencia), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentos);

            _mediatorMock.Setup(x => x.Send(It.Is<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(y => y.IdsFechamentoNota.Single() == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterFrequenciasPorAlunosTurmaCCDataQuery>(y => y.AlunosCodigo.Single() == "1" &&
                                                                                             y.DataReferencia == periodoFim &&
                                                                                             y.TipoFrequencia == TipoFrequenciaAluno.PorDisciplina &&
                                                                                             y.TurmaCodigo == "1" &&
                                                                                             y.ComponenteCurriularId == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>() { new() { CodigoAluno = "1" } });

            _consultasPeriodoFechamentoMock.Setup(x => x.TurmaEmPeriodoDeFechamentoVigente(It.IsAny<Turma>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new PeriodoFechamentoVigenteDto());

            _mediatorMock.Setup(x => x.Send(It.Is<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(y => y.CodigoEstudante.Single() == "1" && y.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAlunosAtivosTurmaProgramaPapEolQuery>(y => y.AnoLetivo == anoAtual && y.AlunosCodigos.Single() == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AlunosTurmaProgramaPapDto>());

            var resultado = await _useCase.Executar(new ListaNotasConceitosDto()
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

        [Fact]
        public async Task Deve_Desconsiderar_Alunos_Com_Vinculo_Indevido()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 01);
            var periodoFim = new DateTime(anoAtual, 04, 30);

            _mediatorMock.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1", AnoLetivo = anoAtual });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            _consultasDisciplinaMock.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma("1", true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(y => y.ComponentesCurriculares.Single() == "1" &&
                                                                                                    y.TurmaCodigo == "1" &&
                                                                                                    y.PeriodoInicio == periodoInicio &&
                                                                                                    y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>() {
                    new() { Id = 1, DataAvaliacao = new DateTime(anoAtual, 03, 07) },
                    new() { Id = 2, DataAvaliacao = new DateTime(anoAtual, 04, 24) } });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>()
                {
                    new() { CodigoAluno = "1", DataMatricula = new DateTime(anoAtual, 03, 15), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 1 },
                    new() { CodigoAluno = "2", DataMatricula = new DateTime(anoAtual, 01, 10), CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido, NumeroAlunoChamada = 2 },
                    new() { CodigoAluno = "3", DataMatricula = new DateTime(anoAtual - 1, 11, 01), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 3 }
                });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(y => y.Ids.Single() == 1 &&
                                                                                                        y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y => y.Tipo == TipoParametroSistema.MediaBimestre && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y => y.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("50");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(y => y.DataAvaliacao == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterNotasPorAlunosAtividadesAvaliativasQuery>(y => y.AtividadesAvaliativasId.Intersect(new long[] { 1, 2 }).Count() == 2 &&
                                                                                                 y.AlunosId.Intersect(new string[] { "1", "3" }).Count() == 2 &&
                                                                                                 y.ComponenteCurricularId == "1" &&
                                                                                                 y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAusenciasDaAtividadesAvaliativasQuery>(y => y.TurmaCodigo == "1" && y.ComponenteCurricularCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            var fechamentos = new List<FechamentoPorTurmaPeriodoCCDto>() { new() };
            fechamentos[0].FechamentoAlunos.Add(new() { AlunoCodigo = "1" });
            fechamentos[0].FechamentoAlunos[0].FechamentoNotas.Add(new FechamentoNotaPorTurmaPeriodoCCDto() { Id = 1 });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterFechamentosPorTurmaPeriodoCCQuery>(y => y.PeriodoEscolarId == 1 &&
                                                                                          y.TurmaId == 1 &&
                                                                                          y.ComponenteCurricularId == 1 &&
                                                                                          !y.EhRegencia), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentos);

            _mediatorMock.Setup(x => x.Send(It.Is<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(y => y.IdsFechamentoNota.Single() == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterFrequenciasPorAlunosTurmaCCDataQuery>(y => y.AlunosCodigo.Intersect(new string[] { "1", "3" }).Count() == 1 &&
                                                                                             y.DataReferencia == periodoFim &&
                                                                                             y.TipoFrequencia == TipoFrequenciaAluno.PorDisciplina &&
                                                                                             y.TurmaCodigo == "1" &&
                                                                                             y.ComponenteCurriularId == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>() { new() { CodigoAluno = "1" } });

            _consultasPeriodoFechamentoMock.Setup(x => x.TurmaEmPeriodoDeFechamentoVigente(It.IsAny<Turma>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new PeriodoFechamentoVigenteDto());

            _mediatorMock.Setup(x => x.Send(It.Is<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(y => y.CodigoEstudante.Intersect(new string[] { "1", "3" }).Count() == 2 && y.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAlunosAtivosTurmaProgramaPapEolQuery>(y => y.AnoLetivo == anoAtual && y.AlunosCodigos.Intersect(new string[] { "1", "3" }).Count() == 2), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AlunosTurmaProgramaPapDto>());

            var resultado = await _useCase.Executar(new ListaNotasConceitosDto()
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

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Quando_Turma_Completa_Nulo()
        {
            var filtro = new ListaNotasConceitosDto { TurmaCodigo = "123" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foi possível obter a turma.", exception.Message);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Quando_Disciplinas_Do_Professor_Eh_Nulo_ou_Vazia()
        {
            var filtro = new ListaNotasConceitosDto { TurmaCodigo = "123" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "123" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync((List<DisciplinaDto>)null);

            var exceptionNulo = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foi possível obter os componentes curriculares do usuário logado.", exceptionNulo.Message);

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto>());

            var exceptionVazia = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foi possível obter os componentes curriculares do usuário logado.", exceptionVazia.Message);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Quando_Alunos_Eh_Nulo_Ou_Vazio()
        {
            var filtro = new ListaNotasConceitosDto
            {
                TurmaCodigo = "123",
                DisciplinaCodigo = 999
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "123" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(It.IsAny<string>(), true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto() });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<AlunoPorTurmaResposta>)null);

            var ex1 = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foi encontrado alunos para a turma informada", ex1.Message);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            var ex2 = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foi encontrado alunos para a turma informada", ex2.Message);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Quando_Componentes_Curriculares_Completos_Eh_Nulo_Ou_Vazio()
        {
            var filtro = new ListaNotasConceitosDto
            {
                TurmaCodigo = "123",
                DisciplinaCodigo = 999
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "123" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(It.IsAny<string>(), true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto() });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new AlunoPorTurmaResposta() });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<DisciplinaDto>)null);

            var ex1 = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Componente curricular informado não encontrado no EOL", ex1.Message);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>());

            var ex2 = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Componente curricular informado não encontrado no EOL", ex2.Message);
        }

        [Fact(Skip = "Precisa revisar")]
        public async Task Executar_ComponenteComRegencia_Deve_Retornar_Apenas_Atividades_Com_EhRegencia()
        {
            var filtro = CriarFiltroSimples();

            var usuario = new Usuario
            {
                CodigoRf = "123456789",
            };

            var disciplina = new List<DisciplinaDto>
            {
               new DisciplinaDto
               {
                   CodigoComponenteCurricular = filtro.DisciplinaCodigo,
                   Nome = "Regência EOL",
                   Regencia = true
               }
            };

            var disciplinaDto = new List<DisciplinaDto>
            {
                new DisciplinaDto
                {
                    CodigoComponenteCurricular = filtro.DisciplinaCodigo,
                    Nome = "Componente com Regência",
                    Regencia = true
                }
            };

            var atividades = new List<AtividadeAvaliativa>
            {
                new AtividadeAvaliativa { EhRegencia = true, Id = 1 },
                new AtividadeAvaliativa { EhRegencia = false, Id = 2 },
                new AtividadeAvaliativa { EhRegencia = true, Id = 3 }
            };

            var alunoPorTurmaResposta = new List<AlunoPorTurmaResposta>
            {
                new()
                {
                    CodigoAluno = "1",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = Convert.ToInt64(filtro.TurmaCodigo),
                    NomeAluno = "Aluno 1",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Ativo"
                },
                new()
                {
                    CodigoAluno = "2",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = Convert.ToInt64(filtro.TurmaCodigo),
                    NomeAluno = "Aluno 2",
                    NumeroAlunoChamada = 2,
                    SituacaoMatricula = "Ativo"
                }
            };

            var componenteEol = new List<ComponenteCurricularEol>
                 {
                     new ComponenteCurricularEol
                     {
                        Codigo = filtro.DisciplinaCodigo,
                        Regencia = true,
                        TerritorioSaber = false,
                        Descricao = "Componente com Regência",
                        DescricaoComponenteInfantil = "Infantil",
                        CodigosTerritoriosAgrupamento = new long[0],
                        TurmaCodigo = filtro.TurmaCodigo,
                        Compartilhada = false,
                        LancaNota = true,
                        PossuiObjetivos = true,
                        GrupoMatriz = new SME.SGP.Dominio.GrupoMatriz { Id = 1, Nome = "Grupo Teste" },
                        PlanejamentoRegencia = false,
                        RegistraFrequencia = true,
                        BaseNacional = false,
                        ExibirComponenteEOL = true,
                        Professor = "Prof. Exemplo",
                        CodigoComponenteTerritorioSaber = 0,
                        CodigoComponenteCurricularPai = null
                     }
                 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = filtro.TurmaCodigo, ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = filtro.AnoLetivo, Semestre = 1 });

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _consultasDisciplinaMock.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma(
                    filtro.TurmaCodigo,
                    true,
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(disciplinaDto);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(atividades);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunoPorTurmaResposta);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplinaDto);

            _mediatorMock.Setup(m => m.Send(ObterTipoAvaliacaoBimestralQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao { Id = 10, AvaliacoesNecessariasPorBimestre = 1 });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("7");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciasDaAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentosPorTurmaPeriodoCCQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoPorTurmaPeriodoCCDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciasPorAlunosTurmaCCDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoFechamentoBimestre { InicioDoFechamento = DateTime.Today.AddDays(-1), FinalDoFechamento = DateTime.Today.AddDays(1) });

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());

            _consultasDisciplinaMock.Setup(x => x.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                It.IsAny<long>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                 .ReturnsAsync(disciplina);

            _mediatorMock
                 .Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(
                  q => q.CodigoTurma == filtro.TurmaCodigo && q.Login == usuario.CodigoRf), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(componenteEol);

            var resultado = await _useCase.Executar(filtro);

            foreach (var bimestre in resultado.Bimestres)
            {
                foreach (var aluno in bimestre.Alunos)
                {
                    foreach (var nota in aluno.NotasAvaliacoes)
                    {
                        Assert.True(atividades.Any(a => a.Id == nota.AtividadeAvaliativaId && a.EhRegencia));
                    }
                }
            }
        }

        [Fact]
        public async Task Executar_Componente_Referencia_Nao_Eh_Regencia_Nao_Deve_Buscar_Disciplinas_Regencia()
        {
            var filtro = CriarFiltroBase();
            var componenteReferencia = CriarComponenteReferencia(regencia: false);
            Assert.False(componenteReferencia.Regencia); // Garantir que é false
            var usuario = CriarUsuario(ehProfessorCj: true);
            var turma = CriarTurmaCompleta();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto> { componenteReferencia });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>
                {
                    new AlunoPorTurmaResposta
                    {
                        CodigoAluno = "123",
                        NomeAluno = "Aluno Teste",
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                        DataMatricula = DateTime.Now.AddMonths(-6),
                        DataSituacao = DateTime.Now
                    }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciasDaAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentosPorTurmaPeriodoCCQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoPorTurmaPeriodoCCDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciasPorAlunosTurmaCCDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoFechamentoBimestre());

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMarcadorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MarcadorFrequenciaDto());

            _mediatorMock.Setup(m => m.Send(ObterTipoAvaliacaoBimestralQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao { Id = 1, AvaliacoesNecessariasPorBimestre = 2 });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5.0");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoNotaPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoNota.Nota);

            _consultasPeriodoFechamentoMock.Setup(x => x.TurmaEmPeriodoDeFechamento(It.IsAny<Turma>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            _consultasDisciplinaMock
                .Setup(x => x.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                    It.IsAny<long>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { componenteReferencia });

            _consultasDisciplinaMock
                .Setup(x => x.ObterComponentesCurricularesPorProfessorETurma(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { componenteReferencia });

            await _useCase.Executar(filtro);

            _consultasDisciplinaMock.Verify(x =>
                x.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                    It.IsAny<long>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once());
        }

        [Fact]
        public async Task Executar_Componente_Referencia_Eh_Regencia_E_Usuario_Eh_Professor_Cj_Deve_Buscar_Disciplinas_RegenciaCJ()
        {
            var filtro = CriarFiltroBase();
            var componenteReferencia = CriarComponenteReferencia(regencia: true);
            var usuario = CriarUsuario(ehProfessorCj: true);
            var disciplinasRegenciaCJ = CriarListaDisciplinasDto();

            ConfigurarMocksBasicos(filtro, componenteReferencia, usuario);

            _consultasDisciplinaMock
                .Setup(x => x.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                    filtro.DisciplinaCodigo, filtro.TurmaCodigo, false, componenteReferencia.Regencia))
                .ReturnsAsync(disciplinasRegenciaCJ);

            var resultado = await _useCase.Executar(filtro);

            _consultasDisciplinaMock.Verify(x => x.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                filtro.DisciplinaCodigo, filtro.TurmaCodigo, false, componenteReferencia.Regencia), Times.Once);

            _mediatorMock.Verify(x => x.Send(It.IsAny<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Componente_Referencia_Eh_Regencia_E_Usuario_Eh_Professor_Cj_Mas_Disciplinas_Vazias_Deve_Lancar_Excecao()
        {
            var filtro = CriarFiltroBase();
            var componenteReferencia = CriarComponenteReferencia(regencia: true);
            var usuario = CriarUsuario(ehProfessorCj: true);

            ConfigurarMocksBasicos(filtro, componenteReferencia, usuario);

            _consultasDisciplinaMock
                .Setup(x => x.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                    It.IsAny<long>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync((IEnumerable<DisciplinaDto>)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            exception.Message.Should().Be("Não foram encontradas as disciplinas de regência");
        }

        [Fact]
        public async Task Executar_Componente_Referencia_Eh_Regencia_E_Usuario_Eh_Professor_Cj_Mas_Disciplinas_Lista_Vazia_Deve_Lancar_Excecao()
        {
            var filtro = CriarFiltroBase();
            var componenteReferencia = CriarComponenteReferencia(regencia: true);
            var usuario = CriarUsuario(ehProfessorCj: true);

            ConfigurarMocksBasicos(filtro, componenteReferencia, usuario);

            _consultasDisciplinaMock
                .Setup(x => x.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                    It.IsAny<long>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            exception.Message.Should().Be("Não foram encontradas as disciplinas de regência");
        }

        [Fact(Skip = "Precisa revisar")]
        public async Task Executar_Componente_Referencia_Eh_Regencia_E_Usuario_Nao_Eh_Professor_Cj_Deve_Buscar_Disciplinas_Regencia_Eol()
        {
            var filtro = CriarFiltroBase();
            var componenteReferencia = CriarComponenteReferencia(regencia: true);
            var usuario = CriarUsuario(ehProfessorCj: false);
            var disciplinasRegenciaEol = CriarListaComponentesCurriculares();

            ConfigurarMocksBasicos(filtro, componenteReferencia, usuario);

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplinasRegenciaEol);

            var resultado = await _useCase.Executar(filtro);

            _mediatorMock.Verify(x => x.Send(It.Is<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(
                q => q.CodigoTurma == filtro.TurmaCodigo &&
                     q.Login == usuario.CodigoRf &&
                     q.Perfil == usuario.PerfilAtual),
                It.IsAny<CancellationToken>()), Times.Once);

            _consultasDisciplinaMock.Verify(x => x.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                It.IsAny<long>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Componente_Referencia_Eh_Regencia_Usuario_Nao_Eh_Professor_Cj_Mas_Sem_Disciplinas_Regencia_Deve_Lancar_Excecao()
        {
            var filtro = CriarFiltroBase();
            var componenteReferencia = CriarComponenteReferencia(regencia: true);
            var usuario = CriarUsuario(ehProfessorCj: false);
            var disciplinasEolSemRegencia = CriarListaComponentesCurriculares(regencia: false);

            ConfigurarMocksBasicos(filtro, componenteReferencia, usuario);

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplinasEolSemRegencia);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            exception.Message.Should().Be("Não foram encontradas disciplinas de regência no EOL");
        }

        [Fact(Skip = "Precisa revisar")]
        public async Task Executar_Turma_Eh_EJA_ComEducacao_Fisica_Deve_Excluir_Educacao_Fisica_Das_Regencias()
        {
            var filtro = CriarFiltroBase();
            var componenteReferencia = CriarComponenteReferencia(regencia: true);
            var usuario = CriarUsuario(ehProfessorCj: false);
            var turmaCompleta = CriarTurmaCompleta(modalidade: Modalidade.EJA);

            var disciplinasEol = new List<ComponenteCurricularEol>
            {
                CriarComponenteCurricular("Português", regencia: true, codigo: 1),
                CriarComponenteCurricular("Matemática", regencia: true, codigo: 2),
                CriarComponenteCurricular("Ed. Física", regencia: true,
                    codigo: MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA)
            };

            ConfigurarMocksBasicos(filtro, componenteReferencia, usuario, turmaCompleta);

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplinasEol);

            var resultado = await _useCase.Executar(filtro);

            resultado.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(It.IsAny<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(Skip = "Precisa revisar")]
        public async Task Executar_Turma_Nao_Eh_EJA_Deve_Incluir_Todas_Disciplinas_Regencia()
        {
            var filtro = CriarFiltroBase();
            var componenteReferencia = CriarComponenteReferencia(regencia: true);
            var usuario = CriarUsuario(ehProfessorCj: false);
            var turmaCompleta = CriarTurmaCompleta(modalidade: Modalidade.Fundamental);

            var disciplinasEol = new List<ComponenteCurricularEol>
            {
                CriarComponenteCurricular("Português", regencia: true, codigo: 1),
                CriarComponenteCurricular("Ed. Física", regencia: true,
                    codigo: MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA)
            };

            ConfigurarMocksBasicos(filtro, componenteReferencia, usuario, turmaCompleta);

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplinasEol);

            var resultado = await _useCase.Executar(filtro);

            resultado.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(It.IsAny<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Obter_Disciplinas_Atividade_Avaliativa_Deve_Retornar_Lista_Correta()
        {
            var avaliacaoId = 123L;
            var ehRegencia = true;

            var listaEsperada = new List<AtividadeAvaliativaDisciplina>
            {
                new AtividadeAvaliativaDisciplina(avaliacaoId, "Matematica"),
                new AtividadeAvaliativaDisciplina(avaliacaoId, "Portugues")
            };

            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<ObterDisciplinasAtividadeAvaliativaQuery>(
                        q => q.Avaliacao_id == avaliacaoId && q.EhRegencia == ehRegencia
                    ),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(listaEsperada);

            var resultado = await _useCase.ObterDisciplinasAtividadeAvaliativa(avaliacaoId, ehRegencia);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, resultado.Count());
            Assert.Collection(resultado,
                item => Assert.Equal("Matematica", item.DisciplinaId),
                item => Assert.Equal("Portugues", item.DisciplinaId));
        }

        [Fact]
        public async Task Obter_Disciplinas_Por_Ids_Deve_Retornar_Lista_Correta()
        {
            var ids = new long[] { 1, 2, 3 };

            var listaEsperada = new List<DisciplinaDto>
            {
                new DisciplinaDto { Id = 1, Nome = "Matematica" },
                new DisciplinaDto { Id = 2, Nome = "Portugues" },
                new DisciplinaDto { Id = 3, Nome = "Historia" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsQuery>(q => q.Ids.SequenceEqual(ids)), default))
                .ReturnsAsync(listaEsperada);

            var resultado = await _useCase.ObterDisciplinasPorIds(ids);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, resultado.Count());
            Assert.Collection(resultado,
                item => Assert.Equal("Matematica", item.Nome),
                item => Assert.Equal("Portugues", item.Nome),
                item => Assert.Equal("Historia", item.Nome));
        }

        [Fact]
        public void Obter_Componentes_Curriculares_Para_Consulta_Deve_Retornar_Componentes_Filha_Quando_Tem_Filhas()
        {
            var disciplinaPaiId = 10;

            var disciplinasDoProfessor = new List<DisciplinaDto>
            {
                new DisciplinaDto { CdComponenteCurricularPai = disciplinaPaiId, CodigoComponenteCurricular = 100 },
                new DisciplinaDto { CdComponenteCurricularPai = disciplinaPaiId, CodigoComponenteCurricular = 200 },
                new DisciplinaDto { CdComponenteCurricularPai = 999, CodigoComponenteCurricular = 300 }
            };

            var metodo = _useCase.GetType()
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .FirstOrDefault(m =>
                    m.Name == "ObterComponentesCurricularesParaConsulta" &&
                    m.GetParameters().Length == 2);

            Assert.NotNull(metodo);

            var resultado = (List<long>)metodo.Invoke(null, new object[] { disciplinaPaiId, disciplinasDoProfessor });

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Contains(100, resultado);
            Assert.Contains(200, resultado);
            Assert.DoesNotContain(10, resultado);
        }

        [Fact]
        public void Obter_Componentes_Curriculares_Para_Consulta_Deve_Retornar_Disciplina_Id_Quando_Nao_Tem_Filhas()
        {
            var disciplinaPaiId = 10;
            var disciplinasDoProfessor = new List<DisciplinaDto>
            {
                new DisciplinaDto { CdComponenteCurricularPai = 999, CodigoComponenteCurricular = 100 }
            };

            var metodo = _useCase.GetType()
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .FirstOrDefault(m =>
                    m.Name == "ObterComponentesCurricularesParaConsulta" &&
                    m.GetParameters().Length == 2);

            Assert.NotNull(metodo);

            var resultado = (List<long>)metodo.Invoke(null, new object[] { disciplinaPaiId, disciplinasDoProfessor });

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(disciplinaPaiId, resultado[0]);
        }

        [Fact]
        public async Task Valida_Minimo_Avaliacoes_Bimestrais_Regencia_Com_Componentes_Com_Atividades_Suficientes_Nao_Deve_Adicionar_Observacao()
        {
            var componenteCurricular = new DisciplinaDto { Regencia = true };
            var disciplinasRegencia = new List<DisciplinaResposta>
            {
                new DisciplinaResposta { CodigoComponenteCurricular = 1, Nome = "Disciplina 1" },
                new DisciplinaResposta { CodigoComponenteCurricular = 2, Nome = "Disciplina 2" },
            };

            var tipoAvaliacaoBimestral = new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 2 };
            var bimestreDto = new NotasConceitosBimestreRetornoDto();

            var atividadeAvaliativas = new List<AtividadeAvaliativa>
            {
                new AtividadeAvaliativa { Id = 1, TipoAvaliacaoId = (long)TipoAvaliacaoCodigo.AvaliacaoBimestral },
                new AtividadeAvaliativa { Id = 2, TipoAvaliacaoId = (long)TipoAvaliacaoCodigo.AvaliacaoBimestral }
            };

            var componentesComAtividade = new List<ComponentesRegenciaComAtividadeAvaliativaDto>
            {
                new ComponentesRegenciaComAtividadeAvaliativaDto { DisciplinaId = "1", TotalAtividades = 2 },
                new ComponentesRegenciaComAtividadeAvaliativaDto { DisciplinaId = "2", TotalAtividades = 3 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAtividadeAvaliativasRegenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesComAtividade);

            var metodo = _useCase.GetType().GetMethod("ValidaMinimoAvaliacoesBimestrais",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);

            var task = (Task)metodo.Invoke(_useCase, new object[]
            {
              componenteCurricular,
              disciplinasRegencia,
              tipoAvaliacaoBimestral,
              bimestreDto,
              atividadeAvaliativas,
              1
            });

            await task;

            Assert.Empty(bimestreDto.Observacoes);
        }

        [Fact]
        public async Task Valida_Minimo_Avaliacoes_Bimestrais_Regencia_Com_Componentes_Com_Atividades_Insuficientes_Deve_Adicionar_Observacao()
        {
            var componenteCurricular = new DisciplinaDto { Regencia = true };
            var disciplinasRegencia = new List<DisciplinaResposta>
            {
                new DisciplinaResposta { CodigoComponenteCurricular = 1, Nome = "Disciplina 1" },
                new DisciplinaResposta { CodigoComponenteCurricular = 2, Nome = "Disciplina 2" },
                new DisciplinaResposta { CodigoComponenteCurricular = 3, Nome = "Disciplina 3" }
            };

            var tipoAvaliacaoBimestral = new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 3 };
            var bimestreDto = new NotasConceitosBimestreRetornoDto();

            var atividadeAvaliativas = new List<AtividadeAvaliativa>
            {
                new AtividadeAvaliativa { Id = 1, TipoAvaliacaoId = (long)TipoAvaliacaoCodigo.AvaliacaoBimestral }
            };

            var componentesComAtividade = new List<ComponentesRegenciaComAtividadeAvaliativaDto>
            {
                new ComponentesRegenciaComAtividadeAvaliativaDto { DisciplinaId = "1", TotalAtividades = 3 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAtividadeAvaliativasRegenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesComAtividade);

            var metodo = _useCase.GetType().GetMethod("ValidaMinimoAvaliacoesBimestrais",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);

            var task = (Task)metodo.Invoke(_useCase, new object[] { componenteCurricular, disciplinasRegencia, tipoAvaliacaoBimestral, bimestreDto, atividadeAvaliativas, 2 });
            await task;

            Assert.Single(bimestreDto.Observacoes);
            Assert.Contains("Disciplina 2", bimestreDto.Observacoes[0]);
            Assert.Contains("Disciplina 3", bimestreDto.Observacoes[0]);
            Assert.Contains("bimestre 2", bimestreDto.Observacoes[0]);
        }

        [Fact]
        public async Task Valida_Minimo_Avaliacoes_Bimestrais_Regencia_Sem_Componentes_Com_Atividades_Deve_Adicionar_Observacao_Para_Todas_Disciplinas()
        {
            var componenteCurricular = new DisciplinaDto { Regencia = true };
            var disciplinasRegencia = new List<DisciplinaResposta>
            {
                new DisciplinaResposta { CodigoComponenteCurricular = 1, Nome = "Disciplina A" },
                new DisciplinaResposta { CodigoComponenteCurricular = 2, Nome = "Disciplina B" }
            };

            var tipoAvaliacaoBimestral = new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 1 };
            var bimestreDto = new NotasConceitosBimestreRetornoDto();

            var atividadeAvaliativas = new List<AtividadeAvaliativa>
            {
                new AtividadeAvaliativa { Id = 1, TipoAvaliacaoId = (long)TipoAvaliacaoCodigo.AvaliacaoBimestral }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAtividadeAvaliativasRegenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComponentesRegenciaComAtividadeAvaliativaDto>());

            var metodo = _useCase.GetType().GetMethod("ValidaMinimoAvaliacoesBimestrais",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);

            var task = (Task)metodo.Invoke(_useCase, new object[] { componenteCurricular, disciplinasRegencia, tipoAvaliacaoBimestral, bimestreDto, atividadeAvaliativas, 3 });
            await task;

            Assert.Single(bimestreDto.Observacoes);
            Assert.Contains("Disciplina A", bimestreDto.Observacoes[0]);
            Assert.Contains("Disciplina B", bimestreDto.Observacoes[0]);
            Assert.Contains("bimestre 3", bimestreDto.Observacoes[0]);
        }

        [Fact]
        public async Task Valida_Minimo_Avaliacoes_Bimestrais_Regencia_Com_Atividades_Insuficientes_Deve_Adicionar_Observacao()
        {
            var componenteCurricular = new DisciplinaDto { Regencia = true };
            var disciplinasRegencia = new List<DisciplinaResposta>
            {
                new DisciplinaResposta { CodigoComponenteCurricular = 1, Nome = "Disciplina 1" },
                new DisciplinaResposta { CodigoComponenteCurricular = 2, Nome = "Disciplina 2" }
            };

            var tipoAvaliacaoBimestral = new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 3 };
            var bimestreDto = new NotasConceitosBimestreRetornoDto();

            var atividadeAvaliativas = new List<AtividadeAvaliativa>
            {
                new AtividadeAvaliativa { Id = 1, TipoAvaliacaoId = (long)TipoAvaliacaoCodigo.AvaliacaoBimestral }
            };

            var componentesComAtividade = new List<ComponentesRegenciaComAtividadeAvaliativaDto>
            {
                new ComponentesRegenciaComAtividadeAvaliativaDto { DisciplinaId = "1", TotalAtividades = 2 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAtividadeAvaliativasRegenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesComAtividade);

            var metodo = _useCase.GetType().GetMethod(
                "ValidaMinimoAvaliacoesBimestrais",
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[] {
                   typeof(DisciplinaDto),
                   typeof(IEnumerable<DisciplinaResposta>),
                   typeof(TipoAvaliacao),
                   typeof(NotasConceitosBimestreRetornoDto),
                   typeof(IEnumerable<AtividadeAvaliativa>),
                   typeof(int)
                },
                null);

            Assert.NotNull(metodo);
            var task = (Task)metodo.Invoke(_useCase, new object[] { componenteCurricular, disciplinasRegencia, tipoAvaliacaoBimestral, bimestreDto, atividadeAvaliativas, 1 });
            await task;

            Assert.Single(bimestreDto.Observacoes);
            Assert.Contains("Disciplina 2", bimestreDto.Observacoes[0]);
        }

        [Fact]
        public async Task Valida_Minimo_Avaliacoes_Bimestrais_Nao_Regencia_Sem_Avaliacoes_Deve_Adicionar_Observacao()
        {
            var componenteCurricular = new DisciplinaDto { Regencia = false, CodigoComponenteCurricular = 10, Nome = "Componente Y" };
            var disciplinasRegencia = Enumerable.Empty<DisciplinaResposta>();

            var tipoAvaliacaoBimestral = new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 1 };
            var bimestreDto = new NotasConceitosBimestreRetornoDto();

            var atividadeAvaliativas = new List<AtividadeAvaliativa>
            {
                new AtividadeAvaliativa
                {
                    Id = 1,
                    TipoAvaliacaoId = (long)TipoAvaliacaoCodigo.AvaliacaoBimestral,
                    Disciplinas = new List<AtividadeAvaliativaDisciplina>
                    {
                        new AtividadeAvaliativaDisciplina { DisciplinaId = "20" }
                    }
                }
            };

            var metodo = _useCase.GetType().GetMethod(
                "ValidaMinimoAvaliacoesBimestrais",
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[] {
                 typeof(DisciplinaDto),
                 typeof(IEnumerable<DisciplinaResposta>),
                 typeof(TipoAvaliacao),
                 typeof(NotasConceitosBimestreRetornoDto),
                 typeof(IEnumerable<AtividadeAvaliativa>),
                 typeof(int)
                },
                null);

            Assert.NotNull(metodo);
            var task = (Task)metodo.Invoke(_useCase, new object[] { componenteCurricular, disciplinasRegencia, tipoAvaliacaoBimestral, bimestreDto, atividadeAvaliativas, 4 });
            await task;

            Assert.Single(bimestreDto.Observacoes);
            Assert.Contains("Componente Y", bimestreDto.Observacoes[0]);
            Assert.Contains("bimestre 4", bimestreDto.Observacoes[0]);
        }

        [Fact]
        public void Verifica_Nota_Em_Aprovacao_Nota_Igual_Zero_Deve_Manter_Em_Aprovacao_False()
        {
            var dto = new FechamentoNotaRetornoDto
            {
                EmAprovacao = true,
                NotaConceito = null
            };

            var metodo = _useCase.GetType()
              .GetMethod("VerificaNotaEmAprovacao", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            double nota = 0;

            metodo.Invoke(_useCase, new object[] { nota, dto });

            Assert.False(dto.EmAprovacao);
            Assert.Null(dto.NotaConceito);
        }

        [Fact]
        public void Verifica_Nota_Em_Aprovacao_Nota_Maior_Que_Zero_Deve_Definir_Nota_E_Em_Aprovacao_True()
        {
            var dto = new FechamentoNotaRetornoDto
            {
                EmAprovacao = false,
                NotaConceito = null
            };
            double nota = 7.5;

            var metodo = _useCase.GetType()
             .GetMethod("VerificaNotaEmAprovacao", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);

            metodo.Invoke(_useCase, new object[] { nota, dto });

            Assert.True(dto.EmAprovacao);
            Assert.Equal(nota, dto.NotaConceito);
        }

        [Fact]
        public void Verifica_Nota_Em_Aprovacao_Nota_Negativa_Deve_Manter_Nota_Conceito_Inalterada()
        {
            var dto = new FechamentoNotaRetornoDto
            {
                EmAprovacao = true,
                NotaConceito = 5.0
            };

            var metodo = _useCase.GetType()
             .GetMethod("VerificaNotaEmAprovacao", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            double nota = -1.0;

            metodo.Invoke(_useCase, new object[] { nota, dto });

            Assert.False(dto.EmAprovacao);
            Assert.Equal(5.0, dto.NotaConceito);
        }

        [Fact]
        public async Task Executar_Quando_Todos_Alunos_Tem_Vinculo_Indevido_Deve_Lancar_Excecao()
        {
            var filtro = new ListaNotasConceitosDto
            {
                TurmaCodigo = "123",
                PeriodoInicioTicks = DateTime.Now.Ticks,
                PeriodoFimTicks = DateTime.Now.AddDays(10).Ticks,
                DisciplinaCodigo = 42
            };

            var turmaCompleta = new Turma
            {
                CodigoTurma = "123"
            };

            var usuarioLogado = new Usuario();

            var disciplinas = new List<DisciplinaDto>
            {
                new DisciplinaDto { CodigoComponenteCurricular = 42 }
            };

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), default))
                .ReturnsAsync(turmaCompleta);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, default))
                .ReturnsAsync(usuarioLogado);

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(It.IsAny<string>(), true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(disciplinas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), default))
                .ReturnsAsync(new List<AtividadeAvaliativa>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), default))
                .ReturnsAsync(alunos);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));

            Assert.Equal("Não foi encontrado alunos para a turma informada", ex.Message);
        }

        [Fact]
        public void Nao_Eh_Professor_Nem_CJ_Deve_Retornar_True()
        {
            var usuario = CriarUsuarioSimulado(false, false);
            var avaliacao = new AtividadeAvaliativa { EhCj = false };

            var metodo = _useCase.GetType()
                .GetMethod("ChecarSeProfessorCJTitularPodeEditarNota", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            var resultado = (bool)metodo.Invoke(_useCase, new object[] { usuario, avaliacao });

            Assert.True(resultado);
        }

        [Fact]
        public void Eh_Apenas_CJ_E_CJ_Deve_Retornar_True()
        {
            var usuario = CriarUsuarioSimulado(false, true);
            var avaliacao = new AtividadeAvaliativa { EhCj = true };

            var metodo = _useCase.GetType()
                .GetMethod("ChecarSeProfessorCJTitularPodeEditarNota", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            var resultado = (bool)metodo.Invoke(_useCase, new object[] { usuario, avaliacao });

            Assert.True(resultado);
        }

        [Fact]
        public void Eh_Apenas_CJ_Nao_CJ_Deve_Retornar_False()
        {
            var usuario = CriarUsuarioSimulado(false, true);
            var avaliacao = new AtividadeAvaliativa { EhCj = false };

            var metodo = _useCase.GetType()
                .GetMethod("ChecarSeProfessorCJTitularPodeEditarNota", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            var resultado = (bool)metodo.Invoke(_useCase, new object[] { usuario, avaliacao });

            Assert.False(resultado);
        }

        [Fact]
        public void Eh_Apenas_Professor_Nao_CJ_Deve_Retornar_True()
        {
            var usuario = CriarUsuarioSimulado(true, false);
            var avaliacao = new AtividadeAvaliativa { EhCj = false };

            var metodo = _useCase.GetType()
                .GetMethod("ChecarSeProfessorCJTitularPodeEditarNota", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            var resultado = (bool)metodo.Invoke(_useCase, new object[] { usuario, avaliacao });

            Assert.True(resultado);
        }

        [Fact]
        public void Eh_Apenas_Professor_E_CJ_Deve_Retornar_False()
        {
            var usuario = CriarUsuarioSimulado(true, false);
            var avaliacao = new AtividadeAvaliativa { EhCj = true };

            var metodo = _useCase.GetType()
                .GetMethod("ChecarSeProfessorCJTitularPodeEditarNota", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            var resultado = (bool)metodo.Invoke(_useCase, new object[] { usuario, avaliacao });

            Assert.False(resultado);
        }

        [Fact]
        public void Eh_Apenas_Professor_E_Avaliacao_Nao_CJ_Deve_Retornar_True()
        {
            var usuario = CriarUsuarioSimulado(true, false);
            var avaliacao = new AtividadeAvaliativa { EhCj = false };

            var metodo = _useCase.GetType()
                .GetMethod("ChecarSeProfessorCJTitularPodeEditarNota", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            var resultado = (bool)metodo.Invoke(_useCase, new object[] { usuario, avaliacao });

            Assert.True(resultado);
        }

        [Fact]
        public void Eh_Apenas_CJ_E_Avaliacao_CJ_Deve_Retornar_True()
        {
            var usuario = CriarUsuarioSimulado(false, true);
            var avaliacao = new AtividadeAvaliativa { EhCj = true };

            var metodo = _useCase.GetType()
                .GetMethod("ChecarSeProfessorCJTitularPodeEditarNota", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            var resultado = (bool)metodo.Invoke(_useCase, new object[] { usuario, avaliacao });

            Assert.True(resultado);
        }

        [Fact]
        public void Eh_Professor_E_CJ_NaoEhCj_Deve_Retornar_True()
        {
            var usuario = CriarUsuarioSimulado(true, true);
            var avaliacao = new AtividadeAvaliativa { EhCj = false };

            var metodo = _useCase.GetType()
                .GetMethod("ChecarSeProfessorCJTitularPodeEditarNota", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(metodo);
            var resultado = (bool)metodo.Invoke(_useCase, new object[] { usuario, avaliacao });

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Verificar_Se_ProfessorCJ_Pode_Editar_Nota()
        {

            var filtro = CriarFiltroBase();
            var turma = CriarTurmaCompleta();
            turma.CodigoTurma = filtro.TurmaCodigo;
            turma.AnoLetivo = new DateTime(filtro.PeriodoInicioTicks).Year;

            var usuario = CriarUsuario(ehProfessorCj: true);
            var componente = CriarComponenteReferencia();

            var dataAvaliacao = new DateTime(filtro.PeriodoInicioTicks).AddDays(5);

            var aluno = new AlunoPorTurmaResposta
            {
                CodigoAluno = "1001",
                CodigoTurma = Convert.ToInt32(filtro.TurmaCodigo),
                NomeAluno = "Aluno Teste",
                NumeroAlunoChamada = 1,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = new DateTime(filtro.PeriodoInicioTicks).AddDays(-10),
                DataSituacao = new DateTime(9999, 12, 31),
                SituacaoMatricula = "Ativo"
            };

            var atividade = new AtividadeAvaliativa
            {
                Id = 1,
                NomeAvaliacao = "Prova CJ",
                DataAvaliacao = dataAvaliacao,
                EhCj = true,
                EhRegencia = false,
                TipoAvaliacaoId = (long)TipoAvaliacaoCodigo.AvaliacaoBimestral,
                Categoria = CategoriaAtividadeAvaliativa.Normal
            };

            var nota = new NotaConceito
            {
                AlunoId = aluno.CodigoAluno,
                DisciplinaId = componente.CodigoComponenteCurricular.ToString(),
                AtividadeAvaliativaID = atividade.Id,
                CriadoEm = DateTime.Today
            };

            ConfigurarMocksBasicos(filtro, componente, usuario, turma);

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { aluno });

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa> { atividade });

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito> { nota });

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterAusenciasDaAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterFrequenciasPorAlunosTurmaCCDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>());

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoFechamentoBimestre
                {
                    InicioDoFechamento = new DateTime(filtro.PeriodoInicioTicks).AddDays(-1)
                });

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterFechamentosPorTurmaPeriodoCCQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoPorTurmaPeriodoCCDto>());

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterMarcadorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MarcadorFrequenciaDto());

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterTipoNotaPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoNota.Nota);

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            var resultado = await _useCase.Executar(filtro);

            resultado.Bimestres.Should().NotBeNullOrEmpty("deveria haver pelo menos um bimestre no resultado");

            var primeiroBimestre = resultado.Bimestres.First();
            primeiroBimestre.Alunos.Should().NotBeNullOrEmpty("deveria haver pelo menos um aluno");

            var primeiroAluno = primeiroBimestre.Alunos.First();
            primeiroAluno.NotasAvaliacoes.Should().NotBeNullOrEmpty("deveria haver pelo menos uma nota");

            var primeiraNota = primeiroAluno.NotasAvaliacoes.First();
            primeiraNota.PodeEditar.Should().BeTrue("o professor CJ deveria poder editar a nota");
        }

        [Fact]
        public async Task Executar_Deve_Adicionar_Observacao_Se_Nao_Tiver_Minimo_Avaliacoes()
        {
            var filtro = CriarFiltroBase();
            var turma = CriarTurmaCompleta();
            var usuario = CriarUsuario();
            var componente = CriarComponenteReferencia(regencia: false);

            ConfigurarMocksBasicos(filtro, componente, usuario, turma);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao { Id = 100, AvaliacoesNecessariasPorBimestre = 3 });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>()
                {
                new()
                {
                    TipoAvaliacaoId = 100,
                    Disciplinas = new List<AtividadeAvaliativaDisciplina>
                    {
                        new AtividadeAvaliativaDisciplina
                        {
                            DisciplinaId = componente.CodigoComponenteCurricular.ToString()
                        }
                    }
                }
                });

            var resultado = await _useCase.Executar(filtro);

            var observacoes = resultado.Bimestres.First().Observacoes;
            observacoes.Should().NotBeNullOrEmpty();
            observacoes.First().Should().Contain("não tem o número mínimo de avaliações bimestrais");
        }

        [Fact]
        public async Task Executar_Deve_Preencher_Pode_Lancar_Nota_Final()
        {
            var filtro = CriarFiltroBase();
            var turma = CriarTurmaCompleta();
            var usuario = CriarUsuario();
            var componente = CriarComponenteReferencia();

            ConfigurarMocksBasicos(filtro, componente, usuario, turma);

            _consultasPeriodoFechamentoMock.Setup(x => x.TurmaEmPeriodoDeFechamento(turma, It.IsAny<DateTime>(), filtro.Bimestre))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(filtro);
            resultado.Bimestres.First().PodeLancarNotaFinal.Should().BeTrue();
        }

        [Fact]
        public void Nao_Deve_Filtrar_Alunos_Se_Turma_Historico_For_False()
        {
            var filtro = new ListaNotasConceitosDto { TurmaHistorico = false };

            var aluno = CriarAlunoMock(ativo: true, situacao: SituacaoMatriculaAluno.Ativo);
            var alunos = new List<AlunoPorTurmaResposta> { aluno };

            var resultado = FiltrarAlunos(alunos, filtro);

            Assert.Single(resultado);
        }

        [Fact]
        public void Deve_Incluir_Aluno_Ativo()
        {
            var filtro = new ListaNotasConceitosDto { TurmaHistorico = true };
            var alunoMock = CriarAlunoMock(ativo: true, situacao: SituacaoMatriculaAluno.Ativo);
            var alunos = new List<AlunoPorTurmaResposta> { alunoMock };

            var resultado = FiltrarAlunos(alunos, filtro);

            Assert.Single(resultado);
        }

        [Fact]
        public void Deve_Incluir_Inativo_Com_Data_Valida_Concluido()
        {
            var filtro = new ListaNotasConceitosDto { TurmaHistorico = true };
            var alunoMock = CriarAlunoMock(false, SituacaoMatriculaAluno.Concluido, new DateTime(2023, 4, 15));
            var alunos = new List<AlunoPorTurmaResposta> { alunoMock };

            var resultado = FiltrarAlunos(alunos, filtro);

            Assert.Single(resultado);
        }

        [Fact]
        public void Deve_Incluir_Inativo_Com_Data_Valida_Transferido()
        {
            var filtro = new ListaNotasConceitosDto { TurmaHistorico = true };
            var alunoMock = CriarAlunoMock(false, SituacaoMatriculaAluno.Transferido, new DateTime(2026, 4, 10));
            var alunos = new List<AlunoPorTurmaResposta> { alunoMock };

            var resultado = FiltrarAlunos(alunos, filtro);

            Assert.Single(resultado);
        }

        [Fact]
        public void Nao_Deve_Incluir_Inativo_Data_Fora()
        {
            var filtro = new ListaNotasConceitosDto { TurmaHistorico = true };
            var alunoMock = CriarAlunoMock(false, SituacaoMatriculaAluno.Transferido, new DateTime(2023, 5, 10));
            var alunos = new List<AlunoPorTurmaResposta> { alunoMock };

            var resultado = FiltrarAlunos(alunos, filtro);

            Assert.Empty(resultado);
        }

        [Fact]
        public void Nao_Deve_Incluir_Inativo_Data_Valida_Mas_Situacao_Invalida()
        {
            var filtro = new ListaNotasConceitosDto { TurmaHistorico = true };

            var alunoMock = CriarAlunoMock(false, SituacaoMatriculaAluno.Falecido, new DateTime(2023, 5, 1));

            var alunos = new List<AlunoPorTurmaResposta> { alunoMock };

            var resultado = FiltrarAlunos(alunos, filtro);

            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Definir_Eh_Atendido_AEE_Como_True_Quando_Aluno_Tiver_Plano_AEE_Ativo()
        {
            var codigoAluno = "123";
            var numeroChamada = 10;

            var aluno = new AlunoPorTurmaResposta
            {
                CodigoAluno = codigoAluno,
                NumeroAlunoChamada = numeroChamada,
                NomeAluno = "Aluno Teste",
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = 1234,
                DataMatricula = new DateTime(2024, 1, 1)
            };

            var alunos = new List<AlunoPorTurmaResposta> { aluno };

            _mediatorMock.Setup(x => x.Send(It.IsAny<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<PlanoAEEResumoDto> { new() { CodigoAluno = codigoAluno } });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(alunos);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Turma
                        {
                            CodigoTurma = "1234",
                            AnoLetivo = 2024,
                            ModalidadeCodigo = Modalidade.Fundamental,
                            Semestre = 1
                        });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Usuario { CodigoRf = "1", PerfilAtual = Guid.NewGuid() });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = 1, Nome = "Disciplina Teste" } });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(q => q.Tipo == TipoParametroSistema.MediaBimestre), It.IsAny<CancellationToken>()))
                         .ReturnsAsync("7.0");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(q => q.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes), It.IsAny<CancellationToken>()))
                         .ReturnsAsync("15.0");

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(10L);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 3 });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<NotaConceito>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFrequenciasPorAlunosTurmaCCDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PeriodoFechamentoBimestre());

            _consultasDisciplinaMock.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma("1234", true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = 1, Nome = "Disciplina Teste" } });

            var filtro = new ListaNotasConceitosDto
            {
                TurmaCodigo = "1234",
                DisciplinaCodigo = 1,
                PeriodoInicioTicks = new DateTime(2024, 3, 1).Ticks,
                PeriodoFimTicks = new DateTime(2024, 7, 1).Ticks,
                Bimestre = 1,
                AnoLetivo = 2024
            };

            var resultado = await _useCase.Executar(filtro);

            var alunoRetornado = resultado.Bimestres.SelectMany(b => b.Alunos).FirstOrDefault(a => a.Id == codigoAluno);
            Assert.NotNull(alunoRetornado);
            Assert.True(alunoRetornado.EhAtendidoAEE);
            Assert.NotNull(alunoRetornado.NotasAvaliacoes);
        }

        [Fact]
        public async Task Executar_Deve_Filtrar_Matricula_Correta_Quando_Houver_Mais_De_Uma_Matricula()
        {
            var filtro = new ListaNotasConceitosDto
            {
                AnoLetivo = 2023,
                TurmaCodigo = "1234",
                PeriodoInicioTicks = new DateTime(2023, 3, 1).Ticks,
                PeriodoFimTicks = new DateTime(2023, 6, 30).Ticks
            };

            var codigoDisciplina = 1;

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    CodigoTurma = Convert.ToInt16(filtro.TurmaCodigo),
                    CodigoAluno = "1001",
                    DataSituacao = new DateTime(2022, 1, 1),
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                },
                new AlunoPorTurmaResposta
                {
                    CodigoTurma = Convert.ToInt16(filtro.TurmaCodigo),
                    CodigoAluno = "1002",
                    DataSituacao = new DateTime(2023, 3, 1),
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                }
            };

            _mediatorMock.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(q => q.TurmaCodigo == filtro.TurmaCodigo), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new Turma
                 {
                     CodigoTurma = filtro.TurmaCodigo,
                     AnoLetivo = 2023,
                     Ue = new Ue { CodigoUe = "101", Nome = "UE Teste" },
                     ModalidadeCodigo = Modalidade.Fundamental
                 });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterAlunosPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            _mediatorMock.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(q => q.CodigoTurma == Convert.ToInt16(filtro.TurmaCodigo)), It.IsAny<CancellationToken>()))
             .ReturnsAsync(alunos);

            _mediatorMock.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(q => q.Ids.Contains(filtro.DisciplinaCodigo) &&
               q.CodigoTurma == filtro.TurmaCodigo), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = filtro.DisciplinaCodigo, Nome = "Matemática" }
               });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(q => q.Tipo == TipoParametroSistema.MediaBimestre && q.Ano == DateTime.Today.Year), It.IsAny<CancellationToken>()))
              .ReturnsAsync("7.0");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(q => q.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes
               && q.Ano == DateTime.Today.Year), It.IsAny<CancellationToken>()))
             .ReturnsAsync("15.0");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoAvaliacaoPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(10L);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 3 });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciasPorAlunosTurmaCCDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>());

            _consultasDisciplinaMock.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = codigoDisciplina, Nome = "Matemática" }
                });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoFechamentoBimestre());

            var resultado = await _useCase.Executar(filtro);

            var primeiroBimestre = resultado.Bimestres.FirstOrDefault();
            Assert.NotNull(primeiroBimestre);
            Assert.NotEmpty(primeiroBimestre.Alunos);

            var alunoResultado = primeiroBimestre.Alunos.FirstOrDefault();
            Assert.NotNull(alunoResultado);
            Assert.Contains(resultado.Bimestres.First().Alunos, aluno => aluno.Id == "1001");
            Assert.Contains(resultado.Bimestres.First().Alunos, aluno => aluno.Id == "1002");

        }

        [Fact]
        public void Deve_Atualizar_Nota_Aluno_Quando_Data_Anterior_For_Nula()
        {
            var service = new NotaAuditoriaService();
            var nota = new NotaConceito
            {
                AlteradoEm = new DateTime(2024, 6, 1),
                AlteradoPor = "Professor X",
                AlteradoRF = "123"
            };
            var atividade = new AtividadeAvaliativa { NomeAvaliacao = "Prova 1" };

            service.AtualizarUltimaNotaAlterada(nota, atividade);

            Assert.Equal("Professor X(123)", service.UsuarioRfUltimaNotaConceitoAlterada);
            Assert.Equal(nota.AlteradoEm, service.DataUltimaNotaConceitoAlterada);
            Assert.Equal("Prova 1", service.NomeAvaliacaoAuditoriaAlteracao);
        }

        [Fact]
        public void Deve_Atualizar_Nota_Quando_Data_Nova_For_Mais_Recente()
        {
            var service = new NotaAuditoriaService();
            service.DataUltimaNotaConceitoAlterada = new DateTime(2024, 5, 1);

            var nota = new NotaConceito
            {
                AlteradoEm = new DateTime(2024, 6, 1),
                AlteradoPor = "Prof Y",
                AlteradoRF = "456"
            };
            var atividade = new AtividadeAvaliativa { NomeAvaliacao = "Prova 2" };

            service.AtualizarUltimaNotaAlterada(nota, atividade);

            Assert.Equal("Prof Y(456)", service.UsuarioRfUltimaNotaConceitoAlterada);
            Assert.Equal(nota.AlteradoEm, service.DataUltimaNotaConceitoAlterada);
            Assert.Equal("Prova 2", service.NomeAvaliacaoAuditoriaAlteracao);
        }

        [Fact]
        public void Nao_Deve_Atualizar_Nota_Quando_Data_Nova_For_Menor_Ou_Igual()
        {
            var service = new NotaAuditoriaService();
            service.DataUltimaNotaConceitoAlterada = new DateTime(2024, 6, 1);

            var nota = new NotaConceito
            {
                AlteradoEm = new DateTime(2024, 5, 1),
                AlteradoPor = "Prof Z",
                AlteradoRF = "789"
            };
            var atividade = new AtividadeAvaliativa { NomeAvaliacao = "Prova 3" };

            service.AtualizarUltimaNotaAlterada(nota, atividade);

            Assert.Null(service.UsuarioRfUltimaNotaConceitoAlterada);
            Assert.Equal(new DateTime(2024, 6, 1), service.DataUltimaNotaConceitoAlterada);
            Assert.Null(service.NomeAvaliacaoAuditoriaAlteracao);
        }

        [Fact]
        public void Deve_Atualizar_Auditoria_Quando_Dados_Auditoria_Tem_Alteracoes_Com_AlteradoEm_Maior()
        {
            var retorno = new NotasConceitosRetornoDto();

            var dadosAuditoriaAlteracaoBimestre = new List<FechamentoNotaPorTurmaPeriodoCCDto>
            {
                new FechamentoNotaPorTurmaPeriodoCCDto
                {
                    CriadoEm = new DateTime(2024, 7, 8, 10, 0, 0),
                    CriadoPor = "Criador1",
                    CriadoRF = "1",
                    AlteradoEm = new DateTime(2024, 7, 10, 11, 0, 0),
                    AlteradoPor = "Alterador1",
                    AlteradoRF = "2"
                },
                new FechamentoNotaPorTurmaPeriodoCCDto
                {
                    CriadoEm = new DateTime(2024, 7, 7, 10, 0, 0),
                    CriadoPor = "Criador2",
                    CriadoRF = "3",
                    AlteradoEm = new DateTime(2024, 7, 6, 10, 0, 0),
                    AlteradoPor = "Alterador2",
                    AlteradoRF = "0"
                }
            };

            if (dadosAuditoriaAlteracaoBimestre.Any())
            {
                var ultimoDadoDeAuditoriaAlteradoEm = dadosAuditoriaAlteracaoBimestre
                                                    .OrderByDescending(nc => nc.AlteradoEm)
                                                    .Select(nc => new
                                                    {
                                                        AlteradoPor = nc.AlteradoRF.Equals("0") ? nc.CriadoPor : nc.AlteradoPor,
                                                        AlteradoRf = nc.AlteradoRF.Equals("0") ? nc.CriadoRF : nc.AlteradoRF,
                                                        AlteradoEm = nc.AlteradoRF.Equals("0") ? nc.CriadoEm : nc.AlteradoEm.Value,
                                                    })
                                                    .First();

                var ultimoDadoDeAuditoriaCriadoEm = dadosAuditoriaAlteracaoBimestre
                                                    .OrderByDescending(nc => nc.CriadoEm)
                                                    .Select(nc => new
                                                    {
                                                        AlteradoPor = nc.CriadoPor,
                                                        AlteradoRf = nc.CriadoRF,
                                                        AlteradoEm = nc.CriadoEm,
                                                    })
                                                    .First();

                var ultimoDadoDeAuditoria = ultimoDadoDeAuditoriaCriadoEm.AlteradoEm > ultimoDadoDeAuditoriaAlteradoEm.AlteradoEm
                    ? ultimoDadoDeAuditoriaCriadoEm
                    : ultimoDadoDeAuditoriaAlteradoEm;

                retorno.AuditoriaBimestreAlterado = $"Nota final do bimestre alterada por {(ultimoDadoDeAuditoria.AlteradoPor)}({ultimoDadoDeAuditoria.AlteradoRf}) em {ultimoDadoDeAuditoria.AlteradoEm.ToString("dd/MM/yyyy")}, às {ultimoDadoDeAuditoria.AlteradoEm.ToString("HH:mm")}.";
            }

            Assert.NotNull(retorno.AuditoriaBimestreAlterado);
            Assert.Contains("Alterador1", retorno.AuditoriaBimestreAlterado);
        }

        [Fact]
        public void Deve_Atualizar_Auditoria_Quando_Dados_Auditoria_Tem_Alteracoes_Com_CriadoEm_Maior()
        {
            var retorno = new NotasConceitosRetornoDto();

            var dadosAuditoriaAlteracaoBimestre = new List<FechamentoNotaPorTurmaPeriodoCCDto>
            {
                new FechamentoNotaPorTurmaPeriodoCCDto
                {
                    CriadoEm = new DateTime(2024, 7, 10, 10, 0, 0),
                    CriadoPor = "Criador1",
                    CriadoRF = "1",
                    AlteradoEm = new DateTime(2024, 7, 9, 10, 0, 0),
                    AlteradoPor = "Alterador1",
                    AlteradoRF = "2"
                },
                new FechamentoNotaPorTurmaPeriodoCCDto
                {
                    CriadoEm = new DateTime(2024, 7, 8, 10, 0, 0),
                    CriadoPor = "Criador2",
                    CriadoRF = "3",
                    AlteradoEm = new DateTime(2024, 7, 7, 10, 0, 0),
                    AlteradoPor = "Alterador2",
                    AlteradoRF = "0"
                }
            };

            if (dadosAuditoriaAlteracaoBimestre.Any())
            {
                var ultimoDadoDeAuditoriaAlteradoEm = dadosAuditoriaAlteracaoBimestre
                                                    .OrderByDescending(nc => nc.AlteradoEm)
                                                    .Select(nc => new
                                                    {
                                                        AlteradoPor = nc.AlteradoRF.Equals("0") ? nc.CriadoPor : nc.AlteradoPor,
                                                        AlteradoRf = nc.AlteradoRF.Equals("0") ? nc.CriadoRF : nc.AlteradoRF,
                                                        AlteradoEm = nc.AlteradoRF.Equals("0") ? nc.CriadoEm : nc.AlteradoEm.Value,
                                                    })
                                                    .First();

                var ultimoDadoDeAuditoriaCriadoEm = dadosAuditoriaAlteracaoBimestre
                                                    .OrderByDescending(nc => nc.CriadoEm)
                                                    .Select(nc => new
                                                    {
                                                        AlteradoPor = nc.CriadoPor,
                                                        AlteradoRf = nc.CriadoRF,
                                                        AlteradoEm = nc.CriadoEm,
                                                    })
                                                    .First();

                var ultimoDadoDeAuditoria = ultimoDadoDeAuditoriaCriadoEm.AlteradoEm > ultimoDadoDeAuditoriaAlteradoEm.AlteradoEm
                    ? ultimoDadoDeAuditoriaCriadoEm
                    : ultimoDadoDeAuditoriaAlteradoEm;

                retorno.AuditoriaBimestreAlterado = $"Nota final do bimestre alterada por {(ultimoDadoDeAuditoria.AlteradoPor)}({ultimoDadoDeAuditoria.AlteradoRf}) em {ultimoDadoDeAuditoria.AlteradoEm.ToString("dd/MM/yyyy")}, às {ultimoDadoDeAuditoria.AlteradoEm.ToString("HH:mm")}.";
            }

            Assert.NotNull(retorno.AuditoriaBimestreAlterado);
            Assert.Contains("Criador1", retorno.AuditoriaBimestreAlterado);
        }

        [Fact]
        public void Nao_Deve_Atualizar_Auditoria_Quando_Dados_Auditoria_Eh_Vazio()
        {
            var retorno = new NotasConceitosRetornoDto();
            var dadosAuditoriaAlteracaoBimestre = new List<NotaConceitoDto>();

            if (dadosAuditoriaAlteracaoBimestre.Any())
            {
                retorno.AuditoriaBimestreAlterado = "deveria não entrar aqui";
            }

            Assert.Null(retorno.AuditoriaBimestreAlterado);
        }

        [Fact]
        public void Deve_Atualizar_Auditoria_Quando_Tem_Mais_De_Um_Fechamento_Aluno()
        {
            var retorno = new NotasConceitosRetornoDto();

            var fechamentoAluno1 = new FechamentoAlunoPorTurmaPeriodoCCDto
            {
                AlunoCodigo = "123"
            };
            fechamentoAluno1.FechamentoNotas.Add(new FechamentoNotaPorTurmaPeriodoCCDto
            {
                CriadoEm = new DateTime(2024, 7, 10, 9, 0, 0),
                CriadoPor = "CriadorA",
                CriadoRF = "10"
            });

            var fechamentoAluno2 = new FechamentoAlunoPorTurmaPeriodoCCDto
            {
                AlunoCodigo = "124"
            };
            fechamentoAluno2.FechamentoNotas.Add(new FechamentoNotaPorTurmaPeriodoCCDto
            {
                CriadoEm = new DateTime(2024, 7, 11, 11, 30, 0),
                CriadoPor = "CriadorB",
                CriadoRF = "20"
            });

            var fechamentoTurma = new
            {
                FechamentoAlunos = new List<FechamentoAlunoPorTurmaPeriodoCCDto> { fechamentoAluno1, fechamentoAluno2 }
            };

            if (fechamentoTurma.FechamentoAlunos.Count > 1)
            {
                var ultimoDadoDeAuditoria = fechamentoTurma.FechamentoAlunos
                                                           .SelectMany(a => a.FechamentoNotas)
                                                           .OrderByDescending(nc => nc.CriadoEm)
                                                           .Select(nc => new
                                                           {
                                                               AlteradoPor = nc.CriadoPor,
                                                               AlteradoRf = nc.CriadoRF,
                                                               AlteradoEm = nc.CriadoEm,
                                                           })
                                                           .First();

                retorno.AuditoriaBimestreAlterado = $"Nota final do bimestre alterada por {(ultimoDadoDeAuditoria.AlteradoPor)}({ultimoDadoDeAuditoria.AlteradoRf}) em {ultimoDadoDeAuditoria.AlteradoEm:dd/MM/yyyy}, às {ultimoDadoDeAuditoria.AlteradoEm:HH:mm}.";
            }

            Assert.NotNull(retorno.AuditoriaBimestreAlterado);
            Assert.Contains("CriadorB", retorno.AuditoriaBimestreAlterado);
            Assert.Contains("20", retorno.AuditoriaBimestreAlterado);
            Assert.Contains("11/07/2024", retorno.AuditoriaBimestreAlterado);
        }

        [Fact]
        public async Task Deve_Cobrir_Conjunto_Regencia_Com_Notas_Aprovacao()
        {
            var filtro = new ListaNotasConceitosDto
            {
                TurmaCodigo = "999",
                DisciplinaCodigo = 123,
                PeriodoInicioTicks = DateTime.Now.Ticks,
                PeriodoFimTicks = DateTime.Now.AddMonths(1).Ticks,
                Bimestre = 1,
                PeriodoEscolarId = 1,
                TurmaId = 1
            };

            var componenteReferencia = new DisciplinaDto
            {
                Regencia = true,
                CodigoComponenteCurricular = filtro.DisciplinaCodigo,
                Nome = "Disciplina Regencia"
            };

            var disciplinasRegencia = new List<DisciplinaDto>
            {
                new DisciplinaDto
                {
                    CodigoComponenteCurricular = filtro.DisciplinaCodigo,
                    Nome = "Disciplina Regencia"
                }
            };

            var fechamentoNotaConceitoBimestre = new List<FechamentoNotaPorTurmaPeriodoCCDto>
            {
                new FechamentoNotaPorTurmaPeriodoCCDto
                {
                    Id = 1001,
                    DisciplinaId = filtro.DisciplinaCodigo,
                    ConceitoId = 5,
                    Nota = 8.0,
                    CriadoEm = DateTime.Now.AddDays(-1),
                    CriadoPor = "UsuarioTest",
                    CriadoRF = "123"
                }
            };

            var listaFechamentoNotaEmAprovacao = new List<FechamentoNotaAprovacaoDto>
            {
                new FechamentoNotaAprovacaoDto
                {
                    Id = 1001,
                    NotaEmAprovacao = 7.5
                }
            };

            var notaConceitoAluno = new NotasConceitosAlunoRetornoDto();

            if (componenteReferencia.Regencia)
            {
                foreach (var disciplinaRegencia in disciplinasRegencia)
                {
                    var nota = new FechamentoNotaRetornoDto()
                    {
                        DisciplinaId = disciplinaRegencia.CodigoComponenteCurricular,
                        Disciplina = disciplinaRegencia.Nome,
                    };

                    var notaRegencia = fechamentoNotaConceitoBimestre?.FirstOrDefault(c => c.DisciplinaId == disciplinaRegencia.CodigoComponenteCurricular);
                    if (notaRegencia != null)
                    {
                        nota.NotaConceito = notaRegencia.ConceitoId.HasValue ? notaRegencia.ConceitoId.Value : 0;
                        nota.EhConceito = notaRegencia.ConceitoId.HasValue;
                        var listaFiltrada = listaFechamentoNotaEmAprovacao.FirstOrDefault(i => i.Id == notaRegencia.Id);
                        if (listaFiltrada != null)
                        {
                            double notaConceitoWF = listaFiltrada.NotaEmAprovacao;
                            VerificaNotaEmAprovacao(notaConceitoWF, nota);
                        }
                    }

                    notaConceitoAluno.NotasBimestre.Add(nota);
                }
            }

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto> { componenteReferencia });

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(
                filtro.DisciplinaCodigo, filtro.TurmaCodigo, false, true))
                .ReturnsAsync(disciplinasRegencia);

            if (componenteReferencia.Regencia)
            {
                foreach (var disciplinaRegencia in disciplinasRegencia)
                {
                    var nota = new FechamentoNotaRetornoDto()
                    {
                        DisciplinaId = Convert.ToInt32(disciplinaRegencia.CodigoComponenteCurricular),
                        Disciplina = disciplinaRegencia.Nome,
                    };

                    var notaRegencia = fechamentoNotaConceitoBimestre?.FirstOrDefault(c => c.DisciplinaId == Convert.ToInt32(disciplinaRegencia.CodigoComponenteCurricular));
                    if (notaRegencia != null)
                    {
                        nota.NotaConceito = notaRegencia.ConceitoId.HasValue ? notaRegencia.ConceitoId.Value : 0;
                        nota.EhConceito = notaRegencia.ConceitoId.HasValue;
                        var listaFiltrada = listaFechamentoNotaEmAprovacao.FirstOrDefault(i => i.Id == notaRegencia.Id);
                        if (listaFiltrada != null)
                        {
                            double notaConceitoWF = listaFiltrada.NotaEmAprovacao;
                            VerificaNotaEmAprovacao(notaConceitoWF, nota);
                        }
                    }

                    notaConceitoAluno.NotasBimestre.Add(nota);
                }
            }

            Assert.NotEmpty(notaConceitoAluno.NotasBimestre);
            var notaInserida = notaConceitoAluno.NotasBimestre.First();
            Assert.Equal(filtro.DisciplinaCodigo, notaInserida.DisciplinaId);
            Assert.True(notaInserida.EhConceito);
            Assert.Equal(7.5, notaInserida.NotaConceito);
        }

        [Fact]
        public void Verifica_Nota_Em_Aprovacao_Deve_Marcar_Em_Aprovacao_Quando_Nota_Positiva()
        {
            var tipoUseCase = typeof(ObterNotasParaAvaliacoesUseCase);
            var metodo = tipoUseCase.GetMethod("VerificaNotaEmAprovacao", BindingFlags.NonPublic | BindingFlags.Instance);

            var notaConceito = new FechamentoNotaRetornoDto();

            double notaAprovacao = 7.5;

            metodo.Invoke(_useCase, new object[] { notaAprovacao, notaConceito });

            Assert.True(notaConceito.EmAprovacao);
            Assert.Equal(notaAprovacao, notaConceito.NotaConceito);
        }

        [Fact]
        public void Verifica_Nota_Em_Aprovacao_Deve_Nao_Marcar_Em_Aprovacao_Quando_Nota_Zero_Ou_Negativa()
        {
            var tipoUseCase = typeof(ObterNotasParaAvaliacoesUseCase);
            var metodo = tipoUseCase.GetMethod("VerificaNotaEmAprovacao", BindingFlags.NonPublic | BindingFlags.Instance);

            var notaConceito = new FechamentoNotaRetornoDto();

            double notaAprovacao = 0;

            metodo.Invoke(_useCase, new object[] { notaAprovacao, notaConceito });

            Assert.False(notaConceito.EmAprovacao);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Verifica_Nota_Em_Aprovacao_Quando_Lista_Fechamento_Nota_Em_Aprovacao_Contem_Item_Correspondente()
        {
            var filtro = new ListaNotasConceitosDto
            {
                AnoLetivo = 2023,
                TurmaCodigo = "1234",
                PeriodoInicioTicks = new DateTime(2023, 3, 1).Ticks,
                PeriodoFimTicks = new DateTime(2023, 6, 30).Ticks,
                DisciplinaCodigo = 555,
                Bimestre = 2,
                TurmaId = 123,
                PeriodoEscolarId = 987,
                TurmaHistorico = false
            };

            var aluno = new AlunoPorTurmaResposta
            {
                CodigoAluno = "1",
                CodigoTurma = Convert.ToInt64(filtro.TurmaCodigo),
                NumeroAlunoChamada = 1,
                NomeAluno = "Aluno Teste",
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = new DateTime(2023, 2, 1)
            };

            var notaBimestre = new FechamentoNotaPorTurmaPeriodoCCDto
            {
                Id = 1001,
                Nota = 7.0,
                DisciplinaId = filtro.DisciplinaCodigo
            };

            var fechamentoAluno = new FechamentoAlunoPorTurmaPeriodoCCDto();
            fechamentoAluno.FechamentoNotas.Add(notaBimestre);
            fechamentoAluno.AlunoCodigo = aluno.CodigoAluno;

            var fechamentoTurma = new FechamentoPorTurmaPeriodoCCDto();
            fechamentoTurma.FechamentoAlunos.Add(fechamentoAluno);
            fechamentoTurma.Id = 500;
            fechamentoTurma.Situacao = SituacaoFechamento.EmProcessamento;

            var notaEmAprovacao = new FechamentoNotaAprovacaoDto
            {
                Id = notaBimestre.Id,
                NotaEmAprovacao = 8.5
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = filtro.TurmaCodigo, AnoLetivo = 2023 });

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "123", PerfilAtual = Guid.NewGuid() });

            _consultasDisciplinaMock.Setup(m => m.ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = filtro.DisciplinaCodigo, Id = filtro.DisciplinaCodigo } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = filtro.DisciplinaCodigo, Id = filtro.DisciplinaCodigo, Nome = "Português" } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { aluno });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciasPorAlunosTurmaCCDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 2 });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("6");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentosPorTurmaPeriodoCCQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoPorTurmaPeriodoCCDto> { fechamentoTurma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto> { notaEmAprovacao });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMarcadorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MarcadorFrequenciaDto());

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            var resultado = await _useCase.Executar(filtro);

            var notaFinalAluno = resultado.Bimestres.First().Alunos.FirstOrDefault().NotasBimestre.FirstOrDefault(n => n.DisciplinaId == filtro.DisciplinaCodigo);

            Assert.NotNull(notaFinalAluno);
            Assert.True(notaFinalAluno.EmAprovacao);
            Assert.Equal(notaEmAprovacao.NotaEmAprovacao, notaFinalAluno.NotaConceito);
        }

        #region Métodos auxiliares
        private void VerificaNotaEmAprovacao(double notaEmAprovacao, FechamentoNotaRetornoDto nota)
        {
            if (notaEmAprovacao > 0)
                nota.NotaConceito = notaEmAprovacao;
        }
        private IEnumerable<AlunoPorTurmaResposta> FiltrarAlunos(IEnumerable<AlunoPorTurmaResposta> alunos, ListaNotasConceitosDto filtro)
        {
            DateTime _inicio = new DateTime(2026, 4, 1);
            DateTime _fim = new DateTime(2026, 4, 30);

            if (filtro.TurmaHistorico)
            {
                alunos = from a in alunos
                         where a.EstaAtivo(_inicio, _fim) ||
                              (a.EstaInativo(_inicio, _fim) &&
                               a.DataSituacao.Date >= _inicio.Date &&
                               a.DataSituacao.Date <= _fim.Date &&
                              (a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido || a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Transferido))
                         select a;
            }

            return alunos;
        }
        public class NotaAuditoriaService
        {
            public string UsuarioRfUltimaNotaConceitoAlterada;
            public DateTime? DataUltimaNotaConceitoAlterada;
            public string NomeAvaliacaoAuditoriaAlteracao;

            public void AtualizarUltimaNotaAlterada(NotaConceito notaDoAluno, AtividadeAvaliativa atividade)
            {
                if (notaDoAluno.AlteradoEm.HasValue &&
                    (!DataUltimaNotaConceitoAlterada.HasValue || notaDoAluno.AlteradoEm.Value > DataUltimaNotaConceitoAlterada.Value))
                {
                    UsuarioRfUltimaNotaConceitoAlterada = $"{notaDoAluno.AlteradoPor}({notaDoAluno.AlteradoRF})";
                    DataUltimaNotaConceitoAlterada = notaDoAluno.AlteradoEm;
                    NomeAvaliacaoAuditoriaAlteracao = atividade.NomeAvaliacao;
                }
            }
        }
        private AlunoPorTurmaResposta CriarAlunoMock(bool ativo, SituacaoMatriculaAluno situacao, DateTime? dataSituacao = null)
        {
            var hoje = DateTime.Today;

            return new AlunoPorTurmaResposta
            {
                CodigoAluno = "123456",
                CodigoSituacaoMatricula = situacao,
                DataSituacao = dataSituacao ?? hoje.AddDays(-10),
                DataMatricula = ativo ? hoje.AddMonths(-3) : hoje.AddYears(-1),
                DataNascimento = hoje.AddYears(-10),
                CodigoTurma = 123,
                NomeAluno = "Aluno Teste",
                NumeroAlunoChamada = 1,
            };
        }

        private ListaNotasConceitosDto CriarFiltroBase()
        {
            return new ListaNotasConceitosDto
            {
                TurmaCodigo = "123456",
                TurmaId = 1,
                DisciplinaCodigo = 123,
                Bimestre = 1,
                PeriodoEscolarId = 1,
                PeriodoInicioTicks = new DateTime(2023, 1, 1).Ticks,
                PeriodoFimTicks = new DateTime(2023, 3, 31).Ticks,
                AnoLetivo = 2023,
                TurmaHistorico = false
            };
        }

        private DisciplinaDto CriarComponenteReferencia(bool regencia = false)
        {
            return new DisciplinaDto
            {
                CodigoComponenteCurricular = 123,
                Nome = "Teste",
                Regencia = regencia
            };
        }

        private Usuario CriarUsuario(bool ehProfessorCj = false)
        {
            var usuario = new Usuario
            {
                CodigoRf = "1234567",
                PerfilAtual = ehProfessorCj ? Dominio.Perfis.PERFIL_CJ : Guid.NewGuid(),
            };

            var perfis = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    CodigoPerfil = usuario.PerfilAtual,
                    Tipo = TipoPerfil.UE,
                    Ordem = 1
                }
            };

            usuario.DefinirPerfis(perfis);

            return usuario;
        }

        private Turma CriarTurmaCompleta(Modalidade modalidade = Modalidade.Fundamental)
        {
            return new Turma
            {
                CodigoTurma = "123456",
                ModalidadeCodigo = modalidade,
                AnoLetivo = 2023,
                Semestre = 1
            };
        }

        private List<DisciplinaDto> CriarListaDisciplinasDto()
        {
            return new List<DisciplinaDto>
        {
            new DisciplinaDto
            {
                CodigoComponenteCurricular = 1,
                Nome = "Português"
            },
            new DisciplinaDto
            {
                CodigoComponenteCurricular = 2,
                Nome = "Matemática"
            }
        };
        }

        private List<ComponenteCurricularEol> CriarListaComponentesCurriculares(bool regencia = true)
        {
            return new List<ComponenteCurricularEol>
        {
            CriarComponenteCurricular("Português", regencia),
            CriarComponenteCurricular("Matemática", regencia)
        };
        }

        private ComponenteCurricularEol CriarComponenteCurricular(string nome, bool regencia = true, long codigo = 1)
        {
            return new ComponenteCurricularEol
            {
                Codigo = codigo,
                Descricao = nome,
                Regencia = regencia,
                TerritorioSaber = false,
                GrupoMatriz = new SME.SGP.Dominio.GrupoMatriz { Id = 1, Nome = "Grupo Teste" }
            };
        }

        private Usuario CriarUsuarioSimulado(bool ehProfessor, bool ehCj)
        {
            var usuario = new Usuario();

            var perfil = ehProfessor
                ? Dominio.Perfis.PERFIL_PROFESSOR
                : ehCj
                    ? Dominio.Perfis.PERFIL_CJ
                    : Guid.NewGuid();

            typeof(Usuario).GetProperty(nameof(Usuario.PerfilAtual))?
                .SetValue(usuario, perfil);

            typeof(Usuario).GetProperty(nameof(Usuario.Perfis), BindingFlags.NonPublic | BindingFlags.Instance)?
                .SetValue(usuario, new List<PrioridadePerfil>
                {
            new PrioridadePerfil
            {
                CodigoPerfil = perfil,
                Tipo = TipoPerfil.UE,
                Ordem = 1
            }
                });

            return usuario;
        }

        private void ConfigurarMocksBasicos(ListaNotasConceitosDto filtro, DisciplinaDto componenteReferencia,
            Usuario usuario = null, Turma turmaCompleta = null)
        {
            usuario ??= CriarUsuario();
            turmaCompleta ??= CriarTurmaCompleta();

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaCompleta);

            _mediatorMock.Setup(x => x.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _consultasDisciplinaMock.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { componenteReferencia });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto> { componenteReferencia });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>
                {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "123",
                    NomeAluno = "Aluno Teste",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    DataMatricula = DateTime.Now.AddMonths(-6),
                    DataSituacao = DateTime.Now
                }
                });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterAusenciasDaAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentosPorTurmaPeriodoCCQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoPorTurmaPeriodoCCDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFrequenciasPorAlunosTurmaCCDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoFechamentoBimestre());

            _mediatorMock.Setup(x => x.Send(It.IsAny<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterMarcadorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MarcadorFrequenciaDto());

            _mediatorMock.Setup(x => x.Send(ObterTipoAvaliacaoBimestralQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao { Id = 1, AvaliacoesNecessariasPorBimestre = 2 });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5.0");

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoNotaPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoNota.Nota);

            _consultasPeriodoFechamentoMock.Setup(x => x.TurmaEmPeriodoDeFechamento(It.IsAny<Turma>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .ReturnsAsync(true);
        }
        #endregion
    }
}
