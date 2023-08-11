using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_validar_situacao_do_parecer_conclusivo : ConselhoDeClasseTesteBase
    {
        private const int RETIDO = 4;
        private const int RETIDO_POR_FREQUENCIA = 5;
        private const int PROMOVIDO = 1;
        private const int PROMOVIDO_PELO_CONSELHO = 2;

        public Ao_validar_situacao_do_parecer_conclusivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>),typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerParecerConclusivo), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerParecerConclusivo), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerParecerConclusivo), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_sem_parecer()
        {
            await CriarDadosNotas(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_8,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                NOTA_4,
                SituacaoConselhoClasse.EmAndamento,
                false);

            await CriaTurmaFechamentoAtual(TipoNota.Nota, NOTA_4, null);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno(ALUNO_CODIGO_2));
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.FirstOrDefault(f=> f.ConselhoClasseId == CONSELHO_CLASSE_ID_1).ConselhoClasseParecerId.ShouldBeNull();
        }
        
        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_retido_por_frequencia_abaixo_75_por_cento()
        {
            await CriarDadosNotas(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                NOTA_6,
                SituacaoConselhoClasse.EmAndamento,
                false);

            await CriaTurmaFechamentoAtual(TipoNota.Nota, NOTA_6, null);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());

            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == RETIDO_POR_FREQUENCIA).ShouldBeTrue();
        }

        [Theory]
        [InlineData(ANO_6)]
        [InlineData(ANO_9)]
        public async Task Ao_validar_situacao_parecer_conclusivo_retido_por_frequencia_abaixo_75_por_cento_com_compensacao_ausencia_atualizar_parecer(string ano)
        {
            await CriarDadosNotas(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ano,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                NOTA_3,
                SituacaoConselhoClasse.EmAndamento,
                false);

            await CriaTurmaFechamentoAtual(TipoNota.Nota, NOTA_3, null);

            //Gerando parecer como retido com frequência
            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseId == CONSELHO_CLASSE_ID_1 && f.ConselhoClasseParecerId == RETIDO_POR_FREQUENCIA).ShouldBeTrue();
            
            //Ajustando frequencia_aluno para ter compensação de ausência
            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138,1,1);
            
            //Gerando parecer como retido
            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            //Retido pq estamos colocando nota abaixo da média no conselho
            parecerConclusivo.Any(f=> f.ConselhoClasseId == CONSELHO_CLASSE_ID_1 && f.ConselhoClasseParecerId == RETIDO).ShouldBeTrue();
        }
        
        [Fact]
        public async Task Ao_validar_situacao_parecer_conclusivo_retido_por_estudante_com_algum_conceito_ns() 
        {
            await CriarDadosConceito(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Conceito,
                ANO_3,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                NAO_SATISFATORIO_ID_3,
                SituacaoConselhoClasse.EmAndamento,
                false);

            await CriaTurmaFechamentoAtual(TipoNota.Conceito, null, NAO_SATISFATORIO_ID_3);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == RETIDO).ShouldBeTrue();
        }

        [Theory]
        [InlineData(ANO_6)]
        [InlineData(ANO_9)]
        public async Task Ao_validar_situacao_parecer_conclusivo_retido_por_estudante_com_nota_numerica_inferior_a_5(string ano)
        {
            await CriarDadosNotas(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ano,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                NOTA_4,
                SituacaoConselhoClasse.EmAndamento,
                false);

            await CriaTurmaFechamentoAtual(TipoNota.Nota, NOTA_4, null);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == RETIDO).ShouldBeTrue();
        }

        [Theory]
        [InlineData(ANO_5)]
        [InlineData(ANO_7)]
        [InlineData(ANO_8)]
        public async Task Ao_validar_situacao_parecer_conclusivo_deve_ser_diferente_de_retido_por_estudante_com_nota_numerica_inferior_a_5(string ano)
        {
            await CriarDadosNotas(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ano,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                NOTA_4,
                SituacaoConselhoClasse.EmAndamento,
                false);

            await CriaTurmaFechamentoAtual(TipoNota.Nota, NOTA_4, null);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());

            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f => f.ConselhoClasseParecerId == RETIDO).ShouldBeFalse();
        }

        [Theory]
        [InlineData(ANO_3)]
        [InlineData(ANO_6)]
        [InlineData(ANO_9)]
        public async Task Ao_validar_situacao_parecer_conclusivo_retido_por_estudante_com_nota_numerica_inferior_a_5_alterado_nota_maior_atualizar_parecer(string ano)
        {
            await CriarDadosNotas(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ano,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                NOTA_4,
                SituacaoConselhoClasse.EmAndamento,
                false);

            await CriaTurmaFechamentoAtual(TipoNota.Nota, NOTA_4, null);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseId == CONSELHO_CLASSE_ID_1 && f.ConselhoClasseParecerId == RETIDO).ShouldBeTrue();

            var listaFechamento = ObterTodos<FechamentoNota>();
            var fichamentoFinal = listaFechamento.Find(f => f.FechamentoAlunoId == 2);

            fichamentoFinal.Nota = NOTA_8;
            await AtualizarNaBase(fichamentoFinal);

            var listaFechamento2 = ObterTodos<FechamentoNota>();

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseId == CONSELHO_CLASSE_ID_1 && f.ConselhoClasseParecerId != RETIDO).ShouldBeTrue();
        }

        [Theory]
        [InlineData(ANO_3)]
        [InlineData(ANO_6)]
        [InlineData(ANO_9)]
        public async Task Ao_validar_situacao_parecer_conclusivo_promovido_por_estudante_com_nota_numerica_superior_a_5_frequencia_acima_75_por_cento(string ano)
        {
            await CriarDadosNotas(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ano,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                NOTA_8,
                SituacaoConselhoClasse.EmAndamento,
                false
                );

            await CriaTurmaFechamentoAtual(TipoNota.Nota, NOTA_8, null);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == PROMOVIDO).ShouldBeTrue();
        }

        [Theory]
        [InlineData(ANO_3)]
        [InlineData(ANO_6)]
        [InlineData(ANO_9)]
        public async Task Ao_validar_situacao_parecer_conclusivo_promovido_pelo_conselho_por_estudante_com_nota_numerica_superior_a_5_frequencia_acima_75_por_cento(string ano)
        {
            await CriarDadosNotas(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ano,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                NOTA_4,
                SituacaoConselhoClasse.EmAndamento,
                false);

            await CriaTurmaFechamentoAtual(TipoNota.Nota, NOTA_4, null);

            await CriarFrequenciaAluno(TipoFrequenciaAluno.Geral,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());

            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                                  TipoNota.Nota,
                                                                                  NAO_SATISFATORIO_ID_3,
                                                                                  NOTA_9,
                                                                                  FECHAMENTO_TURMA_ID_2,
                                                                                  BIMESTRE_4));

            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                                  TipoNota.Nota,
                                                                                  NAO_SATISFATORIO_ID_3,
                                                                                  NOTA_9,
                                                                                  FECHAMENTO_TURMA_ID_3,
                                                                                  BIMESTRE_FINAL));

            await ExecutarReprocessamentoParacerConclusivo(ObterConselhoClasseFechamentoAluno());
            
            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId == PROMOVIDO_PELO_CONSELHO).ShouldBeTrue();
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

        private async Task CriarFrequenciaAluno(TipoFrequenciaAluno tipoFrequenciaAluno, long componenteCurricular, int totalAulas = 1, int totalCompensacoes = 0, int totalPresencas = 1, int totalRemotos = 0, TipoFrequencia tipoFrequencia = TipoFrequencia.C )
        {
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                Tipo = tipoFrequenciaAluno,
                DisciplinaId = componenteCurricular.ToString(),
                PeriodoInicio = DateTimeExtension.HorarioBrasilia().Date,
                PeriodoFim = DateTimeExtension.HorarioBrasilia().Date,
                Bimestre = BIMESTRE_4,
                TotalAulas = totalAulas,
                TotalCompensacoes = totalCompensacoes,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,
                TurmaId = TURMA_CODIGO_1,
                PeriodoEscolarId = NUMERO_LONGO_1,
                TotalPresencas = totalPresencas,
                TotalRemotos = totalRemotos
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
                Valor = (int)tipoFrequencia,
                NumeroAula = 1,
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
        
        private async Task<IEnumerable<SalvarConselhoClasseAlunoNotaDto>> CriarConselhosClasseComNotasNaoAleatorias(TipoNota tipoNota = TipoNota.Nota, long conceitoId = NAO_SATISFATORIO_ID_3, double nota = NOTA_4)
        {
            var conselhosClasseParaPersistir = new List<SalvarConselhoClasseAlunoNotaDto>();
            conselhosClasseParaPersistir.AddRange(await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, tipoNota, true,conceitoId, nota));
            conselhosClasseParaPersistir.AddRange(await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_MATEMATICA_ID_2), tipoNota, true,conceitoId, nota));
            conselhosClasseParaPersistir.AddRange(await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_HISTORIA_ID_7), tipoNota, true,conceitoId, nota));
            conselhosClasseParaPersistir.AddRange(await CriarConselhoClasseTodosBimestres(long.Parse(COMPONENTE_GEOGRAFIA_ID_8), tipoNota, true,conceitoId, nota));
            conselhosClasseParaPersistir.AddRange(await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_INGLES_ID_9, tipoNota, true,conceitoId, nota));
            conselhosClasseParaPersistir.AddRange(await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_ARTES_ID_139, tipoNota, true,conceitoId, nota));

            return conselhosClasseParaPersistir;
        }

        private async Task CriarDadosConceito(string perfil, 
                                              long componente, 
                                              TipoNota tipo, 
                                              string anoTurma,
                                              Modalidade modalidade, 
                                              ModalidadeTipoCalendario modalidadeTipoCalendario, 
                                              bool anoAnterior,
                                              int? conceitoId,
                                              SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado,
                                              bool criarFechamentoDisciplinaAlunoNota = false 
                                              )
        {
            await CriarDados(perfil,componente,tipo, anoTurma,modalidade,modalidadeTipoCalendario,anoAnterior,null, conceitoId,situacaoConselhoClasse, criarFechamentoDisciplinaAlunoNota);
        }
        
        private async Task CriarDadosNotas(string perfil, 
                                           long componente, 
                                           TipoNota tipo, 
                                           string anoTurma,
                                           Modalidade modalidade, 
                                           ModalidadeTipoCalendario modalidadeTipoCalendario, 
                                           bool anoAnterior,
                                           double? notaFixa,
                                           SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado,
                                           bool criarFechamentoDisciplinaAlunoNota = false)
        {
            await CriarDados(perfil,componente,tipo, anoTurma,modalidade,modalidadeTipoCalendario,anoAnterior,notaFixa,null,situacaoConselhoClasse, criarFechamentoDisciplinaAlunoNota);
        }

        private async Task CriarDados(string perfil, 
                                      long componente, 
                                      TipoNota tipo, 
                                      string anoTurma, 
                                      Modalidade modalidade, 
                                      ModalidadeTipoCalendario modalidadeTipoCalendario, 
                                      bool anoAnterior, 
                                      double? nota4, 
                                      int? conceitoNSId, 
                                      SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, 
                                      bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DateTimeExtension.HorarioBrasilia().Date.AddDays(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_4,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse,
                NotaFixa = nota4,
                ConceitoFixo = conceitoNSId,
                CriarPeriodoEscolar = false
            };

            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }
        
        private async Task<IEnumerable<SalvarConselhoClasseAlunoNotaDto>> CriarConselhoClasseTodosBimestres(long componenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
            TipoNota tipoNota = TipoNota.Nota, bool gerarConselhoBimestreFinal = false, long conceitoId = NAO_SATISFATORIO_ID_3, double nota = NOTA_4 )
        {
            var conselhosClasseParaPersistir = new List<SalvarConselhoClasseAlunoNotaDto>
            {
                ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1),
                ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_2, BIMESTRE_2),
                ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_3, BIMESTRE_4),
                ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_4, BIMESTRE_4)
            };

            if (gerarConselhoBimestreFinal)
                conselhosClasseParaPersistir.Add(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,tipoNota, conceitoId, nota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL));

            foreach (var conselhoClasse in conselhosClasseParaPersistir)
                await ExecutarTesteSemValidacao(conselhoClasse);

            return conselhosClasseParaPersistir;
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