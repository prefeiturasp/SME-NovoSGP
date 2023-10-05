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

        [Fact(DisplayName = "Documento - Deve salvar com mais de um arquivo para a classificacao de documento que permite múltiplos arquivos")]
        public async Task Deve_salvar_com_mais_de_um_arquivo_classificacao_documento_permite_multiplos_arquivos()
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

            var turmas = ObterTodos<Dominio.Turma>();
            var turmaId = turmas.FirstOrDefault(c => c.CodigoTurma == TURMA_CODIGO_1)?.Id;

            var arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();
            
            var arquivosCodigos = arquivos.Select(c => c.Codigo).ToArray();
            arquivosCodigos.Any().ShouldBeTrue();
            arquivosCodigos.Length.ShouldBeGreaterThan(1);

            var classificacoesDocumentos = ObterTodos<ClassificacaoDocumento>();
            classificacoesDocumentos.ShouldNotBeNull();

            var classificacao = classificacoesDocumentos.FirstOrDefault(c =>
                c.Id == (int)Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma);

            classificacao.ShouldNotBeNull();
            classificacao.EhRegistroMultiplo.ShouldBeTrue();

            var salvarDocumento = new SalvarDocumentoDto
            {
                UeId = ueId.GetValueOrDefault(),
                AnoLetivo = 2022,
                TipoDocumentoId = classificacao.TipoDocumentoId,
                ClassificacaoId = classificacao.Id,
                UsuarioId = usuarioId.GetValueOrDefault(),
                ArquivosCodigos = arquivosCodigos,
                TurmaId = turmaId,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            (await useCase.Executar(salvarDocumento)).ShouldBeTrue();

            var documentos = ObterTodos<Dominio.Documento>();
            documentos.ShouldNotBeNull();

            var documentoSalvo = documentos.FirstOrDefault();
            documentoSalvo.ShouldNotBeNull();

            var documentosArquivos = ObterTodos<DocumentoArquivo>();
            documentosArquivos.ShouldNotBeNull();

            var arquivosDoDocumento = documentosArquivos.Where(c => c.DocumentoId == documentoSalvo.Id).ToList();
            arquivosDoDocumento.ShouldNotBeNull();
            arquivosDoDocumento.Count.ShouldBe(10);
        }
        
        [Fact(DisplayName = "Documento - Deve falhar ao salvar com mais de um arquivo para a classificacao de documento que não permite múltiplos arquivos")]
        public async Task Deve_falhar_salvar_com_mais_de_um_arquivo_classificacao_documento_nao_permite_multiplos_arquivos()
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

            var turmas = ObterTodos<Dominio.Turma>();
            var turmaId = turmas.FirstOrDefault(c => c.CodigoTurma == TURMA_CODIGO_1)?.Id;

            var arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();

            var arquivosCodigos = arquivos.Select(c => c.Codigo).ToArray();
            arquivosCodigos.Any().ShouldBeTrue();
            arquivosCodigos.Length.ShouldBeGreaterThan(1);

            var classificacoesDocumentos = ObterTodos<ClassificacaoDocumento>();
            classificacoesDocumentos.ShouldNotBeNull();

            var classificacao = classificacoesDocumentos.FirstOrDefault(c =>
                c.Id == (int)Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica);

            classificacao.ShouldNotBeNull();
            classificacao.EhRegistroMultiplo.ShouldBeFalse();

            var salvarDocumento = new SalvarDocumentoDto
            {
                UeId = ueId.GetValueOrDefault(),
                AnoLetivo = 2022,
                TipoDocumentoId = classificacao.TipoDocumentoId,
                ClassificacaoId = classificacao.Id,
                UsuarioId = usuarioId.GetValueOrDefault(),
                ArquivosCodigos = arquivosCodigos,
                TurmaId = turmaId,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            
            async Task DoExecutarSalvar()
            {
                await useCase.Executar(salvarDocumento);
            }

            await Should.ThrowAsync<NegocioException>(DoExecutarSalvar);            
        }
        
        [Fact(DisplayName = "Documento - Deve salvar com um arquivo para a classificacao de documento que não permite múltiplos arquivos")]
        public async Task Deve_salvar_com_um_arquivo_classificacao_documento_nao_permite_multiplos_arquivos()
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

            var turmas = ObterTodos<Dominio.Turma>();
            var turmaId = turmas.FirstOrDefault(c => c.CodigoTurma == TURMA_CODIGO_1)?.Id;

            var arquivos = ObterTodos<Arquivo>();
            arquivos.ShouldNotBeNull();

            var primeiroArquivo = arquivos.FirstOrDefault();
            primeiroArquivo.ShouldNotBeNull();

            var arquivosCodigos = new[] { primeiroArquivo.Codigo };

            var classificacoesDocumentos = ObterTodos<ClassificacaoDocumento>();
            classificacoesDocumentos.ShouldNotBeNull();

            var classificacao = classificacoesDocumentos.FirstOrDefault(c =>
                c.Id == (int)Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica);

            classificacao.ShouldNotBeNull();
            classificacao.EhRegistroMultiplo.ShouldBeFalse();

            var salvarDocumento = new SalvarDocumentoDto
            {
                UeId = ueId.GetValueOrDefault(),
                AnoLetivo = 2022,
                TipoDocumentoId = classificacao.TipoDocumentoId,
                ClassificacaoId = classificacao.Id,
                UsuarioId = usuarioId.GetValueOrDefault(),
                ArquivosCodigos = arquivosCodigos,
                TurmaId = turmaId,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            
            (await useCase.Executar(salvarDocumento)).ShouldBeTrue();
            
            var documentos = ObterTodos<Dominio.Documento>();
            documentos.ShouldNotBeNull();

            var documentoSalvo = documentos.FirstOrDefault();
            documentoSalvo.ShouldNotBeNull();

            var documentosArquivos = ObterTodos<DocumentoArquivo>();
            documentosArquivos.ShouldNotBeNull();

            var arquivosDoDocumento = documentosArquivos.Where(c => c.DocumentoId == documentoSalvo.Id).ToList();
            arquivosDoDocumento.ShouldNotBeNull();
            arquivosDoDocumento.Count.ShouldBe(1);            
        }        
    }
}