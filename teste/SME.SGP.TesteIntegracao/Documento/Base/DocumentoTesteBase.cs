using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Documento
{
    public abstract class DocumentoTesteBase : TesteBaseComuns
    {
        private readonly List<long> Arquivos;
        
        public DocumentoTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            Arquivos = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        protected IListarDocumentosUseCase ObterServicoListarDocumentosUseCase()
        {
            return ServiceProvider.GetService<IListarDocumentosUseCase>();
        }

        protected ISalvarDocumentoUseCase ObterServicoSalvarDocumentoUseCase()
        {
            return ServiceProvider.GetService<ISalvarDocumentoUseCase>();
        }
        
        protected IExcluirDocumentoUseCase ObterServicoExcluirDocumentoUseCase()
        {
            return ServiceProvider.GetService<IExcluirDocumentoUseCase>();
        }

        protected async Task CriarDadosBasicos(FiltroDocumentoDto filtroDocumentoDto)
        {
            await CriarTipoCalendario(filtroDocumentoDto.TipoCalendario);

            await CriarDreUePerfil();

            await CriarPeriodoEscolarTodosBimestres();

            await CriarComponenteCurricular();

            CriarClaimUsuario(filtroDocumentoDto.Perfil);

            await CriarUsuarios();

            await CriarTurma(filtroDocumentoDto.Modalidade);

            await CriarTipoDocumento();
            await CriarClassificacaoDocumento();

            await CriarArquivos();
        }

        protected async Task CriarArquivos()
        {
            foreach (var arquivo in Arquivos)
            {
                await InserirNaBase(new Arquivo
                {
                    Nome = $"Arquivo - {arquivo}",
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.Geral,
                    TipoConteudo = "application/pdf",
                    CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF,
                });
            }
        }
        
        protected async Task CriarDocumentos(Dominio.Enumerados.ClassificacaoDocumento classificacaoDocumento, long? componentecurricularId = null, bool inserirTurma = true)
        {
            var turmas = new List<long> { 1, 2, 3 };
            long documentoId = 1;
            long arquivoId = 1;
            var camposDocumentoArquivo = new [] { "documento_id", "arquivo_id" };

            foreach (var turma in turmas)
            {
                await InserirNaBase(new Dominio.Documento()
                {
                    UsuarioId = USUARIO_ID_1,
                    UeId = UE_ID_1,
                    AnoLetivo = DateTime.Now.Year,
                    ClassificacaoDocumentoId = (long)classificacaoDocumento,
                    TurmaId = inserirTurma ? turma : null,
                    ComponenteCurricularId = componentecurricularId,
                    CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF,
                });
                
                foreach (var arquivo in Arquivos)
                {
                    await InserirNaBase(new Arquivo()
                    {
                        Codigo = Guid.NewGuid(),
                        Nome = $"Arquivo - {arquivo}",
                        Tipo = TipoArquivo.Geral,
                        TipoConteudo = "application/pdf",
                        CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF,
                    });

                    var valoresDocumentoArquivo = new[] { documentoId.ToString(), arquivoId.ToString() };
                    await InserirNaBase(DOCUMENTO_ARQUIVO, camposDocumentoArquivo, valoresDocumentoArquivo);
                    
                    arquivoId++;
                }
                documentoId++;
            }
        }

        private async Task CriarTipoDocumento()
        {
            var camposTipoDocumento = new[] { "descricao" };

            var valoresTipoDocumento = new[] { $"'{Dominio.Enumerados.TipoDocumento.PlanoTrabalho.GetDisplayName()}'"};
            await InserirNaBase(TIPO_DOCUMENTO, camposTipoDocumento, valoresTipoDocumento);

            valoresTipoDocumento = new[] { $"'{Dominio.Enumerados.TipoDocumento.Documento.GetDisplayName()}'"};
            await InserirNaBase(TIPO_DOCUMENTO, camposTipoDocumento, valoresTipoDocumento);

            var tipoDocumento = ObterTodos<Dominio.TipoDocumento>();
        }
        
        private async Task CriarClassificacaoDocumento()
        {
            var camposClassificacaoDocumento = new[] { "descricao", "tipo_documento_id", "ehRegistroMultiplo" };

            var valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.PAEE.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);

            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.PAP.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
            
            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.POA.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
            
            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.POED.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
            
            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.POEI.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
            
            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.POSL.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
            
            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.PEA.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
            
            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.PPP.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
            
            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                FALSE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
            
            valoresClassificacaoDocumento = new[]
            {
                $"'{Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                TRUE
            };
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, camposClassificacaoDocumento,valoresClassificacaoDocumento);
        }

        protected async Task CriarPeriodoEscolarTodosBimestres()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);
        }

    }
}
