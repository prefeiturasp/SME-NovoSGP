using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.Linq;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCase _useCase;

        public ObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarNull_QuandoMediatorRetornaListaVazia()
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

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAlunoDashboardDto>());

            // Act
            var resultado = await _useCase.Executar(frequenciaDto);

            // Assert
            Assert.Null(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRetornarGraficoFrequenciaAlunoDto_QuandoMediatorRetornaDados()
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

            var dadosFrequenciaAlunos = new List<FrequenciaAlunoDashboardDto>
            {
                new FrequenciaAlunoDashboardDto
                {
                    Descricao = "5A",
                    DreCodigo = "DRE01",
                    TipoFrequenciaAluno = TipoFrequencia.C,
                    Presentes = 20,
                    Remotos = 5,
                    Ausentes = 3,
                    TotalAulas = 10,
                    TotalFrequencias = 28
                },
                new FrequenciaAlunoDashboardDto
                {
                    Descricao = "5A",
                    DreCodigo = "DRE01",
                    TipoFrequenciaAluno = TipoFrequencia.F,
                    Presentes = 10,
                    Remotos = 2,
                    Ausentes = 8,
                    TotalAulas = 5,
                    TotalFrequencias = 20
                },
                new FrequenciaAlunoDashboardDto
                {
                    Descricao = "6A",
                    DreCodigo = "DRE01",
                    TipoFrequenciaAluno = TipoFrequencia.R,
                    Presentes = 15,
                    Remotos = 7,
                    Ausentes = 4,
                    TotalAulas = 8,
                    TotalFrequencias = 26
                }
            };

            ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery queryCapturada = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<FrequenciaAlunoDashboardDto>>, CancellationToken>((query, token) =>
                {
                    queryCapturada = query as ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery;
                })
                .ReturnsAsync(dadosFrequenciaAlunos);

            // Act
            var resultado = await _useCase.Executar(frequenciaDto);

            // Assert
            Assert.NotNull(queryCapturada);            

            Assert.Equal(frequenciaDto.AnoLetivo, queryCapturada.AnoLetivo);
            Assert.Equal(frequenciaDto.DreId, queryCapturada.DreId);
            Assert.Equal(frequenciaDto.UeId, queryCapturada.UeId);
            Assert.Equal(frequenciaDto.Modalidade, queryCapturada.Modalidade);
            Assert.Equal(frequenciaDto.AnoTurma, queryCapturada.AnoTurma);
            Assert.Equal(frequenciaDto.Semestre, queryCapturada.Semestre);
            Assert.Equal(frequenciaDto.DataAula, queryCapturada.DataAula);
            Assert.Equal(frequenciaDto.VisaoDre, queryCapturada.VisaoDre);


            Assert.NotNull(resultado);
            Assert.NotNull(resultado.DadosFrequenciaDashboard);

            // Valida agrupamento e mapeamento
            var grupo5A = resultado.DadosFrequenciaDashboard.Where(x => x.TurmaAno == "5A").ToList();
            var grupo6A = resultado.DadosFrequenciaDashboard.Where(x => x.TurmaAno == "6A").ToList();

            // Para "5A"
            var totalPresentes5A = dadosFrequenciaAlunos.Where(x => x.Descricao == "5A").Sum(x => x.Presentes);
            var totalRemotos5A = dadosFrequenciaAlunos.Where(x => x.Descricao == "5A").Sum(x => x.Remotos);
            var totalAusentes5A = dadosFrequenciaAlunos.Where(x => x.Descricao == "5A").Sum(x => x.Ausentes);
            var total5A = totalPresentes5A + totalRemotos5A + totalAusentes5A;

            Assert.Contains(grupo5A, x => x.Descricao == TipoFrequenciaDashboard.Presentes.Name() && x.Quantidade == totalPresentes5A);
            Assert.Contains(grupo5A, x => x.Descricao == TipoFrequenciaDashboard.Remotos.Name() && x.Quantidade == totalRemotos5A);
            Assert.Contains(grupo5A, x => x.Descricao == TipoFrequenciaDashboard.Ausentes.Name() && x.Quantidade == totalAusentes5A);
            Assert.Contains(grupo5A, x => x.Descricao == TipoFrequenciaDashboard.TotalEstudantes.Name() && x.Quantidade == total5A);

            // Para "6A"
            var totalPresentes6A = dadosFrequenciaAlunos.Where(x => x.Descricao == "6A").Sum(x => x.Presentes);
            var totalRemotos6A = dadosFrequenciaAlunos.Where(x => x.Descricao == "6A").Sum(x => x.Remotos);
            var totalAusentes6A = dadosFrequenciaAlunos.Where(x => x.Descricao == "6A").Sum(x => x.Ausentes);
            var total6A = totalPresentes6A + totalRemotos6A + totalAusentes6A;

            Assert.Contains(grupo6A, x => x.Descricao == TipoFrequenciaDashboard.Presentes.Name() && x.Quantidade == totalPresentes6A);
            Assert.Contains(grupo6A, x => x.Descricao == TipoFrequenciaDashboard.Remotos.Name() && x.Quantidade == totalRemotos6A);
            Assert.Contains(grupo6A, x => x.Descricao == TipoFrequenciaDashboard.Ausentes.Name() && x.Quantidade == totalAusentes6A);
            Assert.Contains(grupo6A, x => x.Descricao == TipoFrequenciaDashboard.TotalEstudantes.Name() && x.Quantidade == total6A);

            // Valida totais
            var totalAulasEsperado = dadosFrequenciaAlunos.Sum(x => x.TotalAulas);
            var totalFrequenciasEsperado = dadosFrequenciaAlunos.Sum(x => x.TotalFrequencias);

            // O campo TotalFrequenciaFormatado depende da implementação de TotalFrequenciaEAulasPorPeriodoDto.TotalFrequenciaFormatado,
            // então aqui só valida que não é nulo (ou vazio) se houver dados.
            Assert.False(string.IsNullOrEmpty(resultado.TotalFrequenciaFormatado));

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
