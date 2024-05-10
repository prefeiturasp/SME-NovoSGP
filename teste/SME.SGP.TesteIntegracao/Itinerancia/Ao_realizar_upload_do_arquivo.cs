using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Itinerancia.Base;
using SME.SGP.TesteIntegracao.Itinerancia.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Itinerancia
{
    public class Ao_realizar_upload_do_arquivo : ItineranciaBase
    {
        public Ao_realizar_upload_do_arquivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ArmazenarArquivoFisicoCommand, string>),typeof(ArmazenarArquivoFisicoCommandHandlerFake), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Registro de itinerância - Realizar Upload De Arquivo PDF")]
        public async Task Realizar_upload_arquivo_pdf()
        {

            var useCase = UploadDeArquivoItineranciaUseCase();
            var nomeArquivo = "Arquivo.pdf";
            var extensaoArquivo = "application/pdf";
            var file = await GerarAquivoFakeParaUpload(nomeArquivo,extensaoArquivo);
            
            var salvar =  await useCase.Executar(file);

            var arquivos = ObterTodos<Arquivo>();
            
            ValidarTeste(arquivos, salvar, nomeArquivo, extensaoArquivo);
        }
       
        [Fact(DisplayName = "Registro de itinerância - Realizar Upload De Arquivo PNG")]
        public async Task Realizar_upload_arquivo_png()
        {

            var useCase = UploadDeArquivoItineranciaUseCase();
            var nomeArquivo = "Arquivo.png";
            var extensaoArquivo = "image/png";
            var file = await GerarAquivoFakeParaUpload(nomeArquivo,extensaoArquivo);
            
            var salvar =  await useCase.Executar(file);

            var arquivos = ObterTodos<Arquivo>();
            
            ValidarTeste(arquivos, salvar, nomeArquivo, extensaoArquivo);
        }
        
        [Fact(DisplayName = "Registro de itinerância - Realizar Upload De Arquivo JPEG")]
        public async Task Realizar_upload_arquivo_jpeg()
        {

            var useCase = UploadDeArquivoItineranciaUseCase();
            var nomeArquivo = "Arquivo.jpeg";
            var extensaoArquivo = "image/jpeg";
            var file = await GerarAquivoFakeParaUpload(nomeArquivo,extensaoArquivo);
            
            var salvar =  await useCase.Executar(file);

            var arquivos = ObterTodos<Arquivo>();
            
            ValidarTeste(arquivos, salvar, nomeArquivo, extensaoArquivo);
        }
        
        [Fact(DisplayName = "Registro de itinerância - Realizar Upload De Arquivo MP4")]
        public async Task Realizar_upload_arquivo_mp4()
        {

            var useCase = UploadDeArquivoItineranciaUseCase();
            var nomeArquivo = "Arquivo.mp4";
            var extensaoArquivo = "video/mp4";
            var file = await GerarAquivoFakeParaUpload(nomeArquivo,extensaoArquivo);
            
            var salvar =  await useCase.Executar(file);

            var arquivos = ObterTodos<Arquivo>();
            
            ValidarTeste(arquivos, salvar, nomeArquivo, extensaoArquivo);
        }
        private static void ValidarTeste(List<Arquivo> arquivos, ArquivoArmazenadoItineranciaDto salvar, string nomeArquivo, string extensaoArquivo)
        {
            arquivos.Count.ShouldBeEquivalentTo(1);
            arquivos.FirstOrDefault()?.Codigo.ShouldBeEquivalentTo(salvar.Codigo);
            arquivos.FirstOrDefault()?.Nome.ShouldBeEquivalentTo(nomeArquivo);
            arquivos.FirstOrDefault()?.TipoConteudo.ShouldBeEquivalentTo(extensaoArquivo);
            arquivos.FirstOrDefault()?.Tipo.ShouldBeEquivalentTo(TipoArquivo.Itinerancia);
        }
    }
}