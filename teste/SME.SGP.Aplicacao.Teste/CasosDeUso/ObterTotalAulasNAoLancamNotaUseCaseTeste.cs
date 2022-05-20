using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterTotalAulasNAoLancamNotaUseCaseTeste
    {
        private readonly ObterTotalAulasNaoLancamNotaUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public ObterTotalAulasNAoLancamNotaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterTotalAulasNaoLancamNotaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Total_Aulas_Nao_Lancam_Nota()
        {
            //Arange
            mediator.Setup(x => x.Send(It.IsAny<ObterTotalAulasNaoLancamNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TotalAulasNaoLancamNotaDto>());
            //Act 
            var totalAulas = await useCase.Executar("2370993", 1,"");

            //Assert
            Assert.NotNull(totalAulas);
        }
    }
}
