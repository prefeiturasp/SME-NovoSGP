using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaProfessor
{
    public class ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase _useCase;

        public ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Ambos_Parametros_Existem_Deve_Enviar_Ambos_Comandos()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Ativo = true, Nome = "DiasGeracaoPendenciaAvaliacaoProfessor", Valor = "10" },
                new ParametrosSistema { Ativo = true, Nome = "DiasGeracaoPendenciaAvaliacaoCP", Valor = "20" }
            };
            ConfigurarMockParametros(parametros);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.Is<ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand>(c => c.DiasParaGeracaoDePendencia == 10), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ExecutarVerificacaoPendenciaAvaliacaoCPCommand>(c => c.DiasParaGeracaoDePendencia == 20), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Apenas_Parametro_Professor_Existe_Deve_Enviar_Apenas_Comando_Professor()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Ativo = true, Nome = "DiasGeracaoPendenciaAvaliacaoProfessor", Valor = "15" }
            };
            ConfigurarMockParametros(parametros);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.Is<ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand>(c => c.DiasParaGeracaoDePendencia == 15), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExecutarVerificacaoPendenciaAvaliacaoCPCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Apenas_Parametro_Cp_Existe_Deve_Enviar_Apenas_Comando_Cp()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Ativo = true, Nome = "DiasGeracaoPendenciaAvaliacaoCP", Valor = "25" }
            };
            ConfigurarMockParametros(parametros);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.Is<ExecutarVerificacaoPendenciaAvaliacaoCPCommand>(c => c.DiasParaGeracaoDePendencia == 25), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Nenhum_Parametro_Existe_Nao_Deve_Enviar_Comandos()
        {
            ConfigurarMockParametros(new List<ParametrosSistema>());

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Parametros_Existem_Mas_Inativos_Nao_Deve_Enviar_Comandos()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Ativo = false, Nome = "DiasGeracaoPendenciaAvaliacaoProfessor", Valor = "10" },
                new ParametrosSistema { Ativo = false, Nome = "DiasGeracaoPendenciaAvaliacaoCP", Valor = "20" }
            };
            ConfigurarMockParametros(parametros);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExecutarVerificacaoPendenciaAvaliacaoCPCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private void ConfigurarMockParametros(IEnumerable<ParametrosSistema> parametros)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametros);
        }
    }
}
