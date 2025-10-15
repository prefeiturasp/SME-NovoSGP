using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ExcluirAtribuicaoEsporadicaTeste
{
    public class ObterPeriodoAtribuicaoPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPeriodoAtribuicaoPorUeUseCase _useCase;

        public ObterPeriodoAtribuicaoPorUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPeriodoAtribuicaoPorUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_ShouldReturnPeriodoAtribuicao_WhenUeEhUnidadeInfantil()
        {
            long ueId = 123;
            int anoLetivo = 2024;
            var ueMock = new Ue { Id = ueId, TipoEscola = Dominio.TipoEscola.EMEI };
            long tipoCalendarioIdMock = 1;
            var periodosEscolaresMock = new List<PeriodoEscolar>
        {
            new PeriodoEscolar { Bimestre = 1, PeriodoInicio = new DateTime(2024, 3, 1), PeriodoFim = new DateTime(2024, 5, 31) },
            new PeriodoEscolar { Bimestre = 2, PeriodoInicio = new DateTime(2024, 6, 1), PeriodoFim = new DateTime(2024, 8, 31) }
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUePorIdQuery>(q => q.Id == ueId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(q =>
                    q.Modalidade == ModalidadeTipoCalendario.Infantil && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioIdMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(q => q.TipoCalendarioId == tipoCalendarioIdMock), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodosEscolaresMock);

            var result = await _useCase.Executar(ueId, anoLetivo);

            Assert.NotNull(result);
            Assert.Equal(periodosEscolaresMock.First().PeriodoInicio, result.DataInicio);
            Assert.Equal(periodosEscolaresMock.Last().PeriodoFim, result.DataFim);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterUePorIdQuery>(q => q.Id == ueId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(q =>
                    q.Modalidade == ModalidadeTipoCalendario.Infantil && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(q => q.TipoCalendarioId == tipoCalendarioIdMock), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_ShouldReturnPeriodoAtribuicao_WhenUeNaoEhUnidadeInfantil()
        {
            long ueId = 456;
            int anoLetivo = 2024;
            var ueMock = new Ue { Id = ueId, TipoEscola = Dominio.TipoEscola.EMEF };
            long tipoCalendarioIdMock = 2;
            var periodosEscolaresMock = new List<PeriodoEscolar>
        {
            new PeriodoEscolar { Bimestre = 1, PeriodoInicio = new DateTime(2024, 2, 1), PeriodoFim = new DateTime(2024, 4, 30) },
            new PeriodoEscolar { Bimestre = 2, PeriodoInicio = new DateTime(2024, 5, 1), PeriodoFim = new DateTime(2024, 7, 31) }
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUePorIdQuery>(q => q.Id == ueId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(q =>
                    q.Modalidade == ModalidadeTipoCalendario.FundamentalMedio && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioIdMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(q => q.TipoCalendarioId == tipoCalendarioIdMock), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodosEscolaresMock);

            var result = await _useCase.Executar(ueId, anoLetivo);

            Assert.NotNull(result);
            Assert.Equal(periodosEscolaresMock.First().PeriodoInicio, result.DataInicio);
            Assert.Equal(periodosEscolaresMock.Last().PeriodoFim, result.DataFim);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterUePorIdQuery>(q => q.Id == ueId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(q =>
                    q.Modalidade == ModalidadeTipoCalendario.FundamentalMedio && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(q => q.TipoCalendarioId == tipoCalendarioIdMock), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_ShouldThrowNegocioException_WhenUeNotFound()
        {
            long ueId = 999;
            int anoLetivo = 2024;
            Ue ueMock = null;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUePorIdQuery>(q => q.Id == ueId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(ueId, anoLetivo));

            Assert.Equal("UE não encontrada", exception.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterUePorIdQuery>(q => q.Id == ueId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_ShouldThrowNegocioException_WhenTipoCalendarioIdNotFound_FundamentalMedio()
        {
            long ueId = 456;
            int anoLetivo = 2024;
            var ueMock = new Ue { Id = ueId, TipoEscola = Dominio.TipoEscola.EMEFM }; 
            long tipoCalendarioIdMock = 0;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUePorIdQuery>(q => q.Id == ueId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(q =>
                    q.Modalidade == ModalidadeTipoCalendario.FundamentalMedio && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioIdMock);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(ueId, anoLetivo));

            Assert.Equal("Tipo do calendario não encontrado", exception.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterUePorIdQuery>(q => q.Id == ueId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(q =>
                    q.Modalidade == ModalidadeTipoCalendario.FundamentalMedio && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
