using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Itinerancia.Base;
using SME.SGP.TesteIntegracao.Itinerancia.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Itinerancia
{
    public class Ao_excluir_arquivo : ItineranciaBase
    {
        public Ao_excluir_arquivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ExcluirArquivoMinioCommand, bool>), typeof(ExcluirArquivoMinioCommandHandlerFakeTrue), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Registro de itinerância - Excluir Arquivo Existente")]
        public async Task Excluir_arquivo_existente()
        {
            var codigoArquivo = await CriarArquivoParaExclusao();
            var useCase = ExcluirArquivoItineranciaUseCase();

            var arquivoAntesDeExcluir = ObterTodos<Arquivo>();
            arquivoAntesDeExcluir.Count.ShouldBeEquivalentTo(1);
            arquivoAntesDeExcluir.FirstOrDefault()?.Codigo.ShouldBeEquivalentTo(codigoArquivo);
            
            await useCase.Executar(codigoArquivo);

            var arquivoDepoisDeExcluir = ObterTodos<Arquivo>();
            arquivoDepoisDeExcluir.Count.ShouldBeEquivalentTo(0);
        }
        
        [Fact(DisplayName = "Registro de itinerância - Excluir Arquivo Não Existente")]
        public async Task Excluir_arquivo_nao_existente()
        {
            var codigoArquivo = Guid.NewGuid();
            var useCase = ExcluirArquivoItineranciaUseCase();

            var arquivoAntesDeExcluir = ObterTodos<Arquivo>();
            arquivoAntesDeExcluir.Count.ShouldBeEquivalentTo(0);
            
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(codigoArquivo));
        }

        private async Task<Guid> CriarArquivoParaExclusao()
        {
            var useCase = UploadDeArquivoItineranciaUseCase();
            var file = await GerarAquivoFakeParaUpload();
            var salvar = await useCase.Executar(file);
            return salvar.Codigo;
        }
    }
}