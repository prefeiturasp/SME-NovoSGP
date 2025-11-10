using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNivelEscritaAlfabetizacao;
using SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoNivelEscritaAlfabetizacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase _sut;

        private static readonly Faker<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto> _dtoFaker =
            new Faker<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>("pt_BR")
                .RuleFor(s => s.DreCodigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(s => s.UeCodigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(s => s.AnoLetivo, f => DateTime.Now.Year.ToString());

        public ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _sut = new ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_QuandoDadosSaoObtidosESalvosComSucesso_DeveRetornarTrue()
        {
            // Arrange
            var dadosConsolidados = _dtoFaker.Generate(5);
            var mensagemRabbit = new MensagemRabbit();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacaoNivelEscritaAlfabetizacaoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosConsolidados);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacaoNivelEscritaAlfabetizacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_QuandoQueryRetornaNulo_DeveRetornarFalseENaoChamarSalvar()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacaoNivelEscritaAlfabetizacaoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((IEnumerable<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>)null);
            // Act
            var resultado = await _sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeFalse();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacaoNivelEscritaAlfabetizacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
