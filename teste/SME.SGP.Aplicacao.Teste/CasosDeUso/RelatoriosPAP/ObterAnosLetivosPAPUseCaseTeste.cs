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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterAnosLetivosPAPUseCaseTeste
    {

        private readonly ObterAnosLetivosPAPUseCase  obterAnosLetivosPAPUseCase;
        private readonly Mock<IMediator> mediator;
        public ObterAnosLetivosPAPUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            obterAnosLetivosPAPUseCase = new ObterAnosLetivosPAPUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Consutar_Anos_Letivos_PAP()
        {
            //Arrange
            var listaRetorno = new List<ObterAnoLetivoPAPRetornoDto>();
            listaRetorno.Add(new ObterAnoLetivoPAPRetornoDto() { Ano = 2020, EhSugestao = true });

            mediator.Setup(a => a.Send(It.IsAny<ObterAnosLetivosPAPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaRetorno);

            //Act
            var anos = await obterAnosLetivosPAPUseCase.Executar();

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterAnosLetivosPAPQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(listaRetorno.Any());
        }
    }
}
