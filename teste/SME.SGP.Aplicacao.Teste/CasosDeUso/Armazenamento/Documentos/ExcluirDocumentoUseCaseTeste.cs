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

        public ExcluirDocumentoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ExcluirDocumentoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Excluir_o_Documento_e_Arquivo()
        {
            var fileNameGuid = Guid.NewGuid();

            var file = new Arquivo()
            {
                Id = 1,
                Codigo = fileNameGuid
            };

            var documento = new Documento()
            {
                Id = 1,
                ArquivoId = 1
            };

            //fileNameGuid

            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterDocumentoPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(documento);

            mediator.Setup(a => a.Send(It.IsAny<ObterArquivoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(file);

            mediator.Setup(a => a.Send(It.IsAny<ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ExcluirDocumentoPorIdCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            //Act
            var retorno = await useCase.Executar(1);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterDocumentoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterArquivoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ExcluirDocumentoPorIdCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno);
        }

        [Fact]
        public async Task Deve_Excluir_Somente_O_Documento()
        {
            var fileNameGuid = Guid.NewGuid();

            var documento = new Documento()
            {
                Id = 1,
                ArquivoId = null
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
