using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class UploadArquivoCommandTeste
    {
        private readonly UploadArquivoCommandHandler uploadArquivoCommandHandler;
        private UploadArquivoCommand uploadArquivoCommand;
        private readonly Mock<IMediator> mediator;
        private Mock<IFormFile> fileMock;

        public UploadArquivoCommandTeste()
        {
            mediator = new Mock<IMediator>();
            fileMock = new Mock<IFormFile>();
            uploadArquivoCommandHandler = new UploadArquivoCommandHandler(mediator.Object);
        }

        [Fact(DisplayName = "Deve Aceitar o Tipo de Arquivo PDF")]
        public async void Deve_Aceitar_Arquivo()
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
            //fileMock.Setup(_ => _.ContentType).Returns("application/pdf");
            //fileMock.Setup(_ => _.Length).Returns(ms.Length);
            //fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            //fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));

            //var file = fileMock.Object;
            //uploadArquivoCommand = new UploadArquivoCommand(file, TipoArquivo.Geral, TipoConteudoArquivo.PDF);

            //mediator.Setup(a => a.Send(It.IsAny<ArmazenarArquivoFisicoCommand>(), It.IsAny<CancellationToken>()))
            //   .ReturnsAsync(true);

            //mediator.Setup(a => a.Send(It.IsAny<UploadArquivoCommand>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(fileNewName);

            //mediator.Setup(a => a.Send(It.IsAny<SalvarArquivoRepositorioCommand>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(new ArquivoArmazenadoDto() { Id = 1, Codigo = fileNewName});

            //var retorno = await uploadArquivoCommandHandler.Handle(uploadArquivoCommand, new CancellationToken());
            
            ////Asert
            //mediator.Verify(x => x.Send(It.IsAny<SalvarArquivoRepositorioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            //mediator.Verify(x => x.Send(It.IsAny<ArmazenarArquivoFisicoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            //Assert.IsType<Guid>(retorno);

        }

        [Fact(DisplayName = "Não Aceitar o Tipo de Arquivo TXT")]
        public async void Nao_Deve_Aceitar_Arquivo()
        {
            //var fileNewName = Guid.NewGuid();

            //var physicalFile = new FileInfo("ArquivosTestes\\arquivo_teste.txt");
            //var ms = new MemoryStream();
            //var writer = new StreamWriter(ms);
            //writer.Write(physicalFile.OpenRead());
            //writer.Flush();
            //ms.Position = 0;
            //var fileName = physicalFile.Name;

            ////Setup mock file using info from physical file
            //fileMock.Setup(_ => _.FileName).Returns(fileName);
            //fileMock.Setup(_ => _.ContentType).Returns("text/plain");
            //fileMock.Setup(_ => _.Length).Returns(ms.Length);
            //fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            //fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));

            //var file = fileMock.Object;
            //uploadArquivoCommand = new UploadArquivoCommand(file, TipoArquivo.Geral, TipoConteudoArquivo.PDF);

            //await Assert.ThrowsAsync<NegocioException>(async () => await uploadArquivoCommandHandler.Handle(uploadArquivoCommand, new CancellationToken()));

        }

    }
}