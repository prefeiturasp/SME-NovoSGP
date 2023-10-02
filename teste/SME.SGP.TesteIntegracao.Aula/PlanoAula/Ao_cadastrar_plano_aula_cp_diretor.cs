using System;
using System.Collections.Generic;
using System.Linq;
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
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaTurmaQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaTurmaQueryFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Cadastro do plano de aula pelo CP")]
        public async Task Deve_cadastrar_plano_aula_usuario_cp()
        {
            var planoAulaDto = ObterPlanoAula();
            var filtroPlanoAulaDiretor = ObterFiltroPlanoAulaPorPerfil(ObterPerfilCP());
            await CriarDadosBasicos(filtroPlanoAulaDiretor);

            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaDto);

            retorno.ShouldNotBeNull();
            Assert.True(retorno.Id > 0);
            retorno.Descricao.ShouldNotBeNull();
            retorno.LicaoCasa.ShouldNotBeNull();
            retorno.RecuperacaoAula.ShouldNotBeNull();
            
            var planoAlunaTodos = ObterTodos<Dominio.PlanoAula>();
            planoAlunaTodos.Count.ShouldBeGreaterThanOrEqualTo(1);
            
            var objetivoAprendizagemAula = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAula.Count(w=> !w.Excluido).ShouldBe(3);
            objetivoAprendizagemAula.Count(w=> w.Excluido).ShouldBe(0);
        }
        [Fact(DisplayName = "Cadastro do plano de aula pelo Diretor")]
        public async Task Deve_cadastrar_plano_aula_usuario_diretor()
        {
            var planoAulaDto = ObterPlanoAula();
            var filtroPlanoAulaDiretor = ObterFiltroPlanoAulaPorPerfil(ObterPerfilDiretor());
            await CriarDadosBasicos(filtroPlanoAulaDiretor);

            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaDto);
            
            retorno.ShouldNotBeNull();
            Assert.True(retorno.Id > 0);
            retorno.Descricao.ShouldNotBeNull();
            retorno.LicaoCasa.ShouldNotBeNull();
            retorno.RecuperacaoAula.ShouldNotBeNull();
            
            var planoAlunaTodos = ObterTodos<Dominio.PlanoAula>();
            planoAlunaTodos.Count.ShouldBeGreaterThanOrEqualTo(1);
            
            var objetivoAprendizagemAula = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAula.Count(w=> !w.Excluido).ShouldBe(3);
            objetivoAprendizagemAula.Count(w=> w.Excluido).ShouldBe(0);
        }
        private FiltroPlanoAula ObterFiltroPlanoAulaPorPerfil(string perfil)
        {
            return new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = perfil,
                QuantidadeAula = 1,
                DataAula = DateTimeExtension.HorarioBrasilia().Date,
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_24_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoReabertura = true
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
                        Id = 1
                    },
                    new()
                    {
                        ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                        Id = 2
                    },
                    new()
                    {
                        ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                        Id = 3
                    },
                },
                RecuperacaoAula = "<p><span>Recuperacao de Aula</span></p>"
            };
        }
    }
}