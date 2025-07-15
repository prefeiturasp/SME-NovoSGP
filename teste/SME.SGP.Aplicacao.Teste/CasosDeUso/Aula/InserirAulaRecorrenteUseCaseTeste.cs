using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class InserirAulaRecorrenteUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveEnviarCommandValidoERetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var command = new InserirAulaRecorrenteCommand
            {
                DataAula = DateTime.Today.AddDays(1),
                Quantidade = 2,
                CodigoTurma = "3C",
                ComponenteCurricularId = 1010,
                NomeComponenteCurricular = "Ciências",
                TipoCalendarioId = 2025,
                TipoAula = TipoAula.Normal,
                CodigoUe = "UE987",
                EhRegencia = false,
                Usuario = new Usuario
                {
                    CodigoRf = "654321",
                    Nome = "Maria Professora"
                },
                RecorrenciaAula = new RecorrenciaAula()
            };

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<InserirAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new InserirAulaRecorrenteUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<InserirAulaRecorrenteCommand>(c =>
                c.CodigoTurma == command.CodigoTurma &&
                c.ComponenteCurricularId == command.ComponenteCurricularId &&
                c.Usuario.CodigoRf == command.Usuario.CodigoRf
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
