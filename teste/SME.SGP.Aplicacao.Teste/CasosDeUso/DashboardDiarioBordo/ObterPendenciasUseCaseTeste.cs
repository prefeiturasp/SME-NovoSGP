using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardDiarioBordo
{

    public class ObterPendenciasUseCaseTeste
    {
        private readonly ObterPendenciasUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public ObterPendenciasUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterPendenciasUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Pendencias()
        {
            long value = 1;
            //Arrange
            var usuarioId = mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoIdQuery>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(value);

            mediator.Setup(a => a.Send(It.IsAny<ObterPendenciasPorUsuarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PaginacaoResultadoDto<PendenciaDto>());

            //Act
            var pendencias = await useCase.Executar("1", 1, "");

            //Assert
            Assert.NotNull(usuarioId);
            Assert.NotNull(pendencias);
            

        }
    }
}
