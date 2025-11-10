using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoReuniaoPedagogicaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly NotificacaoReuniaoPedagogicaUseCase useCase;

        public NotificacaoReuniaoPedagogicaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new NotificacaoReuniaoPedagogicaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Nula_Deve_Teste_Teste()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Valor = "2" },
                new ParametrosSistema { Valor = "5" }
            };

            var eventos1 = new List<SME.SGP.Dominio.Evento>
            {
                new SME.SGP.Dominio.Evento { Id = 1, Nome = "Evento 1" },
                new SME.SGP.Dominio.Evento { Id = 2, Nome = "Evento 2" }
            };

            var eventos2 = new List<SME.SGP.Dominio.Evento>
            {
                new SME.SGP.Dominio.Evento { Id = 3, Nome = "Evento 3" }
            };

            mediator.Setup(x => x.Send(It.Is<ObterParametrosSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.DiasNotificacaoReuniaoPedagogica &&
                q.Ano == DateTime.Now.Year), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);

            mediator.SetupSequence(x => x.Send(It.IsAny<ObterEventoPorTipoEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventos1)
                .ReturnsAsync(eventos2);

            mediator.Setup(x => x.Send(It.IsAny<NotificarEventoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);

            mediator.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterEventoPorTipoEDataQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediator.Verify(x => x.Send(It.IsAny<NotificarEventoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Executar_Quando_Nao_Houver_Eventos_Deve_Teste_Teste()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Valor = "3" }
            };

            var eventos = new List<SME.SGP.Dominio.Evento>();

            mediator.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);

            mediator.Setup(x => x.Send(It.IsAny<ObterEventoPorTipoEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventos);

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);

            mediator.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterEventoPorTipoEDataQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<NotificarEventoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
