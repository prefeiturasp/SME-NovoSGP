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
    public class Ao_cadastrar_plano_aula_para_componente_territorio_do_saber : PlanoAulaTesteBase
    {
        public Ao_cadastrar_plano_aula_para_componente_territorio_do_saber(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_cadastrar_plano_aula()
        {
            var planoAulaDto = ObterPlanoAula();
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCJ(),
                QuantidadeAula = 1,
                DataAula = DateTimeExtension.HorarioBrasilia(),
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_24_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214.ToString(),
                TipoCalendarioId = TIPO_CALENDARIO_1,
            });

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(3);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(0);
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
    }
}