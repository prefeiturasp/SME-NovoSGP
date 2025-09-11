using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ParecerConclusivo
{
    public class ReprocessarParecerConclusivoAlunoUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveEnviarComandoECancelarComSucesso()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var dto = new ConselhoClasseFechamentoAlunoDto
            {
                ConselhoClasseId = 123,
                FechamentoTurmaId = 456,
                AlunoCodigo = "789"
            };

            var json = JsonConvert.SerializeObject(dto);
            var mensagemRabbit = new MensagemRabbit(json);

            // Configura o comportamento do Mediator
            mediatorMock.Setup(m => m.Send(
                It.IsAny<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParecerConclusivoDto
                {
                   Id = 123,
                });

            var useCase = new ReprocessarParecerConclusivoAlunoUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>(cmd =>
                    cmd.ConselhoClasseId == dto.ConselhoClasseId &&
                    cmd.FechamentoTurmaId == dto.FechamentoTurmaId &&
                    cmd.AlunoCodigo == dto.AlunoCodigo),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
