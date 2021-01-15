using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Dominio;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class UploadDocumentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly UploadDocumentoUseCase useCase;
        private Mock<IFormFile> fileMock;

        public UploadDocumentoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new UploadDocumentoUseCase(mediator.Object);
            fileMock = new Mock<IFormFile>();
        }

        [Fact]
        public async Task Deve_Fazer_Upload_PDF()
        {
            //var fileNewName = Guid.NewGuid();

            //var physicalFile = new FileInfo("ArquivosTestes\\plano_de_aula.pdf");
            //var ms = new MemoryStream();
            //var writer = new StreamWriter(ms);
            //writer.Write(physicalFile.OpenRead());
            //writer.Flush();
            //ms.Position = 0;
            //var fileName = physicalFile.Name;

            ////Setup mock file using info from physical file
            //fileMock.Setup(_ => _.FileName).Returns(fileName);
            //fileMock.Setup(_ => _.Length).Returns(ms.Length);
            //fileMock.Setup(_ => _.ContentType).Returns("application/pdf");
            //fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            //fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));



            //mediator.Setup(a => a.Send(It.IsAny<UploadArquivoCommand>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(fileNewName);

            ////Act
            //var file = fileMock.Object;
            //var retorno = await useCase.Executar(file, TipoConteudoArquivo.PDF);

            ////Asert
            //mediator.Verify(x => x.Send(It.IsAny<UploadArquivoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            //Assert.IsType<Guid>(retorno);
        }
    }
}
