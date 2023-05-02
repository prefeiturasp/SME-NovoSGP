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
    public class Ao_registrar_plano_aula_com_atribuicao_encerrada: PlanoAulaTesteBase
    {
        public Ao_registrar_plano_aula_com_atribuicao_encerrada(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Nao_deve_cadastrar_plano_aula_componente_diferente_regencia_com_objetivos_aprendizagem()
        {
            var planoAulaDto = ObterPlanoAula(true, long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));

            await CriarDadosBasicos(ObterFiltroPlanoAula(COMPONENTE_LINGUA_PORTUGUESA_ID_138, 
                Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio));
                
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await Assert.ThrowsAsync<NegocioException>(() => salvarPlanoAulaUseCase.Executar(planoAulaDto));
        }

        private FiltroPlanoAula ObterFiltroPlanoAula(string componenteCurricular, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario)
        {
            return new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = modalidade,
                Perfil = ObterPerfilProfessor(),
                QuantidadeAula = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 5, 2),
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_24_07_FIM_BIMESTRE_2,
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