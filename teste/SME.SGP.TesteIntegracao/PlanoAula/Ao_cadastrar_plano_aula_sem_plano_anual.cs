using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAula.Base;
using SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAula
{
    public class Ao_cadastrar_plano_aula_sem_plano_anual : PlanoAulaTesteBase
    {
        public Ao_cadastrar_plano_aula_sem_plano_anual(CollectionFixture collectionFixture) : base(collectionFixture){}
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            RegistraFake(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A));
            RegistraFake(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake));
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Cadastro do plano de aula quando não há plano anual (não deve permitir)")]
        public async Task Nao_deve_cadastrar_plano_aula_sem_plano_anual()
        {
            var planoAulaDto = ObterPlanoAula();
            var filtroPlanoAulaDiretor = ObterFiltroPlanoAulaPorPerfil(ObterPerfilProfessor());
            await CriarDadosBasicos(filtroPlanoAulaDiretor);

            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();
            var ex = await Assert.ThrowsAsync<NegocioException>(() =>  salvarPlanoAulaUseCase.Executar(planoAulaDto));
            ex.Message.ShouldNotBeNullOrEmpty();
            ex.Message.ShouldBeEquivalentTo(MensagemNegocioPlanoAula.NAO_EXISTE_PLANO_ANUAL_CADASTRADO);
        }
        private FiltroPlanoAula ObterFiltroPlanoAulaPorPerfil(string perfil)
        {
            var filtro = new FiltroPlanoAula()
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
                CriarPeriodoReabertura = true,
                CriarPlanejamentoAnual = false
            };
            
            return filtro;
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