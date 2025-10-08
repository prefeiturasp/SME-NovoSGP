using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap;
using SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDadosBrutosParaConsolidacaoIndicadoresPap;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra.Dtos.Frequencia;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryHandler _handler;
        private readonly Faker _faker;

        public ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _handler = new ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryHandler(_mediatorMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_QuandoNaoEncontrarAlunos_DeveRetornarTuplaNulaESair()
        {
            // Arrange
            var query = new ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery(2025);
            var listaVaziaDeAlunos = new List<DadosMatriculaAlunoTipoPapDto>();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAlunosTurmaPapQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaVaziaDeAlunos);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.dadosAlunoTurma.Should().BeNull();
            resultado.dificuldadesPap.Should().BeNull();
            resultado.frequenciaBaixa.Should().BeNull();

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosTurmaPapQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterIndicadoresPapSgpConsolidadoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterQuantidadeAbaixoFrequenciaPorTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_QuandoEncontrarAlunos_DeveOrquestrarBuscasERetornarDadosBrutos()
        {
            // Arrange
            const int anoLetivo = 2025;
            var query = new ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery(anoLetivo);

            var alunosMock = new List<DadosMatriculaAlunoTipoPapDto>
            {
                new DadosMatriculaAlunoTipoPapDto { ComponenteCurricularId = ComponentesCurricularesConstants.CODIGO_PAP_PROJETO_COLABORATIVO },
                new DadosMatriculaAlunoTipoPapDto { ComponenteCurricularId = ComponentesCurricularesConstants.CODIGO_PAP_RECUPERACAO_APRENDIZAGENS },
                new DadosMatriculaAlunoTipoPapDto { ComponenteCurricularId = ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_ALFABETIZACAO },
            };

            var indicadoresMock = new List<ContagemDificuldadeIndicadoresPapPorTipoDto>
            {
                new ContagemDificuldadeIndicadoresPapPorTipoDto { NomeDificuldade = _faker.Lorem.Word() }
            };

            var frequenciaMock = new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>
            {
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = _faker.Random.Int(1, 100) }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAlunosTurmaPapQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterIndicadoresPapSgpConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(indicadoresMock);

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterQuantidadeAbaixoFrequenciaPorTurmaQuery>(q => q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciaMock);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosTurmaPapQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterIndicadoresPapSgpConsolidadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterQuantidadeAbaixoFrequenciaPorTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            resultado.dadosAlunoTurma.Should().NotBeNull();
            resultado.dificuldadesPap.Should().BeEquivalentTo(indicadoresMock);
            resultado.frequenciaBaixa.Should().BeEquivalentTo(frequenciaMock);

            // Valida se o método privado DefinirTipoPap foi executado corretamente
            resultado.dadosAlunoTurma.Should().HaveCount(3);
            resultado.dadosAlunoTurma.ElementAt(0).TipoPap.Should().Be(TipoPap.PapColaborativo);
            resultado.dadosAlunoTurma.ElementAt(1).TipoPap.Should().Be(TipoPap.RecuperacaoAprendizagens);
            resultado.dadosAlunoTurma.ElementAt(2).TipoPap.Should().Be(TipoPap.Pap2Ano);
        }
    }
}
