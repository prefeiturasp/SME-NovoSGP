using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ExcluirDocumentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExcluirDocumentoUseCase useCase;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ExcluirDocumentoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            unitOfWork = new Mock<IUnitOfWork>();
            useCase = new ExcluirDocumentoUseCase(mediator.Object,unitOfWork.Object);
        }

        [Fact]
        public async Task Deve_Excluir_Somente_O_Documento()
        {
            var fileNameGuid = Guid.NewGuid();

            var documento = new Documento()
            {
                Id = 1,
            };

            //fileNameGuid

            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterDocumentoPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(documento);

            mediator.Setup(a => a.Send(It.IsAny<ExcluirDocumentoPorIdCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            //Act
            var retorno = await useCase.Executar(1);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterDocumentoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterArquivoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(x => x.Send(It.IsAny<ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(x => x.Send(It.IsAny<ExcluirDocumentoPorIdCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno);
        }
    }
}
