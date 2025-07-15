using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class AtribuirResponsavelGeralDoPlanoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly AtribuirResponsavelGeralDoPlanoUseCase _useCase;

        public AtribuirResponsavelGeralDoPlanoUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new AtribuirResponsavelGeralDoPlanoUseCase(_mediator.Object);
        }

        [Fact(DisplayName = "Deve chamar o mediator com o comando correto e retornar true")]
        public async Task Deve_Chamar_Mediator_Com_Comando_Correto_E_Retornar_True()
        {
            var planoAEEId = 1L;
            var responsavelRF = "123456";
            var responsavelNome = "Professor Teste";
            var resultadoEsperado = true;

            _mediator.Setup(x => x.Send(It.Is<AtribuirResponsavelGeralDoPlanoCommand>(
                    c => c.PlanoAEEId == planoAEEId &&
                         c.ResponsavelRF == responsavelRF &&
                         c.ResponsavelNome == responsavelNome),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await _useCase.Executar(planoAEEId, responsavelRF, responsavelNome);

            _mediator.Verify(x => x.Send(
                It.Is<AtribuirResponsavelGeralDoPlanoCommand>(c =>
                    c.PlanoAEEId == planoAEEId &&
                    c.ResponsavelRF == responsavelRF &&
                    c.ResponsavelNome == responsavelNome),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(resultadoEsperado, resultado);
        }

        [Fact(DisplayName = "Deve retornar false quando o mediator retornar false")]
        public async Task Deve_Retornar_False_Quando_Mediator_Retornar_False()
        {
            var planoAEEId = 1L;
            var responsavelRF = "123456";
            var responsavelNome = "Professor Teste";
            var resultadoEsperado = false;

            _mediator.Setup(x => x.Send(It.IsAny<AtribuirResponsavelGeralDoPlanoCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await _useCase.Executar(planoAEEId, responsavelRF, responsavelNome);

            Assert.Equal(resultadoEsperado, resultado);
        }

        [Theory(DisplayName = "Deve validar parâmetros de entrada")]
        [InlineData(0, "123456", "Nome Responsável")]
        [InlineData(1, "", "Nome Responsável")]
        [InlineData(1, "123456", "")]
        [InlineData(1, null, "Nome Responsável")]
        [InlineData(1, "123456", null)]
        public async Task Deve_Validar_Parametros_Entrada(long planoAEEId, string responsavelRF, string responsavelNome)
        {
            _mediator.Setup(x => x.Send(It.IsAny<AtribuirResponsavelGeralDoPlanoCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(planoAEEId, responsavelRF, responsavelNome);
            if (planoAEEId <= 0 || string.IsNullOrWhiteSpace(responsavelRF) || string.IsNullOrWhiteSpace(responsavelNome))
            {
                Assert.True(resultado);
            }
            else
            {
                Assert.False(resultado);
            }
        }
    }
}