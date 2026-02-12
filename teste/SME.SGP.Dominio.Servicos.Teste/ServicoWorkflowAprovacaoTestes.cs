using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoWorkflowAprovacaoTestes
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioWorkflowAprovacao> _repositorioWorkflowAprovacaoMock;
        private readonly Mock<IRepositorioWorkflowAprovacaoNivel> _workflowAprovacaoNivelMock;
        private readonly Mock<IRepositorioNotificacao> _repositorioNotificacaoMock;
        private readonly Mock<IRepositorioAulaConsulta> _repositorioAulaMock;
        private readonly Mock<IRepositorioTurmaConsulta> _repositorioTurmaMock;
        private readonly Mock<IServicoUsuario> _servicoUsuarioMock;
        private readonly Mock<IRepositorioPendencia> _repositorioPendenciaMock;
        private readonly Mock<IRepositorioFechamentoNota> _repositorioFechamentoNotaMock;
        private readonly Mock<IRepositorioFechamentoReabertura> _repositorioFechamentoReaberturaMock;
        private readonly Mock<IServicoNotificacao> _servicoNotificacaoMock;
        private readonly Mock<IRepositorioUeConsulta> _repositorioUeMock;

        private readonly ServicoWorkflowAprovacao _servico;
        private readonly Faker _faker;

        public ServicoWorkflowAprovacaoTestes()
        {
            // Inicialização dos Mocks
            _mediatorMock = new Mock<IMediator>();
            _repositorioWorkflowAprovacaoMock = new Mock<IRepositorioWorkflowAprovacao>();
            _workflowAprovacaoNivelMock = new Mock<IRepositorioWorkflowAprovacaoNivel>();
            _repositorioNotificacaoMock = new Mock<IRepositorioNotificacao>();
            _repositorioAulaMock = new Mock<IRepositorioAulaConsulta>();
            _repositorioTurmaMock = new Mock<IRepositorioTurmaConsulta>();
            _servicoUsuarioMock = new Mock<IServicoUsuario>();
            _repositorioPendenciaMock = new Mock<IRepositorioPendencia>();
            _repositorioFechamentoNotaMock = new Mock<IRepositorioFechamentoNota>();
            _repositorioFechamentoReaberturaMock = new Mock<IRepositorioFechamentoReabertura>();
            _servicoNotificacaoMock = new Mock<IServicoNotificacao>();
            _repositorioUeMock = new Mock<IRepositorioUeConsulta>();
            var configurationMock = new Mock<IConfiguration>();

            _faker = new Faker("pt_BR");

            _servico = new ServicoWorkflowAprovacao(
                _repositorioNotificacaoMock.Object,
                Mock.Of<IRepositorioWorkflowAprovacaoNivelNotificacao>(),
                _servicoUsuarioMock.Object,
                _servicoNotificacaoMock.Object,
                _workflowAprovacaoNivelMock.Object,
                Mock.Of<IRepositorioWorkflowAprovacaoNivelUsuario>(),
                Mock.Of<IRepositorioEvento>(),
                Mock.Of<IConfiguration>(),
                _repositorioAulaMock.Object,
                _repositorioTurmaMock.Object,
                _repositorioUeMock.Object,
                _repositorioWorkflowAprovacaoMock.Object,
                _repositorioFechamentoReaberturaMock.Object,
                _repositorioFechamentoNotaMock.Object,
                _repositorioPendenciaMock.Object,
                _mediatorMock.Object
            );
        }

        [Fact]
        public async Task DadoAprovacaoDeNivelFinalParaReposicaoAula_QuandoAprovar_EntaoDeveAprovarAulaENotificarCriador()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, notificacaoId, 1);
            var aula = new Aula { Id = _faker.Random.Long(1), Status = EntidadeStatus.AguardandoAprovacao, CriadoRF = "123456" };
            var turma = new Turma { CodigoTurma = "T1", Ue = new Ue { Dre = new Dre() } };
            var usuario = new Usuario { CodigoRf = "123456" };

            _repositorioAulaMock.Setup(r => r.ObterPorWorkflowId(workflow.Id)).ReturnsAsync(aula);
            _repositorioTurmaMock.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>())).ReturnsAsync(turma);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(usuario);

            // Act
            await _servico.Aprovar(workflow, true, "Aprovado", notificacaoId);

            // Assert
            aula.Status.Should().Be(EntidadeStatus.Aprovado);
            _repositorioAulaMock.Verify(r => r.Salvar(It.Is<Aula>(a => a.Status == EntidadeStatus.Aprovado)), Times.Once);
            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.Aprovado)), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarLeituraNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DadoReprovacaoDeNivelParaReposicaoAula_QuandoAprovarComFlagReprovado_EntaoDeveReprovarAulaENotificarCriador()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var motivoReprovacao = "Dados inconsistentes";
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, notificacaoId, 1);
            var aula = new Aula { Id = _faker.Random.Long(1), Status = EntidadeStatus.AguardandoAprovacao, CriadoRF = "123456" };
            var turma = new Turma { Nome = "Turma Teste", CodigoTurma = "T1", Ue = new Ue { Nome = "UE Teste", Dre = new Dre { Nome = "DRE Teste" } } };
            var usuario = new Usuario { CodigoRf = "123456" };

            _repositorioAulaMock.Setup(r => r.ObterPorWorkflowId(workflow.Id)).ReturnsAsync(aula);
            _repositorioTurmaMock.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>())).ReturnsAsync(turma);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(usuario);

            // Act
            await _servico.Aprovar(workflow, false, motivoReprovacao, notificacaoId);

            // Assert
            aula.Status.Should().Be(EntidadeStatus.Recusado);
            _repositorioAulaMock.Verify(r => r.Salvar(It.Is<Aula>(a => a.Status == EntidadeStatus.Recusado)), Times.Once);
            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.Reprovado)), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<NotificarUsuarioCommand>(c => c.Mensagem.Contains(motivoReprovacao)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DadoAprovacaoDeNivelFinalParaAlteracaoNotaFechamento_QuandoAprovar_EntaoDeveAtualizarNotas()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.AlteracaoNotaFechamento, notificacaoId, 1);
            workflow.TurmaId = "T1";
            var turma = new Turma { CodigoTurma = "T1", Ue = new Ue { CodigoUe = "UE1", TipoEscola = TipoEscola.EMEF, Dre = new Dre { CodigoDre = "DRE1", Abreviacao = "DRE" } } };
            var notasEmAprovacao = CriarNotasEmAprovacao(workflow.Id);
            var notaTipoValor = new NotaTipoValor { TipoNota = TipoNota.Nota };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery>(q => q.WorkflowId == workflow.Id), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(notasEmAprovacao);
            _repositorioTurmaMock.Setup(r => r.ObterTurmaComUeEDrePorCodigo(workflow.TurmaId)).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaTipoValorPorTurmaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(notaTipoValor);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<AlunoPorTurmaResposta>());

            // Act
            await _servico.Aprovar(workflow, true, "OK", notificacaoId);

            // Assert
            _repositorioPendenciaMock.Verify(r => r.AtualizarPendencias(It.IsAny<long>(), SituacaoPendencia.Resolvida, TipoPendencia.AlteracaoNotaFechamento), Times.Once);
            _repositorioFechamentoNotaMock.Verify(r => r.Salvar(It.IsAny<FechamentoNota>()), Times.Exactly(notasEmAprovacao.Count()));
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirWFAprovacaoNotaFechamentoCommand>(c => c.WfAprovacaoNotaFechamento.Id == notasEmAprovacao.First().WfAprovacao.Id), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DadoReprovacaoDeNivelParaRegistroItinerancia_QuandoAprovarComFlagReprovado_EntaoDeveReprovarItinerancia()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var motivoReprovacao = "Motivo teste";
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.RegistroItinerancia, notificacaoId, 1);
            var itineranciaWf = new WfAprovacaoItinerancia { ItineranciaId = 99, WfAprovacaoId = workflow.Id };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterWorkflowAprovacaoItineranciaPorIdQuery>(q => q.WorkflowId == workflow.Id), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(itineranciaWf);

            // Act
            await _servico.Aprovar(workflow, false, motivoReprovacao, notificacaoId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<AprovarItineranciaCommand>(c => c.StatusAprovacao == false && c.ItineranciaId == itineranciaWf.ItineranciaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<NotificacaoRegistroItineranciaRecusadoCommand>(c => c.Observacoes == motivoReprovacao), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DadoUmWorkflowSemNiveisReprovados_QuandoExcluirWorkflowNotificacoes_EntaoDeveMarcarTodosNiveisComoExcluidos()
        {
            // Arrange
            var workflowId = _faker.Random.Long(1);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, 1, 2);
            workflow.Id = workflowId;

            _repositorioWorkflowAprovacaoMock.Setup(r => r.ObterEntidadeCompleta(workflowId, 0)).ReturnsAsync(workflow);

            // Act
            await _servico.ExcluirWorkflowNotificacoes(workflowId);

            // Assert
            _workflowAprovacaoNivelMock.Verify(r => r.Salvar(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.Excluido)), Times.Exactly(2));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once); // Apenas o primeiro nível tem notificação no mock
            _repositorioWorkflowAprovacaoMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacao>(wf => wf.Excluido == true)), Times.Once);
        }

        [Fact]
        public async Task DadoUmWorkflowComNivelReprovado_QuandoExcluirWorkflowNotificacoes_EntaoNaoDeveFazerNada()
        {
            // Arrange
            var workflowId = _faker.Random.Long(1);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, 1, 2);
            workflow.Id = workflowId;
            // Força um nível a estar reprovado
            workflow.Niveis.First().Status = WorkflowAprovacaoNivelStatus.Reprovado;

            _repositorioWorkflowAprovacaoMock.Setup(r => r.ObterEntidadeCompleta(workflowId, 0)).ReturnsAsync(workflow);

            // Act
            await _servico.ExcluirWorkflowNotificacoes(workflowId);

            // Assert
            // Verifica que nenhuma alteração foi persistida
            _workflowAprovacaoNivelMock.Verify(r => r.Salvar(It.IsAny<WorkflowAprovacaoNivel>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _repositorioWorkflowAprovacaoMock.Verify(r => r.SalvarAsync(It.IsAny<WorkflowAprovacao>()), Times.Never);
        }

        [Fact]
        public async Task DadoUmWorkflowDeAprovacao_QuandoExecutarConfiguracaoInicial_EntaoDeveEnviarNotificacaoApenasParaOPrimeiroNivel()
        {
            // Arrange
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, 1, 3); // 3 níveis
            workflow.NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao;

            // Mocks específicos para este cenário
            var servicoNotificacaoMock = new Mock<IServicoNotificacao>();
            var servico = CriarServicoComDependencia(servicoNotificacaoMock.Object);

            var novoCodigoNotificacao = _faker.Random.Long(100);
            servicoNotificacaoMock.Setup(s => s.ObtemNovoCodigoAsync()).ReturnsAsync(novoCodigoNotificacao);

            // Act
            await servico.ConfiguracaoInicial(workflow, 1L);

            // Assert
            // Apenas o nível 1 deve ter uma notificação adicionada
            workflow.Niveis.First(n => n.Nivel == 1).Notificacoes.Should().HaveCount(1);
            workflow.Niveis.First(n => n.Nivel == 2).Notificacoes.Should().BeEmpty();
            workflow.Niveis.First(n => n.Nivel == 3).Notificacoes.Should().BeEmpty();
        }
        [Fact]
        public async Task DadoAprovacaoDeNivelIntermediario_QuandoAprovar_EntaoDeveEnviarNotificacaoParaProximoNivel()
        {
            // Arrange
            var notificacaoIdNivel1 = _faker.Random.Long(1, 10);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, notificacaoIdNivel1, 2); // Workflow com 2 níveis
            workflow.UeId = "UE-TESTE";
            var proximoNivel = workflow.Niveis.First(n => n.Nivel == 2);
            proximoNivel.Cargo = Cargo.Diretor;

            var novoCodigoNotificacao = _faker.Random.Long(11, 20);

            _servicoNotificacaoMock.Setup(s => s.ObtemNovoCodigoAsync()).ReturnsAsync(novoCodigoNotificacao);
            _repositorioUeMock.Setup(r => r.ObterPorCodigo(workflow.UeId)).Returns(new Ue());
            _servicoNotificacaoMock.Setup(s => s.ObterFuncionariosPorNivelAsync(It.IsAny<string>(), It.IsAny<Cargo?>(), true, true))
                                   .ReturnsAsync(new List<(Cargo? cargo, string Id)> { (Cargo.Diretor, "RF-DIRETOR") });
            _servicoUsuarioMock.Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                               .ReturnsAsync(new Usuario { Id = 99, CodigoRf = "RF-DIRETOR" });

            // Act
            await _servico.Aprovar(workflow, true, "Aprovado pelo Coordenador", notificacaoIdNivel1);

            // Assert
            // Verifica se o primeiro nível foi salvo como aprovado
            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacaoNivel>(n => n.Nivel == 1 && n.Status == WorkflowAprovacaoNivelStatus.Aprovado)), Times.Once);
        }

        [Fact]
        public async Task DadoAulaInexistente_QuandoVerificarAulaReposicao_EntaoDeveExcluirWorkflowENotificacaoERetornarMensagem()
        {
            // Arrange
            var workflowId = _faker.Random.Long(1, 10);
            var notificacaoCodigo = _faker.Random.Long(11, 20);
            var workflowParaExcluir = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, notificacaoCodigo, 1);

            _repositorioAulaMock.Setup(r => r.VerificarAulaPorWorkflowId(workflowId)).ReturnsAsync(false);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterNotificacaoPorCodigoQuery>(q => q.Codigo == notificacaoCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Notificacao { Id = 999 });

            _repositorioWorkflowAprovacaoMock.Setup(r => r.ObterEntidadeCompleta(workflowId, 0)).ReturnsAsync(workflowParaExcluir);

            // Act
            var resultado = await _servico.VerificaAulaReposicao(workflowId, notificacaoCodigo);

            // Assert
            resultado.Should().Be("Não existe aula para esse fluxo de aprovação. A notificação foi excluída.");

            // Verifica se a exclusão em cascata foi acionada
            _repositorioWorkflowAprovacaoMock.Verify(r => r.ObterEntidadeCompleta(workflowId, 0), Times.Once);
            _repositorioWorkflowAprovacaoMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacao>(wf => wf.Excluido)), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task DadoAprovacaoDeNivelFinalParaAlteracaoNotaConselho_QuandoAprovar_EntaoDeveEnviarComandoDeAprovacao()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.AlteracaoNotaConselho, notificacaoId, 1);
            workflow.TurmaId = "TURMA-CONSELHO";
            workflow.CriadoRF = "RF-CRIADOR";
            workflow.CriadoPor = "NOME-CRIADOR";

            // Act
            await _servico.Aprovar(workflow, true, "Aprovado", notificacaoId);

            // Assert
            _mediatorMock.Verify(m => m.Send(
                It.IsAny<AprovarWorkflowAlteracaoNotaConselhoCommand>(),
                It.IsAny<CancellationToken>()), Times.Once);

            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.Aprovado)), Times.Once);
        }

        [Fact]
        public async Task DadoReprovacaoDeNivelParaAlteracaoNotaFechamento_QuandoReprovar_EntaoDeveNotificarReprovacaoEExcluirWfNotas()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var motivoReprovacao = "Justificativa insuficiente para alteração.";
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.AlteracaoNotaFechamento, notificacaoId, 1);
            workflow.TurmaId = "T1";
            var notasEmAprovacao = CriarNotasEmAprovacao(workflow.Id);
            var turma = new Turma { CodigoTurma = "T1", Ue = new Ue { CodigoUe = "UE1", TipoEscola = TipoEscola.EMEF, Dre = new Dre { CodigoDre = "DRE1", Abreviacao = "DRE" } } };
            var alunoDto = new AlunoPorTurmaResposta { CodigoAluno = notasEmAprovacao.First().CodigoAluno };
            var usuarioSolicitante = new Usuario { Id = 123, Nome = "Professor Solicitante" };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery>(q => q.WorkflowId == workflow.Id), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(notasEmAprovacao);
            _repositorioTurmaMock.Setup(r => r.ObterTurmaComUeEDrePorCodigo(workflow.TurmaId)).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<AlunoPorTurmaResposta> { alunoDto });
            _mediatorMock.Setup(m => m.Send(It.Is<ObterUsuarioPorCodigoRfLoginQuery>(q => q.Login == notasEmAprovacao.First().UsuarioSolicitanteRf), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuarioSolicitante);


            // Act
            await _servico.Aprovar(workflow, false, motivoReprovacao, notificacaoId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaCodigoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.Reprovado)), Times.Once);
        }


        [Fact]
        public async Task DadoUmWorkflowDeAviso_QuandoExecutarConfiguracaoInicial_EntaoDeveEnviarNotificacaoParaTodosOsNiveis()
        {
            // Arrange
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, 1, 2);
            workflow.NotificacaoCategoria = NotificacaoCategoria.Aviso; // Categoria diferente de aprovação
            workflow.UeId = "UE-TESTE";
            workflow.Niveis.ToList().ForEach(n =>
            {
                n.Cargo = Cargo.AD; // Atribui um cargo para buscar usuários
                n.Adicionar(new Usuario() { CodigoRf = $"RF-{n.Nivel}" }); // Adiciona um usuário pré-existente
            });

            var novoCodigoNotificacao = _faker.Random.Long(100);
            _servicoNotificacaoMock.Setup(s => s.ObtemNovoCodigoAsync()).ReturnsAsync(novoCodigoNotificacao);

            _repositorioUeMock.Setup(r => r.ObterPorCodigo(workflow.UeId)).Returns(new Ue());
            _servicoNotificacaoMock.Setup(s => s.ObterFuncionariosPorNivelAsync(It.IsAny<string>(), It.IsAny<Cargo?>(), true, true))
                                   .ReturnsAsync(new List<(Cargo? cargo, string Id)>()); // Retorna vazio para focar nos usuários já existentes no nível

            // Act
            await _servico.ConfiguracaoInicial(workflow, 1L);

            // Assert
            _repositorioNotificacaoMock.Verify(r => r.SalvarAsync(It.Is<Notificacao>(n => n.Codigo == novoCodigoNotificacao)), Times.Once);

            // Como não é um workflow de aprovação, o status não deve ser alterado
            _workflowAprovacaoNivelMock.Verify(r => r.Salvar(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.AguardandoAprovacao)), Times.Never);
        }

        [Fact]
        public async Task DadoReprovacaoDeNivelParaAlteracaoParecerConclusivo_QuandoReprovar_EntaoDeveEnviarComandoDeReprovacao()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var motivoReprovacao = "O parecer não atende às diretrizes pedagógicas.";
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.AlteracaoParecerConclusivo, notificacaoId, 1);
            workflow.TurmaId = "TURMA-PARECER";
            workflow.CriadoRF = "RF-CRIADOR";
            workflow.CriadoPor = "NOME-CRIADOR";

            // Act
            await _servico.Aprovar(workflow, false, motivoReprovacao, notificacaoId);

            // Assert
            // Verifica se o comando de reprovação foi enviado com os dados corretos
            _mediatorMock.Verify(m => m.Send(
                It.Is<ReprovarWorkflowAlteracaoParecerConclusivoCommand>(cmd =>
                    cmd.WorkflowId == workflow.Id &&
                    cmd.TurmaCodigo == workflow.TurmaId &&
                    cmd.Motivo == motivoReprovacao),
                It.IsAny<CancellationToken>()), Times.Once);

            // Garante que o nível foi persistido como reprovado
            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.Reprovado)), Times.Once);
        }

        [Fact]
        public async Task DadoAprovacaoDeNivelIntermediarioParaUeCIEJA_QuandoAprovar_EntaoDeveBuscarFuncionariosComFuncaoAtividadeCIEJA()
        {
            // Arrange
            var notificacaoIdNivel1 = _faker.Random.Long(1);
            var workflow = CriarWorkflow(
                WorkflowAprovacaoTipo.Evento_Liberacao_Excepcional,
                notificacaoIdNivel1,
                2);

            workflow.UeId = "UE-CIEJA";

            var proximoNivel = workflow.Niveis.First(n => n.Nivel == 2);
            proximoNivel.Cargo = Cargo.Diretor;

            var ueCieja = new Ue { TipoEscola = TipoEscola.CIEJA };
            var diretorCiejaRf = "RF-COORD-GERAL";
            var usuarioDiretorCieja = new Usuario
            {
                Id = 101,
                CodigoRf = diretorCiejaRf
            };

            _repositorioUeMock
                .Setup(r => r.ObterPorCodigo(workflow.UeId))
                .Returns(ueCieja);

            _servicoNotificacaoMock
                .Setup(s => s.ObterFuncionariosPorNivelFuncaoAtividadeAsync(
                    workflow.UeId,
                    FuncaoAtividade.COORDERNADOR_GERAL_CIEJA,
                    true,
                    true))
                .ReturnsAsync(new List<(FuncaoAtividade?, string)>
                {
            (FuncaoAtividade.COORDERNADOR_GERAL_CIEJA, diretorCiejaRf)
                });

            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(usuarioDiretorCieja);

            // Act
            await _servico.Aprovar(workflow, true, "OK", notificacaoIdNivel1);

            // Assert
            _servicoNotificacaoMock.Verify(
                s => s.ObterFuncionariosPorNivelFuncaoAtividadeAsync(
                    workflow.UeId,
                    FuncaoAtividade.COORDERNADOR_GERAL_CIEJA,
                    true,
                    true),
                Times.AtLeastOnce);

            _servicoNotificacaoMock.Verify(
                s => s.ObterFuncionariosPorNivelAsync(
                    It.IsAny<string>(),
                    It.IsAny<Cargo?>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()),
                Times.Never);

            _repositorioNotificacaoMock.Verify(
                r => r.SalvarAsync(
                    It.Is<Notificacao>(n =>
                        n.UsuarioId == usuarioDiretorCieja.Id)),
                Times.Once);
        }



        [Fact]
        public async Task DadoAulaExistente_QuandoVerificarAulaReposicao_EntaoDeveRetornarNuloENaoExecutarAcoesDeExclusao()
        {
            // Arrange
            var workflowId = _faker.Random.Long(1);
            var notificacaoCodigo = _faker.Random.Long(10);

            // Configura o repositório para indicar que a aula existe
            _repositorioAulaMock.Setup(r => r.VerificarAulaPorWorkflowId(workflowId)).ReturnsAsync(true);

            // Act
            var resultado = await _servico.VerificaAulaReposicao(workflowId, notificacaoCodigo);

            // Assert
            resultado.Should().BeNull();

            // Garante que nenhuma operação de exclusão foi chamada
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterNotificacaoPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _repositorioWorkflowAprovacaoMock.Verify(r => r.ObterEntidadeCompleta(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DadoReprovacaoDeNivelParaAlteracaoNotaConselho_QuandoReprovar_EntaoDeveEnviarComandoDeRecusa()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var motivoReprovacao = "A alteração não foi validada pelo conselho de classe.";
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.AlteracaoNotaConselho, notificacaoId, 1);
            workflow.TurmaId = "TURMA-CONSELHO-RECUSA";

            // Act
            await _servico.Aprovar(workflow, false, motivoReprovacao, notificacaoId);

            // Assert
            // Verifica se o comando de recusa foi enviado com os dados corretos
            _mediatorMock.Verify(m => m.Send(
                It.Is<RecusarAprovacaoNotaConselhoCommand>(cmd =>
                    cmd.WorkflowId == workflow.Id),
                It.IsAny<CancellationToken>()), Times.Once);

            // Garante que o nível foi persistido como reprovado
            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.Reprovado)), Times.Once);
        }

        [Fact]
        public async Task DadoTentativaDeReprovacaoSemMotivo_QuandoAprovar_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.ReposicaoAula, notificacaoId, 1);

            // Act
            // A ação de chamar o método com observação vazia/nula na reprovação
            Func<Task> act = async () => await _servico.Aprovar(workflow, false, string.Empty, notificacaoId);

            // Assert
            // Verifica se a exceção de negócio esperada foi lançada
            await act.Should().ThrowAsync<NegocioException>()
                     .WithMessage("Para recusar é obrigatório informar uma observação.");

            // Garante que nenhuma alteração de status foi persistida
            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.IsAny<WorkflowAprovacaoNivel>()), Times.Never);
        }

        [Fact]
        public async Task DadoAprovacaoDeNivelParaUeConveniada_QuandoAprovar_EntaoDeveBuscarFuncionariosComFuncaoExterna()
        {
            // Arrange
            var notificacaoIdNivel1 = _faker.Random.Long(1);

            var workflow = CriarWorkflow(
                WorkflowAprovacaoTipo.Evento_Data_Passada,
                notificacaoIdNivel1,
                2);

            workflow.UeId = "UE-CONVENIADA";

            var proximoNivel = workflow.Niveis.First(n => n.Nivel == 2);
            proximoNivel.Cargo = Cargo.Diretor;

            var ueConveniada = new Ue
            {
                TipoEscola = TipoEscola.CRPCONV
            };

            var diretorConveniadaRf = "RF-DIRETOR-EXT";

            var usuarioDiretorConveniada = new Usuario
            {
                Id = 102,
                CodigoRf = diretorConveniadaRf
            };

            _repositorioUeMock
                .Setup(r => r.ObterPorCodigo(workflow.UeId))
                .Returns(ueConveniada);

            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<ObterFuncionariosPorUeEFuncaoExternaQuery>(q =>
                        q.CodigoUE == workflow.UeId &&
                        q.CodigoFuncaoExterna == (int)FuncaoExterna.Diretor),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FuncionarioDTO>
                {
            new FuncionarioDTO
            {
                CodigoRF = diretorConveniadaRf
            }
                });

            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(usuarioDiretorConveniada);

            // Act
            await _servico.Aprovar(workflow, true, "OK", notificacaoIdNivel1);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(
                    It.Is<ObterFuncionariosPorUeEFuncaoExternaQuery>(q =>
                        q.CodigoUE == workflow.UeId &&
                        q.CodigoFuncaoExterna == (int)FuncaoExterna.Diretor),
                    It.IsAny<CancellationToken>()),
                Times.AtLeastOnce);

            _servicoNotificacaoMock.Verify(
                s => s.ObterFuncionariosPorNivelAsync(
                    It.IsAny<string>(),
                    It.IsAny<Cargo?>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()),
                Times.Never);

            _servicoNotificacaoMock.Verify(
                s => s.ObterFuncionariosPorNivelFuncaoAtividadeAsync(
                    It.IsAny<string>(),
                    It.IsAny<FuncaoAtividade>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()),
                Times.Never);

            _repositorioNotificacaoMock.Verify(
                r => r.SalvarAsync(
                    It.Is<Notificacao>(n =>
                        n.UsuarioId == usuarioDiretorConveniada.Id)),
                Times.Once);
        }


        [Fact]
        public async Task DadoAprovacaoDeNivelFinalParaAlteracaoParecerConclusivo_QuandoAprovar_EntaoDeveEnviarComandoDeAprovacao()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.AlteracaoParecerConclusivo, notificacaoId, 1);
            workflow.TurmaId = "TURMA-PARECER-APROVADO";
            workflow.CriadoRF = "RF-CRIADOR";
            workflow.CriadoPor = "NOME-CRIADOR";

            // Act
            await _servico.Aprovar(workflow, true, "Aprovado", notificacaoId);

            // Assert
            _mediatorMock.Verify(m => m.Send(
                It.Is<AprovarWorkflowAlteracaoParecerConclusivoCommand>(cmd =>
                    cmd.WorkflowId == workflow.Id &&
                    cmd.TurmaCodigo == workflow.TurmaId),
                It.IsAny<CancellationToken>()), Times.Once);

            _workflowAprovacaoNivelMock.Verify(r => r.SalvarAsync(It.Is<WorkflowAprovacaoNivel>(n => n.Status == WorkflowAprovacaoNivelStatus.Aprovado)), Times.Once);
        }


        [Fact]
        public async Task DadoAprovacaoFinalMasEventoNaoEncontrado_QuandoAprovar_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var notificacaoId = _faker.Random.Long(1);
            var workflow = CriarWorkflow(WorkflowAprovacaoTipo.Evento_Data_Passada, notificacaoId, 1);

            var repositorioEventoMock = new Mock<IRepositorioEvento>();
            repositorioEventoMock.Setup(r => r.ObterPorWorkflowId(workflow.Id)).Returns((Evento)null); // Simula não encontrar o evento
            var servico = CriarServicoComDependencia(repositorioEventoMock.Object);

            // Act
            Func<Task> act = async () => await servico.Aprovar(workflow, true, "OK", notificacaoId);

            // Assert
            await act.Should().ThrowAsync<NegocioException>()
                     .WithMessage("Não foi possível localizar o evento deste fluxo de aprovação.");

            repositorioEventoMock.Verify(r => r.SalvarAsync(It.IsAny<Evento>()), Times.Never);
        }

        #region Métodos de Apoio
        private WorkflowAprovacao CriarWorkflow(WorkflowAprovacaoTipo tipo, long notificacaoId, int quantidadeNiveis)
        {
            var workflow = new WorkflowAprovacao
            {
                Id = _faker.Random.Long(1),
                Tipo = tipo
            };

            for (int i = 1; i <= quantidadeNiveis; i++)
            {
                var nivel = new WorkflowAprovacaoNivel
                {
                    Id = _faker.Random.Long(i),
                    Nivel = i,
                    Status = i == 1 ? WorkflowAprovacaoNivelStatus.AguardandoAprovacao : WorkflowAprovacaoNivelStatus.SemStatus
                };

                if (i == 1)
                {
                    var notificacao = new Notificacao { Id = notificacaoId, Codigo = notificacaoId, Usuario = new Usuario() };
                    nivel.Adicionar(notificacao);
                }

                workflow.Adicionar(nivel);
            }

            return workflow;
        }

        private IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> CriarNotasEmAprovacao(long workflowId)
        {
            var codigoAluno = _faker.Random.Number(1000, 9999).ToString();
            return new List<WfAprovacaoNotaFechamentoTurmaDto>
            {
                new WfAprovacaoNotaFechamentoTurmaDto
                {
                    CodigoAluno = codigoAluno,
                    UsuarioSolicitanteRf = "RF-SOLICITANTE",
                    Bimestre = 1,
                    ComponenteCurricularDescricao = "Matemática",
                    WfAprovacao = new WfAprovacaoNotaFechamento { Id = 1, FechamentoNotaId = 10, Nota = 9.5, NotaAnterior = 5.0 },
                    FechamentoNota = new FechamentoNota
                    {
                        Id = 10,
                        Nota = 5.0,
                        FechamentoAluno = new FechamentoAluno
                        {
                            AlunoCodigo = codigoAluno,
                            FechamentoTurmaDisciplina = new FechamentoTurmaDisciplina
                            {
                                FechamentoTurma = new FechamentoTurma { TurmaId = 100, PeriodoEscolarId = 200 }
                            }
                        }
                    }
                }
            };
        }
        private ServicoWorkflowAprovacao CriarServicoComDependencia(object dependencia)
        {
            // Este método permite criar uma nova instância do serviço,
            // substituindo um mock específico para um determinado teste.
            return new ServicoWorkflowAprovacao(
                _repositorioNotificacaoMock.Object,
                Mock.Of<IRepositorioWorkflowAprovacaoNivelNotificacao>(),
                _servicoUsuarioMock.Object,
                dependencia is IServicoNotificacao sn ? sn : Mock.Of<IServicoNotificacao>(),
                _workflowAprovacaoNivelMock.Object,
                Mock.Of<IRepositorioWorkflowAprovacaoNivelUsuario>(),
                dependencia is IRepositorioEvento re ? re : Mock.Of<IRepositorioEvento>(),
                Mock.Of<IConfiguration>(),
                _repositorioAulaMock.Object,
                _repositorioTurmaMock.Object,
                Mock.Of<IRepositorioUeConsulta>(),
                _repositorioWorkflowAprovacaoMock.Object,
                Mock.Of<IRepositorioFechamentoReabertura>(),
                _repositorioFechamentoNotaMock.Object,
                _repositorioPendenciaMock.Object,
                _mediatorMock.Object
            );
        }

        // Método de Apoio para o teste de mensagem de conceito
        private IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> CriarNotasEmAprovacaoComConceito(long workflowId)
        {
            return new List<WfAprovacaoNotaFechamentoTurmaDto>
            {
                new WfAprovacaoNotaFechamentoTurmaDto
                {
                    Bimestre = 2,
                    WfAprovacao = new WfAprovacaoNotaFechamento {
                        ConceitoId = (long)ConceitoValores.NS,
                        ConceitoIdAnterior = (long)ConceitoValores.S,
                        CriadoEm = DateTime.Now
                    },
                    FechamentoNota = new FechamentoNota { FechamentoAluno = new FechamentoAluno { FechamentoTurmaDisciplina = new FechamentoTurmaDisciplina { FechamentoTurma = new FechamentoTurma() } } }
                }
            };
        }

        // Método de Apoio para injetar dependências específicas
        private ServicoWorkflowAprovacao CriarServicoComDependencia(IRepositorioEvento repositorioEvento = null, IConfiguration configuration = null)
        {
            return new ServicoWorkflowAprovacao(
                _repositorioNotificacaoMock.Object,
                Mock.Of<IRepositorioWorkflowAprovacaoNivelNotificacao>(),
                _servicoUsuarioMock.Object,
                _servicoNotificacaoMock.Object,
                _workflowAprovacaoNivelMock.Object,
                Mock.Of<IRepositorioWorkflowAprovacaoNivelUsuario>(),
                repositorioEvento ?? Mock.Of<IRepositorioEvento>(),
                configuration ?? Mock.Of<IConfiguration>(),
                _repositorioAulaMock.Object,
                _repositorioTurmaMock.Object,
                _repositorioUeMock.Object,
                _repositorioWorkflowAprovacaoMock.Object,
                _repositorioFechamentoReaberturaMock.Object,
                _repositorioFechamentoNotaMock.Object,
                _repositorioPendenciaMock.Object,
                _mediatorMock.Object
            );
        }
        #endregion
    }
}