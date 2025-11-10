using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificarResultadoInsatisfatorioUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly NotificarResultadoInsatisfatorioUseCase _useCase;

        public NotificarResultadoInsatisfatorioUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new NotificarResultadoInsatisfatorioUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Executar_Teste()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Ativo = true, Nome = "DiasNotificacaoResultadoInsatisfatorio", Valor = "5" },
                new ParametrosSistema { Ativo = false, Nome = "OutroParametro", Valor = "10" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);

            _mediatorMock.Setup(m => m.Send(It.IsAny<NotificarResultadoInsatisfatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = new MensagemRabbit("teste");
            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarResultadoInsatisfatorioCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Quando_Nao_Existir_Parametro_Ativo_Deve_Teste_Teste()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Ativo = false, Nome = "DiasNotificacaoResultadoInsatisfatorio", Valor = "5" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);

            var mensagem = new MensagemRabbit("teste");
            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarResultadoInsatisfatorioCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Nulo_Deve_Teste_Teste()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<ParametrosSistema>)null);

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(""));
            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarResultadoInsatisfatorioCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
