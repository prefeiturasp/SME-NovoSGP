using Moq;
using SME.SGP.Aplicacao.Queries.Abrangencia.VerificaSeUsuarioPossuiAbrangencia;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Abrangencia.VerificaSeUsuarioPossuiAbrangencia
{
    public class VerificaSeUsuarioPossuiAbrangenciaQueryHandlerTeste
    {
        private readonly Mock<IRepositorioAbrangencia> _mockRepositorio;
        private readonly VerificaSeUsuarioPossuiAbrangenciaQueryHandler _handler;

        public VerificaSeUsuarioPossuiAbrangenciaQueryHandlerTeste()
        {
            _mockRepositorio = new Mock<IRepositorioAbrangencia>();
            _handler = new VerificaSeUsuarioPossuiAbrangenciaQueryHandler(_mockRepositorio.Object);
        }

        [Fact]
        public void Construtor_DevelancarArgumentNullException_QuandoRepositorioForNulo()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new VerificaSeUsuarioPossuiAbrangenciaQueryHandler(null));

            Assert.Equal("repositorioAbrangencia", exception.ParamName);
        }

        [Fact]
        public async Task Handle_DeveRetornarTrue_QuandoUsuarioPossuirAbrangencia()
        {
            var usuarioRf = "1234567";
            var query = new VerificaSeUsuarioPossuiAbrangenciaQuery(usuarioRf);
            var cancellationToken = CancellationToken.None;

            _mockRepositorio
                .Setup(r => r.VerificaSeUsuarioPossuiAbrangencia(usuarioRf))
                .ReturnsAsync(true);

            var resultado = await _handler.Handle(query, cancellationToken);

            Assert.True(resultado);
            _mockRepositorio.Verify(r => r.VerificaSeUsuarioPossuiAbrangencia(usuarioRf), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoUsuarioNaoPossuirAbrangencia()
        {
            var usuarioRf = "7654321";
            var query = new VerificaSeUsuarioPossuiAbrangenciaQuery(usuarioRf);
            var cancellationToken = CancellationToken.None;

            _mockRepositorio
                .Setup(r => r.VerificaSeUsuarioPossuiAbrangencia(usuarioRf))
                .ReturnsAsync(false);

            var resultado = await _handler.Handle(query, cancellationToken);

            Assert.False(resultado);
            _mockRepositorio.Verify(r => r.VerificaSeUsuarioPossuiAbrangencia(usuarioRf), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametroCorreto()
        {
            var usuarioRf = "9876543";
            var query = new VerificaSeUsuarioPossuiAbrangenciaQuery(usuarioRf);
            var cancellationToken = CancellationToken.None;

            _mockRepositorio
                .Setup(r => r.VerificaSeUsuarioPossuiAbrangencia(It.IsAny<string>()))
                .ReturnsAsync(true);

            await _handler.Handle(query, cancellationToken);

            _mockRepositorio.Verify(
                r => r.VerificaSeUsuarioPossuiAbrangencia(usuarioRf),
                Times.Once,
                "O método deve ser chamado exatamente uma vez com o RF correto");
        }

        [Fact]
        public async Task Handle_DeveRespeitarCancellationToken()
        {
            var usuarioRf = "1234567";
            var query = new VerificaSeUsuarioPossuiAbrangenciaQuery(usuarioRf);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            _mockRepositorio
                .Setup(r => r.VerificaSeUsuarioPossuiAbrangencia(usuarioRf))
                .ThrowsAsync(new OperationCanceledException());

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _handler.Handle(query, cancellationTokenSource.Token));
        }

        [Fact]
        public async Task Handle_DevePropagaExcecaoDoRepositorio()
        {
            var usuarioRf = "1234567";
            var query = new VerificaSeUsuarioPossuiAbrangenciaQuery(usuarioRf);
            var cancellationToken = CancellationToken.None;
            var mensagemErro = "Erro ao acessar o banco de dados";

            _mockRepositorio
                .Setup(r => r.VerificaSeUsuarioPossuiAbrangencia(usuarioRf))
                .ThrowsAsync(new Exception(mensagemErro));

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(query, cancellationToken));

            Assert.Equal(mensagemErro, exception.Message);
        }
    }
}
