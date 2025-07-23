using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Linq;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase _useCase;

        public ObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarNull_QuandoMediatorRetornaListaVazia()
        {
            // Arrange
            var FrequenciasConsolidadacaoPorTurmaEAnoDto = new FrequenciasConsolidadacaoPorTurmaEAnoDto
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 123,
                Modalidade = (int)Modalidade.Fundamental,
                AnoTurma = "5A",
                Semestre = 1,
                DataAula = new DateTime(2025, 7, 10),
                VisaoDre = false
            };
            var dataInicio = new DateTime(2025, 7, 1);
            var dataFim = new DateTime(2025, 7, 7);
            int? mes = null;
            var tipoConsolidadoFrequencia = TipoConsolidadoFrequencia.Semanal;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaGlobalMensalSemanalDto>());

            // Act
            var resultado = await _useCase.Executar(FrequenciasConsolidadacaoPorTurmaEAnoDto, dataInicio, dataFim, mes, tipoConsolidadoFrequencia);

            // Assert
            Assert.Null(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task Executar_DeveRetornarGraficoFrequenciaAlunoDto_QuandoMediatorRetornaDados_Semanal()
        {
            // Arrange            
            var frequenciaDto = new FrequenciasConsolidadacaoPorTurmaEAnoDto
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 123,
                Modalidade = (int)Modalidade.Fundamental,
                AnoTurma = "5A",
                Semestre = 1,
                DataAula = new DateTime(2025, 7, 10),
                VisaoDre = false
            };
            var lista = new List<FrequenciaGlobalMensalSemanalDto>
            {
                new FrequenciaGlobalMensalSemanalDto
                {
                    ModalidadeTurma = Modalidade.Fundamental,
                    NomeTurma = "5A",
                    AnoTurma = 5,
                    QuantidadeAbaixoMinimoFrequencia = 2,
                    QuantidadeAcimaMinimoFrequencia = 18,
                    TotalAulas = 10,
                    TotalFrequencias = 200,
                    VisaoDre = false,
                    UeId = 123,
                    AbreviacaoDre = "DRE01"
                },
                new FrequenciaGlobalMensalSemanalDto
                {
                    ModalidadeTurma = Modalidade.Fundamental,
                    NomeTurma = "6A",
                    AnoTurma = 6,
                    QuantidadeAbaixoMinimoFrequencia = 3,
                    QuantidadeAcimaMinimoFrequencia = 17,
                    TotalAulas = 12,
                    TotalFrequencias = 240,
                    VisaoDre = false,
                    UeId = 124,
                    AbreviacaoDre = "DRE02"
                }
            };
            var dataInicio = new DateTime(2025, 7, 1);
            var dataFim = new DateTime(2025, 7, 7);
            int? mes = null;
            var tipoConsolidadoFrequencia = TipoConsolidadoFrequencia.Semanal;

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(lista);

            // Act
            var resultado = await _useCase.Executar(frequenciaDto, dataInicio, dataFim, mes, tipoConsolidadoFrequencia);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.DadosFrequenciaDashboard);

            var grupo5A = resultado.DadosFrequenciaDashboard.Where(x => x.TurmaAno == "EF-5A").ToList();
            var grupo6A = resultado.DadosFrequenciaDashboard.Where(x => x.TurmaAno == "EF-6A").ToList();

            Assert.Contains(grupo5A, x => x.Descricao == DashboardFrequenciaConstants.QuantidadeAbaixoMinimoFrequenciaDescricao && x.Quantidade == 2);
            Assert.Contains(grupo5A, x => x.Descricao == DashboardFrequenciaConstants.QuantidadeAcimaMinimoFrequenciaDescricao && x.Quantidade == 18);
            Assert.Contains(grupo5A, x => x.Descricao == TipoFrequenciaDashboard.TotalEstudantes.Name() && x.Quantidade == 20);

            Assert.Contains(grupo6A, x => x.Descricao == DashboardFrequenciaConstants.QuantidadeAbaixoMinimoFrequenciaDescricao && x.Quantidade == 3);
            Assert.Contains(grupo6A, x => x.Descricao == DashboardFrequenciaConstants.QuantidadeAcimaMinimoFrequenciaDescricao && x.Quantidade == 17);
            Assert.Contains(grupo6A, x => x.Descricao == TipoFrequenciaDashboard.TotalEstudantes.Name() && x.Quantidade == 20);

            Assert.False(string.IsNullOrEmpty(resultado.TotalFrequenciaFormatado));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRetornarGraficoFrequenciaAlunoDto_QuandoMediatorRetornaDados_Mensal()
        {
            // Arrange
            var frequenciaDto = new FrequenciasConsolidadacaoPorTurmaEAnoDto
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 123,
                Modalidade = (int)Modalidade.EducacaoInfantil,
                AnoTurma = "Infantil",
                Semestre = 1,
                DataAula = new DateTime(2025, 7, 10),
                VisaoDre = false
            };
            var lista = new List<FrequenciaGlobalMensalSemanalDto>
            {
                new FrequenciaGlobalMensalSemanalDto
                {
                    ModalidadeTurma = Modalidade.EducacaoInfantil,
                    NomeTurma = "Infantil",
                    AnoTurma = 1,
                    QuantidadeAbaixoMinimoFrequencia = 1,
                    QuantidadeAcimaMinimoFrequencia = 19,
                    TotalAulas = 8,
                    TotalFrequencias = 160,
                    VisaoDre = false,
                    UeId = 123,
                    AbreviacaoDre = "DRE01"
                }
            };
            int mes = 7;
            var tipoConsolidadoFrequencia = TipoConsolidadoFrequencia.Mensal;

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(lista);

            // Act
            var resultado = await _useCase.Executar(frequenciaDto, null, null, mes, tipoConsolidadoFrequencia);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.DadosFrequenciaDashboard);

            var grupoInfantil = resultado.DadosFrequenciaDashboard.Where(x => x.TurmaAno == "EI-Infantil").ToList();

            Assert.Contains(grupoInfantil, x => x.Descricao == DashboardFrequenciaConstants.QuantidadeAbaixoMinimoFrequenciaDescricao && x.Quantidade == 1);
            Assert.Contains(grupoInfantil, x => x.Descricao == DashboardFrequenciaConstants.QuantidadeAcimaMinimoFrequenciaDescricao && x.Quantidade == 19);
            Assert.Contains(grupoInfantil, x => x.Descricao == TipoFrequenciaDashboard.TotalCriancas.Name() && x.Quantidade == 20);

            Assert.False(string.IsNullOrEmpty(resultado.TotalFrequenciaFormatado));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }


    }
}
