using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
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

            await CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138), ueId: UE_ID_1);
            var qdadeDocumentos = ObterTodos<Dominio.Documento>().Count;
            var qdadeArquivos = ObterTodos<Dominio.Arquivo>().Count;
            await CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma, 
                                  long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138), ueId: UE_ID_2, 
                                  documentoId: qdadeDocumentos+1,
                                  arquivoId: qdadeArquivos+1);
            qdadeDocumentos = ObterTodos<Dominio.Documento>().Count;
            qdadeArquivos = ObterTodos<Dominio.Arquivo>().Count;
            await CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma, 
                                  long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138), ueId: UE_ID_3,
                                  documentoId: qdadeDocumentos + 1,
                                  arquivoId: qdadeArquivos + 1);

            var obterServicoListarDocumentosUse = ObterServicoListarDocumentosUseCase();
            //Filtro por Ue
            await ValidarListagemDocumentos_Turma_Componente(obterServicoListarDocumentosUse, 3, ueId: UE_ID_1);
            //Filtro por Dre
            await ValidarListagemDocumentos_Turma_Componente(obterServicoListarDocumentosUse, 6, dreId: DRE_ID_2);
            //Filtro TODOS Dre e Ue
            await ValidarListagemDocumentos_Turma_Componente(obterServicoListarDocumentosUse, 9);
        }

        private async Task ValidarListagemDocumentos_Turma_Componente(IListarDocumentosUseCase obterServicoListarDocumentosUse, int qdadeRegistrosAssert, long? dreId = null, long? ueId = null)
        {
            var retorno = await obterServicoListarDocumentosUse.Executar(
                new Dto.FiltroListagemDocumentosDto()
                {
                    UeId = ueId,
                    DreId = dreId,
                    TipoDocumentoId = (long)Dominio.Enumerados.TipoDocumento.Documento,
                    ClassificacaoId = (long)Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma
                });
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBeEquivalentTo(qdadeRegistrosAssert);
            retorno.Items.Any(a => a.Classificacao.Equals(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma.GetDisplayName())).ShouldBeTrue();
            retorno.Items.Any(a => a.Classificacao.Equals(Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica.GetDisplayName())).ShouldBeFalse();
            retorno.Items.Any(a => !string.IsNullOrEmpty(a.TurmaComponenteCurricular)).ShouldBeTrue();
            retorno.Items.Any(c => c.Arquivos.Any(a => string.IsNullOrEmpty(a.Nome))).ShouldBeFalse();
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