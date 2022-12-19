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

            await CriarDocumentos(Dominio.Enumerados.TipoDocumento.Documento);

            var arquivos = ObterTodos<Arquivo>();
            var documentos = ObterTodos<Dominio.Documento>();

            var obterServicoListarDocumentosUse = ObterServicoListarDocumentosUseCase();
            var retorno = await obterServicoListarDocumentosUse.Executar(UE_ID_1, (long)Dominio.Enumerados.TipoDocumento.Documento, (long)Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma);

            // var verificarExistenciaPlanoAee = ObterServicoVerificarExistenciaPlanoAEEPorEstudanteUseCase();
            // var ex = await Assert.ThrowsAsync<NegocioException>(() => verificarExistenciaPlanoAee.Executar(aluno.Items.FirstOrDefault().Codigo));
            // ex.Message.ShouldNotBeNullOrEmpty();
        }
    }
}