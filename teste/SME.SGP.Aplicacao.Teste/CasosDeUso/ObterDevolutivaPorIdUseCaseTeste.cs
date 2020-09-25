using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterDevolutivaPorIdUseCaseTeste
    {
        private readonly ObterDevolutivaPorIdUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public ObterDevolutivaPorIdUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDevolutivaPorIdUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Devolutiva()
        {
            // Arrange 
            mediator.Setup(a => a.Send(It.IsAny<ObterDevolutivaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Devolutiva() { Id = 1, Descricao = "teste", CriadoEm = DateTime.Today });

            mediator.Setup(a => a.Send(It.IsAny<ObterIdsDiariosBordoPorDevolutivaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long> { 1, 2 });

            // Act
            var devolutiva = await useCase.Executar(1);

            // Assert
            Assert.NotNull(devolutiva);
            Assert.NotNull(devolutiva.Auditoria);
            Assert.True(devolutiva.DiariosIds.Count() == 2);
        }
    }
}
