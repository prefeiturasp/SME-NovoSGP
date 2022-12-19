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
        
        protected async Task CriarDocumentos(Dominio.Enumerados.TipoDocumento tipoDocumento)
        {
            var turmas = new List<long> { 1, 2, 3 };

            foreach (var turma in turmas)
            {
                foreach (var arquivo in Arquivos)
                {
                    await InserirNaBase(new Dominio.Documento()
                    {
                        UsuarioId = USUARIO_ID_1,
                        ArquivoId = arquivo,
                        UeId = UE_ID_1,
                        AnoLetivo = DateTime.Now.Year,
                        ClassificacaoDocumentoId = (long)tipoDocumento,
                        TurmaId = turma,
                        CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF,
                    });                    
                }
            }
        }

        private async Task CriarClassificacaoDocumento()
        {
            var tipoDocumentoPlanoTrabalho = new Dominio.TipoDocumento()
            {
                Id = (long)Dominio.Enumerados.TipoDocumento.PlanoTrabalho,
                Descricao = Dominio.Enumerados.TipoDocumento.PlanoTrabalho.GetDisplayName()
            };
            
            var tipoDocumentoDocumento = new Dominio.TipoDocumento()
            {
                Id = (long)Dominio.Enumerados.TipoDocumento.Documento,
                Descricao = Dominio.Enumerados.TipoDocumento.Documento.GetDisplayName()
            };
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.PAEE.GetDisplayName(),
                TipoDocumento = tipoDocumentoPlanoTrabalho,
            });
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.PAP.GetDisplayName(),
                TipoDocumento = tipoDocumentoPlanoTrabalho,
            });
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.POA.GetDisplayName(),
                TipoDocumento = tipoDocumentoPlanoTrabalho,
            });     
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.POED.GetDisplayName(),
                TipoDocumento = tipoDocumentoPlanoTrabalho,
            });  
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.POEI.GetDisplayName(),
                TipoDocumento = tipoDocumentoPlanoTrabalho,
            });   
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.POSL.GetDisplayName(),
                TipoDocumento = tipoDocumentoPlanoTrabalho,
            });
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.PEA.GetDisplayName(),
                TipoDocumento = tipoDocumentoDocumento,
            });
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.PPP.GetDisplayName(),
                TipoDocumento = tipoDocumentoDocumento,
            });
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.CartaPedagogica.GetDisplayName(),
                TipoDocumento = tipoDocumentoDocumento,
                EhRegistroMultiplo = true
            });
            
            await InserirNaBase(new Dominio.ClassificacaoDocumento()
            {
                Descricao = Dominio.Enumerados.ClassificacaoDocumento.DocumentosTurma.GetDisplayName(),
                TipoDocumento = tipoDocumentoDocumento,
                EhRegistroMultiplo = true
            });
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
