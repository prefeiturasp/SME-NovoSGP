using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoFrequenciaUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificacaoFrequenciaUeUseCase useCase;

        public NotificacaoFrequenciaUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificacaoFrequenciaUeUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Periodo_Encerrado_Existir_Deve_Enviar_Comando_E_Retornar_True_Teste()
        {
            var periodo = new PeriodoEscolar
            {
                Id = 1,
                PeriodoInicio = DateTime.Now.AddDays(-10),
                PeriodoFim = DateTime.Now.AddDays(-1),
                Bimestre = 1,
                TipoCalendarioId = 1
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(periodo);

            mediatorMock.Setup(x => x.Send(It.IsAny<NotificaFrequenciaPeriodoUeCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificaFrequenciaPeriodoUeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Periodo_Encerrado_Nao_Existir_Deve_Retornar_True_Sem_Enviar_Comando_Teste()
        {
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((PeriodoEscolar)null);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificaFrequenciaPeriodoUeCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
