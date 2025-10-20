using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotaUltimoAnoConsolidado;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterNotaUltimoAnoConsolidadoQueryHandlerTestes
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoNotaConsulta> _repositorioMock;
        private readonly ObterNotaUltimoAnoConsolidadoQueryHandler _handler;

        public ObterNotaUltimoAnoConsolidadoQueryHandlerTestes()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsolidacaoNotaConsulta>();
            _handler = new ObterNotaUltimoAnoConsolidadoQueryHandler(_repositorioMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarZero_QuandoNaoHouverAnoConsolidado()
        {
            // Arrange
            _repositorioMock.Setup(r => r.ObterUltimoAnoConsolidadoAsync())
                .ReturnsAsync((int?)null);
            var query = new ObterNotaUltimoAnoConsolidadoQuery();
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            Assert.Equal(0, resultado);
        }

        [Fact]
        public async Task Handle_DeveRetornarAnoConsolidado_QuandoHouverAnoConsolidado()
        {
            // Arrange
            var anoConsolidadoEsperado = 2022;
            _repositorioMock.Setup(r => r.ObterUltimoAnoConsolidadoAsync())
                .ReturnsAsync(anoConsolidadoEsperado);
            var query = new ObterNotaUltimoAnoConsolidadoQuery();
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            Assert.Equal(anoConsolidadoEsperado, resultado);
        }
    }
}
