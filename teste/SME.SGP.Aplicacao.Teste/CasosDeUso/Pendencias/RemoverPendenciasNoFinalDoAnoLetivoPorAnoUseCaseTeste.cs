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
    public class RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase useCase;

        public RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase(mediator.Object);
        }

        [Fact(DisplayName = "RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase - Deve executar a rotina mesmo com a mesnagem rabbit nula e efetuar a chamada para a fila de exclusão de pendência por UE")]
        public async Task DeveExecutarRemoverPendenciasNoFinalDoAnoLetivoPorAnoComMensagemRabbitNula()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterIdsDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long>() { 1 });

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);

            var filtros = new FiltroRemoverPendenciaFinalAnoLetivoDto(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 1);

            mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorUe &&
                ((FiltroRemoverPendenciaFinalAnoLetivoDto)y.Filtros).Equals(filtros)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
