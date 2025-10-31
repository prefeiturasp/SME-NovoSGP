using MediatR;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Dre
{
    public class ObterDresUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDresUseCase _useCase;

        public ObterDresUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDresUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Query_E_Retornar_Lista_De_Dres()
        {
            var listaDresRetorno = new List<SME.SGP.Dominio.Dre>
            {
                new SME.SGP.Dominio.Dre { Nome = "DIRETORIA REGIONAL DE EDUCACAO CAPELA DO SOCORRO", CodigoDre = "108300" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaDresRetorno);

            var resultado = await _useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(listaDresRetorno.First().CodigoDre, resultado.First().CodigoDre);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
