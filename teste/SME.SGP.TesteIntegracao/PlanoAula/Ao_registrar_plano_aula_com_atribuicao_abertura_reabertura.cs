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
    public class Ao_registrar_plano_aula_com_atribuicao_abertura_reabertura: PlanoAulaTesteBase
    {
        public Ao_registrar_plano_aula_com_atribuicao_abertura_reabertura(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            RegistraFake(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A));
            RegistraFake(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake));
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Nao_deve_cadastrar_plano_aula_componente_diferente_regencia_sem_reabertura()
        {
            var planoAulaDto = ObterPlanoAula(true, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));

            var filtroPlanoAula = ObterFiltroPlanoAula(COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            filtroPlanoAula.CriarPeriodoEscolarBimestre = false;
            filtroPlanoAula.CriarPeriodoEscolarTodosBimestres = false;
            filtroPlanoAula.CriarPeriodoReabertura = false;
            filtroPlanoAula.CriarPlanejamentoAnual = false;

            await CriarDadosBasicos(filtroPlanoAula);
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPlanejamentoAnualTodosBimestres(COMPONENTE_LINGUA_PORTUGUESA_ID_138);
                
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await Assert.ThrowsAsync<NegocioException>(() => salvarPlanoAulaUseCase.Executar(planoAulaDto));
        }
        
        [Fact]
        public async Task Deve_cadastrar_plano_aula_componente_diferente_regencia_com_reabertura()
        {
            var planoAulaDto = ObterPlanoAula(true, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));

            var filtroPlanoAula = ObterFiltroPlanoAula(COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            filtroPlanoAula.CriarPeriodoEscolarBimestre = false;
            filtroPlanoAula.CriarPeriodoEscolarTodosBimestres = false;
            filtroPlanoAula.CriarPeriodoReabertura = false;
            filtroPlanoAula.CriarPlanejamentoAnual = false;

            await CriarDadosBasicos(filtroPlanoAula);
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);

            await CriarPlanejamentoAnualTodosBimestres(COMPONENTE_LINGUA_PORTUGUESA_ID_138);
                
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);

            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(3);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(0);
        }
        
        [Fact]
        public async Task Nao_deve_cadastrar_plano_aula_componente_diferente_regencia_sem_periodo_abertura()
        {
            var planoAulaDto = ObterPlanoAula(true, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));

            var filtroPlanoAula = ObterFiltroPlanoAula(COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            filtroPlanoAula.CriarPeriodoEscolarBimestre = false;
            filtroPlanoAula.CriarPeriodoEscolarTodosBimestres = false;
            filtroPlanoAula.CriarPeriodoReabertura = false;
            filtroPlanoAula.CriarPlanejamentoAnual = false;

            await CriarDadosBasicos(filtroPlanoAula);
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPeriodoAberturaCustomizadoQuartoBimestre(false);

            await CriarPlanejamentoAnualTodosBimestres(COMPONENTE_LINGUA_PORTUGUESA_ID_138);
                
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await Assert.ThrowsAsync<NegocioException>(() => salvarPlanoAulaUseCase.Executar(planoAulaDto));
        }
        
        [Fact]
        public async Task Deve_cadastrar_plano_aula_componente_diferente_regencia_com_periodo_abertura()
        {
            var planoAulaDto = ObterPlanoAula(true, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));

            var filtroPlanoAula = ObterFiltroPlanoAula(COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            filtroPlanoAula.CriarPeriodoEscolarBimestre = false;
            filtroPlanoAula.CriarPeriodoEscolarTodosBimestres = false;
            filtroPlanoAula.CriarPeriodoReabertura = false;
            filtroPlanoAula.CriarPlanejamentoAnual = false;

            await CriarDadosBasicos(filtroPlanoAula);
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPeriodoAberturaCustomizadoQuartoBimestre();

            await CriarPlanejamentoAnualTodosBimestres(COMPONENTE_LINGUA_PORTUGUESA_ID_138);
                
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);

            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(3);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(0);
        }

        private FiltroPlanoAula ObterFiltroPlanoAula(string componenteCurricular, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            return new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_4,
                Modalidade = modalidade,
                Perfil = ObterPerfilProfessor(),
                QuantidadeAula = 1,
                DataAula = dataReferencia.AddDays(-10).Date,
                DataInicio = dataReferencia.AddDays(-20),
                DataFim = dataReferencia.AddDays(-5),
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = tipoCalendario,
                ComponenteCurricularCodigo = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoReabertura = true
            };
        }

        private PlanoAulaDto ObterPlanoAula(bool incluirObjetivosAprendizagem, long componenteCurricular)
        {
            var planoAula = new PlanoAulaDto()
            {
                ComponenteCurricularId = componenteCurricular,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = "<p><span>Objetivos específicos e desenvolvimento da aula</span></p>",
                LicaoCasa = null,
                RecuperacaoAula = null
                
            };

            if (incluirObjetivosAprendizagem)
            {
                planoAula.ObjetivosAprendizagemComponente = new List<ObjetivoAprendizagemComponenteDto>()
                {
                    new() { ComponenteCurricularId = componenteCurricular, Id = 1 },
                    new() { ComponenteCurricularId = componenteCurricular, Id = 2 },
                    new() { ComponenteCurricularId = componenteCurricular, Id = 3 },
                };
            }

            return planoAula;
        }
    }
}