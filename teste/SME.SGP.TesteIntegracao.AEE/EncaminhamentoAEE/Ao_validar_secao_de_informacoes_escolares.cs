using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.EncaminhamentoAee.ServicosFake;
using SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_validar_secao_de_informacoes_escolares : EncaminhamentoAEETesteBase
    {
        public Ao_validar_secao_de_informacoes_escolares(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmasAlunoPorFiltroPlanoAEEQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterNecessidadesEspeciaisAlunoEolQuery, InformacoesEscolaresAlunoDto>), typeof(ObterNecessidadesEspeciaisAlunoEolQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEAnoQuery, AlunoReduzidoDto>), typeof(ObterAlunoPorCodigoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<EhGestorDaEscolaQuery, bool>), typeof(EhGestorDaEscolaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesDaTurmaCompletosQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>), typeof(ObterFuncionariosDreOuUePorPerfisQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Valide_informacoes_do_object_card()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.Aluno.Nome.ShouldBe("NOME ALUNO 1");
            informacoes.Aluno.DataNascimento.Date.ShouldBe(new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-10).Year, 1, 1).Date);
            informacoes.Aluno.CodigoAluno.ShouldBe("1");
            informacoes.Aluno.Situacao.ShouldBe("RECLASSIFICADO SAIDA");
            informacoes.Aluno.DataSituacao.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(-10).Date);
            informacoes.Aluno.CodigoTurma.ShouldBe("1");
            informacoes.responsavelEncaminhamentoAEE.Id.ShouldBe(2);
        }

        [Fact]
        public async Task Valide_informacoes_da_secao_informacoes_escolares()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                CodigoAluno = CODIGO_ALUNO_1,
                TurmaId = TURMA_CODIGO_1,
                TotalAulas = 30,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Bimestre = BIMESTRE_1,
                TotalPresencas = 28,
                TotalAusencias = 1,
                TotalCompensacoes = 1,
                Tipo = TipoFrequenciaAluno.Geral,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseInformacoesEscolares();
            var informacoes = await useCase.Executar(ALUNO_CODIGO_1, TURMA_CODIGO_1);
            
            informacoes.ShouldNotBeNull();
            informacoes.CodigoAluno.ShouldBe(CODIGO_ALUNO_1);
            informacoes.DescricaoNecessidadeEspecial.ShouldBe("Cego");
            informacoes.FrequenciaGlobal.ShouldBe("100,00");
            informacoes.FrequenciaAlunoPorBimestres.ShouldNotBeNull();
            var frenquenciaBimestre = informacoes.FrequenciaAlunoPorBimestres.FirstOrDefault();
            frenquenciaBimestre.Frequencia.ShouldBe(100);
            frenquenciaBimestre.QuantidadeCompensacoes.ShouldBe(1);
            frenquenciaBimestre.QuantidadeCompensacoes.ShouldBe(1);
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento devolvido quando perfil professor")]
        public async Task Deve_permitir_alterar_encaminhamento_devolvido_quando_perfil_professor()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Devolvido,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento rascunho quando perfil professor")]
        public async Task Deve_permitir_alterar_encaminhamento_rascunho_quando_perfil_professor()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Não deve permitir alteração de encaminhamento diferente de rascunho ou devolvido quando perfil professor")]
        public async Task Nao_deve_permitir_alterar_encaminhamento_diferente_rascunho_ou_devolvido_quando_perfil_professor()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento devolvido quando perfil professor infantil")]
        public async Task Deve_permitir_alterar_encaminhamento_devolvido_quando_perfil_professor_infantil()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Devolvido,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento rascunho quando perfil professor infantil")]
        public async Task Deve_permitir_alterar_encaminhamento_rascunho_quando_perfil_professor_infantil()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Não deve permitir alteração de encaminhamento diferente de rascunho ou devolvido quando perfil professor infantil")]
        public async Task Nao_deve_permitir_alterar_encaminhamento_diferente_rascunho_ou_devolvido_quando_perfil_professor_infantil()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento aguardando parecer da coordenação quando perfil gestor escolar")]
        public async Task Deve_permitir_alterar_encaminhamento_aguardando_parecer_coordenacao_quando_perfil_gestor_escolar()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento devolvido quando perfil gestor escolar")]
        public async Task Deve_permitir_alterar_encaminhamento_devolvido_quando_perfil_gestor_escolar()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
        
            await CriarDadosBase(filtroAee);
        
            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Devolvido,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        
            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Não deve permitir alteração de encaminhamento diferente de encaminhado ou devolvido quando perfil gestor escolar")]
        public async Task Nao_deve_permitir_alterar_encaminhamento_diferente_encaminhado_ou_devolvido_quando_perfil_gestor_escolar()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
        
            await CriarDadosBase(filtroAee);
        
            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        
            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento aguardando atribuição de PAAI quando perfil coordenador CEFAI")]
        public async Task Deve_permitir_alterar_encaminhamento_aguardando_atribuicao_paai_quando_perfil_cefai()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCEFAI(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.AtribuicaoPAAI,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Não deve permitir alteração de encaminhamento diferente de aguardando atribuição de PAAI quando perfil coordenador CEFAI")]
        public async Task Nao_deve_permitir_alterar_encaminhamento_diferente_aguardando_atribuicao_de_paai_quando_perfil_gestor_escolar()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCEFAI(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
        
            await CriarDadosBase(filtroAee);
        
            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        
            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento aguardando análise do AEE quando perfil PAEE")]
        public async Task Deve_permitir_alterar_encaminhamento_aguardando_analise_do_aee_quando_perfil_paee()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilPaee(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Não deve permitir alteração de encaminhamento diferente de aguardando análise do AEE quando perfil PAEE")]
        public async Task Nao_deve_permitir_alterar_encaminhamento_diferente_aguardando_analise_paee_quando_perfil_paee()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilPaee(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
        
            await CriarDadosBase(filtroAee);
        
            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Devolvido,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        
            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Deve permitir alteração de encaminhamento aguardando análise do AEE quando perfil PAAI")]
        public async Task Deve_permitir_alterar_encaminhamento_aguardando_analise_do_aee_quando_perfil_paai()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilPaai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - Não deve permitir alteração de encaminhamento diferente de aguardando análise do AEE quando perfil PAAI")]
        public async Task Nao_deve_permitir_alterar_encaminhamento_diferente_aguardando_analise_paee_quando_perfil_paai()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilPaai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
        
            await CriarDadosBase(filtroAee);
        
            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Devolvido,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        
            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.PodeEditar.ShouldBeFalse();
        }
    }
}
