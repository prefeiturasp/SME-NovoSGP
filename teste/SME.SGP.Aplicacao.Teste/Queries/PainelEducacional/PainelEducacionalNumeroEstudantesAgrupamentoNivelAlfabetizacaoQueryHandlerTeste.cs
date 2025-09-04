using Bogus;
using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroAlunos;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioConsolidacaoAlfabetizacaoNivelEscrita> _repositorioMock;
        private readonly PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandler _sut;
        private readonly Faker _faker;

        public PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioConsolidacaoAlfabetizacaoNivelEscrita>();
            _sut = new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandler(_repositorioMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_QuandoRepositorioRetornaDados_DeveMapearParaDtoCorretamente()
        {
            // Arrange
            var query = new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(2025, 1, "108300", "094002");
            var dadosDoRepositorio = new List<ContagemNivelEscritaDto>
            {
                new ContagemNivelEscritaDto { NivelEscrita = "PS", Quantidade = 10 },
                new ContagemNivelEscritaDto {NivelEscrita = "A", Quantidade = 20}
            };

            _repositorioMock.Setup(r => r.ObterNumeroAlunos(query.AnoLetivo, query.Periodo, query.CodigoDre, query.CodigoUe))
                            .ReturnsAsync(dadosDoRepositorio);

            // Act
            var resultado = await _sut.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);

            var resultadoPreSilabico = resultado.FirstOrDefault(r => r.NivelAlfabetizacao == NivelAlfabetizacao.PreSilabico);
            resultadoPreSilabico.Should().NotBeNull();
            resultadoPreSilabico.TotalAlunos.Should().Be(10);
            resultadoPreSilabico.Dre.Should().Be("108300");

            var resultadoAlfabetico = resultado.FirstOrDefault(r => r.NivelAlfabetizacao == NivelAlfabetizacao.Alfabetico);
            resultadoAlfabetico.Should().NotBeNull();
            resultadoAlfabetico.TotalAlunos.Should().Be(20);
            resultadoAlfabetico.Ano.Should().Be(2025);

            _repositorioMock.Verify(r => r.ObterNumeroAlunos(2025, 1, "108300", "094002"), Times.Once);
        }

        [Theory(DisplayName = "Deve mapear corretamente a string do nível de escrita para o enum correspondente")]
        [InlineData("PS", NivelAlfabetizacao.PreSilabico)]
        [InlineData("SSV", NivelAlfabetizacao.SilabicoSemValor)]
        [InlineData("SCV", NivelAlfabetizacao.SilabicoComValor)]
        [InlineData("SA", NivelAlfabetizacao.SilabicoAlfabetico)]
        [InlineData("A", NivelAlfabetizacao.Alfabetico)]
        [InlineData(" ps ", NivelAlfabetizacao.PreSilabico)] // Testa Trim e ToUpper
        [InlineData("a", NivelAlfabetizacao.Alfabetico)]   // Testa ToUpper
        public async Task Handle_DeveMapearNivelEscritaStringParaEnumCorretamente(string nivelEscritaString, NivelAlfabetizacao nivelEsperado)
        {
            // Arrange
            var query = new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(2025, 1);
            var registroUnico = new List<ContagemNivelEscritaDto>
            {
                new ContagemNivelEscritaDto { NivelEscrita = nivelEscritaString, Quantidade = _faker.Random.Int(1, 50) }
            };

            _repositorioMock.Setup(r => r.ObterNumeroAlunos(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(registroUnico);

            // Act
            var resultado = await _sut.Handle(query, CancellationToken.None);

            // Assert
            var dto = resultado.First();
            dto.NivelAlfabetizacao.Should().Be(nivelEsperado);
            dto.NivelAlfabetizacaoDescricao.Should().Be(nivelEsperado.Description());
        }

        [Fact]
        public async Task Handle_QuandoRepositorioRetornaVazio_DeveRetornarVazio()
        {
            // Arrange
            var query = new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(2025, 1);
            _repositorioMock.Setup(r => r.ObterNumeroAlunos(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(new List<ContagemNivelEscritaDto>());
            // Act
            var resultado = await _sut.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }
    }
}
