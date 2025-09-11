using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class CadastrarParecerPAAIPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly CadastrarParecerPAAIPlanoAEEUseCase useCase;

        public CadastrarParecerPAAIPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new CadastrarParecerPAAIPlanoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Parecer_For_Cadastrado_Com_Sucesso()
        {
            // Arrange
            var planoAEEId = 1L;
            var planoDto = new PlanoAEECadastroParecerDto { Parecer = "Parecer PAAI válido" };

            mediator.Setup(m => m.Send(It.Is<CadastrarParecerPAAICommand>(
                    c => c.PlanoAEEId == planoAEEId && c.ParecerPAAI == planoDto.Parecer),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(planoAEEId, planoDto);

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.Is<CadastrarParecerPAAICommand>(
                c => c.PlanoAEEId == planoAEEId && c.ParecerPAAI == planoDto.Parecer),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Parecer_Nao_For_Cadastrado()
        {
            // Arrange
            var planoAEEId = 1L;
            var planoDto = new PlanoAEECadastroParecerDto { Parecer = "Parecer PAAI inválido" };

            mediator.Setup(m => m.Send(It.IsAny<CadastrarParecerPAAICommand>(),
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
            var planoDto = new PlanoAEECadastroParecerDto { Parecer = "Parecer PAAI com erro" };

            mediator.Setup(m => m.Send(It.IsAny<CadastrarParecerPAAICommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao cadastrar parecer PAAI"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                useCase.Executar(planoAEEId, planoDto));
        }

        [Theory]
        [InlineData(0, null)] 
        [InlineData(0, "")]   
        [InlineData(1, null)] 
        [InlineData(1, "")]   
        public async Task Nao_Deve_Lancar_Excecao_Para_Parametros_Invalidos(long planoAEEId, string parecer)
        {
            // Arrange
            var planoDto = new PlanoAEECadastroParecerDto { Parecer = parecer };

            mediator.Setup(m => m.Send(It.IsAny<CadastrarParecerPAAICommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(planoAEEId, planoDto);

            // Assert
            Assert.True(resultado); // Verifica que o fluxo continua
            mediator.Verify(m => m.Send(It.IsAny<CadastrarParecerPAAICommand>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
