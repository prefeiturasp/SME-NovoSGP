using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Documento
{
    public class Ao_salvar_documento : DocumentoTesteBase
    {
        public Ao_salvar_documento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Deve salvar com mais de um arquivo para o tipo Documento da Turma")]
        public async Task Deve_salvar_com_mais_de_um_arquivo_tipo_documento_turma()
        {
            var filtro = new FiltroDocumentoDto
            {
                Modalidade = Modalidade.Medio,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };
            
            await CriarDadosBasicos(filtro);

            var useCase = ObterServicoSalvarDocumentoUseCase();

            var ues = ObterTodos<Ue>();
            ues.ShouldNotBeNull();

            var ueId = ues.FirstOrDefault(c => c.CodigoUe == UE_CODIGO_1)?.Id;
            ueId.ShouldNotBeNull();

            var usuarios = ObterTodos<Usuario>();
            usuarios.ShouldNotBeNull();

            var usuarioId = usuarios.FirstOrDefault(c => c.Login == USUARIO_PROFESSOR_LOGIN_2222222)?.Id;
            usuarioId.ShouldNotBeNull();

            var turmas = ObterTodos<Turma>();
            var turmaId = turmas.FirstOrDefault(c => c.CodigoTurma == TURMA_CODIGO_1)?.Id;

            var arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();

            var arquivosCodigos = arquivos.Select(c => c.Codigo).ToArray();
            
            var salvarDocumento = new SalvarDocumentoDto
            {
                UeId = ueId.GetValueOrDefault(),
                AnoLetivo = 2022,
                TipoDocumentoId = (int)Dominio.Enumerados.TipoDocumento.Documento,
                ClassificacaoId = (int)Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma,
                UsuarioId = usuarioId.GetValueOrDefault(),
                ArquivosCodigos = arquivosCodigos,
                TurmaId = turmaId,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            (await useCase.Executar(salvarDocumento)).ShouldBeTrue();
        }
    }
}