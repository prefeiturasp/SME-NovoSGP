using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Documento
{
    public class Ao_listar_documentos : DocumentoTesteBase
    {
        public Ao_listar_documentos(CollectionFixture collectionFixture) : base(collectionFixture) {}

        [Fact(DisplayName = "Documento - Listando documentos")]
        public async Task Ao_listar_documentos_com_turma_e_componente()
        {
            var filtro = new FiltroDocumentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            };

            await CriarDadosBasicos(filtro);

            await CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma);

            var arquivos = ObterTodos<Arquivo>();
            var documentos = ObterTodos<Dominio.Documento>();
            var classificacao = ObterTodos<Dominio.ClassificacaoDocumento>();

            var obterServicoListarDocumentosUse = ObterServicoListarDocumentosUseCase();
            var retorno = await obterServicoListarDocumentosUse.Executar(UE_ID_1, (long)Dominio.Enumerados.TipoDocumento.Documento, (long)Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma);
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBeEquivalentTo(30);
            retorno.Items.Any(a=> a.TipoDocumento.Equals(Dominio.Enumerados.TipoDocumento.Documento.GetDisplayName())).ShouldBeTrue();
            retorno.Items.Any(a=> a.TipoDocumento.Equals(Dominio.Enumerados.TipoDocumento.PlanoTrabalho.GetDisplayName())).ShouldBeFalse();
        }
    }
}