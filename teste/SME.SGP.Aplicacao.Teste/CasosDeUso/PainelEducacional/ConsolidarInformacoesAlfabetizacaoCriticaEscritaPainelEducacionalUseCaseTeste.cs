using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAlfabetizacaoCriticaEscrita;
using SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoAlfabetizacaoCriticaEscrita;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase _sut;
        private readonly Faker<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto> _dtoFaker;

        public ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _sut = new ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase(_mediatorMock.Object);

            _dtoFaker = new Faker<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>("pt_BR")
                .RuleFor(s => s.DreCodigo, f => f.Random.AlphaNumeric(10).ToUpper())
                .RuleFor(s => s.UeCodigo, f => f.Random.AlphaNumeric(10).ToUpper())
                .RuleFor(s => s.QuantidadeNaoAlfabetizados, f => f.Random.Int(1, 50))
                .RuleFor(s => s.PercentualNaoAlfabetizados, f => f.Random.Decimal(1, 100));
        }

        [Fact]
        public async Task Executar_QuandoDadosSaoObtidosESalvosComSucesso_DeveRetornarTrue()
        {
            // Arrange
            var dadosConsolidados = _dtoFaker.Generate(3);
            var mensagemRabbit = new MensagemRabbit();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosConsolidados);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_QuandoQueryRetornaNulo_DeveRetornarFalseENaoChamarSalvar()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>)null);

            // Act
            var resultado = await _sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeFalse();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_QuandoMediatorLancaExcecao_DevePropagarExcecao()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit();
            var mensagemErro = "Erro simulado ao buscar dados";

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(mensagemErro));

            // Act
            Func<Task> act = async () => await _sut.Executar(mensagemRabbit);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage(mensagemErro);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
