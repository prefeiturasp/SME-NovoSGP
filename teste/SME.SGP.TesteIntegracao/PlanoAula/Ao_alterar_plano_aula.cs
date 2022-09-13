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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAula
{
    public class Ao_alterar_plano_aula : PlanoAulaTesteBase
    {
        public Ao_alterar_plano_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        private const string NOVA_DESCRICAO_PlANO_AULA = "<p><span>Objetivos específicos e desenvolvimento da aula EDITADO</span></p>";

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_alterar_plano_componentes_com_objetivos()
        {
            var planoAulaDto = ObterPlanoAula();
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();
            var obterPlanoAulaUseCase = ObterServicoObterPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                QuantidadeAula = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 5, 2),
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_08_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoAbertura = true
            });

            await salvarPlanoAulaUseCase.Executar(planoAulaDto);

            var listaPlanoAulaPersistido = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            var planoAulaPersistido = listaPlanoAulaPersistido.FirstOrDefault();

            var planoAulaAlteradoDto = ObterPlanoAulaAlterado(planoAulaPersistido.Id, NOVA_DESCRICAO_PlANO_AULA);

            await salvarPlanoAulaUseCase.Executar(planoAulaAlteradoDto);

            var listaPlanoAulaEditado = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            listaPlanoAulaEditado.ShouldNotBeNull();
            listaPlanoAulaEditado.FirstOrDefault().Id.ShouldBe(planoAulaPersistido.Id);
            listaPlanoAulaEditado.FirstOrDefault().Descricao.ShouldNotBe(planoAulaPersistido.Descricao);
        }

        [Fact]
        public async Task Deve_alterar_plano_componentes_sem_objetivos()
        {
            var planoAulaDto = ObterPlanoAulaSemObjetivos();
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();
            var obterPlanoAulaUseCase = ObterServicoObterPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                QuantidadeAula = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 5, 2),
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_08_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoAbertura = true
            });

            await salvarPlanoAulaUseCase.Executar(planoAulaDto);

            var listaPlanoAulaPersistido = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            var planoAulaPersistido = listaPlanoAulaPersistido.FirstOrDefault();

            var planoAulaAlteradoDto = ObterPlanoAulaAlteradoSemObetivos(planoAulaPersistido.Id, NOVA_DESCRICAO_PlANO_AULA);

            await salvarPlanoAulaUseCase.Executar(planoAulaAlteradoDto);

            var listaPlanoAulaEditado = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            listaPlanoAulaEditado.ShouldNotBeNull();
            listaPlanoAulaEditado.FirstOrDefault().Id.ShouldBe(planoAulaPersistido.Id);
            listaPlanoAulaEditado.FirstOrDefault().Descricao.ShouldNotBe(planoAulaPersistido.Descricao);
        }

        [Fact]
        public async Task Deve_alterar_plano_pelo_CP()
        {
            var planoAulaDto = ObterPlanoAula();
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();
            var obterPlanoAulaUseCase = ObterServicoObterPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                QuantidadeAula = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 5, 2),
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_08_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoAbertura = true
            });

            await salvarPlanoAulaUseCase.Executar(planoAulaDto);

            var listaPlanoAulaPersistido = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            var planoAulaPersistido = listaPlanoAulaPersistido.FirstOrDefault();

            var planoAulaAlteradoDto = ObterPlanoAulaAlterado(planoAulaPersistido.Id, NOVA_DESCRICAO_PlANO_AULA);

            await salvarPlanoAulaUseCase.Executar(planoAulaAlteradoDto);

            var listaPlanoAulaEditado = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            listaPlanoAulaEditado.ShouldNotBeNull();
            listaPlanoAulaEditado.FirstOrDefault().Id.ShouldBe(planoAulaPersistido.Id);
            listaPlanoAulaEditado.FirstOrDefault().Descricao.ShouldNotBe(planoAulaPersistido.Descricao);
        }

        private PlanoAulaDto ObterPlanoAula()
        {
            return new PlanoAulaDto()
            {
                ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = "<p><span>Objetivos específicos e desenvolvimento da aula</span></p>",
                LicaoCasa = null,
                ObjetivosAprendizagemComponente = new List<ObjetivoAprendizagemComponenteDto>()
                {
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 1
                    },
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 2
                    },
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 3
                    },
                },
                RecuperacaoAula = null
            };
        }

        private PlanoAulaDto ObterPlanoAulaSemObjetivos()
        {
            return new PlanoAulaDto()
            {
                ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = "<p><span>Objetivos específicos e desenvolvimento da aula</span></p>",
                LicaoCasa = null,
                ObjetivosAprendizagemComponente = null,
                RecuperacaoAula = null
            };
        }

        private PlanoAulaDto ObterPlanoAulaAlterado(long id, string descricao)
        {
            return new PlanoAulaDto()
            {
                Id = id,
                ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = descricao,
                LicaoCasa = null,
                ObjetivosAprendizagemComponente = new List<ObjetivoAprendizagemComponenteDto>()
                {
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 1
                    },
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 2
                    },
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 3
                    },
                },
                RecuperacaoAula = null
            };
        }

        private PlanoAulaDto ObterPlanoAulaAlteradoSemObetivos(long id, string descricao)
        {
            return new PlanoAulaDto()
            {
                Id = id,
                ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = descricao,
                LicaoCasa = null,
                ObjetivosAprendizagemComponente = null,
                RecuperacaoAula = null
            };
        }
    }
}