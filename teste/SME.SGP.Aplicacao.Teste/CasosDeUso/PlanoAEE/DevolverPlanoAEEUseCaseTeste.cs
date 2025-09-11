using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class DevolverPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DevolverPlanoAEEUseCase _useCase;
        private readonly DevolucaoPlanoAEEDto _param;

        public DevolverPlanoAEEUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new DevolverPlanoAEEUseCase(_mediatorMock.Object);
            _param = new DevolucaoPlanoAEEDto { PlanoAEEId = 1, Motivo = "Motivo de devolução" };
        }

        [Fact]
        public async Task Executar_PlanoAEENaoEncontrado_DeveLancarNegocioException()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((SME.SGP.Dominio.PlanoAEE)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(_param));
            Assert.Equal("Plano AEE não encontrado.", exception.Message);
        }

        [Theory]
        [InlineData(true, true)]  // Parâmetro ativo
        [InlineData(false, false)] // Parâmetro inativo
        public async Task Executar_PlanoAEEEncontrado_DeveExecutarComSucessoEVerificarGeracaoPendencia(bool parametroAtivo, bool deveGerarPendencia)
        {
            // Arrange
            var planoAEE = new SME.SGP.Dominio.PlanoAEE { Id = 1, Situacao = SituacaoPlanoAEE.Validado };
            var parametroSistema = new SME.SGP.Dominio.ParametrosSistema { Ativo = parametroAtivo };

            ConfigurarMocksBasicos(planoAEE, parametroSistema);

            // Act
            var resultado = await _useCase.Executar(_param);

            // Assert
            Assert.True(resultado);
            Assert.Equal(SituacaoPlanoAEE.Devolvido, planoAEE.Situacao);

            VerificarChamadasBasicas();
            VerificarChamadaGerarPendencia(deveGerarPendencia);
        }

        [Fact]
        public async Task Executar_ParametroNulo_DeveExecutarSemGerarPendencia()
        {
            // Arrange
            var planoAEE = new SME.SGP.Dominio.PlanoAEE { Id = 1, Situacao = SituacaoPlanoAEE.Validado };

            ConfigurarMocksBasicos(planoAEE, null);

            // Act
            var resultado = await _useCase.Executar(_param);

            // Assert
            Assert.True(resultado);
            Assert.Equal(SituacaoPlanoAEE.Devolvido, planoAEE.Situacao);
            VerificarChamadaGerarPendencia(false);
        }

        [Theory]
        [InlineData(SituacaoPlanoAEE.Validado)]
        [InlineData(SituacaoPlanoAEE.Encerrado)]
        public async Task Executar_DiferentesSituacoes_DeveAlterarParaDevolvido(SituacaoPlanoAEE situacaoInicial)
        {
            // Arrange
            var planoAEE = new SME.SGP.Dominio.PlanoAEE { Id = 1, Situacao = situacaoInicial };
            var parametroSistema = new SME.SGP.Dominio.ParametrosSistema { Ativo = false };

            ConfigurarMocksBasicos(planoAEE, parametroSistema);

            // Act
            var resultado = await _useCase.Executar(_param);

            // Assert
            Assert.True(resultado);
            Assert.Equal(SituacaoPlanoAEE.Devolvido, planoAEE.Situacao);
        }

        [Fact]
        public async Task Executar_ErroAoPersistir_DevePropagarException()
        {
            // Arrange
            var planoAEE = new SME.SGP.Dominio.PlanoAEE { Id = 1, Situacao = SituacaoPlanoAEE.Validado };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(planoAEE);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PersistirPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new Exception("Erro ao persistir"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(_param));
            Assert.Equal("Erro ao persistir", exception.Message);
        }

        [Fact]
        public async Task Executar_ParametroAtivo_DeveVerificarAnoAtual()
        {
            // Arrange
            var planoAEE = new SME.SGP.Dominio.PlanoAEE { Id = 1, Situacao = SituacaoPlanoAEE.Validado };
            var parametroSistema = new SME.SGP.Dominio.ParametrosSistema { Ativo = true };

            ConfigurarMocksBasicos(planoAEE, parametroSistema);

            // Act
            await _useCase.Executar(_param);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(
                q => q.TipoParametroSistema == TipoParametroSistema.GerarPendenciasPlanoAEE && q.Ano == DateTime.Today.Year),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        private void ConfigurarMocksBasicos(SME.SGP.Dominio.PlanoAEE planoAEE, SME.SGP.Dominio.ParametrosSistema parametroSistema)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(planoAEE);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametroSistema);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PersistirPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ResolverPendenciaPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarPendenciaDevolucaoPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);
        }

        private void VerificarChamadasBasicas()
        {
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPlanoAEEPorIdQuery>(q => q.Id == _param.PlanoAEEId),
                It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PersistirPlanoAEECommand>(),
                It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ResolverPendenciaPlanoAEECommand>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        private void VerificarChamadaGerarPendencia(bool deveSerChamada)
        {
            var timesChamada = deveSerChamada ? Times.Once() : Times.Never();

            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarPendenciaDevolucaoPlanoAEECommand>(),
                It.IsAny<CancellationToken>()), timesChamada);
        }
    }
}
