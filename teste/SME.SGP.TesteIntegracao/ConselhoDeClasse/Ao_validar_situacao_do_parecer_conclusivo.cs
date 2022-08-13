using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_validar_situacao_do_parecer_conclusivo : ConselhoDeClasseTesteBase
    {
        private const int RETIDO = 4;
        private const int RETIDO_POR_FREQUENCIA = 5;
        private const int PROMOVIDO = 1;
        public Ao_validar_situacao_do_parecer_conclusivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorTurmasCodigoQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>),typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_sem_parecer()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_8,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);

            await CriarConselhosClasseComNotasNaoAleatorias();
            
            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno(ALUNO_CODIGO_2));
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.FirstOrDefault(f=> f.ConselhoClasseId == CONSELHO_CLASSE_ID_1).ConselhoClasseParecerId.ShouldBeNull();
        }
        
        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_retido_por_frequencia_abaixo_75_por_cento()
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
            
            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());

            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == RETIDO_POR_FREQUENCIA).ShouldBeTrue();
        }

        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_retido_por_estudante_com_algum_conceito_ns() 
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Conceito,
                ANO_3,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CriarConselhosClasseComNotasNaoAleatorias(TipoNota.Conceito);
            
            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == RETIDO).ShouldBeTrue();
        }
        
        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_retido_por_estudante_com_nota_numerica_inferior_a_5()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_8,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CriarConselhosClasseComNotasNaoAleatorias(); 
            
            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == RETIDO).ShouldBeTrue();
        }
        
        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_promovido_por_estudante_com_nota_numerica_superior_a_5_frequencia_acima_75_por_cento()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_8,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CriarConselhosClasseComNotasNaoAleatorias(TipoNota.Nota,NAO_SATISFATORIO_ID_3,NOTA_9);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == PROMOVIDO).ShouldBeTrue();
        }

        private ConselhoClasseFechamentoAlunoDto ObterConselhoClasseFechamentoAluno(string alunoCodigo = ALUNO_CODIGO_1)
        {
            return new ConselhoClasseFechamentoAlunoDto()
            {
                AlunoCodigo = alunoCodigo,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1
            };
        }

        private async Task ExecutarReprocessamentoParacerConclusivo(ConselhoClasseFechamentoAlunoDto conselhoClasseFechamentoAluno)
        {
            var reprocessarParecerConclusivoAlunoUseCase = ServiceProvider.GetService<IReprocessarParecerConclusivoAlunoUseCase>();

            var retorno = await reprocessarParecerConclusivoAlunoUseCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(conselhoClasseFechamentoAluno)));

            retorno.ShouldBeTrue();
        }

        private async Task CriarFrequenciaAluno(TipoFrequenciaAluno tipoFrequenciaAluno, long componenteCurricular)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                Tipo = tipoFrequenciaAluno,
                DisciplinaId = componenteCurricular.ToString(),
                PeriodoInicio = DATA_02_05_INICIO_BIMESTRE_2,
                PeriodoFim = DATA_02_05_INICIO_BIMESTRE_2,
                Bimestre = BIMESTRE_2,
                TotalAulas = 1,
                TotalCompensacoes = 0,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,
                TurmaId = TURMA_CODIGO_1,
                PeriodoEscolarId = NUMERO_LONGO_1,
                TotalPresencas = 1,
                TotalRemotos = 0
            });
            
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                RegistroFrequenciaId = 1,
                Valor = (int)TipoFrequencia.C,
                NumeroAula = 1,
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
        
        private async Task CriarConselhosClasseComNotasNaoAleatorias(TipoNota tipoNota = TipoNota.Nota, long conceitoId = NAO_SATISFATORIO_ID_3, double nota = NOTA_4)
        {
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, tipoNota, true,conceitoId, nota);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_MATEMATICA_ID_2), tipoNota, true,conceitoId, nota);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_HISTORIA_ID_7), tipoNota, true,conceitoId, nota);
            await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_GEOGRAFIA_ID_8), tipoNota, true,conceitoId, nota);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_INGLES_ID_9, tipoNota, true,conceitoId, nota);
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_ARTES_ID_139, tipoNota, true,conceitoId, nota);
        }

        private async Task CriarDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroNotasDto()
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
        
        private async Task CriarConselhoClasseTodosBimestres(long componenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
            TipoNota tipoNota = TipoNota.Nota, bool gerarConselhoBimestreFinal = false, long conceitoId = NAO_SATISFATORIO_ID_3, 
            double nota = NOTA_4 )
        {
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular, 
                tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1));
                
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_2, BIMESTRE_2));
            
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_3, BIMESTRE_3));
            
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_4, BIMESTRE_4));

            if (gerarConselhoBimestreFinal)
                await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                    tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL));
        }
        
        private SalvarConselhoClasseAlunoNotaDto ObterSalvarConselhoClasseAlunoNotaDto(long componenteCurricular, TipoNota tipoNota 
            ,long conceitoId, double nota,long fechamentoTurma = FECHAMENTO_TURMA_ID_2, int bimestre = BIMESTRE_2)
        {
            return new SalvarConselhoClasseAlunoNotaDto()
            {
                ConselhoClasseNotaDto = ObterConselhoClasseNotaDto(componenteCurricular,tipoNota, conceitoId, nota),
                CodigoAluno = ALUNO_CODIGO_1,
                ConselhoClasseId = 0,
                FechamentoTurmaId = fechamentoTurma,
                CodigoTurma = TURMA_CODIGO_1,
                Bimestre = bimestre
            };
        }
        
        private ConselhoClasseNotaDto ObterConselhoClasseNotaDto(long componenteCurricular, TipoNota tipoNota,long conceitoId, double nota)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = componenteCurricular,
                Justificativa = JUSTIFICATIVA,
                Conceito = tipoNota == TipoNota.Conceito ? conceitoId : null,
                Nota = tipoNota == TipoNota.Nota ? nota : null
            };
        }
    }
}