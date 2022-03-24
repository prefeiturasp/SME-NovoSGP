using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasNotificacaoTeste
    {
        private readonly ConsultasNotificacao consultasNotificacao;
        private readonly Mock<IRepositorioNotificacaoConsulta> repositorioNotificacaoConsulta;
        private readonly Mock<IRepositorioNotificacao> repositorioNotificacao;
        private readonly Mock<IRepositorioUsuario> repositorioUsuario;
        private readonly Mock<IMediator> mediator;

        public ConsultasNotificacaoTeste()
        {
            repositorioNotificacao = new Mock<IRepositorioNotificacao>();
            repositorioNotificacaoConsulta = new Mock<IRepositorioNotificacaoConsulta>();

            repositorioUsuario = new Mock<IRepositorioUsuario>();
            mediator = new Mock<IMediator>();

            var context = new DefaultHttpContext();
            var obj = new HttpContextAccessor();
            obj.HttpContext = context;

            consultasNotificacao = new ConsultasNotificacao(repositorioNotificacao.Object, new ContextoHttp(obj), mediator.Object);
        }

        [Fact(DisplayName = "DeveDispararExcecaoAoInstanciarSemDependencias")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasNotificacao(null, null, null));
        }

        [Fact(DisplayName = "ListarNotificacoesBasicaPorAnoLetivoRF")]
        public async Task DeveListarNotificacoesBasicaPorAnoLetivoRF()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterNotificacoesPorAnoLetivoERfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Notificacao>()
                {
                    new Notificacao { Id = 1 },
                });

            var notificacoes = await consultasNotificacao.ListarPorAnoLetivoRf(2019, "1");

            mediator.Verify(r => r.Send(It.IsAny<ObterNotificacoesPorAnoLetivoERfQuery>(), It.IsAny<CancellationToken>()));
            Assert.True(notificacoes.Any());
            Assert.True(notificacoes.First().Id == 1);
        }

        [Fact(DisplayName = "ObterNotificacaoBasicaListaPorAnoLetivoeRf")]
        public async Task ObterNotificacaoBasicaListaPorAnoLetivoeRf()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterNotificacoesPorAnoLetivoERfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Notificacao>()
                {
                    new Notificacao { Id = 1 },
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var notificacoes = await consultasNotificacao.ObterNotificacaoBasicaLista(2019, "1");

            mediator.Verify(r => r.Send(It.IsAny<ObterNotificacoesPorAnoLetivoERfQuery>(), It.IsAny<CancellationToken>()));
            mediator.Verify(r => r.Send(It.IsAny<ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery>(), It.IsAny<CancellationToken>()));
            Assert.True(notificacoes.Notificacoes.Any());
            Assert.True(notificacoes.QuantidadeNaoLidas == 1);
        }

        [Fact(DisplayName = "ObterQuantidadeNaoLidasPorAnoERF")]
        public async Task DeveObterQuantidadeNotificacoesNoLidasPorAnoERF()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var qtd = await consultasNotificacao.QuantidadeNotificacoesNaoLidas(2019, "1");

            mediator.Verify(r => r.Send(It.IsAny<ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery>(), It.IsAny<CancellationToken>()));
            Assert.True(qtd == 1);
        }
    }
}