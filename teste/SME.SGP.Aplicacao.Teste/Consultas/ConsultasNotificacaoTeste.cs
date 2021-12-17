using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Contexto;
using System;
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

            consultasNotificacao = new ConsultasNotificacao(repositorioNotificacao.Object, repositorioUsuario.Object, new ContextoHttp(obj), mediator.Object);
        }

        [Fact(DisplayName = "DeveDispararExcecaoAoInstanciarSemDependencias")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasNotificacao(null, null, null, null));
        }

        [Fact(DisplayName = "ListarNotificacoesBasicaPorAnoLetivoRF")]
        public void DeveListarNotificacoesBasicaPorAnoLetivoRF()
        {
            consultasNotificacao.ListarPorAnoLetivoRf(2019, "1");
            repositorioNotificacaoConsulta.Verify(r => r.ObterNotificacoesPorAnoLetivoERf(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        [Fact(DisplayName = "ObterNotificacaoBasicaListaPorAnoLetivoeRf")]
        public void DeveObterPlanoAnualResumidoPorAnoLetivoeRf()
        {
            consultasNotificacao.ObterNotificacaoBasicaLista(2019, "1");
            repositorioNotificacaoConsulta.Verify(r => r.ObterNotificacoesPorAnoLetivoERf(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        [Fact(DisplayName = "ObterQuantidadeNaoLidasPorAnoERF")]
        public void DeveObterQuantidadeNotificacoesNoLidasPorAnoERF()
        {
            consultasNotificacao.QuantidadeNotificacoesNaoLidas(2019, "1");
            repositorioNotificacaoConsulta.Verify(r => r.ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoERf(It.IsAny<int>(), It.IsAny<string>()));
        }
    }
}