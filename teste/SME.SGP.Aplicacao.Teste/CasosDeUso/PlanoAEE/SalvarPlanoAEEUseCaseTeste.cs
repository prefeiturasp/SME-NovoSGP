using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class SalvarPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly SalvarPlanoAEEUseCase salvarPlanoAEEUseCase;
        private readonly PlanoAEEPersistenciaDto planoAeeDto;

        public SalvarPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            salvarPlanoAEEUseCase = new SalvarPlanoAEEUseCase(mediator.Object);

            planoAeeDto = new PlanoAEEPersistenciaDto
            {
                TurmaCodigo = "1",
                AlunoCodigo = "123",
                Id = 1,
            };
        }

        [Fact]
        public async Task Deve_Disparar_Excecao_Quando_Turma_Nao_Encontrada()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => salvarPlanoAEEUseCase.Executar(planoAeeDto));

            Assert.Equal(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA, ex.Message);
        }

        [Fact]
        public async Task Deve_Disparar_Excecao_Quando_Turma_Nao_For_Regular()
        {
            // Arrange
            var turma = new Turma { TipoTurma = TipoTurma.Programa };

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => salvarPlanoAEEUseCase.Executar(planoAeeDto));

            Assert.Equal(MensagemNegocioTurma.TURMA_DEVE_SER_TIPO_REGULAR, ex.Message);

        }

        [Fact]
        public async Task Deve_Disparar_Excecao_Quando_Aluno_Nao_Encontrado()
        {
            // Arrange
            var turma = new Turma { TipoTurma = TipoTurma.Regular };

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AlunoPorTurmaResposta)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => salvarPlanoAEEUseCase.Executar(planoAeeDto));

            Assert.Equal(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO, ex.Message);
        }

        [Fact]
        public async Task Deve_Validar_Questao_Periodo_Escolar_Quando_Versao_Maior_Que_1()
        {
            // Arrange
            var turma = new Turma { Id = 1, TipoTurma = TipoTurma.Regular, CodigoTurma = "1" };
            var aluno = new AlunoPorTurmaResposta
            {
                NomeAluno = "Aluno Teste",
                CodigoAluno = "123",
                NumeroAlunoChamada = 1
            };
            var retornoEsperado = GetRetornoPlanoAEEDto();
            var periodoEscolar = new PeriodoEscolar { Id = 1 };
            var questao = new QuestaoDto { Id = 1, TipoQuestao = TipoQuestao.PeriodoEscolar };
            var resposta = new RespostaQuestaoDto { QuestaoId = 1, Texto = "2" };

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            mediator.Setup(x => x.Send(It.IsAny<SalvarPlanoAeeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.PlanoAEE { TurmaId = 1 });

            mediator.Setup(x => x.Send(It.IsAny<ObterVersaoPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PlanoAEEVersaoDto { Id = 2, Numero = 2 });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarAtualPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolar);

            mediator.Setup(x => x.Send(It.IsAny<ObterRespostasPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { resposta });

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestionarioPlanoAEEIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestoesPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { questao });

            // Act
            var resultado = await salvarPlanoAEEUseCase.Executar(planoAeeDto);

            // Assert
            mediator.Verify(x => x.Send(It.IsAny<AtualizarPlanoAEERespostaPeriodoEscolarCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_Resposta_Quando_Periodo_Ja_Estiver_Correto()
        {
            // Arrange
            var turma = new Turma { Id = 1, TipoTurma = TipoTurma.Regular, CodigoTurma = "1" };
            var aluno = new AlunoPorTurmaResposta
            {
                NomeAluno = "Aluno Teste",
                CodigoAluno = "123",
                NumeroAlunoChamada = 1
            };
            var retornoEsperado = GetRetornoPlanoAEEDto();
            var periodoEscolar = new PeriodoEscolar { Id = 1 };
            var questao = new QuestaoDto { Id = 1, TipoQuestao = TipoQuestao.PeriodoEscolar };
            var resposta = new RespostaQuestaoDto { QuestaoId = 1, Texto = "1" }; // Mesmo ID do período

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            mediator.Setup(x => x.Send(It.IsAny<SalvarPlanoAeeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.PlanoAEE { TurmaId = 1 });

            mediator.Setup(x => x.Send(It.IsAny<ObterVersaoPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PlanoAEEVersaoDto { Id = 2, Numero = 2 });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarAtualPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolar);

            mediator.Setup(x => x.Send(It.IsAny<ObterRespostasPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { resposta });

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestionarioPlanoAEEIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestoesPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { questao });

            // Act
            var resultado = await salvarPlanoAEEUseCase.Executar(planoAeeDto);

            // Assert
            mediator.Verify(x => x.Send(It.IsAny<AtualizarPlanoAEERespostaPeriodoEscolarCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static RetornoPlanoAEEDto GetRetornoPlanoAEEDto(long planoId = 1, long planoVersaoId = 2)
        {
            return new RetornoPlanoAEEDto(planoId, planoVersaoId);
        }
    }
}
