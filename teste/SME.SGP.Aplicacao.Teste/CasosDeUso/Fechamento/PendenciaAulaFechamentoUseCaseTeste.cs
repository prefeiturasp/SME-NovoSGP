using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class PendenciaAulaFechamentoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PendenciaAulaFechamentoUseCase _useCase;

        public PendenciaAulaFechamentoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PendenciaAulaFechamentoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Processamento_Com_Sucesso()
        {
            var mensagemRabbit = new MensagemRabbit();
            var dreUeDto = new DreUeDto (1, 2);

            mensagemRabbit.Mensagem = System.Text.Json.JsonSerializer.Serialize(dreUeDto);

            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                ModalidadeCodigo = Modalidade.Fundamental
            };

            var aulas = new List<Dominio.Aula>
            {
                new Dominio.Aula
                {
                    Id = 1,
                    TurmaId = "TURMA001",
                    DisciplinaId = "1",
                    Turma = turma
                }
            };

            var periodoEscolar = new PeriodoEscolar
            {
                Id = 1,
                Bimestre = 1,
                PeriodoInicio = DateTime.Now.AddDays(-30),
                PeriodoFim = DateTime.Now.AddDays(30)
            };

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplinaPendenciaDto
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                DisciplinaId = 1,
                Bimestre = 1,
                TipoTurma = TipoTurma.Regular
            };

            var componenteCurricular = new DisciplinaDto
            {
                Id = 1,
                Nome = "Matemática",
                RegistraFrequencia = true
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.Aula>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ExcluirNotificacaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarFechamentoEmAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar> { periodoEscolar });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaDTOQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentoTurmaDisciplina);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoTurmaDisciplinaPendenciaDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<IncluirFilaGeracaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Excluir_Notificacoes_Pendencias_Para_Cada_Turma()
        {
            var mensagemRabbit = new MensagemRabbit();
            var dreUeDto = new DreUeDto(1, 2);
            mensagemRabbit.Mensagem = System.Text.Json.JsonSerializer.Serialize(dreUeDto);

            var turma1 = new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                ModalidadeCodigo = Modalidade.Fundamental
            };

            var turma2 = new Turma
            {
                Id = 2,
                CodigoTurma = "TURMA002",
                ModalidadeCodigo = Modalidade.Fundamental
            };

            var aulas = new List<Dominio.Aula>
            {
                new Dominio.Aula { Id = 1, TurmaId = "TURMA001", DisciplinaId = "1", Turma = turma1 },
                new Dominio.Aula { Id = 2, TurmaId = "TURMA002", DisciplinaId = "1", Turma = turma2 }
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.Aula>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarFechamentoEmAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoTurmaDisciplinaPendenciaDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ExcluirNotificacaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            _mediatorMock.Verify(x => x.Send(It.Is<ExcluirNotificacaoPendenciasFechamentoCommand>(
                cmd => cmd.TurmaCodigo == "TURMA001"), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(x => x.Send(It.Is<ExcluirNotificacaoPendenciasFechamentoCommand>(
                cmd => cmd.TurmaCodigo == "TURMA002"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Gerar_Pendencias_Fechamento_Quando_Existem_Periodos_Em_Aberto()
        {
            var mensagemRabbit = new MensagemRabbit();
            var dreUeDto = new DreUeDto(1, 2);
            mensagemRabbit.Mensagem = System.Text.Json.JsonSerializer.Serialize(dreUeDto);

            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                ModalidadeCodigo = Modalidade.Fundamental
            };

            var aulas = new List<Dominio.Aula>
            {
                new Dominio.Aula { Id = 1, TurmaId = "TURMA001", DisciplinaId = "1", Turma = turma }
            };

            var periodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1 };

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplinaPendenciaDto
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                DisciplinaId = 1,
                Bimestre = 1,
                TipoTurma = TipoTurma.Regular
            };

            var componenteCurricular = new DisciplinaDto
            {
                Id = 1,
                Nome = "Matemática",
                RegistraFrequencia = true
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.Aula>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarFechamentoEmAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar> { periodoEscolar });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaDTOQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentoTurmaDisciplina);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoTurmaDisciplinaPendenciaDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<IncluirFilaGeracaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ExcluirNotificacaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            _mediatorMock.Verify(x => x.Send(It.IsAny<IncluirFilaGeracaoPendenciasFechamentoCommand>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Nao_Deve_Gerar_Pendencias_Quando_Tipo_Turma_Eh_Programa()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit();
            var dreUeDto = new DreUeDto(1, 2);
            mensagemRabbit.Mensagem = System.Text.Json.JsonSerializer.Serialize(dreUeDto);

            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                ModalidadeCodigo = Modalidade.Fundamental
            };

            var aulas = new List<Dominio.Aula>
            {
                new Dominio.Aula { Id = 1, TurmaId = "TURMA001", DisciplinaId = "1", Turma = turma }
            };

            var periodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1 };

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplinaPendenciaDto
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                DisciplinaId = 1,
                Bimestre = 1,
                TipoTurma = TipoTurma.Programa 
            };

            var componenteCurricular = new DisciplinaDto
            {
                Id = 1,
                Nome = "Matemática",
                RegistraFrequencia = true
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.Aula>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarFechamentoEmAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar> { periodoEscolar });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaDTOQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentoTurmaDisciplina);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoTurmaDisciplinaPendenciaDto>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ExcluirNotificacaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            _mediatorMock.Verify(x => x.Send(It.IsAny<IncluirFilaGeracaoPendenciasFechamentoCommand>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Componente_Curricular_Nao_Encontrado()
        {
            var mensagemRabbit = new MensagemRabbit();
            var dreUeDto = new DreUeDto(1, 2);
            mensagemRabbit.Mensagem = System.Text.Json.JsonSerializer.Serialize(dreUeDto);

            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                ModalidadeCodigo = Modalidade.Fundamental
            };

            var aulas = new List<Dominio.Aula>
            {
                new Dominio.Aula
                {
                    Id = 1,
                    TurmaId = "TURMA001",
                    DisciplinaId = "1",
                    Turma = turma
                }
            };

            var periodoEscolar = new PeriodoEscolar
            {
                Id = 1,
                Bimestre = 1
            };

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplinaPendenciaDto
            {
                Id = 1,
                CodigoTurma = "TURMA001",
                DisciplinaId = 1,
                Bimestre = 1,
                TipoTurma = TipoTurma.Regular
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulas);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.Aula>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarFechamentoEmAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar> { periodoEscolar });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaDTOQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentoTurmaDisciplina);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NegocioException("Componente curricular não encontrado.", new Exception("Detalhe interno")));

            _mediatorMock.Setup(x => x.Send(It.IsAny<ExcluirNotificacaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));
            Assert.Equal("Componente curricular não encontrado.", excecao.Message); // ajuste conforme mensagem lançada no use case
        }

        [Fact]
        public async Task Executar_Deve_Processar_Todas_Modalidades_Na_Busca_Por_Pendencias()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit();
            var dreUeDto = new DreUeDto(1, 2);
            mensagemRabbit.Mensagem = System.Text.Json.JsonSerializer.Serialize(dreUeDto);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.Aula>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.Aula>());

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoTurmaDisciplinaPendenciaDto>());

            // Act
            await _useCase.Executar(mensagemRabbit);

            // Assert
            // Verifica se foram feitas 3 chamadas: PlanoAula, AtividadeAvaliativa e Frequencia
            _mediatorMock.Verify(x => x.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _mediatorMock.Verify(x => x.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Salvar_Log_Quando_Ocorre_Excecao()
        {
            var mensagemRabbit = new MensagemRabbit();
            var dreUeDto = new DreUeDto(1, 2);
            mensagemRabbit.Mensagem = System.Text.Json.JsonSerializer.Serialize(dreUeDto);

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro de teste", new Exception("Inner")));

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var ex = await Record.ExceptionAsync(() => _useCase.Executar(mensagemRabbit));

            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
            Assert.Equal("Erro de teste", ex.Message);

            _mediatorMock.Verify(x =>
                x.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}