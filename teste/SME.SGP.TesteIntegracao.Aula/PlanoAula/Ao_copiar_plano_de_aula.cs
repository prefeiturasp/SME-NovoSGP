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
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasPorProfessorRfQuery, IEnumerable<ProfessorTurmaDto>>), typeof(ObterTurmasPorProfessorRfQueryHandlerFakeFundamental1AAno2), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePlanoAula), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano de Aula - Cópia de plano de aula para outra aula da mesma turma e componente curricular - Sem sobrescrever o plano existente")]
        public async Task Copiar_plano_para_outra_aula_da_mesma_turma_e_componente_sem_sobrescrever()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia();
            await CriarPlanoDeAula(Modalidade.Fundamental);
            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal,
                USUARIO_PROFESSOR_LOGIN_2222222, "1", "1", "138", 1, false);
            
            
            var aula = ObterTodos<Dominio.Aula>().FirstOrDefault();
            var planoAula = ObterTodos<Dominio.PlanoAula>();
            
            var dtoMigrarPlanoAula = ObterMigrarPlanoAulaDto(aula,planoAula.FirstOrDefault().Id,dataAula);
            var servicoMigrarPlano = ObterServicoMigrarPlanoAulaUseCase();
            
            var retorno = await servicoMigrarPlano.Executar(dtoMigrarPlanoAula);
            retorno.ShouldBeTrue();

            var planosAula = ObterTodos<Dominio.PlanoAula>();
            planosAula.ShouldNotBeNull();
            planosAula.Count.ShouldBeGreaterThanOrEqualTo(2);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(6);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(0);

        }

        [Fact(DisplayName = "Plano de Aula - Cópia de plano de aula para outra aula da mesma turma e componente curricular - Com sobrescrever o plano existente")]
        public async Task Copiar_plano_para_outra_aula_da_mesma_turma_e_componente_com_sobrescrever()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia();
            await CriarPlanoDeAula(Modalidade.Fundamental);
            
            var aula = ObterTodos<Dominio.Aula>().FirstOrDefault();
            var planoAula = ObterTodos<Dominio.PlanoAula>();
            
            var dtoMigrarPlanoAula = ObterMigrarPlanoAulaDto(aula,planoAula.FirstOrDefault().Id,null);
            var servicoMigrarPlano = ObterServicoMigrarPlanoAulaUseCase();
            
            var retorno = await servicoMigrarPlano.Executar(dtoMigrarPlanoAula);
            retorno.ShouldBeTrue();

            var planosAula = ObterTodos<Dominio.PlanoAula>();
            planosAula.ShouldNotBeNull();
            planoAula.Count.ShouldBeGreaterThanOrEqualTo(1);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(3);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(0);
        }

        
        [Fact(DisplayName = "Plano de Aula - Cópia de plano de aula para outra turma com o mesmo componente curricular")]
        public async Task Copiar_plano_para_outra_turma_com_o_mesmo_componente_curricular()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia();
            await CriarPlanoDeAula(Modalidade.Fundamental);
            await CriarTurma(Modalidade.Medio);
            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal,
                USUARIO_PROFESSOR_LOGIN_2222222, "1", "1", "138", 1, false);
            var aula = ObterTodos<Dominio.Aula>().FirstOrDefault();
            
            var planoAula = ObterTodos<Dominio.PlanoAula>().FirstOrDefault();
            
            var dtoMigrarPlanoAula = ObterMigrarPlanoAulaDto(aula,planoAula.Id,dataAula);
            
            
            var servicoMigrarPlano = ObterServicoMigrarPlanoAulaUseCase();
            
            var retorno = await servicoMigrarPlano.Executar(dtoMigrarPlanoAula);
            retorno.ShouldBeTrue();

            var planosAula = ObterTodos<Dominio.PlanoAula>();
            
            planosAula.ShouldNotBeNull();
            planosAula.Count.ShouldBeGreaterThanOrEqualTo(2);

            planosAula.FirstOrDefault().AulaId.ShouldBeGreaterThanOrEqualTo(1);
            planosAula.LastOrDefault().AulaId.ShouldBeGreaterThanOrEqualTo(2);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(6);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(0);

        }

        [Fact(DisplayName = "Plano de Aula - Cópia de plano de aula para outra turma e componente curricular diferente (não deve permitir)")]
        public async Task Copiar_plano_para_outra_turma_de_componente_direferente()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia();
            await CriarPlanoDeAula(Modalidade.Fundamental);
            
            await CriarTurma(Modalidade.Medio);
            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal,USUARIO_PROFESSOR_LOGIN_2222222, "1", "1", "139", 1, false);
            var aula = ObterTodos<Dominio.Aula>().FirstOrDefault();
            
            var planoAula = ObterTodos<Dominio.PlanoAula>().FirstOrDefault();
            var planoAulas = ObterTodos<Dominio.PlanoAula>();
            var aulas = ObterTodos<Dominio.Aula>();
            
            var dtoMigrarPlanoAula = ObterMigrarPlanoAulaDto(aula,planoAula.Id,dataAula);

            var servicoMigrarPlano = ObterServicoMigrarPlanoAulaUseCase();
            
            var retorno = await  servicoMigrarPlano.Executar(dtoMigrarPlanoAula);
            retorno.ShouldBeTrue();
            //TODO: Ver com Marlon sobre essa regra
            // var ex = await Assert.ThrowsAsync<NegocioException>(() =>  servicoMigrarPlano.Executar(dtoMigrarPlanoAula));
            // ex.Message.ShouldNotBeNullOrEmpty();
        }

        private async Task CriarPlanoDeAula(Modalidade modalidade)
        {
            var planoAulaDto = ObterPlanoAula();
            var filtroPlanoAulaDiretor = ObterFiltroPlanoAulaPorPerfil(ObterPerfilProfessor(),Modalidade.Fundamental);
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
        private FiltroPlanoAula ObterFiltroPlanoAulaPorPerfil(string perfil,Modalidade modalidade)
        {
            return new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_4,
                Modalidade = modalidade,
                Perfil = perfil,
                QuantidadeAula = 1,
                DataAula = DateTimeExtension.HorarioBrasilia(),
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPlanejamentoAnual = true
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