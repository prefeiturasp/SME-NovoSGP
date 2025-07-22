using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Relatorio
{
    public class GerarRelatorioFrequenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GerarRelatorioFrequenciaUseCase _useCase;
        private readonly Faker _faker;

        public GerarRelatorioFrequenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new GerarRelatorioFrequenciaUseCase(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void DeveLancarExcecao_QuandoMediatorForNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new GerarRelatorioFrequenciaUseCase(null));
        }

        [Fact]
        public async Task DeveLancarNegocioException_QuandoUsuarioNaoForEncontrado()
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaDto();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                         .ReturnsAsync((Usuario)null);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foi possível localizar o usuário.", excecao.Message);
        }

        [Fact]
        public async Task DeveLancarNegocioException_QuandoDreForTodas()
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaDto { CodigoDre = "-99" };
            var usuario = CriarUsuarioMock();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                         .ReturnsAsync(usuario);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não é possível gerar esse relatório para todas as DREs", excecao.Message);
        }

        [Theory]
        [InlineData(Modalidade.Fundamental)]
        [InlineData(Modalidade.EducacaoInfantil)]
        [InlineData(Modalidade.Medio)]
        public async Task DevePreencherTodosBimestres_QuandoSelecionadoTodos_ParaModalidadesNaoSemestrais(Modalidade modalidade)
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaDto
            {
                CodigoDre = "108300",
                Modalidade = modalidade,
                Bimestres = new List<int> { -99 }
            };

            var usuario = CriarUsuarioMock();
            var bimestresEsperados = new List<int> { 0, 1, 2, 3, 4 };
            FiltroRelatorioFrequenciaDto filtroCapturado = null;

            ConfigurarMocks(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) =>
                         {
                             var comando = Assert.IsType<GerarRelatorioCommand>(cmd);
                             filtroCapturado = Assert.IsType<FiltroRelatorioFrequenciaDto>(comando.Filtros);
                         })
                         .ReturnsAsync(true);

            // Act
            await _useCase.Executar(filtro);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarRelatorioCommand>(), default), Times.Once);
            Assert.NotNull(filtroCapturado);
            Assert.Equal(usuario.Nome, filtroCapturado.NomeUsuario);
            Assert.Equal(usuario.CodigoRf, filtroCapturado.CodigoRf);
            Assert.Equal(bimestresEsperados, filtroCapturado.Bimestres);
        }

        [Theory]
        [InlineData(Modalidade.EJA)]
        [InlineData(Modalidade.CELP)]
        public async Task DevePreencherTodosBimestres_QuandoSelecionadoTodos_ParaModalidadesSemestrais(Modalidade modalidade)
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaDto
            {
                CodigoDre = "108300",
                Modalidade = modalidade,
                Bimestres = new List<int> { -99 }
            };

            var usuario = new Usuario { Nome = "Usuario Teste EJA", CodigoRf = "rf456" };
            var bimestresEsperados = new List<int> { 0, 1, 2 };
            FiltroRelatorioFrequenciaDto filtroCapturado = null;

            ConfigurarMocks(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) =>
                         {
                             var comando = Assert.IsType<GerarRelatorioCommand>(cmd);
                             filtroCapturado = Assert.IsType<FiltroRelatorioFrequenciaDto>(comando.Filtros);
                         })
                         .ReturnsAsync(true);

            // Act
            await _useCase.Executar(filtro);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarRelatorioCommand>(), default), Times.Once);
            Assert.NotNull(filtroCapturado);
            Assert.Equal(usuario.Nome, filtroCapturado.NomeUsuario);
            Assert.Equal(usuario.CodigoRf, filtroCapturado.CodigoRf);
            Assert.Equal(bimestresEsperados, filtroCapturado.Bimestres);
        }

        [Fact]
        public async Task DeveExecutarComSucesso_QuandoFiltroForValido_SemTodosBimestres()
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaDto
            {
                CodigoDre = "108300",
                Modalidade = Modalidade.Fundamental,
                Bimestres = new List<int> { 1, 3 }
            };

            var usuario = new Usuario { Nome = "Usuario Final", CodigoRf = "rf789" };
            FiltroRelatorioFrequenciaDto filtroCapturado = null;

            ConfigurarMocks(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) =>
                         {
                             var comando = Assert.IsType<GerarRelatorioCommand>(cmd);
                             filtroCapturado = Assert.IsType<FiltroRelatorioFrequenciaDto>(comando.Filtros);
                         })
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarRelatorioCommand>(), default), Times.Once);

            Assert.NotNull(filtroCapturado);
            Assert.Equal(usuario.Nome, filtroCapturado.NomeUsuario);
            Assert.Equal(usuario.CodigoRf, filtroCapturado.CodigoRf);
            Assert.Equal(filtro.Bimestres, filtroCapturado.Bimestres);
        }

        private void ConfigurarMocks(Usuario usuario)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                         .ReturnsAsync(usuario);
        }

        private Usuario CriarUsuarioMock()
        {
            return new Usuario
            {
                Id = _faker.Random.Long(1, 10000),
                Nome = _faker.Person.FullName,
                CodigoRf = _faker.Random.Int().ToString(),
                PerfilAtual = Guid.NewGuid()
            };
        }
    }
}
