using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ObterTurmaItinerarioEnsinoMedioQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public abstract class ConselhoDeClasseTesteBase : TesteBaseComuns
    {
        protected const long AULA_ID_1 = 1;

        protected const string TIPO_FREQUENCIA_COMPARECEU = "C";
        protected const string TIPO_FREQUENCIA_FALTOU = "F";
        protected const string TIPO_FREQUENCIA_REMOTO = "R";

        protected const string CODIGO_ALUNO_99999 = "99999";

        protected const string ALUNO_CODIGO_1 = "1";
        protected const string ALUNO_CODIGO_2 = "2";
        protected const string ALUNO_CODIGO_3 = "3";
        protected const string ALUNO_CODIGO_4 = "4";

        protected const double NOTA_1 = 1;
        protected const double NOTA_2 = 2;
        protected const double NOTA_3 = 3;
        protected const double NOTA_4 = 4;
        protected const double NOTA_5 = 5;
        protected const double NOTA_6 = 6;
        protected const double NOTA_7 = 7;
        protected const double NOTA_8 = 8;
        protected const double NOTA_9 = 9;
        protected const double NOTA_10 = 10;

        protected const string NOTA = "NOTA";
        protected const string CONCEITO = "CONCEITO";

        protected const int CONSELHO_CLASSE_ID_1 = 1;
        protected const int FECHAMENTO_TURMA_ID_1 = 1;
        protected const int FECHAMENTO_TURMA_ID_2 = 2;
        protected const int FECHAMENTO_TURMA_ID_3 = 3;
        protected const int FECHAMENTO_TURMA_ID_4 = 4;
        protected const int FECHAMENTO_TURMA_ID_5 = 5;
        protected const int CONSELHO_CLASSE_ALUNO_ID_1 = 1;
        protected const string JUSTIFICATIVA = "Nota pós conselho";
        private const string COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO = "componente_curricular_grupo_area_ordenacao";

        protected ConselhoDeClasseTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerComRegistroFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterDisciplinasTurmasEolQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaAlunoPorCodigoAlunoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmaAlunoPorCodigoAlunoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<RemoverArquivosExcluidosCommand, bool>), typeof(RemoverArquivosExcluidosCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>), typeof(ObterDisciplinasPorCodigoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>), typeof(ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected async Task ExecutarTeste(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            await ExecutarTeste(filtroConselhoClasseDto.SalvarConselhoClasseAlunoNotaDto,
                filtroConselhoClasseDto.ConsiderarAnoAnterior,
                filtroConselhoClasseDto.TipoNota,
                filtroConselhoClasseDto.SituacaoConselho);
        }

        protected async Task ExecutarTesteSemValidacao(FiltroConselhoClasseDto filtroConselhoClasseDto,
                                                       bool anoAnterior,
                                                       TipoNota tipoNota,
                                                       SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento)
        {
            var comando = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();

            await comando.Executar(filtroConselhoClasseDto.SalvarConselhoClasseAlunoNotaDto);

            var consolidacaoAluno = ServiceProvider.GetService<IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase>();

            var mensagem = new MensagemConsolidacaoConselhoClasseAlunoDto(filtroConselhoClasseDto.AlunoCodigo,
                TURMA_ID_1,
                filtroConselhoClasseDto.BimestreConselhoClasse == 0 ? null : filtroConselhoClasseDto.BimestreConselhoClasse,
                false,
                filtroConselhoClasseDto.SalvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular);

            await consolidacaoAluno.Executar(new MensagemRabbit(JsonConvert.SerializeObject(mensagem)));
        }

        protected async Task ExecutarTesteSemValidacao(SalvarConselhoClasseAlunoNotaDto salvarConselhoClasseAlunoNotaDto)
        {
            var comando = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            comando.ShouldNotBeNull();

            await comando.Executar(salvarConselhoClasseAlunoNotaDto);

            var consolidacaoAluno = ServiceProvider.GetService<IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase>();

            var mensagem = new MensagemConsolidacaoConselhoClasseAlunoDto(salvarConselhoClasseAlunoNotaDto.CodigoAluno,
                TURMA_ID_1,
                salvarConselhoClasseAlunoNotaDto.Bimestre == 0 ? null : salvarConselhoClasseAlunoNotaDto.Bimestre,
                false,
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular);

            await consolidacaoAluno.Executar(new MensagemRabbit(JsonConvert.SerializeObject(mensagem)));
        }

        protected async Task<NegocioException> ValidarTesteComExcecao(SalvarConselhoClasseAlunoNotaDto salvarConselhoClasseAlunoNotaDto)
        {
            var salvarConselhoClasseAlunoNotaUseCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            return await Assert.ThrowsAsync<NegocioException>(async () => await salvarConselhoClasseAlunoNotaUseCase.Executar(salvarConselhoClasseAlunoNotaDto));
        }

        protected async Task ExecutarTeste(SalvarConselhoClasseAlunoNotaDto salvarConselhoClasseAlunoNotaDto,
                                    bool anoAnterior,
                                    TipoNota tipoNota,
                                    SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                                    bool ehPerfilGestor = false,
                                    bool ehEja = false,
                                    params long[] componentesRegencia)
        {
            var ehBimestreFinal = salvarConselhoClasseAlunoNotaDto.Bimestre == 0;

            var comando = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();

            var dtoRetorno = await comando.Executar(salvarConselhoClasseAlunoNotaDto);
            dtoRetorno.ShouldNotBeNull();

            var consolidacaoNotaAluno = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            var conselhosClasse = ObterTodos<ConselhoClasse>();
            conselhosClasse.ShouldNotBeNull();

            var situacaoConselhoClasseInserida = ehBimestreFinal
                                    ? conselhosClasse.LastOrDefault()
                                    : conselhosClasse.FirstOrDefault();

            (conselhosClasse.FirstOrDefault().Situacao == situacaoConselhoClasse).ShouldBeTrue();

            var conselhosDeClasseAlunos = ObterTodos<ConselhoClasseAluno>();
            conselhosDeClasseAlunos.ShouldNotBeNull();

            if (ehBimestreFinal)
                conselhosDeClasseAlunos.Count(a => a.AlunoCodigo.Equals(salvarConselhoClasseAlunoNotaDto.CodigoAluno)).ShouldBe(ehEja ? 3 : 5);
            else
                conselhosDeClasseAlunos.Any(s => !s.AlunoCodigo.Equals(salvarConselhoClasseAlunoNotaDto.CodigoAluno)).ShouldBeFalse();

            var conselhoClasseAlunoInserido = conselhosDeClasseAlunos.Where(f => f.ConselhoClasseId == situacaoConselhoClasseInserida.Id);
            conselhoClasseAlunoInserido.Any(s => !s.AlunoCodigo.Equals(salvarConselhoClasseAlunoNotaDto.CodigoAluno)).ShouldBeFalse();

            var conselhosClasseNotas = ObterTodos<ConselhoClasseNota>();
            conselhosClasseNotas.ShouldNotBeNull();

            var classeNota = conselhosClasseNotas.LastOrDefault(nota => nota.ConselhoClasseAlunoId == conselhoClasseAlunoInserido.FirstOrDefault().Id);

            classeNota.ShouldNotBeNull();
            classeNota.Justificativa.ShouldBe(salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Justificativa);

            if (anoAnterior && !ehPerfilGestor)
            {
                classeNota.Nota.ShouldBeNull();
                classeNota.ConceitoId.ShouldBeNull();

                var aprovacoesNotasConselho = ObterTodos<WFAprovacaoNotaConselho>();

                aprovacoesNotasConselho.ShouldNotBeNull();

                var aprovacaoNotaConselho = aprovacoesNotasConselho.FirstOrDefault(aprovacao => aprovacao.ConselhoClasseNotaId == classeNota.Id);
                aprovacaoNotaConselho.ShouldNotBeNull();

                if (tipoNota == TipoNota.Nota)
                    aprovacaoNotaConselho.Nota.ShouldBe(salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Nota);
                else
                    aprovacaoNotaConselho.ConceitoId.ShouldBe(salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Conceito);

            }
            else
            {
                if (tipoNota == TipoNota.Nota)
                    classeNota.Nota.ShouldBe(salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Nota);
                else
                    classeNota.ConceitoId.ShouldBe(salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Conceito);
            }

            var consolidacaoAluno = ServiceProvider.GetService<IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase>();

            var mensagem = new MensagemConsolidacaoConselhoClasseAlunoDto(salvarConselhoClasseAlunoNotaDto.CodigoAluno,
                TURMA_ID_1,
                salvarConselhoClasseAlunoNotaDto.Bimestre,
                false,
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular);

            await consolidacaoAluno.Executar(new MensagemRabbit(JsonConvert.SerializeObject(mensagem)));

            var alunosConsolidacao = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            alunosConsolidacao.ShouldNotBeNull();

            var alunoConsolidado = alunosConsolidacao.LastOrDefault(nota =>
                    nota.AlunoCodigo.Equals(salvarConselhoClasseAlunoNotaDto.CodigoAluno));


            var notasConsolidacao = ObterTodos<ConselhoClasseConsolidadoTurmaAlunoNota>();
            notasConsolidacao.ShouldNotBeNull();

            var notaConsolidacao = componentesRegencia.NaoEhNulo() && componentesRegencia.Any() ? notasConsolidacao.LastOrDefault(s => s.ConselhoClasseConsolidadoTurmaAlunoId == alunoConsolidado.Id && componentesRegencia.Contains(s.ComponenteCurricularId.Value)) : notasConsolidacao.LastOrDefault(s => s.ConselhoClasseConsolidadoTurmaAlunoId == alunoConsolidado.Id && s.ComponenteCurricularId.Equals(salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular));
            notaConsolidacao.ShouldNotBeNull();

            if (tipoNota == TipoNota.Nota)
            {
                var atual = notaConsolidacao.Nota;
                (salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Nota == atual).ShouldBeTrue();
            }
            else
            {
                var conceitoId = notaConsolidacao.ConceitoId;
                (salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Conceito == conceitoId).ShouldBeTrue();
            }
        }

        protected async Task CriarDadosBase(FiltroConselhoClasseDto filtroConselhoClasse)
        {
            await CriarDadosBaseSemFechamentoTurmaSemAberturaReabertura(filtroConselhoClasse);

            if (filtroConselhoClasse.CriarFechamentoDisciplinaAlunoNota)
                await CriarFechamentoTurmaDisciplinaAlunoNota(filtroConselhoClasse);
            else
                await CriarFechamentoTurma(filtroConselhoClasse.Bimestre);

            if (filtroConselhoClasse.CriarPeriodoAbertura)
                await CriarPeriodoAberturaCustomizadoQuartoBimestre();

            if (filtroConselhoClasse.CriarPeriodoReabertura)
                await CriarPeriodoReabertura(filtroConselhoClasse.TipoCalendarioId, filtroConselhoClasse.ConsiderarAnoAnterior);

        }

        protected async Task CriarDadosBaseSemFechamentoTurmaSemAberturaReabertura(FiltroConselhoClasseDto filtroConselhoClasse)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtroConselhoClasse.Perfil);

            await CriarUsuarios();

            await CriarTurmaTipoCalendario(filtroConselhoClasse);

            if (filtroConselhoClasse.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(filtroConselhoClasse);


            await CriarAula(filtroConselhoClasse.ComponenteCurricular, filtroConselhoClasse.DataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);

            await CriarParametrosNotas();

            await CriarAbrangencia(filtroConselhoClasse.Perfil);

            await CriarCiclo();

            await CriarNotasTipoEParametros(filtroConselhoClasse.ConsiderarAnoAnterior);

            await CriarConselhoClasseRecomendacao();

            await CriaConceito();

            await CriarConselhoClasseParecerAno();
        }

        protected async Task CriarFechamentoTurmaDisciplinaAlunoNota(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            var periodosEscolares = ObterTodos<PeriodoEscolar>();

            var fechamentoTurmaId = 1;
            var fechamentoTurmaDisciplinaId = 1;

            //Lançamento de fechamento bimestral 
            foreach (var periodoEscolar in periodosEscolares)
            {
                await CriarFechamentoTurma(periodoEscolar.Id);

                await CriarFechamentoTurmaDisciplina(long.Parse(filtroConselhoClasseDto.ComponenteCurricular), fechamentoTurmaId);

                await CriarFechamentoTurmaAluno(fechamentoTurmaDisciplinaId);

                fechamentoTurmaDisciplinaId++;
                fechamentoTurmaId++;
            }

            //Lançamento de fechamento Final
            await CriarFechamentoTurma(null);

            await CriarFechamentoTurmaDisciplina(long.Parse(filtroConselhoClasseDto.ComponenteCurricular), fechamentoTurmaId);

            await CriarFechamentoTurmaAluno(fechamentoTurmaDisciplinaId);

            await CriarFechamentoTurmaAlunoNota(filtroConselhoClasseDto);
        }

        protected async Task CriaTurmaFechamentoAtual(TipoNota tipoNota, double? nota, long? conceito)
        {
            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodo = periodosEscolares.Find(p => p.Bimestre == BIMESTRE_4);

            await CriarFechamentoTurma(periodo.Id);

            await CriarFechamentoTurmaDisciplina(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, FECHAMENTO_TURMA_ID_2);


            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = ALUNO_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                FechamentoAlunoId = 1,
                Nota = tipoNota == TipoNota.Nota ? nota : null,
                ConceitoId = tipoNota == TipoNota.Conceito ? conceito : null,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await CriarFechamentoTurma(null);

            await CriarFechamentoTurmaDisciplina(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, FECHAMENTO_TURMA_ID_3);

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 2,
                AlunoCodigo = ALUNO_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                FechamentoAlunoId = 2,
                Nota = tipoNota == TipoNota.Nota ? nota : null,
                ConceitoId = tipoNota == TipoNota.Conceito ? conceito : null,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                Situacao = SituacaoConselhoClasse.Concluido,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task InserirConselhoClassePadrao(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            var fechamentoTurmas = ObterTodos<FechamentoTurma>();

            long conselhoClasseId = 1;

            long conselhoClasseAlunoId = 1;

            foreach (var fechamentoTurma in fechamentoTurmas)
            {
                await InserirNaBase(new ConselhoClasse()
                {
                    FechamentoTurmaId = fechamentoTurma.Id,
                    Situacao = filtroConselhoClasseDto.SituacaoConselhoClasse,
                    CriadoEm = DateTime.Now,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                foreach (var alunoCodigo in ObterAlunos())
                {
                    await InserirNaBase(new ConselhoClasseAluno()
                    {
                        ConselhoClasseId = conselhoClasseId,
                        AlunoCodigo = alunoCodigo,
                        CriadoEm = DateTime.Now,
                        CriadoPor = SISTEMA_NOME,
                        CriadoRF = SISTEMA_CODIGO_RF
                    });

                    foreach (var componenteCurricular in ObterComponentesCurriculares())
                    {
                        await InserirNaBase(new ConselhoClasseNota()
                        {
                            ComponenteCurricularCodigo = componenteCurricular,
                            ConselhoClasseAlunoId = conselhoClasseAlunoId,
                            Justificativa = JUSTIFICATIVA,
                            Nota = filtroConselhoClasseDto.TipoNota == TipoNota.Nota ? new Random().Next(0, 10) : null,
                            ConceitoId = filtroConselhoClasseDto.TipoNota == TipoNota.Conceito ? new Random().Next(1, 3) : null,
                            CriadoEm = DateTime.Now,
                            CriadoPor = SISTEMA_NOME,
                            CriadoRF = SISTEMA_CODIGO_RF
                        });
                    }
                    conselhoClasseAlunoId++;
                }

                conselhoClasseId++;
            }
        }

        protected IEnumerable<long> ObterComponentesCurriculares()
        {
            return new List<long>()
            {
                long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                long.Parse(COMPONENTE_HISTORIA_ID_7),
                long.Parse(COMPONENTE_GEOGRAFIA_ID_8),
                long.Parse(COMPONENTE_CIENCIAS_ID_89),
                long.Parse(COMPONENTE_EDUCACAO_FISICA_ID_6),
                COMPONENTE_CURRICULAR_ARTES_ID_139,
                long.Parse(COMPONENTE_MATEMATICA_ID_2)
            };
        }

        protected async Task CriarFechamentoTurmaDisciplina(long componenteCurricular, int fechamentoTurmaId)
        {
            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = componenteCurricular,
                FechamentoTurmaId = fechamentoTurmaId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFechamentoTurma(long? periodoEscolarId)
        {
            await InserirNaBase(new FechamentoTurma()
            {
                PeriodoEscolarId = periodoEscolarId == 0 ? null : periodoEscolarId,
                TurmaId = TURMA_ID_1,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarComponenteGrupoAreaOrdenacao()
        {
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_1, CODIGO_1, CODIGO_1);
        }

        private async Task CriarConselhoClasseRecomendacao()
        {
            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Recomendacao = "Recomendação aluno",
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Recomendacao = "Recomendação familia",
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFechamentoTurmaAluno(long fechamentoTurmaDisciplinaId)
        {
            foreach (var aluno in ObterAlunos())
            {
                await InserirNaBase(new FechamentoAluno()
                {
                    FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId,
                    AlunoCodigo = aluno,
                    CriadoEm = DateTime.Now,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        private async Task CriarFechamentoTurmaAlunoNota(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            var fechamentoAlunos = ObterTodos<FechamentoAluno>();

            foreach (var fechamentoAluno in fechamentoAlunos)
            {
                await InserirNaBase(new FechamentoNota()
                {
                    DisciplinaId = long.Parse(filtroConselhoClasseDto.ComponenteCurricular),
                    FechamentoAlunoId = fechamentoAluno.Id,
                    Nota = filtroConselhoClasseDto.TipoNota == TipoNota.Nota ? filtroConselhoClasseDto.NotaFixa.HasValue ? filtroConselhoClasseDto.NotaFixa : new Random().Next(1, 10) : null,
                    ConceitoId = filtroConselhoClasseDto.TipoNota == TipoNota.Conceito ? filtroConselhoClasseDto.ConceitoFixo.HasValue ? filtroConselhoClasseDto.ConceitoFixo : new Random().Next(1, 3) : null,
                    CriadoEm = DateTime.Now,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        private List<string> ObterAlunos()
        {
            return new List<string>()
                { ALUNO_CODIGO_1, ALUNO_CODIGO_2, ALUNO_CODIGO_3, ALUNO_CODIGO_4, ALUNO_CODIGO_5 };
        }

        protected async Task CriarTurmaTipoCalendario(FiltroConselhoClasseDto filtroConselhoClasse)
        {
            await CriarTipoCalendario(filtroConselhoClasse.TipoCalendario, filtroConselhoClasse.ConsiderarAnoAnterior);
            await CriarTurma(filtroConselhoClasse.Modalidade, filtroConselhoClasse.AnoTurma, filtroConselhoClasse.ConsiderarAnoAnterior);
        }

        private async Task CriarNotasTipoEParametros(bool consideraAnoAnterior = false)
        {
            var dataBase = consideraAnoAnterior ? new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 01) : new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = dataBase,
                TipoNota = TipoNota.Nota,
                Descricao = NOTA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = dataBase,
                TipoNota = TipoNota.Conceito,
                Descricao = CONCEITO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 1,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 2,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 3,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 4,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 5,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 6,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 7,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 8,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaParametro()
            {
                Minima = 0,
                Media = 5,
                Maxima = 10,
                Incremento = 0.5,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarCiclo()
        {
            await InserirNaBase(new Ciclo()
            {
                Descricao = ALFABETIZACAO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_1,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_2,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_3,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = INTERDISCIPLINAR,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_4,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_5,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_6,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = AUTORAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_7,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_8,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_9,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = MEDIO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_1,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_2,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_3,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_4,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_ALFABETIZACAO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 5,
                Ano = ANO_1,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_BASICA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 6,
                Ano = ANO_2,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_COMPLEMENTAR,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 7,
                Ano = ANO_3,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_FINAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 8,
                Ano = ANO_4,
                Modalidade = Modalidade.EJA
            });
        }

        protected async Task CriarAbrangencia(string perfil)
        {
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = new Guid(perfil),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_ID_1
            });
        }

        private async Task CriarParametrosNotas()
        {
            var dataAtualAnoAnterior = DateTimeExtension.HorarioBrasilia().AddYears(-1);

            var dataAtualAnoAtual = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.DataInicioSGP,
                Valor = dataAtualAnoAnterior.Year.ToString(),
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.DataInicioSGP,
                Valor = dataAtualAnoAtual.Year.ToString(),
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Descricao = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Valor = NUMERO_50,
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Descricao = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Valor = NUMERO_50,
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = MEDIA_BIMESTRAL,
                Descricao = MEDIA_BIMESTRAL,
                Tipo = TipoParametroSistema.MediaBimestre,
                Valor = NUMERO_5,
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = MEDIA_BIMESTRAL,
                Descricao = MEDIA_BIMESTRAL,
                Tipo = TipoParametroSistema.MediaBimestre,
                Valor = NUMERO_5,
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = MEDIA_BIMESTRAL,
                Descricao = MEDIA_BIMESTRAL,
                Tipo = TipoParametroSistema.AprovacaoAlteracaoNotaConselho,
                Valor = NUMERO_5,
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "AprovacaoAlteracaoNotaConselho",
                Descricao = "Solicita aprovação nas alterações de notas do conselho",
                Tipo = TipoParametroSistema.AprovacaoAlteracaoNotaConselho,
                Valor = string.Empty,
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "PercentualFrequenciaCritico",
                Descricao = "Percentual de frequência para definir aluno em situação crítica",
                Tipo = TipoParametroSistema.PercentualFrequenciaCritico,
                Valor = "75",
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "PercentualFrequenciaCritico",
                Descricao = "Percentual de frequência para definir aluno em situação crítica na Base Nacional Comum",
                Tipo = TipoParametroSistema.PercentualFrequenciaCriticoBaseNacional,
                Valor = "50",
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoEscolar(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            if (filtroConselhoClasseDto.Modalidade == Modalidade.EJA || filtroConselhoClasseDto.Modalidade == Modalidade.CELP)
            {
                await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, filtroConselhoClasseDto.ConsiderarAnoAnterior);
                await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, filtroConselhoClasseDto.ConsiderarAnoAnterior);
                await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_2, filtroConselhoClasseDto.ConsiderarAnoAnterior);
                await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_2, filtroConselhoClasseDto.ConsiderarAnoAnterior);

                return;
            }

            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, filtroConselhoClasseDto.ConsiderarAnoAnterior);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, filtroConselhoClasseDto.ConsiderarAnoAnterior);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, filtroConselhoClasseDto.ConsiderarAnoAnterior);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, filtroConselhoClasseDto.ConsiderarAnoAnterior);
        }

        protected async Task CriarPeriodoEscolarCustomizadoQuartoBimestre(bool periodoEscolarValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await CriarPeriodoEscolar(dataReferencia.AddDays(-285), dataReferencia.AddDays(-210), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-200), dataReferencia.AddDays(-125), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-20), periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5), BIMESTRE_4, TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoAberturaCustomizadoQuartoBimestre(bool periodoEscolarValido = true)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new PeriodoFechamento()
            { CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-209),
                FinalDoFechamento = dataReferencia.AddDays(-205)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento = dataReferencia.AddDays(-116)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento = dataReferencia.AddDays(-116)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = dataReferencia.AddDays(-38),
                FinalDoFechamento = dataReferencia.AddDays(-34)
            });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                PeriodoFechamentoId = 1,
                InicioDoFechamento = periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5),
                FinalDoFechamento = periodoEscolarValido ? dataReferencia.AddDays(4) : dataReferencia.AddDays(-2)
            });
        }

        private ComponenteCurricularDto ObterComponenteCurricular(long componenteCurricularId)
        {
            if (componenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME
                };
            else if (componenteCurricularId == COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_DESCONHECIDO_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME
                };

            return null;
        }

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222, TipoAula tipoAula = TipoAula.Normal)
        {
            await InserirNaBase(ObterAula(componenteCurricularCodigo, dataAula, recorrencia, quantidadeAula, rf, tipoAula));
        }

        private Dominio.Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222, TipoAula tipoAula = TipoAula.Normal)
        {
            return new Dominio.Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componenteCurricularCodigo,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = quantidadeAula,
                DataAula = dataAula,
                RecorrenciaAula = recorrencia,
                TipoAula = tipoAula,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false
            };
        }

        protected async Task CriarPeriodoEscolarEAberturaPadrao()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarMotivosAusencias(string descricao)
        {
            await InserirNaBase(new MotivoAusencia() { Descricao = descricao });
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId, bool considerarAnoAnterior = false)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = considerarAnoAnterior ? DATA_01_01.AddYears(-1) : DATA_01_01,
                Fim = DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        protected async Task CriaConceito()
        {
            await InserirNaBase(new Conceito()
            {
                Descricao = "Excelente",
                Aprovado = true,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "E",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Conceito()
            {
                Descricao = "Bom",
                Aprovado = true,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "B",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Conceito()
            {
                Descricao = "Ruim",
                Aprovado = false,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "R",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected static ConselhoClasseNotaDto ObterFiltroConselhoClasseNotaDto(double? nota, long? conceito, string justificavtiva, long codigoComponenteCurricular)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = codigoComponenteCurricular,
                Conceito = conceito,
                Nota = nota,
                Justificativa = justificavtiva
            };
        }

        protected async Task CriarConselhoClasseTodosBimestres(long componenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
            TipoNota tipoNota = TipoNota.Nota, bool gerarConselhoBimestreFinal = false, bool ehEja = false)
        {
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                tipoNota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1));

            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                    tipoNota, FECHAMENTO_TURMA_ID_2, BIMESTRE_2));

            if (!ehEja)
            {
                await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                tipoNota, FECHAMENTO_TURMA_ID_4, BIMESTRE_4));

                await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                    tipoNota, FECHAMENTO_TURMA_ID_3, BIMESTRE_3));
            }

            if (gerarConselhoBimestreFinal)
            {
                await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(componenteCurricular,
                    tipoNota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL));
            }
        }

        protected SalvarConselhoClasseAlunoNotaDto ObterSalvarConselhoClasseAlunoNotaDto(long componenteCurricular,
            TipoNota tipoNota, long fechamentoTurma = FECHAMENTO_TURMA_ID_3, int bimestre = BIMESTRE_3)
        {
            return ObterSalvarConselhoClasseAlunoNotaDto(0, ALUNO_CODIGO_1, componenteCurricular, tipoNota, fechamentoTurma,
                bimestre);
        }
        protected SalvarConselhoClasseAlunoNotaDto ObterSalvarConselhoClasseAlunoNotaMaiorQueDezDto(long componenteCurricular,
            TipoNota tipoNota, long fechamentoTurma = FECHAMENTO_TURMA_ID_3, int bimestre = BIMESTRE_3)
        {
            return new SalvarConselhoClasseAlunoNotaDto
            {
                ConselhoClasseNotaDto = new ConselhoClasseNotaDto()
                {
                    CodigoComponenteCurricular = componenteCurricular,
                    Justificativa = JUSTIFICATIVA,
                    Conceito = null,
                    Nota = 11
                },
                CodigoAluno = ALUNO_CODIGO_1,
                ConselhoClasseId = 0,
                FechamentoTurmaId = fechamentoTurma,
                CodigoTurma = TURMA_CODIGO_1,
                Bimestre = bimestre
            };
        }

        protected SalvarConselhoClasseAlunoNotaDto ObterSalvarConselhoClasseAlunoNotaDto(long conselhoClasseId, string alunoCodigo,
            long componenteCurricular, TipoNota tipoNota, long fechamentoTurma, int bimestre)
        {
            return new SalvarConselhoClasseAlunoNotaDto
            {
                ConselhoClasseNotaDto = ObterConselhoClasseNotaDto(componenteCurricular, tipoNota),
                CodigoAluno = alunoCodigo,
                ConselhoClasseId = conselhoClasseId,
                FechamentoTurmaId = fechamentoTurma,
                CodigoTurma = TURMA_CODIGO_1,
                Bimestre = bimestre
            };
        }


        private async Task CriarConselhoClasseParecerAno()
        {
            await InserirNaBase(new ConselhoClasseParecer()
            {
                Nome = "Promovido",
                Aprovado = true,
                Frequencia = true,
                Conselho = false,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                Nota = true,
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecer()
            {
                Nome = "Promovido pelo conselho",
                Aprovado = true,
                Frequencia = false,
                Conselho = true,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                Nota = false,
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecer()
            {
                Nome = "Continuidade dos estudos",
                Aprovado = true,
                Frequencia = true,
                Conselho = false,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                Nota = false,
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecer()
            {
                Nome = "Retido",
                Aprovado = false,
                Frequencia = false,
                Conselho = true,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                Nota = true,
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecer()
            {
                Nome = "Retido por frequência",
                Aprovado = false,
                Frequencia = true,
                Conselho = false,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                Nota = false,
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3,
                AnoTurma = 1,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3,
                AnoTurma = 2,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3,
                AnoTurma = 4,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3,
                AnoTurma = 5,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3,
                AnoTurma = 7,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3,
                AnoTurma = 8,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 1,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 1,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 2,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 4,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 5,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 1,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 2,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 3,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 4,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 3,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 6,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 9,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 1,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 2,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 1,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 2,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 3,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 4,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 3,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 6,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 9,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 1,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 2,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 3,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 1,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 2,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 3,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 4,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 3,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 6,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 9,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 1,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 2,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 3,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 2,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 3,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 4,
                Modalidade = 3,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 3,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 6,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 7,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 8,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 9,
                Modalidade = 5,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 1,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 2,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 3,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 3,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 4,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 4,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5,
                AnoTurma = 4,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 4,
                Modalidade = 6,
                InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
        }

        private ConselhoClasseNotaDto ObterConselhoClasseNotaDto(long componenteCurricular, TipoNota tipoNota)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = componenteCurricular,
                Justificativa = JUSTIFICATIVA,
                Conceito = tipoNota == TipoNota.Conceito ? new Random().Next(1, 3) : null,
                Nota = tipoNota == TipoNota.Nota ? new Random().Next(1, 10) : null
            };
        }

        protected IConsolidarConselhoClasseUseCase RetornarConsolidarConselhoClasseUseCase()
        {
            return ServiceProvider.GetService<IConsolidarConselhoClasseUseCase>();
        }

        protected class FiltroConselhoClasseDto
        {
            public FiltroConselhoClasseDto()
            {
                CriarPeriodoEscolar = true;
                TipoCalendarioId = TIPO_CALENDARIO_1;
                CriarPeriodoAbertura = true;
                ConsiderarAnoAnterior = false;
            }

            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public long? Bimestre { get; set; }
            public string ComponenteCurricular { get; set; }
            public long TipoCalendarioId { get; set; }
            public bool CriarPeriodoEscolar { get; set; }
            public bool CriarPeriodoAbertura { get; set; }
            public TipoNota TipoNota { get; set; }
            public string AnoTurma { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
            public DateTime DataAula { get; set; }
            public bool CriarFechamentoDisciplinaAlunoNota { get; set; }
            public SituacaoConselhoClasse SituacaoConselhoClasse { get; set; }
            public bool CriarConselhoClasseFinal { get; set; }
            public double? NotaFixa { get; set; }
            public int? ConceitoFixo { get; set; }
            public SituacaoConselhoClasse SituacaoConselho { get; set; }
            public string AlunoCodigo { get; set; }
            public int BimestreConselhoClasse { get; set; }
            public SalvarConselhoClasseAlunoNotaDto SalvarConselhoClasseAlunoNotaDto { get; set; }
            public bool CriarPeriodoReabertura { get; set; } = true;
        }
    }
}