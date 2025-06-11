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
        public async Task Executar_DeveLancarExcecao_QuandoNotificacaoNaoEncontrada()
        {
            var id = 123;
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotificacaoPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((SME.SGP.Dominio.Notificacao)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(id));
            Assert.Equal($"Notificação de Id: '{id}' não localizada.", ex.Message);
        }

        [Fact]
        public async Task Executar_DeveChamarSalvarELeitura_QuandoNaoLidaEMarcarComoLidaAoObterDetalhe()
        {
            var id = 1;
            var notificacao = new SME.SGP.Dominio.Notificacao
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
            var notificacao = new SME.SGP.Dominio.Notificacao
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
            var notificacao = new SME.SGP.Dominio.Notificacao
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
    }
}
