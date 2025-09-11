using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ObterTiposFrequenciasUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTiposFrequenciasUseCase _useCase;

        public ObterTiposFrequenciasUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTiposFrequenciasUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarTodosOsTiposDeFrequenciaQuandoNaoHaFiltroDeModalidade()
        {
            // Arrange
            var filtro = new TipoFrequenciaFiltroDto();

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            var expectedCount = Enum.GetValues(typeof(TipoFrequencia)).Length;
            Assert.Equal(expectedCount, result.Count());

            foreach (TipoFrequencia tipo in Enum.GetValues(typeof(TipoFrequencia)))
            {
                Assert.Contains(result, d => d.Valor == tipo.ShortName() && d.Descricao == tipo.ShortName());
            }

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(Modalidade.EducacaoInfantil, TipoParametroSistema.HabilitaFrequenciaRemotaEICEI)]
        [InlineData(Modalidade.EJA, TipoParametroSistema.HabilitaFrequenciaRemotaEJA)]
        [InlineData(Modalidade.CIEJA, TipoParametroSistema.HabilitaFrequenciaRemotaCIEJA)]
        [InlineData(Modalidade.Fundamental, TipoParametroSistema.HabilitaFrequenciaRemotaEF)]
        [InlineData(Modalidade.Medio, TipoParametroSistema.HabilitaFrequenciaRemotaEM)]
        [InlineData(Modalidade.CMCT, TipoParametroSistema.HabilitaFrequenciaRemotaCMCT)]
        [InlineData(Modalidade.MOVA, TipoParametroSistema.HabilitaFrequenciaRemotaMOVA)]
        [InlineData(Modalidade.ETEC, TipoParametroSistema.HabilitaFrequenciaRemotaETEC)]
        public async Task Executar_DeveRetornarTodosOsTiposDeFrequenciaQuandoParametroHabilitaRemotaNaoExisteOuEhZero(Modalidade modalidade, TipoParametroSistema tipoParametroEsperado)
        {
            // Arrange
            var anoLetivo = 2025;
            var filtro = new TipoFrequenciaFiltroDto { Modalidade = (int)modalidade, AnoLetivo = anoLetivo };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == tipoParametroEsperado && q.Ano == anoLetivo
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ParametrosSistema)null);

            var result1 = await _useCase.Executar(filtro);

            // Assert 1
            Assert.NotNull(result1);
            Assert.DoesNotContain(result1, d => d.Valor == TipoFrequencia.R.ShortName());
            var expectedCountSemR = Enum.GetValues(typeof(TipoFrequencia)).Length - 1;
            Assert.Equal(expectedCountSemR, result1.Count());

            var parametroZero = new ParametrosSistema { Valor = "0" };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == tipoParametroEsperado && q.Ano == anoLetivo
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(parametroZero);

            // Act 2
            var result2 = await _useCase.Executar(filtro);

            // Assert 2
            Assert.NotNull(result2);
            Assert.DoesNotContain(result2, d => d.Valor == TipoFrequencia.R.ShortName());
            Assert.Equal(expectedCountSemR, result2.Count());

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(Modalidade.EducacaoInfantil, TipoParametroSistema.HabilitaFrequenciaRemotaEICEI)]
        [InlineData(Modalidade.EJA, TipoParametroSistema.HabilitaFrequenciaRemotaEJA)]
        [InlineData(Modalidade.CIEJA, TipoParametroSistema.HabilitaFrequenciaRemotaCIEJA)]
        public async Task Executar_DeveRetornarTodosOsTiposDeFrequenciaIncluindoRemotoQuandoHabilitaRemotaEhDiferenteDeZero(Modalidade modalidade, TipoParametroSistema tipoParametroEsperado)
        {
            // Arrange
            var anoLetivo = 2025;
            var filtro = new TipoFrequenciaFiltroDto { Modalidade = (int)modalidade, AnoLetivo = anoLetivo };

            var parametroHabilitado = new ParametrosSistema { Valor = "1" };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == tipoParametroEsperado && q.Ano == anoLetivo
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(parametroHabilitado);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            var expectedCount = Enum.GetValues(typeof(TipoFrequencia)).Length;
            Assert.Equal(expectedCount, result.Count());
            Assert.Contains(result, d => d.Valor == TipoFrequencia.R.ShortName());

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveUsarAnoAtualSeAnoLetivoNuloNoFiltro()
        {
            // Arrange
            var modalidade = Modalidade.Fundamental;
            var filtro = new TipoFrequenciaFiltroDto { Modalidade = (int)modalidade, AnoLetivo = null };
            var anoAtual = DateTime.Now.Year;

            var parametroHabilitado = new ParametrosSistema { Valor = "1" };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.HabilitaFrequenciaRemotaEF && q.Ano == anoAtual
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(parametroHabilitado);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result, d => d.Valor == TipoFrequencia.R.ShortName());

            _mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.HabilitaFrequenciaRemotaEF && q.Ano == anoAtual
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoSeMediatorFalharAoObterParametro()
        {
            // Arrange
            var modalidade = Modalidade.EJA;
            var anoLetivo = 2025;
            var filtro = new TipoFrequenciaFiltroDto { Modalidade = (int)modalidade, AnoLetivo = anoLetivo };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter parâmetro do sistema"));

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(filtro));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
            Assert.Contains("Erro ao obter parâmetro do sistema", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}