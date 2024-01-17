using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Extensions;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Documento
{
    public class Ao_listar_documentos : DocumentoTesteBase
    {
        public Ao_listar_documentos(CollectionFixture collectionFixture) : base(collectionFixture) {}

        [Fact(DisplayName = "Documento - Listando documentos com turma e componente")]
        public async Task Ao_listar_documentos_com_turma_e_componente()
        {
            var filtro = new FiltroDocumentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            };

            await CriarDadosBasicos(filtro);

            await CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));

            var obterServicoListarDocumentosUse = ObterServicoListarDocumentosUseCase();
            var retorno = await obterServicoListarDocumentosUse.Executar(
                new Dto.FiltroListagemDocumentosDto() 
                {
                    UeId = UE_ID_1,
                    TipoDocumentoId = (long)Dominio.Enumerados.TipoDocumento.Documento,
                    ClassificacaoId = (long)Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma
                });
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBeEquivalentTo(3);
            retorno.Items.Any(a=> a.Classificacao.Equals(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma.GetDisplayName())).ShouldBeTrue();
            retorno.Items.Any(a=> a.Classificacao.Equals(Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica.GetDisplayName())).ShouldBeFalse();
            retorno.Items.Any(a=> !string.IsNullOrEmpty(a.TurmaComponenteCurricular)).ShouldBeTrue();
            retorno.Items.Any(c => c.Arquivos.Any(a=> string.IsNullOrEmpty(a.Nome))).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Documento - Listando documentos sem turma e componente")]
        public async Task Ao_listar_documentos_sem_turma_e_componente()
        {
            var filtro = new FiltroDocumentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            };

            await CriarDadosBasicos(filtro);

            await CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica);

            var obterServicoListarDocumentosUse = ObterServicoListarDocumentosUseCase();
            var retorno = await obterServicoListarDocumentosUse.Executar(
                new Dto.FiltroListagemDocumentosDto()
                {
                    UeId = UE_ID_1, 
                    TipoDocumentoId = (long)Dominio.Enumerados.TipoDocumento.Documento, 
                    ClassificacaoId = (long)Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica
                });
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBeEquivalentTo(3);
            retorno.Items.Any(a=> a.Classificacao.Equals(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma.GetDisplayName())).ShouldBeFalse();
            retorno.Items.Any(a=> a.Classificacao.Equals(Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica.GetDisplayName())).ShouldBeTrue();
            retorno.Items.Any(a=> string.IsNullOrEmpty(a.TurmaComponenteCurricular)).ShouldBeTrue();
            retorno.Items.Any(c => c.Arquivos.Any(a=> string.IsNullOrEmpty(a.Nome))).ShouldBeFalse();
        }
    }
}