using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;
using ObterTurmaItinerarioEnsinoMedioQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse
{
    public class Ao_alterar_parecer_conclusivo : ConselhoDeClasseTesteBase
    {
        public Ao_alterar_parecer_conclusivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake138), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorTurmasCodigoLPQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosEAnoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosEAnoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Conselho Classe - Alterar parecer conclusivo do aluno para ano letivo atual")]
        public async Task Ao_alterar_parecer_conclusivo_ano_letivo_atual()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = null,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_4,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = false,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, false, ATIVIDADE_AVALIATIVA_1);

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoConselhoClasse.EmAndamento
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseParecerId = PARECER_CONCLUSIVO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            }); 

            await InserirNaBase(new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_8,
                Justificativa = "Justificativa",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var obterPareceresConclusivosUseCase = ServiceProvider.GetService<IAlterarParecerConclusivoUseCase>();
            var dto = new AlterarParecerConclusivoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                ParecerConclusivoId = PARECER_CONCLUSIVO_ID_2
            };

            var retorno = await obterPareceresConclusivosUseCase.Executar(dto);
            retorno.ShouldNotBeNull();
            retorno.EmAprovacao.ShouldBeFalse();
            var conselhoAlunos = ObterTodos<ConselhoClasseAluno>();
            conselhoAlunos.ShouldNotBeNull();
            var conselho = conselhoAlunos.FirstOrDefault();
            conselho.ConselhoClasseParecerId.ShouldBe(PARECER_CONCLUSIVO_ID_2);
            conselho.ParecerAlteradoManual.ShouldBeTrue();
        }

        [Fact(DisplayName = "Conselho Classe - Alterar parecer conclusivo do aluno para ano letivo anterior")]
        public async Task Ao_alterar_parecer_conclusivo_ano_letivo_anterior()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia().AddYears(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = null,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_4,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = false,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                ConsiderarAnoAnterior = true
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, false, ATIVIDADE_AVALIATIVA_1);

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoConselhoClasse.EmAndamento
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseParecerId = PARECER_CONCLUSIVO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_8,
                Justificativa = "Justificativa",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var obterPareceresConclusivosUseCase = ServiceProvider.GetService<IAlterarParecerConclusivoUseCase>();
            var dto = new AlterarParecerConclusivoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                ParecerConclusivoId = PARECER_CONCLUSIVO_ID_2
            };

            var retorno = await obterPareceresConclusivosUseCase.Executar(dto);
            retorno.ShouldNotBeNull();
            retorno.EmAprovacao.ShouldBeTrue();
            var wfaprovacoes = ObterTodos<WFAprovacaoParecerConclusivo>();
            wfaprovacoes.ShouldNotBeNull();
            var wf = wfaprovacoes.FirstOrDefault();
            wf.ConselhoClasseParecerId.ShouldBe(PARECER_CONCLUSIVO_ID_2);
            wf.ParecerAlteradoManual.ShouldBeTrue();
        }

        [Fact(DisplayName = "Conselho Classe - Limpar parecer conclusivo do aluno ativo")]
        public async Task Ao_limpar_parecer_conclusivo_aluno_ativo()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = null,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_4,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = false,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, false, ATIVIDADE_AVALIATIVA_1);

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoConselhoClasse.EmAndamento
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseParecerId = PARECER_CONCLUSIVO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_8,
                Justificativa = "Justificativa",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var obterPareceresConclusivosUseCase = ServiceProvider.GetService<IAlterarParecerConclusivoUseCase>();
            var dto = new AlterarParecerConclusivoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                ParecerConclusivoId = null
            };

            var retorno = await Assert.ThrowsAsync<NegocioException>(async () => await obterPareceresConclusivosUseCase.Executar(dto));
            retorno.ShouldNotBeNull();
            retorno.Message.ShouldBe(MensagemNegocioConselhoClasse.LIMPAR_PARECER_CONCLUSIVO_APENAS_ALUNO_INATIVO);
        }

        [Fact(DisplayName = "Conselho Classe - Limpar parecer conclusivo do aluno para ano letivo atual")]
        public async Task Ao_limpar_parecer_conclusivo_ano_letivo_atual()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = null,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_4,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = false,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, false, ATIVIDADE_AVALIATIVA_1);

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoConselhoClasse.EmAndamento
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                AlunoCodigo = ALUNO_CODIGO_14,
                ConselhoClasseParecerId = PARECER_CONCLUSIVO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_8,
                Justificativa = "Justificativa",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var obterPareceresConclusivosUseCase = ServiceProvider.GetService<IAlterarParecerConclusivoUseCase>();
            var dto = new AlterarParecerConclusivoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_14,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                ParecerConclusivoId = null
            };

            var retorno = await obterPareceresConclusivosUseCase.Executar(dto);
            retorno.ShouldNotBeNull();
            retorno.EmAprovacao.ShouldBeFalse();
            var conselhoAlunos = ObterTodos<ConselhoClasseAluno>();
            conselhoAlunos.ShouldNotBeNull();
            var conselho = conselhoAlunos.FirstOrDefault();
            conselho.ConselhoClasseParecerId.ShouldBeNull();
            conselho.ParecerAlteradoManual.ShouldBeTrue();
        }


        [Fact(DisplayName = "Conselho Classe - Limpar parecer conclusivo do aluno para ano letivo anterior")]
        public async Task Ao_limpar_parecer_conclusivo_ano_letivo_anterior()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia().AddYears(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = null,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_4,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = false,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                ConsiderarAnoAnterior = true
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, false, ATIVIDADE_AVALIATIVA_1);

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoConselhoClasse.EmAndamento
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                AlunoCodigo = ALUNO_CODIGO_14,
                ConselhoClasseParecerId = PARECER_CONCLUSIVO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_8,
                Justificativa = "Justificativa",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var obterPareceresConclusivosUseCase = ServiceProvider.GetService<IAlterarParecerConclusivoUseCase>();
            var dto = new AlterarParecerConclusivoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_14,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                ParecerConclusivoId = null
            };

            var retorno = await obterPareceresConclusivosUseCase.Executar(dto);
            retorno.ShouldNotBeNull();
            retorno.EmAprovacao.ShouldBeTrue();
            var wfaprovacoes = ObterTodos<WFAprovacaoParecerConclusivo>();
            wfaprovacoes.ShouldNotBeNull();
            var wf = wfaprovacoes.FirstOrDefault();
            wf.ConselhoClasseParecerId.ShouldBeNull();
            wf.ParecerAlteradoManual.ShouldBeTrue();
        }

        [Fact(DisplayName = "Conselho Classe - Aprovar wf de alteração do parecer conclusivo")]
        public async Task Ao_aprovar_wf_alteracao_parecer_conclusivo()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia().AddYears(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = null,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_4,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = false,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                ConsiderarAnoAnterior = true
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, false, ATIVIDADE_AVALIATIVA_1);

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoConselhoClasse.EmAndamento
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                AlunoCodigo = ALUNO_CODIGO_14,
                ConselhoClasseParecerId = PARECER_CONCLUSIVO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_8,
                Justificativa = "Justificativa",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });


            await InserirNaBase(new WorkflowAprovacao()
            {
                DreId = DRE_ID_1.ToString(),
                UeId = UE_ID_1.ToString(),
                TurmaId = TURMA_ID_1.ToString(),
                NotifacaoMensagem = "Mensagem",
                NotifacaoTitulo = "Titulo",
                NotificacaoTipo = NotificacaoTipo.Fechamento,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                Tipo = WorkflowAprovacaoTipo.AlteracaoParecerConclusivo,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new WFAprovacaoParecerConclusivo()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ConselhoClasseParecerAnteriorId = PARECER_CONCLUSIVO_ID_1,
                ConselhoClasseParecerId = PARECER_CONCLUSIVO_ID_2,
                ParecerAlteradoManual = true,
                UsuarioSolicitanteId = USUARIO_ID_1,
                WfAprovacaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await ServiceProvider.GetService<IMediator>().Send(new AprovarWorkflowAlteracaoParecerConclusivoCommand(1, TURMA_CODIGO_1, USUARIO_PROFESSOR_CODIGO_RF_2222222, USUARIO_PROFESSOR_NOME_2222222));

            var conselhoAlunos = ObterTodos<ConselhoClasseAluno>();
            conselhoAlunos.ShouldNotBeNull();
            var conselho = conselhoAlunos.FirstOrDefault();
            conselho.ConselhoClasseParecerId.ShouldBe(PARECER_CONCLUSIVO_ID_2);
            conselho.ParecerAlteradoManual.ShouldBeTrue();

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count().ShouldBe(1);
        }

        [Fact(DisplayName = "Conselho Classe - Aprovar wf de limpar do parecer conclusivo")]
        public async Task Ao_aprovar_wf_limpar_parecer_conclusivo()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia().AddYears(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = null,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_4,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = false,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                ConsiderarAnoAnterior = true
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, false, ATIVIDADE_AVALIATIVA_1);

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoConselhoClasse.EmAndamento
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                AlunoCodigo = ALUNO_CODIGO_14,
                ConselhoClasseParecerId = PARECER_CONCLUSIVO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_8,
                Justificativa = "Justificativa",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });


            await InserirNaBase(new WorkflowAprovacao()
            {
                DreId = DRE_ID_1.ToString(),
                UeId = UE_ID_1.ToString(),
                TurmaId = TURMA_ID_1.ToString(),
                NotifacaoMensagem = "Mensagem",
                NotifacaoTitulo = "Titulo",
                NotificacaoTipo = NotificacaoTipo.Fechamento,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                Tipo = WorkflowAprovacaoTipo.AlteracaoParecerConclusivo,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new WFAprovacaoParecerConclusivo()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ConselhoClasseParecerAnteriorId = PARECER_CONCLUSIVO_ID_1,
                ConselhoClasseParecerId = null,
                ParecerAlteradoManual = true,
                UsuarioSolicitanteId = USUARIO_ID_1,
                WfAprovacaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await ServiceProvider.GetService<IMediator>().Send(new AprovarWorkflowAlteracaoParecerConclusivoCommand(1, TURMA_CODIGO_1, USUARIO_PROFESSOR_CODIGO_RF_2222222, USUARIO_PROFESSOR_NOME_2222222));

            var conselhoAlunos = ObterTodos<ConselhoClasseAluno>();
            conselhoAlunos.ShouldNotBeNull();
            var conselho = conselhoAlunos.FirstOrDefault();
            conselho.ConselhoClasseParecerId.ShouldBeNull();
            conselho.ParecerAlteradoManual.ShouldBeTrue();

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count().ShouldBe(1);
            var notificacao = notificacoes.First();
            notificacao.Mensagem.Contains("Sem parecer");
        }
    }
}
