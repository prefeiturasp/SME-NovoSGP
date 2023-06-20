using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_listar_opcoes_impressao : ConselhoDeClasseTesteBase
    {
        public Ao_listar_opcoes_impressao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>),typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake),ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>),typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake),ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorTurmasCodigoQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTipoNotaPorTurmaQuery, TipoNota>), typeof(ObterTipoNotaPorTurmaQueryHandlerFakeNota), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Deve_listar_4_bimestres_para_modalidade_do_ensino_fundamental_e_medio()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_8,
                Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);
            
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_MATEMATICA_ID_2));
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_HISTORIA_ID_7));
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_GEOGRAFIA_ID_8));
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_INGLES_ID_9);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_ARTES_ID_139);

            var useCase = ServiceProvider.GetService<IObterBimestresComConselhoClasseTurmaUseCase>();
            
            var retorno = (await useCase.Executar(TURMA_ID_1))
                .Where(c => c.Bimestre != 0)
                .GroupBy(c => c.Bimestre);
            
            retorno.Count().ShouldBe(4);
        }
        
        [Fact]
        public async Task Deve_listar_2_bimestres_para_eja()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_8,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);
            
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, ehEja: true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_MATEMATICA_ID_2), ehEja: true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_HISTORIA_ID_7), ehEja: true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_GEOGRAFIA_ID_8), ehEja: true);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_INGLES_ID_9, ehEja: true);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_ARTES_ID_139, ehEja: true);

            var useCase = ServiceProvider.GetService<IObterBimestresComConselhoClasseTurmaUseCase>();

            var retorno = (await useCase.Executar(TURMA_ID_1))
                .Where(c => c.Bimestre != 0)
                .GroupBy(c => c.Bimestre);
            
            retorno.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Deve_exibir_opcao_final_apos_inicio_ultimo_bimestre()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_8,
                Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);
            
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_MATEMATICA_ID_2), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_HISTORIA_ID_7), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_GEOGRAFIA_ID_8), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_INGLES_ID_9, TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_ARTES_ID_139, TipoNota.Nota, true);
            
            
            var useCase = ServiceProvider.GetService<IObterBimestresComConselhoClasseTurmaUseCase>();

            (await useCase.Executar(TURMA_ID_1)).Any(c => c.Bimestre == 0).ShouldBeTrue();
        }
        
        [Fact]
        public async Task Nao_deve_exibir_opcao_final()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_8,
                Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);
            
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_MATEMATICA_ID_2));
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_HISTORIA_ID_7));
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_GEOGRAFIA_ID_8));
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_INGLES_ID_9);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_ARTES_ID_139);

            var useCase = ServiceProvider.GetService<IObterBimestresComConselhoClasseTurmaUseCase>();

            (await useCase.Executar(TURMA_ID_1)).Any(c => c.Bimestre == 0).ShouldBeFalse();
        }        
        
        private async Task CriarDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse,
                CriarPeriodoEscolar = true
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }    
    }
}