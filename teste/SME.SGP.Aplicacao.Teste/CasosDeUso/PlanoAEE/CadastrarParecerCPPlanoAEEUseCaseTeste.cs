using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class CadastrarParecerCPPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly CadastrarParecerCPPlanoAEEUseCase useCase;

        public CadastrarParecerCPPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new CadastrarParecerCPPlanoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Parecer_For_Cadastrado_Com_Sucesso()
        {
            // Arrange
            var planoAEEId = 1L;
            var planoDto = new PlanoAEECadastroParecerDto { Parecer = "Parecer teste" };

            mediator.Setup(m => m.Send(It.Is<CadastrarParecerCPCommand>(
                    c => c.PlanoAEEId == planoAEEId && c.ParecerCoordenacao == planoDto.Parecer),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(planoAEEId, planoDto);

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.Is<CadastrarParecerCPCommand>(
                c => c.PlanoAEEId == planoAEEId && c.ParecerCoordenacao == planoDto.Parecer),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Parecer_Nao_For_Cadastrado()
        {
            // Arrange
            var planoAEEId = 1L;
            var planoDto = new PlanoAEECadastroParecerDto { Parecer = "Parecer teste" };

            mediator.Setup(m => m.Send(It.IsAny<CadastrarParecerCPCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await useCase.Executar(planoAEEId, planoDto);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Do_Command()
        {
            // Arrange
            var planoAEEId = 1L;
            var planoDto = new PlanoAEECadastroParecerDto { Parecer = "Parecer teste" };

            mediator.Setup(m => m.Send(It.IsAny<CadastrarParecerCPCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro ao cadastrar parecer"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() =>
                useCase.Executar(planoAEEId, planoDto));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(0, "")]
        [InlineData(0, " ")]
        [InlineData(1, null)]
        [InlineData(1, "")]
        [InlineData(1, " ")]
        public async Task Nao_Deve_Lancar_Excecao_Para_Parametros_Invalidos(long planoAEEId, string parecer)
        {
            var planoDto = new PlanoAEECadastroParecerDto { Parecer = parecer };

            mediator.Setup(m => m.Send(It.IsAny<CadastrarParecerCPCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(planoAEEId, planoDto);

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<CadastrarParecerCPCommand>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

