using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Recorrencias
{
    public class ExecutarGravarRecorrenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IComandosEvento> comandosEventoMock;
        private readonly ExecutarGravarRecorrenciaUseCase useCase;

        public ExecutarGravarRecorrenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            comandosEventoMock = new Mock<IComandosEvento>();
            useCase = new ExecutarGravarRecorrenciaUseCase(mediatorMock.Object, comandosEventoMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator",
                () => new ExecutarGravarRecorrenciaUseCase(null, comandosEventoMock.Object));
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Chamar_Comando_E_Retornar_True()
        {
            var dto = new EventoDto { Nome = "Teste Evento" };
            var evento = new Dominio.Evento { Nome = "Evento Dominio" };
            var parametro = new ExecutarGravarRecorrenciaUseCase.ExecutarGravarRecorrenciaParametro
            {
                Dto = dto,
                Evento = evento
            };

            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(parametro));

            comandosEventoMock.Setup(c => c.GravarRecorrencia(
                It.IsAny<EventoDto>(),
                It.IsAny<Dominio.Evento>()))
                .Returns(Task.CompletedTask);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            comandosEventoMock.Verify(c => c.GravarRecorrencia(
                It.Is<EventoDto>(e => e.Nome == dto.Nome),
                It.Is<Dominio.Evento>(e => e.Nome == evento.Nome)),
                Times.Once);
        }
    }
}
