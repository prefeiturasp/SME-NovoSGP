using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class AtribuirResponsavelPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioPlanoAEE> repositorioPlanoAEE;
        private readonly AtribuirResponsavelPlanoAEEUseCase useCase;

        public AtribuirResponsavelPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioPlanoAEE = new Mock<IRepositorioPlanoAEE>();
            useCase = new AtribuirResponsavelPlanoAEEUseCase(mediator.Object, repositorioPlanoAEE.Object);
        }

        [Fact]
        public async Task Deve_Atribuir_Responsavel_Quando_Plano_E_Turma_Existem()
        {
            // Arrange
            var planoAEEId = 1L;
            var responsavelRF = "123456";
            var planoAEE = new SME.SGP.Dominio.PlanoAEE { TurmaId = 1 };
            var turma = new Turma();

            repositorioPlanoAEE.Setup(r => r.ObterPorIdAsync(planoAEEId))
                .ReturnsAsync(planoAEE);

            mediator.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == planoAEE.TurmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(m => m.Send(It.Is<AtribuirResponsavelPlanoAEECommand>(c =>
                c.PlanoAEE == planoAEE &&
                c.ResponsavelRF == responsavelRF &&
                c.Turma == turma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(planoAEEId, responsavelRF);

            // Assert
            Assert.True(resultado);
            repositorioPlanoAEE.Verify(r => r.ObterPorIdAsync(planoAEEId), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<AtribuirResponsavelPlanoAEECommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Plano_Nao_Existe()
        {
            // Arrange
            var planoAEEId = 1L;
            var responsavelRF = "123456";

            repositorioPlanoAEE.Setup(r => r.ObterPorIdAsync(planoAEEId))
                .ReturnsAsync((SME.SGP.Dominio.PlanoAEE)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(planoAEEId, responsavelRF));
            Assert.Equal(MensagemNegocioPlanoAee.Plano_aee_nao_encontrado, ex.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Turma_Nao_Existe()
        {
            // Arrange
            var planoAEEId = 1L;
            var responsavelRF = "123456";
            var planoAEE = new SME.SGP.Dominio.PlanoAEE { TurmaId = 1 };

            repositorioPlanoAEE.Setup(r => r.ObterPorIdAsync(planoAEEId))
                .ReturnsAsync(planoAEE);

            mediator.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == planoAEE.TurmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(planoAEEId, responsavelRF));
            Assert.Equal(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA, ex.Message);
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Do_Repositorio()
        {
            // Arrange
            var planoAEEId = 1L;
            var responsavelRF = "123456";

            repositorioPlanoAEE.Setup(r => r.ObterPorIdAsync(planoAEEId))
                .ThrowsAsync(new Exception("Erro no repositório"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(planoAEEId, responsavelRF));
        }

        [Fact]
        public async Task Deve_Retornar_Falso_Quando_Command_Falhar()
        {
            // Arrange
            var planoAEEId = 1L;
            var responsavelRF = "123456";
            var planoAEE = new SME.SGP.Dominio.PlanoAEE { TurmaId = 1 };
            var turma = new Turma();

            repositorioPlanoAEE.Setup(r => r.ObterPorIdAsync(planoAEEId))
                .ReturnsAsync(planoAEE);

            mediator.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == planoAEE.TurmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(m => m.Send(It.Is<AtribuirResponsavelPlanoAEECommand>(c =>
                c.PlanoAEE == planoAEE &&
                c.ResponsavelRF == responsavelRF &&
                c.Turma == turma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await useCase.Executar(planoAEEId, responsavelRF);

            // Assert
            Assert.False(resultado);
        }
    }
}
