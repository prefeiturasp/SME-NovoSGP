using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Documento
{
    public class Ao_excluir_documento : DocumentoTesteBase
    {
        public Ao_excluir_documento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Documento - Deve excluir um documento e suas dependências em documento_arquivo e arquivo")]
        public async Task Deve_excluir_um_documento_suas_dependencias_em_documento_arquivo_arquivo()
        {
            var filtro = new FiltroDocumentoDto
            {
                Modalidade = Modalidade.Medio,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };
            
            await CriarDadosBasicos(filtro);
            
            await CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));

            var documentos = ObterTodos<Dominio.Documento>();
            documentos.ShouldNotBeNull();
            documentos.Count.ShouldBeEquivalentTo(3);

            var documentosArquivos = ObterTodos<DocumentoArquivo>();
            documentosArquivos.ShouldNotBeNull();
            documentosArquivos.Count.ShouldBeEquivalentTo(30);
            
            var arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();
            arquivos.Count.ShouldBeEquivalentTo(40);

            var excluirDocumentoUseCase = ObterServicoExcluirDocumentoUseCase();
            var retorno = await excluirDocumentoUseCase.Executar(1);
            retorno.ShouldBeTrue();
            
            documentos = ObterTodos<Dominio.Documento>();
            documentos.ShouldNotBeNull();
            documentos.Count.ShouldBeEquivalentTo(2);
            
            documentosArquivos = ObterTodos<DocumentoArquivo>();
            documentosArquivos.ShouldNotBeNull();
            documentosArquivos.Count.ShouldBeEquivalentTo(20);
            
            arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();
            arquivos.Count.ShouldBeEquivalentTo(30);
        }
        
        [Fact(DisplayName = "Documento - Deve excluir todos os documentos e suas dependências em documento_arquivo e arquivo")]
        public async Task Deve_excluir_todos_documentos_suas_dependencias_em_documento_arquivo_arquivo()
        {
            var filtro = new FiltroDocumentoDto
            {
                Modalidade = Modalidade.Medio,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };
            
            await CriarDadosBasicos(filtro);
            
            await CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));

            var documentos = ObterTodos<Dominio.Documento>();
            documentos.ShouldNotBeNull();
            documentos.Count.ShouldBeEquivalentTo(3);

            var documentosArquivos = ObterTodos<DocumentoArquivo>();
            documentosArquivos.ShouldNotBeNull();
            documentosArquivos.Count.ShouldBeEquivalentTo(30);
            
            var arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();
            arquivos.Count.ShouldBeEquivalentTo(40);

            //Excluir documento 1
            var excluirDocumentoUseCase = ObterServicoExcluirDocumentoUseCase();
            var retorno = await excluirDocumentoUseCase.Executar(1);
            retorno.ShouldBeTrue();
            
            documentos = ObterTodos<Dominio.Documento>();
            documentos.ShouldNotBeNull();
            documentos.Count.ShouldBeEquivalentTo(2);
            
            documentosArquivos = ObterTodos<DocumentoArquivo>();
            documentosArquivos.ShouldNotBeNull();
            documentosArquivos.Count.ShouldBeEquivalentTo(20);
            
            arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();
            arquivos.Count.ShouldBeEquivalentTo(30);
            
            //Excluir documento 2
            retorno = await excluirDocumentoUseCase.Executar(2);
            retorno.ShouldBeTrue();
            
            documentos = ObterTodos<Dominio.Documento>();
            documentos.ShouldNotBeNull();
            documentos.Count.ShouldBeEquivalentTo(1);
            
            documentosArquivos = ObterTodos<DocumentoArquivo>();
            documentosArquivos.ShouldNotBeNull();
            documentosArquivos.Count.ShouldBeEquivalentTo(10);
            
            arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();
            arquivos.Count.ShouldBeEquivalentTo(20);
            
            //Excluir documento 3
            retorno = await excluirDocumentoUseCase.Executar(3);
            retorno.ShouldBeTrue();
            
            documentos = ObterTodos<Dominio.Documento>();
            documentos.ShouldNotBeNull();
            documentos.Count.ShouldBeEquivalentTo(0);
            
            documentosArquivos = ObterTodos<DocumentoArquivo>();
            documentosArquivos.ShouldNotBeNull();
            documentosArquivos.Count.ShouldBeEquivalentTo(0);
            
            arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();
            arquivos.Count.ShouldBeEquivalentTo(10);//Inicial
        }
    }
}