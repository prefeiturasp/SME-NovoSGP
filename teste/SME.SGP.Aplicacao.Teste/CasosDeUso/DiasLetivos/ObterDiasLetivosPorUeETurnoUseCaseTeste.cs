using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiasLetivos
{
    public class ObterDiasLetivosPorUeETurnoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDiasLetivosPorUeETurnoUseCase _useCase;

        public ObterDiasLetivosPorUeETurnoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDiasLetivosPorUeETurnoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Data_Inicio_Maior_Que_Fim_Deve_Lancar_Excecao()
        {
            var filtro = new FiltroDiasLetivosPorUeEDataDTO { DataInicio = DateTime.Now.AddDays(1), DataFim = DateTime.Now };
            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
        }

        [Fact]
        public async Task Executar_Quando_Tipo_Calendario_Nao_Encontrado_Deve_Lancar_Excecao()
        {
            var filtro = new SME.SGP.Infra.FiltroDiasLetivosPorUeEDataDTO { DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(1), TipoTurno = (int)TipoTurno.Manha };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
        }

        [Fact]
        public async Task Executar_Quando_Periodos_Escolares_Nao_Encontrados_Deve_Lancar_Excecao()
        {
            var filtro = new FiltroDiasLetivosPorUeEDataDTO { DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(1), TipoTurno = (int)TipoTurno.Manha };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<PeriodoEscolar>());
            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
        }

        [Fact]
        public async Task Executar_Quando_Turno_EJA_Deve_Chamar_Query_Correta()
        {
            var filtro = new Infra.FiltroDiasLetivosPorUeEDataDTO { DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(1), TipoTurno = (int)TipoTurno.Noturno };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<PeriodoEscolar> { new PeriodoEscolar() });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<DiaLetivoDto>());

            await _useCase.Executar(filtro);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Processar_Dias_Corretamente_Com_Base_Nos_Periodos_E_Eventos()
        {
            var dataInicio = new DateTime(2025, 10, 2); // Quinta
            var filtro = new SME.SGP.Infra.FiltroDiasLetivosPorUeEDataDTO { DataInicio = dataInicio, DataFim = dataInicio.AddDays(3), TipoTurno = (int)TipoTurno.Manha };
            var periodos = new List<PeriodoEscolar> { new PeriodoEscolar { PeriodoInicio = dataInicio, PeriodoFim = dataInicio.AddDays(2) } };
            var eventos = new List<DiaLetivoDto> { new DiaLetivoDto { Data = dataInicio.AddDays(2), EhLetivo = true } };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(periodos);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(eventos);

            var resultado = (await _useCase.Executar(filtro)).ToList();

            Assert.Equal(4, resultado.Count);
            Assert.False(resultado[0].EhLetivo);
            Assert.False(resultado[1].EhLetivo);
            Assert.True(resultado[2].EhLetivo);
            Assert.False(resultado[3].EhLetivo);
        }
    }
}

