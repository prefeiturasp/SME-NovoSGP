using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_gerar_notificacao_nota_fechamento : NotaFechamentoBimestreTesteBase
    {
        public Ao_gerar_notificacao_nota_fechamento(CollectionFixture collectionFixture) : base(collectionFixture) {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosEAnoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosEAnoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Fechamento Nota - Deve gerar a notificação de forma dinâmica")]
        public async Task Deve_gerar_notificacao_de_forma_dinamica()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota();

            await CriarDadosBase(filtroFechamentoNota);

            await CriarFechamentoTurma();
            
            await CriarFechamentoTurmaDisciplina(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CriarFechamentoAlunoNota(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CriarNotificacao();

            await InserirNaBase(new WorkflowAprovacao()
            {
                UeId = "1",
                DreId = "1",
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                NotifacaoMensagem = "<p>A(s) nota(s) do Xº bimestre de ANO da turma X da ESCOLA foram alteradas <mensagemDinamicaTabelaPorAluno>",
                NotifacaoTitulo = "Alteração em nota(s) final - ESCOLA - TURMA (ano anterior)",
                NotificacaoTipo = NotificacaoTipo.Notas,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF,
                Tipo = WorkflowAprovacaoTipo.AlteracaoNotaFechamento
            });
            
            await InserirNaBase(new WorkflowAprovacaoNivel()
            {
               Status = WorkflowAprovacaoNivelStatus.SemStatus,
               Cargo = Cargo.CP,
               Nivel = 1,
               CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF,
               WorkflowId = 1
            });
            
            await InserirNaBase(new WorkflowAprovacaoNivelNotificacao()
            {
                WorkflowAprovacaoNivelId = 1,
                NotificacaoId = 1
            });
                
            await InserirNaBase(new WfAprovacaoNotaFechamento()
            {
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                WfAprovacaoId = 1,
            });

            await InserirNaBase(new WfAprovacaoNotaFechamento()
            {
                FechamentoNotaId = 2,
                Nota = NOTA_8,
                CriadoEm = System.DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                WfAprovacaoId = 1
            });

            var useCase = ServiceProvider.GetService<IObterNotificacaoPorIdUseCase>();
            var notificaoDetalhe = await useCase.Executar(1);
            notificaoDetalhe.ShouldNotBeNull();
            notificaoDetalhe.Mensagem.Contains("<mensagemDinamicaTabelaPorAluno>").ShouldBeFalse();
        }

        private async Task CriarNotificacao()
        {
            await InserirNaBase(new Notificacao()
            {
                Categoria = NotificacaoCategoria.Workflow_Aprovacao,
                Codigo = 1,
                DreId = "1",
                Mensagem = "<p>A(s) nota(s) do Xº bimestre de ANO da turma X da ESCOLA foram alteradas <mensagemDinamicaTabelaPorAluno>",
                Status = NotificacaoStatus.Pendente,
                Tipo = NotificacaoTipo.Notas,
                Titulo = "Alteração em nota(s) final - ESCOLA - TURMA (ano anterior)",
                TurmaId = "1",
                UeId = "1",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = 1
            });
        }

        private async Task<FiltroFechamentoNotaDto> ObterFiltroFechamentoNota()
        {
            return await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
        }

        private async Task CriarFechamentoAlunoNota(long disciplinaId)
        {
            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = ALUNO_CODIGO_2222222,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = disciplinaId,
                Nota = NOTA_9,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                FechamentoAlunoId = 1
            });
            
            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = ALUNO_CODIGO_3333333,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = disciplinaId,
                Nota = NOTA_10,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                FechamentoAlunoId = 2
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = ALUNO_CODIGO_4444444,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = disciplinaId,
                Nota = NOTA_9,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                FechamentoAlunoId = 3
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = ALUNO_CODIGO_1111111,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = disciplinaId,
                Nota = NOTA_7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                FechamentoAlunoId = 2
            });
        }

        private async Task CriarFechamentoTurmaDisciplina(long disciplinaId)
        {
            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = disciplinaId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                FechamentoTurmaId = 1
            });
        }

        private async Task CriarFechamentoTurma()
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = 1,
                PeriodoEscolarId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private static async Task<FiltroFechamentoNotaDto> ObterFiltroFechamentoNota(string perfil, 
            ModalidadeTipoCalendario tipoCalendario, bool considerarAnoAnterior, Modalidade modalidade, string anoTurma, 
            TipoFrequenciaAluno tipoFrequenciaAluno, string componenteCurricular)
        {
            return await Task.FromResult(new FiltroFechamentoNotaDto
            {
                Perfil = perfil,
                TipoCalendario = tipoCalendario,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                Modalidade = modalidade,
                AnoTurma = anoTurma,
                TipoFrequenciaAluno = tipoFrequenciaAluno,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                ComponenteCurricular = componenteCurricular
            });
        }
    }
}
