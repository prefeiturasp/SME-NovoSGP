using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.OpenApi.Extensions;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.PlanoAula.Base;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Enumerados;
using ClassificacaoDocumento = SME.SGP.Dominio.ClassificacaoDocumento;

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

        protected async Task CriarDadosBasicos(FiltroDocumentoDto filtroDocumentoDto)
        {
            await CriarTipoCalendario(filtroDocumentoDto.TipoCalendario);

            await CriarDreUePerfil();

            await CriarPeriodoEscolarTodosBimestres();

            await CriarComponenteCurricular();

            CriarClaimUsuario(filtroDocumentoDto.Perfil);

            await CriarUsuarios();

            await CriarTurma(filtroDocumentoDto.Modalidade);

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
            long documentoId = 0;
            long arquivoId = 0;
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
                documentoId++;
                
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
                    arquivoId++;

                    var valoresDocumentoArquivo = new[] { documentoId.ToString(), arquivoId.ToString() };
                    await InserirNaBase(DOCUMENTO_ARQUIVO, camposDocumentoArquivo, valoresDocumentoArquivo);
                }
            }
        }

        private async Task CriarClassificacaoDocumento()
        {
            await InserirNaBase(TIPO_DOCUMENTO, 
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                $"'{Dominio.Enumerados.TipoDocumento.PlanoTrabalho.GetDisplayName()}'");
            
            await InserirNaBase(TIPO_DOCUMENTO, 
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                $"'{Dominio.Enumerados.TipoDocumento.Documento.GetDisplayName()}'");
            
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.PAEE).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.PAEE.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE);

            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.PAP).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.PAP.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE);
            
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.POA).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.POA.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE);
             
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.POED).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.POED.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE);
            
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.POEI).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.POEI.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE);
            
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.POSL).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.POSL.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho).ToString(),
                FALSE);
            
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.PEA).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.PEA.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                FALSE);
            
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.PPP).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.PPP.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                FALSE);
            
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                FALSE);
            
            await InserirNaBase(CLASSIFICACAO_DOCUMENTO, 
                ((long)Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma).ToString(),
                $"'{Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma.GetDisplayName()}'",
                ((long)Dominio.Enumerados.TipoDocumento.Documento).ToString(),
                TRUE);
              
        }

        protected async Task CriarPeriodoEscolarTodosBimestres()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);
        }

    }
}
