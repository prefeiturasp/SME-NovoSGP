using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ExcluirDocumentoArquivoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExcluirDocumentoArquivoUseCase useCase;

        public ExcluirDocumentoArquivoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ExcluirDocumentoArquivoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Excluir_O_Arquivo()
        {
            var fileNameGuid = Guid.NewGuid();

            var file = new Arquivo()
            {
                Id = 1,
                Codigo = fileNameGuid
            };

            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterArquivoPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(file);

            mediator.Setup(a => a.Send(It.IsAny<ExcluirDocumentoArquivoUseCase>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            //Act
            var retorno = await useCase.Executar((1, fileNameGuid));

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterArquivoPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno);
        }
    }
}
