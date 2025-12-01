using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class ExcluirAulaRecorrenteUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveEnviarCommandERetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var command = new ExcluirAulaRecorrenteCommand(
                aulaId: 987,
                recorrenciaAula: new RecorrenciaAula(), // você pode preencher se necessário
                componenteCurricularNome: "Matemática",
                usuario: new Usuario { CodigoRf = "123456", Nome = "Professor Teste" }
            );

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new ExcluirAulaRecorrenteUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
