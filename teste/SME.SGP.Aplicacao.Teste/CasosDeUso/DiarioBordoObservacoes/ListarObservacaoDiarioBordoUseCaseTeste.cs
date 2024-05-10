using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ListarObservacaoDiarioBordoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ListarObservacaoDiarioBordoUseCase listarObservacaoDiarioBordoUseCase;

        public ListarObservacaoDiarioBordoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            listarObservacaoDiarioBordoUseCase = new ListarObservacaoDiarioBordoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Excluir_Observacao_Diario_De_Bordo()
        {
            //Arrange
            var mockRetorno = new List<ListarObservacaoDiarioBordoDto> {
                    new ListarObservacaoDiarioBordoDto{
                        Observacao="teste",
                        Proprietario=true
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ListarObservacaoDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetorno);

            //Act
            var retorno = await listarObservacaoDiarioBordoUseCase.Executar(1);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ListarObservacaoDiarioBordoQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }
    }
}
