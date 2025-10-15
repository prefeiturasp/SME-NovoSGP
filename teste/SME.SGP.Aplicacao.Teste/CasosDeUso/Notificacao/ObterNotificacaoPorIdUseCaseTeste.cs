using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterNotificacaoPorIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterNotificacaoPorIdUseCase useCase;

        public ObterNotificacaoPorIdUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterNotificacaoPorIdUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_DeveLancarExcecao_QuandoMediatorNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterNotificacaoPorIdUseCase(null));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoNotificacaoNaoEncontrada()
        {
            var id = 123;
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Dominio.Notificacao)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(id));
            Assert.Equal($"Notificação de Id: '{id}' não localizada.", ex.Message);
        }

        [Fact]
        public async Task Executar_DeveChamarSalvarELeitura_QuandoNaoLidaEMarcarComoLidaAoObterDetalhe()
        {
            var id = 1;
            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Teste",
                Mensagem = "Mensagem de teste",
                Status = NotificacaoStatus.Pendente,
                Tipo = NotificacaoTipo.Planejamento,
                Categoria = NotificacaoCategoria.Aviso,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                WorkflowAprovacaoNivel = null
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarNotificacaoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(123);

            mediatorMock.Setup(m => m.Send(It.IsAny<NotificarLeituraNotificacaoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(Unit.Value);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(string.Empty);

            var resultado = await useCase.Executar(id);

            notificacao.Equals(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<NotificarLeituraNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemPadrao_QuandoRelatorioNaoExiste()
        {
            var id = 789;
            var guid = Guid.NewGuid();
            var notificacao = new Dominio.Notificacao
            {
                Id = id,
                CriadoEm = DateTime.Now,
                AlteradoEm = DateTime.Now,
                Mensagem = $"Arquivo: {guid}",
                Tipo = NotificacaoTipo.Relatorio,
                Categoria = NotificacaoCategoria.Informe,
                Titulo = "Relatório",
                CriadoPor = "Autor",
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoRelatorioPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((int)TipoRelatorio.AEAdesao); // Tipo diferente de Itinerancias
            mediatorMock.Setup(m => m.Send(It.IsAny<VerificarExistenciaRelatorioPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false); // Arquivo não existe
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(string.Empty);

            var resultado = await useCase.Executar(id);

            Assert.Equal("O arquivo não está mais disponível, solicite a geração do relatório novamente.", resultado.Mensagem);
        }

        [Fact(DisplayName = "Deve retornar detalhes da notificação quando encontrada")]
        public async Task DeveRetornarDetalhesQuandoNotificacaoEncontrada()
        {
            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Teste",
                Mensagem = "Mensagem de teste",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Aviso,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                WorkflowAprovacaoNivel = null,
                Tipo = NotificacaoTipo.Planejamento,
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(notificacao);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTipoRelatorioPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int)TipoRelatorio.Itinerancias);


            var resultado = await useCase.Executar(notificacao.Id);
            notificacao.Equals(resultado);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagem_QuandoCategoriaAlerta()
        {
            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Alerta",
                Mensagem = "Mensagem de alerta",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Alerta,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Planejamento,
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal("Mensagem de alerta", resultado.Mensagem);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemInformativo_QuandoCategoriaInforme()
        {
            var mensagemAnexos = " - Anexos informativos";
            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Informe",
                Mensagem = "Mensagem informativa",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Informe,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Planejamento,
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(mensagemAnexos);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal("Mensagem informativa - Anexos informativos", resultado.Mensagem);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemWorkflowAprovacao_QuandoCategoriaWorkflow()
        {
            var workflowAprovacao = new SME.SGP.Dominio.WorkflowAprovacao
            {
                Id = 1,
                Tipo = WorkflowAprovacaoTipo.Basica
            };

            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Workflow",
                Mensagem = "Mensagem workflow",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Workflow_Aprovacao,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Planejamento,
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterWorkflowAprovacaoPorNotificacaoIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(workflowAprovacao);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal("Mensagem workflow", resultado.Mensagem);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemWorkflowParecerConclusivo_QuandoTipoAlteracaoParecerConclusivo()
        {
            var mensagemEspecifica = "Mensagem de alteração de parecer conclusivo";
            var workflowAprovacao = new SME.SGP.Dominio.WorkflowAprovacao
            {
                Id = 1,
                Tipo = WorkflowAprovacaoTipo.AlteracaoParecerConclusivo
            };

            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Workflow Parecer",
                Mensagem = "Mensagem workflow",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Workflow_Aprovacao,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Planejamento,
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterWorkflowAprovacaoPorNotificacaoIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(workflowAprovacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMensagemNotificacaoAlteracaoParecerConclusivoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(mensagemEspecifica);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal(mensagemEspecifica, resultado.Mensagem);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemWorkflowNotaFechamento_QuandoTipoAlteracaoNotaFechamento()
        {
            var mensagemEspecifica = "Mensagem de alteração de nota de fechamento";
            var workflowAprovacao = new SME.SGP.Dominio.WorkflowAprovacao
            {
                Id = 1,
                Tipo = WorkflowAprovacaoTipo.AlteracaoNotaFechamento
            };

            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Workflow Nota",
                Mensagem = "Mensagem workflow",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Workflow_Aprovacao,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Planejamento,
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterWorkflowAprovacaoPorNotificacaoIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(workflowAprovacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMensagemNotificacaoAlteracaoNotaFechamentoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(mensagemEspecifica);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal(mensagemEspecifica, resultado.Mensagem);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemWorkflowNotaConselho_QuandoTipoAlteracaoNotaConselho()
        {
            var mensagemEspecifica = "Mensagem de alteração de nota pós conselho";
            var workflowAprovacao = new SME.SGP.Dominio.WorkflowAprovacao
            {
                Id = 1,
                Tipo = WorkflowAprovacaoTipo.AlteracaoNotaConselho
            };

            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Workflow Conselho",
                Mensagem = "Mensagem workflow",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Workflow_Aprovacao,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Planejamento,
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterWorkflowAprovacaoPorNotificacaoIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(workflowAprovacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMensagemNotificacaoAlteracaoNotaPosConselhoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(mensagemEspecifica);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal(mensagemEspecifica, resultado.Mensagem);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemOriginal_QuandoTipoRelatorioItinerancias()
        {
            var guid = Guid.NewGuid();
            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Relatório Itinerâncias",
                Mensagem = $"Arquivo: {guid}",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Informe,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Relatorio,
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoRelatorioPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((int)TipoRelatorio.Itinerancias);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(string.Empty);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal($"Arquivo: {guid}", resultado.Mensagem);
        }

        [Fact]
        public async Task Executar_DeveRetornarObservacaoWorkflow_QuandoWorkflowAprovacaoNivelExiste()
        {
            var observacao = "Observação do workflow";
            var workflowAprovacaoNivel = new WorkflowAprovacaoNivel
            {
                Observacao = observacao
            };

            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Teste",
                Mensagem = "Mensagem de teste",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Aviso,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Planejamento,
                WorkflowAprovacaoNivel = workflowAprovacaoNivel
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal(observacao, resultado.Observacao);
        }

        [Fact]
        public async Task Executar_DeveRetornarObservacaoVazia_QuandoWorkflowAprovacaoNivelNulo()
        {
            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Teste",
                Mensagem = "Mensagem de teste",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Aviso,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Planejamento,
                WorkflowAprovacaoNivel = null
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal(string.Empty, resultado.Observacao);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemOriginal_QuandoTipoRelatorioSemCodigo()
        {
            var notificacao = new Dominio.Notificacao
            {
                Id = 1,
                Titulo = "Relatório sem GUID",
                Mensagem = "Mensagem sem GUID válido",
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Informe,
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                Codigo = 123,
                Tipo = NotificacaoTipo.Relatorio,
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(notificacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(string.Empty);

            var resultado = await useCase.Executar(notificacao.Id);

            Assert.Equal("Mensagem sem GUID válido", resultado.Mensagem);
        }
    }
}