using Bogus;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNivelEscritaAlfabetizacao;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;
using System.Threading;
using System;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommandHandlerTeste
    {
        private readonly Mock<IRepositorioConsolidacaoAlfabetizacaoNivelEscrita> _repositorioMock;
        private readonly SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommandHandler _sut;

        // Faker para gerar dados de teste para a DTO
        private static readonly Faker<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto> _dtoFaker =
            new Faker<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>("pt_BR")
                .RuleFor(s => s.DreCodigo, f => f.Random.Number(100000, 999999).ToString())
                .RuleFor(s => s.UeCodigo, f => f.Random.Number(100000, 999999).ToString())
                .RuleFor(s => s.AnoLetivo, f => DateTime.Now.Year.ToString())
                .RuleFor(s => s.NivelEscrita, f => f.PickRandom(new[] { "A", "PS", "SA", "SCV", "SSV" }))
                .RuleFor(s => s.Periodo, f => f.Random.Short(1, 4))
                .RuleFor(s => s.QuantidadeAlunos, f => f.Random.Int(5, 35));

        public SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommandHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioConsolidacaoAlfabetizacaoNivelEscrita>();
            _sut = new SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommandHandler(_repositorioMock.Object);
        }

        [Fact]
        public async Task Handle_QuandoComandoComListaNulaOuVazia_DeveRetornarFalse()
        {
            // Arrange
            var comandoComListaVazia = new SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand(new List<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>());
            var comandoComListaNula = new SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand(null);

            // Act
            var resultadoListaVazia = await _sut.Handle(comandoComListaVazia, CancellationToken.None);
            var resultadoListaNula = await _sut.Handle(comandoComListaNula, CancellationToken.None);

            // Assert
            resultadoListaVazia.Should().BeFalse();
            resultadoListaNula.Should().BeFalse();
            _repositorioMock.Verify(r => r.ExcluirConsolidacaoNivelEscrita(), Times.Never);
            _repositorioMock.Verify(r => r.SalvarConsolidacaoNivelEscrita(It.IsAny<ConsolidacaoAlfabetizacaoNivelEscrita>()), Times.Never);
        }

        [Fact]
        public async Task Handle_QuandoComandoValido_DeveExcluirSalvarERetornarTrue()
        {
            // Arrange
            var listaDeConsolidacoes = _dtoFaker.Generate(3);
            var comando = new SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand(listaDeConsolidacoes);

            _repositorioMock.Setup(r => r.ExcluirConsolidacaoNivelEscrita()).Returns(Task.CompletedTask);
            _repositorioMock.Setup(r => r.SalvarConsolidacaoNivelEscrita(It.IsAny<ConsolidacaoAlfabetizacaoNivelEscrita>())).ReturnsAsync(true);

            // Act
            var resultado = await _sut.Handle(comando, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _repositorioMock.Verify(r => r.ExcluirConsolidacaoNivelEscrita(), Times.Once);
            _repositorioMock.Verify(r => r.SalvarConsolidacaoNivelEscrita(It.IsAny<ConsolidacaoAlfabetizacaoNivelEscrita>()), Times.Exactly(listaDeConsolidacoes.Count));
        }

        [Fact]
        public async Task Handle_QuandoRepositorioLancaExcecao_DeveLancarExcecaoComMensagemCustomizada()
        {
            // Arrange
            var listaDeConsolidacoes = _dtoFaker.Generate(2);
            var comando = new SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand(listaDeConsolidacoes);
            var mensagemErroOriginal = "Erro de conexão com o banco de dados";

            _repositorioMock.Setup(r => r.ExcluirConsolidacaoNivelEscrita()).ThrowsAsync(new Exception(mensagemErroOriginal));

            // Act
            Func<Task> act = async () => await _sut.Handle(comando, CancellationToken.None);

            // Assert
            var excecao = await act.Should().ThrowAsync<Exception>();
            excecao.WithMessage($"Erro ao salvar consolidação do nível de escrita/alfabetização. {mensagemErroOriginal}");
        }
    }
}
