using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;
using ObterTurmaItinerarioEnsinoMedioQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_gerar_parecer_conclusivo : ConselhoDeClasseTesteBase
    {
        public Ao_gerar_parecer_conclusivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>),typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake),ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>),typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake),ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorTurmasCodigoQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesCurricularesEOLPorTurmaECodigoUeQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>), typeof(ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaNotasTodosComponentesCurricularesQuery, bool>), typeof(VerificaNotasTodosComponentesCurricularesQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_gerar_parecer_conclusivo_aluno()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_7,
                Modalidade.Fundamental,
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
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_EDUCACAO_FISICA_ID_6), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_CIENCIAS_ID_89), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_EDUCACAO_FISICA_ID_6), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_INFORMATICA_OIE_ID_1060, TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_LEITURA_OSL_ID_1061, TipoNota.Nota, true);


            var conselhoClasseFechamentoAluno = new ConselhoClasseFechamentoAlunoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1
            };
            
            var gerarParecerConclusivoUseCase = ServiceProvider.GetService<IGerarParecerConclusivoUseCase>();;

            var retorno = await gerarParecerConclusivoUseCase.Executar(conselhoClasseFechamentoAluno);

            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Nome.ShouldNotBeEmpty();
        }
        
        [Fact]
        public async Task Ao_reprocessar_parecer_conclusivo_aluno()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_6,
                Modalidade.Fundamental,
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
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_EDUCACAO_FISICA_ID_6), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_CIENCIAS_ID_89), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_EDUCACAO_FISICA_ID_6), TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_INFORMATICA_OIE_ID_1060, TipoNota.Nota, true);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_LEITURA_OSL_ID_1061, TipoNota.Nota, true);

            var conselhoClasseFechamentoAluno = new ConselhoClasseFechamentoAlunoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1
            };
            
            var reprocessarParecerConclusivoAlunoUseCase = ServiceProvider.GetService<IReprocessarParecerConclusivoAlunoUseCase>();
            
            var retorno = await reprocessarParecerConclusivoAlunoUseCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(conselhoClasseFechamentoAluno)));
            
            retorno.ShouldBeTrue();
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId > 0).ShouldBeTrue();
            parecerConclusivo.Exists(p => p.ParecerAlteradoManual).ShouldBeFalse();
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
                SituacaoConselhoClasse = situacaoConselhoClasse
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }
    }
}