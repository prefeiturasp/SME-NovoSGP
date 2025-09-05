using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAlfabetizacaoCriticaEscrita;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommandHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita> _repositorioMock;
        private readonly SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommandHandler _sut;
        private readonly Faker _faker;

        public SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommandHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioMock = new Mock<IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita>();
            _sut = new SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommandHandler(_mediatorMock.Object, _repositorioMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Theory(DisplayName = "Deve retornar false para entradas inválidas sem acionar dependências")]
        [MemberData(nameof(ComandosInvalidos))]
        public async Task Handle_QuandoComandoInvalido_DeveRetornarFalse(SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand comando)
        {
            // Arrange & Act
            var resultado = await _sut.Handle(comando, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<IEnumerable<Ue>>>(), It.IsAny<CancellationToken>()), Times.Never);
            _repositorioMock.Verify(r => r.ExcluirConsolidacaoAlfabetizacaoCriticaEscrita(), Times.Never);
            _repositorioMock.Verify(r => r.SalvarConsolidacaoAlfabetizacaoCriticaEscrita(It.IsAny<ConsolidacaoAlfabetizacaoCriticaEscrita>()), Times.Never);
        }

        [Fact]
        public async Task Handle_QuandoComandoValido_DeveOrdenarEnriquecerSalvarERetornarTrue()
        {
            // Arrange
            var dtosNaoOrdenados = new List<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>
            {
                new() { DreCodigo = "DRE1", UeCodigo = "UE1", PercentualNaoAlfabetizados = 50, QuantidadeNaoAlfabetizados = 10 },
                new() { DreCodigo = "DRE2", UeCodigo = "UE2", PercentualNaoAlfabetizados = 75, QuantidadeNaoAlfabetizados = 30 }, // Deverá ser o primeiro
                new() { DreCodigo = "DRE2", UeCodigo = "UE3", PercentualNaoAlfabetizados = 75, QuantidadeNaoAlfabetizados = 20 }  // Deverá ser o segundo
            };
            var comando = new SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand(dtosNaoOrdenados);

            var uesRetorno = new List<Ue>
            {
                new() { CodigoUe = "UE1", Nome = "EMEF UE 1", Dre = new Dre { CodigoDre = "DRE1", Nome = "DRE UM" } },
                new() { CodigoUe = "UE2", Nome = "EMEF UE 2", Dre = new Dre { CodigoDre = "DRE2", Nome = "DRE DOIS" } },
                new() { CodigoUe = "UE3", Nome = "EMEF UE 3", Dre = new Dre { CodigoDre = "DRE2", Nome = "DRE DOIS" } }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesComDrePorCodigoUesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(uesRetorno);

            var capturadorDeEntidades = new List<ConsolidacaoAlfabetizacaoCriticaEscrita>();
            _repositorioMock.Setup(r => r.SalvarConsolidacaoAlfabetizacaoCriticaEscrita(It.IsAny<ConsolidacaoAlfabetizacaoCriticaEscrita>()))
                           .Callback<ConsolidacaoAlfabetizacaoCriticaEscrita>(entidade => capturadorDeEntidades.Add(entidade))
                           .ReturnsAsync(true);

            // Act
            var resultado = await _sut.Handle(comando, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.Is<ObterUesComDrePorCodigoUesQuery>(q => q.UesCodigos.Length == 3), It.IsAny<CancellationToken>()), Times.Once);
            _repositorioMock.Verify(r => r.ExcluirConsolidacaoAlfabetizacaoCriticaEscrita(), Times.Once);
            _repositorioMock.Verify(r => r.SalvarConsolidacaoAlfabetizacaoCriticaEscrita(It.IsAny<ConsolidacaoAlfabetizacaoCriticaEscrita>()), Times.Exactly(3));

            // Valida a ordem e os dados enriquecidos
            capturadorDeEntidades.Should().HaveCount(3);

            // Posição 1
            capturadorDeEntidades[0].Posicao.Should().Be(1);
            capturadorDeEntidades[0].UeCodigo.Should().Be("UE2");
            capturadorDeEntidades[0].UeNome.Should().Be("EMEF UE 2");
            capturadorDeEntidades[0].DreNome.Should().Be("DRE DOIS");
            capturadorDeEntidades[0].TotalAlunosNaoAlfabetizados.Should().Be(30);

            // Posição 2
            capturadorDeEntidades[1].Posicao.Should().Be(2);
            capturadorDeEntidades[1].UeCodigo.Should().Be("UE3");

            // Posição 3
            capturadorDeEntidades[2].Posicao.Should().Be(3);
            capturadorDeEntidades[2].UeCodigo.Should().Be("UE1");
        }

        public static IEnumerable<object[]> ComandosInvalidos =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand(null) },
                new object[] { new SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand(new List<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>()) }
            };
    }
}
