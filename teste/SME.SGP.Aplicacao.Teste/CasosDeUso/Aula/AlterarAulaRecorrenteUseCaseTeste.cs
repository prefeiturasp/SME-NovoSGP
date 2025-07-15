using System.Threading;
using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class AlterarAulaRecorrenteUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveEnviarCommandERetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var command = new AlterarAulaRecorrenteCommand
            {
                AulaId = 123,
                DataAula = DateTime.Today,
                Quantidade = 2,
                CodigoTurma = "1A",
                ComponenteCurricularId = 456,
                NomeComponenteCurricular = "Matemática",
                TipoCalendarioId = 789,
                TipoAula = TipoAula.Normal,
                CodigoUe = "UE123",
                EhRegencia = false,
                Usuario = new SME.SGP.Dominio.Usuario { CodigoRf = "123456" },
                RecorrenciaAula = new SME.SGP.Dominio.RecorrenciaAula()
            };

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<AlterarAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new AlterarAulaRecorrenteUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<AlterarAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
