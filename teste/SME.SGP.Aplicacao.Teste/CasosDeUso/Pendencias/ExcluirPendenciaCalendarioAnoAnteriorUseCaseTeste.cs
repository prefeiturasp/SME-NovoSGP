using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class ExcluirPendenciaCalendarioAnoAnteriorUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExcluirPendenciaCalendarioAnoAnteriorUseCase useCase;

        public ExcluirPendenciaCalendarioAnoAnteriorUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ExcluirPendenciaCalendarioAnoAnteriorUseCase(mediator.Object);
        }

        [Fact(DisplayName = "ExcluirPendenciaCalendarioAnoAnteriorUseCase - Deve executar com excluir pendência calendário ano anterior mesmo com a mensagem rabbit nula acionando a próxima fila")]
        public async Task DeveExecutarComSucessoExcluirPendenciaCalendarioAnoAnteriorComMensagemRabbitNulaAcionandoProximaFila()
        {
            var anoLetivoConsiderado = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
            var idsUe = new List<long>() { 1, 2 };

            mediator.Setup(x => x.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idsUe);

            var mensagensEsperadaProximaFilaUE = new List<ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto>()
            {
                new (anoLetivoConsiderado, 1), new(anoLetivoConsiderado, 2)
            };

            foreach (var mensagemEsperada in mensagensEsperadaProximaFilaUE)
            {
                mediator.Setup(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUes &&
                    ((ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto)y.Filtros).Equals(mensagemEsperada)), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);
            }

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);

            foreach (var mensagemEsperada in mensagensEsperadaProximaFilaUE)
            {
                mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUes &&
                    ((ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto)y.Filtros).Equals(mensagemEsperada)), It.IsAny<CancellationToken>()), Times.Once);
            }                
        }
    }
}
