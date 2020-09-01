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
    public class ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase useCase;

        public ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Listar_Carta_Intencoes_Observacao()
        {
            //Arrange
            var mockRetorno = new List<CartaIntencoesObservacaoDto> {
                    new CartaIntencoesObservacaoDto{
                        TurmaId = 1,
                        ComponenteCurricularId = 512,
                        Proprietario = true,
                        Observacao = "Teste de Observação"
                    }
                };

            var param = new BuscaCartaIntencoesObservacaoDto("2172463", 512);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaIdPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            mediator.Setup(a => a.Send(It.IsAny<ListarCartaIntencoesObservacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetorno);


            //Act
            var retorno = await useCase.Executar(param);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ListarCartaIntencoesObservacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }
    }
}
