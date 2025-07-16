using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ExigeAprovacaoDeNotaQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExigeAprovacaoDeNotaQueryHandler _handler;

        public ExigeAprovacaoDeNotaQueryHandlerTeste()
        {
            _faker = new Faker();
            _mediatorMock = new Mock<IMediator>();
            _handler = new ExigeAprovacaoDeNotaQueryHandler(_mediatorMock.Object);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoMediatorNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExigeAprovacaoDeNotaQueryHandler(null));
        }

        [Fact(DisplayName = "Deve retornar true quando ano letivo da turma não é o atual o usuário não é gestor escolar e parametro AprovacaoAlteracaoNotaFechamento")]
        public async Task DeveChamarMediator_E_RetornarExigeAprovacaoDeNota()
        {
            // Arrange
            var anoAnterior = DateTime.Today.Year - 1;
            var turma = new Turma { AnoLetivo = _faker.Random.Int(2000, anoAnterior) };
            var query = new ExigeAprovacaoDeNotaQuery(turma);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { PerfilAtual = Perfis.PERFIL_COORDENADOR_NAAPA });
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });
            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(resultado);
        }

        [Theory(DisplayName = "Deve retornar false quando o usuário é gestor")]
        [InlineData("44E1E074-37D6-E911-ABD6-F81654FE895D" /*PERFIL_CP*/)]
        [InlineData("32C01A4F-B251-4A0F-933D-5B61C8B5DDBF" /*PERFIL_COORDENADOR_CELP*/)]
        [InlineData("46E1E074-37D6-E911-ABD6-F81654FE895D" /*PERFIL_DIRETOR*/)]
        [InlineData("45E1E074-37D6-E911-ABD6-F81654FE895D" /*PERFIL_AD*/)]
        public async Task DeveChamarMediator_E_RetornarExigeAprovacaoDeNota_False_QuandoUsuarioEhGestor(string perfil)
        {
            // Arrange
            var anoAtual = DateTime.Today.Year;
            var turma = new Turma { AnoLetivo = _faker.Random.Int(anoAtual, anoAtual + 5) };
            var query = new ExigeAprovacaoDeNotaQuery(turma);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { PerfilAtual = Guid.Parse(perfil) });
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });
            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.False(resultado);
        }

        [Fact(DisplayName = "Deve retornar false quando AprovacaoAlteracaoNotaFechamento estiver inativo")]
        public async Task DeveChamarMediator_E_RetornarExigeAprovacaoDeNota_False_QuandoParametroInativo()
        {
            // Arrange
            var anoAnterior = DateTime.Today.Year - 1;
            var turma = new Turma { AnoLetivo = _faker.Random.Int(2000, anoAnterior) };
            var query = new ExigeAprovacaoDeNotaQuery(turma);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { PerfilAtual = Perfis.PERFIL_POSL });
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = false });
            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.False(resultado);
        }

        [Fact(DisplayName = "Deve lançar exceção quando parametro aprovação não for encontrado")]
        public async Task DeveLancarExcecao_QuandoParametroAprovacaoNaoEncontrado()
        {
            // Arrange
            var anoAnterior = DateTime.Today.Year - 1;
            var turma = new Turma { AnoLetivo = _faker.Random.Int(2000, anoAnterior) };
            var query = new ExigeAprovacaoDeNotaQuery(turma);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { PerfilAtual = Perfis.PERFIL_PROFESSOR });
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotafechamento' para o ano {turma.AnoLetivo}"));
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact(DisplayName = "Deve retornar false quando ano letivo da turma é o atual")]
        public async Task DeveChamarMediator_E_RetornarExigeAprovacaoDeNota_False_QuandoAnoLetivoAtual()
        {
            // Arrange
            var anoAtual = DateTime.Today.Year;
            var turma = new Turma { AnoLetivo = anoAtual };
            var query = new ExigeAprovacaoDeNotaQuery(turma);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { PerfilAtual = Perfis.PERFIL_PROFESSOR });
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });
            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.False(resultado);
        }
    }
}
