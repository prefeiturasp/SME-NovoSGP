using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaAusenciaFechamento
{
    public class ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase _useCase;

        public ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Ativo_Existe_Deve_Enviar_Comandos_Para_Modalidades()
        {
            var parametro = new ParametrosSistema
            {
                Ativo = true,
                Nome = "DiasGeracaoPendenciaAusenciaFechamento",
                Valor = "15"
            };
            var parametros = new List<ParametrosSistema> { parametro };
            ConfigurarMockParametros(parametros);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(
                It.Is<ExecutarVerificacaoPendenciaAusenciaFechamentoCommand>(c =>
                    c.DiasParaGeracaoDePendencia == 15 &&
                    c.ModalidadeTipoCalendario == ModalidadeTipoCalendario.FundamentalMedio),
                It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ExecutarVerificacaoPendenciaAusenciaFechamentoCommand>(c =>
                    c.DiasParaGeracaoDePendencia == 15 &&
                    c.ModalidadeTipoCalendario == ModalidadeTipoCalendario.EJA),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Nao_Existe_Nao_Deve_Enviar_Comandos()
        {
            ConfigurarMockParametros(new List<ParametrosSistema>());

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(
                It.IsAny<ExecutarVerificacaoPendenciaAusenciaFechamentoCommand>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Inativo_Nao_Deve_Enviar_Comandos()
        {
            var parametro = new ParametrosSistema
            {
                Ativo = false,
                Nome = "DiasGeracaoPendenciaAusenciaFechamento",
                Valor = "15"
            };
            var parametros = new List<ParametrosSistema> { parametro };
            ConfigurarMockParametros(parametros);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(
                It.IsAny<ExecutarVerificacaoPendenciaAusenciaFechamentoCommand>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        private void ConfigurarMockParametros(IEnumerable<ParametrosSistema> parametros)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametros);
        }
    }
}
