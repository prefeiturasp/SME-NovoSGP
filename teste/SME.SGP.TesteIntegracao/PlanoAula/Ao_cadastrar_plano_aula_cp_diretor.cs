using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAula.Base;
using SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAula
{
    public class Ao_cadastrar_plano_aula_cp_diretor : PlanoAulaTesteBase
    {
        public Ao_cadastrar_plano_aula_cp_diretor(CollectionFixture collectionFixture) : base(collectionFixture){}
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));

        }

        //[Fact]
        public async Task Deve_cadastrar_plano_aula_usuario_cp()
        {
            
        }
        [Fact]
        public async Task Deve_cadastrar_plano_aula_usuario_diretor()
        {
            var filtroPlanoAulaDiretor = ObterFiltroPlanoAulaPorPerfil("46e1e074-37d6-e911-abd6-f81654fe895d");
            await CriarDadosBasicos(filtroPlanoAulaDiretor);
            // await CriarTipoCalendario(DateTime.Now.Year, ModalidadeTipoCalendario.FundamentalMedio, Periodo.Anual);
            // await CriarAula(COMPONENTE_LINGUA_PORTUGUESA_ID_138,DateTime.Now,RecorrenciaAula.AulaUnica,1,USUARIO_LOGIN_DIRETOR999998);
            // await CriarTurma(Modalidade.Fundamental);
            var planoAulaDto = ObterPlanoAula();

            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaDto);

            retorno.ShouldNotBeNull();
        }

        private FiltroPlanoAula ObterFiltroPlanoAulaPorPerfil(string perfil)
        {
            return new FiltroPlanoAula()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(2),
                Bimestre = 2,
                DataAula = DateTime.Now,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                CriarPeriodo = false,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolarEAbertura = false,
                quantidadeAula = 1
            };
        }
        private PlanoAulaDto ObterPlanoAula()
        {
            return new PlanoAulaDto()
            {
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = "<p><span>Objetivos específicos e desenvolvimento da aula</span></p>",
                LicaoCasa = "<p><span>Lição de Casa</span></p>",
                ObjetivosAprendizagemComponente = new List<ObjetivoAprendizagemComponenteDto>()
                {
                    new()
                    {
                        ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                        Id = 1008
                    },
                    new()
                    {
                        ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                        Id = 1009
                    },
                    new()
                    {
                        ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                        Id = 1006
                    },
                },
                RecuperacaoAula = "<p><span>Recuperacao de Aula</span></p>"
            };
        }
    }
}