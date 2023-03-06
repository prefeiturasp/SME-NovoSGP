using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.Itinerancia.Base
{
    public class ItineranciaBase  : TesteBaseComuns
    {
        public ItineranciaBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected IExcluirArquivoItineranciaUseCase ExcluirArquivoItineranciaUseCase()
        {
            return ServiceProvider.GetService<IExcluirArquivoItineranciaUseCase>();
        }
        
        protected IUploadDeArquivoItineranciaUseCase UploadDeArquivoItineranciaUseCase()
        {
            return ServiceProvider.GetService<IUploadDeArquivoItineranciaUseCase>();
        }        
        protected IExcluirArmazenamentoPorAquivoUseCase ExcluirArmazenamentoPorAquivoUseCase()
        {
            return ServiceProvider.GetService<IExcluirArmazenamentoPorAquivoUseCase>();
        }

        protected async Task<IFormFile> GerarAquivoFakeParaUpload(string nomeArquivo = "arquivo.png",string extensaoArquivo = "image/png")
        {
            var fileMock = new Mock<IFormFile>();
            var content = "Fake File";
            var fileName = nomeArquivo;
            var contentType = extensaoArquivo;
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            await writer.WriteAsync(content);
            await writer.FlushAsync();
            
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns(contentType);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            return fileMock.Object;
        }
    }
}