using Bogus;
using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterIndicadoresPapQueryHandlerTeste
    {
        private readonly Mock<IRepositorioPainelEducacionalConsultaIndicadoresPap> _repositorioMock;
        private readonly ObterIndicadoresPapQueryHandler _handler;
        private readonly Faker<PainelEducacionalConsolidacaoPapBase> _consolidacaoFaker;

        public ObterIndicadoresPapQueryHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsultaIndicadoresPap>();
            _handler = new ObterIndicadoresPapQueryHandler(_repositorioMock.Object);

            _consolidacaoFaker = new Faker<PainelEducacionalConsolidacaoPapBase>("pt_BR")
                .RuleFor(c => c.NomeDificuldadeTop1, f => f.Lorem.Word())
                .RuleFor(c => c.NomeDificuldadeTop2, f => f.Lorem.Word())
                .RuleFor(c => c.TotalAlunos, f => f.Random.Int(10, 50))
                .RuleFor(c => c.TotalTurmas, f => f.Random.Int(1, 5));
        }

        [Fact(DisplayName = "Deve chamar o repositório de UE quando o código da UE for informado")]
        public async Task Handle_QuandoCodigoUePreenchido_DeveChamarRepositorioDeUe()
        {
            // Arrange
            var query = new ObterIndicadoresPapQuery(2025, "DRE-1", "UE-1");

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositorioMock.Verify(r => r.ObterConsolidacoesUePorAno(query.AnoLetivo, query.CodigoDre, query.CodigoUe), Times.Once);
            _repositorioMock.Verify(r => r.ObterConsolidacoesDrePorAno(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _repositorioMock.Verify(r => r.ObterConsolidacoesSmePorAno(It.IsAny<int>()), Times.Never);
        }

        [Fact(DisplayName = "Deve chamar o repositório de DRE quando apenas o código da DRE for informado")]
        public async Task Handle_QuandoApenasCodigoDrePreenchido_DeveChamarRepositorioDeDre()
        {
            // Arrange
            var query = new ObterIndicadoresPapQuery(2025, "DRE-1", string.Empty);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositorioMock.Verify(r => r.ObterConsolidacoesDrePorAno(query.AnoLetivo, query.CodigoDre), Times.Once);
            _repositorioMock.Verify(r => r.ObterConsolidacoesUePorAno(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _repositorioMock.Verify(r => r.ObterConsolidacoesSmePorAno(It.IsAny<int>()), Times.Never);
        }

        [Fact(DisplayName = "Deve chamar o repositório de SME quando nenhum código for informado")]
        public async Task Handle_QuandoNenhumCodigoPreenchido_DeveChamarRepositorioDeSme()
        {
            // Arrange
            var query = new ObterIndicadoresPapQuery(2025, null, null);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositorioMock.Verify(r => r.ObterConsolidacoesSmePorAno(query.AnoLetivo), Times.Once);
            _repositorioMock.Verify(r => r.ObterConsolidacoesUePorAno(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _repositorioMock.Verify(r => r.ObterConsolidacoesDrePorAno(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName = "Deve mapear e ordenar os dados corretamente quando o repositório retorna informações")]
        public async Task Handle_QuandoRepositorioRetornaDados_DeveMapearEOrdenarCorretamente()
        {
            // Arrange
            var query = new ObterIndicadoresPapQuery(2025, null, null);

            var dadosMock = new List<PainelEducacionalConsolidacaoPapBase>
            {
                _consolidacaoFaker.RuleFor(c => c.TipoPap, TipoPap.Pap2Ano).Generate(), // Fora de ordem
                _consolidacaoFaker.RuleFor(c => c.TipoPap, TipoPap.PapColaborativo).Generate()
            };

            _repositorioMock.Setup(r => r.ObterConsolidacoesSmePorAno(query.AnoLetivo)).ReturnsAsync(dadosMock);

            var primeiroItem = dadosMock.First();

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.NomeDificuldadeTop1.Should().Be(primeiroItem.NomeDificuldadeTop1);
            resultado.NomeDificuldadeTop2.Should().Be(primeiroItem.NomeDificuldadeTop2);
            resultado.QuantidadesPorTipoPap.Should().HaveCount(2);
            resultado.QuantidadesPorTipoPap.Should().BeInAscendingOrder(q => q.TipoPap);

            var itemMapeado = resultado.QuantidadesPorTipoPap.First(q => q.TipoPap == primeiroItem.TipoPap);
            itemMapeado.TotalAlunos.Should().Be(primeiroItem.TotalAlunos);
            itemMapeado.TotalTurmas.Should().Be(primeiroItem.TotalTurmas);
        }

        [Fact(DisplayName = "Deve retornar DTO vazio quando o repositório não retorna dados")]
        public async Task Handle_QuandoRepositorioRetornaVazio_DeveRetornarDtoVazio()
        {
            // Arrange
            var query = new ObterIndicadoresPapQuery(2025, "DRE-1", "UE-1");
            _repositorioMock.Setup(r => r.ObterConsolidacoesUePorAno(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                          .ReturnsAsync(new List<PainelEducacionalConsolidacaoPapBase>());

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.NomeDificuldadeTop1.Should().BeNull();
            resultado.NomeDificuldadeTop2.Should().BeNull();
            resultado.QuantidadesPorTipoPap.Should().NotBeNull();
            resultado.QuantidadesPorTipoPap.Should().BeEmpty();
        }
    }
}
