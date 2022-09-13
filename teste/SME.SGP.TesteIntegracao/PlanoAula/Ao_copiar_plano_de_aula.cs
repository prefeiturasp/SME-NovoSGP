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
    public class Ao_copiar_plano_de_aula : PlanoAulaTesteBase
    {
        public Ao_copiar_plano_de_aula(CollectionFixture collectionFixture) : base(collectionFixture){}
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery, IEnumerable<AbrangenciaTurmaRetorno>>), typeof(ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery, PlanejamentoAnual>), typeof(ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryHandlerFake), ServiceLifetime.Scoped));

        }

        [Fact(DisplayName = "Cópia de plano de aula para outra aula da mesma turma e componente curricular - Sem sobrescrever o plano existente")]
        public async Task Copiar_plano_para_outra_aula_da_mesma_turma_e_componente_sem_sobrescrever()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia();
            await CriarPlanoDeAula();
            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal,
                USUARIO_PROFESSOR_LOGIN_2222222, "1", "1", "138", 1, false);
            
            
            var aula = ObterTodos<Dominio.Aula>().FirstOrDefault();
            var planoAula = ObterTodos<Dominio.PlanoAula>();
            
            var dtoMigrarPlanoAula = ObterMigrarPlanoAulaDto(aula,planoAula.FirstOrDefault().Id,dataAula);
            var servicoMigrarPlano = ObterServicoMigrarPlanoAulaUseCase();
            
            var retorno = await servicoMigrarPlano.Executar(dtoMigrarPlanoAula);
            
            var planosAula = ObterTodos<Dominio.PlanoAula>();
            planosAula.ShouldNotBeNull();
            planosAula.Count.ShouldBeGreaterThanOrEqualTo(2);

            retorno.ShouldBeTrue();
        }

        [Fact(DisplayName = "Cópia de plano de aula para outra aula da mesma turma e componente curricular - Com sobrescrever o plano existente")]
        public async Task Copiar_plano_para_outra_aula_da_mesma_turma_e_componente_com_sobrescrever()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia();
            await CriarPlanoDeAula();
            
            var aula = ObterTodos<Dominio.Aula>().FirstOrDefault();
            var planoAula = ObterTodos<Dominio.PlanoAula>();
            
            var dtoMigrarPlanoAula = ObterMigrarPlanoAulaDto(aula,planoAula.FirstOrDefault().Id,null);
            var servicoMigrarPlano = ObterServicoMigrarPlanoAulaUseCase();
            
            var retorno = await servicoMigrarPlano.Executar(dtoMigrarPlanoAula);
            
            var planosAula = ObterTodos<Dominio.PlanoAula>();
            planosAula.ShouldNotBeNull();
            planoAula.Count.ShouldBeGreaterThanOrEqualTo(1);

            retorno.ShouldBeTrue();
        }

       // [Fact(DisplayName = "Cópia de plano de aula para outra turma e componente curricular diferente (não deve permitir)")]
        public async Task Copiar_plano_para_outra_turma_de_componente_direferente()
        {
            
        }

        private async Task CriarPlanoDeAula()
        {
            var planoAulaDto = ObterPlanoAula();
            var filtroPlanoAulaDiretor = ObterFiltroPlanoAulaPorPerfil(ObterPerfilProfessor());
            await CriarDadosBasicos(filtroPlanoAulaDiretor);

            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaDto);

            retorno.ShouldNotBeNull();
        }
        private MigrarPlanoAulaDto ObterMigrarPlanoAulaDto(Dominio.Aula aula, long planoAulaId,
            DateTime? data, string turmaId = null, string disciplinaId = null)
        {
            var dataPlanoAulaTurmaDto = new List<DataPlanoAulaTurmaDto>()
            {
                new DataPlanoAulaTurmaDto(){Data = data ?? aula.DataAula,TurmaId = turmaId ?? aula.TurmaId,Sobreescrever = false}
            };
            return new MigrarPlanoAulaDto()
            {
                IdsPlanoTurmasDestino = dataPlanoAulaTurmaDto,
                PlanoAulaId = planoAulaId,
                DisciplinaId = disciplinaId ?? aula.DisciplinaId,
                MigrarLicaoCasa = true,
                MigrarRecuperacaoAula = true,
                MigrarObjetivos = true
            };
        }
        private FiltroPlanoAula ObterFiltroPlanoAulaPorPerfil(string perfil)
        {
            return new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = perfil,
                QuantidadeAula = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 5, 2),
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_08_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoAbertura = true
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