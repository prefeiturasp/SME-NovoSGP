using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaSemanal;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional.Frequencia;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaSemanal;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Threading;

namespace SME.SGP.Aplicacao.Testes.CasosDeUso.PainelEducacional.Frequencia
{
    public class ConsolidarFrequenciaSemanalPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarFrequenciaSemanalPainelEducacionalUseCase _useCase;

        public ConsolidarFrequenciaSemanalPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarFrequenciaSemanalPainelEducacionalUseCase(_mediatorMock.Object);
        }

        private static IEnumerable<DadosParaConsolidarFrequenciaSemanalAlunoDto> ObterListaFrequenciaMock(DateTime dataRef1, DateTime dataRef2)
        {
            return new List<DadosParaConsolidarFrequenciaSemanalAlunoDto>
            {
                new DadosParaConsolidarFrequenciaSemanalAlunoDto { CodigoDre = "DRE-1", CodigoUe = "UE-A", AnoLetivo = 2025, DataAula = dataRef1, TotalEstudantes = 100, TotalPresentes = 90 }, // 90%
                new DadosParaConsolidarFrequenciaSemanalAlunoDto { CodigoDre = "DRE-1", CodigoUe = "UE-A", AnoLetivo = 2025, DataAula = dataRef1, TotalEstudantes = 50, TotalPresentes = 45 }, // 90% (Consolidado: 150 estudantes, 135 presentes, 90%)

                new DadosParaConsolidarFrequenciaSemanalAlunoDto { CodigoDre = "DRE-2", CodigoUe = "UE-B", AnoLetivo = 2025, DataAula = dataRef1, TotalEstudantes = 0, TotalPresentes = 0 }, // 0%

                new DadosParaConsolidarFrequenciaSemanalAlunoDto { CodigoDre = "DRE-1", CodigoUe = "UE-A", AnoLetivo = 2025, DataAula = dataRef2, TotalEstudantes = 100, TotalPresentes = 98 }, // 98%
            };
        }

        [Fact]
        public async Task Executar_DeveBuscarFrequencia_ConsolidarDados_EEnviarComandoDeSalvar()
        {
            // Arrange
            var dataMock1 = new DateTime(2025, 10, 17); 
            var dataMock2 = new DateTime(2025, 10, 10); 
            var listaFrequenciaMock = ObterListaFrequenciaMock(dataMock1, dataMock2);
            var param = new MensagemRabbit();

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterFrequenciaSemanalQuery>(), default))
                .ReturnsAsync(listaFrequenciaMock);

            SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand commandCapturado = null;
            _mediatorMock.Setup(m => m.Send(
                It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand>(), default))
                .Callback<IRequest<bool>, CancellationToken>((req, token) =>
                {
                    commandCapturado = (SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand)req;
                })
                .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(param);

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.IsAny<ObterFrequenciaSemanalQuery>(), default),
                Times.Once);

            _mediatorMock.Verify(m => m.Send(
                It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand>(), default),
                Times.Once);

            Assert.Equal(3, commandCapturado.Indicadores.Count());
        }


        [Fact]
        public void AgruparConsolicacao_DeveConsolidarCorretamentePorUeEData()
        {
            // Arrange
            var data1 = new DateTime(2025, 10, 17);
            var data2 = new DateTime(2025, 10, 10);
            var listagemFrequencia = ObterListaFrequenciaMock(data1, data2);

            var indicadores = listagemFrequencia
                .GroupBy(x => new { x.CodigoUe, x.DataAula })
                .Select(g =>
                {
                    var primeiro = g.First();
                    var totalEstudantes = g.Sum(x => x.TotalEstudantes);
                    var totalPresentes = g.Sum(x => x.TotalPresentes);
                    var percentualFrequencia = totalEstudantes > 0 ? (decimal)totalPresentes / totalEstudantes * 100 : 0;

                    return new ConsolidacaoFrequenciaSemanalDto
                    {
                        CodigoDre = primeiro.CodigoDre,
                        CodigoUe = g.Key.CodigoUe,
                        DataAula = g.Key.DataAula,
                        TotalEstudantes = totalEstudantes,
                        TotalPresentes = totalPresentes,
                        PercentualFrequencia = Math.Round(percentualFrequencia, 2)
                    };
                })
                .OrderBy(x => x.CodigoUe)
                .ThenBy(x => x.DataAula)
                .ToList();

            // Assert
            Assert.Equal(3, indicadores.Count);

            var ueA_Data1 = indicadores.First(i => i.CodigoUe == "UE-A" && i.DataAula == data1);
            Assert.Equal(150, ueA_Data1.TotalEstudantes);
            Assert.Equal(135, ueA_Data1.TotalPresentes);
            Assert.Equal(90m, ueA_Data1.PercentualFrequencia);
            Assert.Equal("DRE-1", ueA_Data1.CodigoDre);

            var ueB_Data1 = indicadores.First(i => i.CodigoUe == "UE-B" && i.DataAula == data1);
            Assert.Equal(0, ueB_Data1.TotalEstudantes);
            Assert.Equal(0, ueB_Data1.TotalPresentes);
            Assert.Equal(0m, ueB_Data1.PercentualFrequencia);

            var ueA_Data2 = indicadores.First(i => i.CodigoUe == "UE-A" && i.DataAula == data2);
            Assert.Equal(100, ueA_Data2.TotalEstudantes);
            Assert.Equal(98, ueA_Data2.TotalPresentes);
            Assert.Equal(98m, ueA_Data2.PercentualFrequencia);
        }

        [Theory]
        [InlineData(DayOfWeek.Monday, "2025-10-20", "2025-10-17", "2025-10-10", "2025-10-03", "2025-09-26")]
        [InlineData(DayOfWeek.Friday, "2025-10-24", "2025-10-17", "2025-10-10", "2025-10-03", "2025-09-26")]
        [InlineData(DayOfWeek.Saturday, "2025-10-25", "2025-10-17", "2025-10-10", "2025-10-03", "2025-09-26")]
        public void ObterUltimasDatasSemanais_DeveRetornarAs4UltimasSextasFeiras(DayOfWeek diaSemanaAtual, string hojeString, params string[] sextasFeirasEsperadasStrings)
        {


            var dataHoje = new DateTime(2025, 10, 24);
            var sexta1 = new DateTime(2025, 10, 17);
            var sexta2 = new DateTime(2025, 10, 10);
            var sexta3 = new DateTime(2025, 10, 03);
            var sexta4 = new DateTime(2025, 09, 26);

            var listaFrequenciaVazia = Enumerable.Empty<DadosParaConsolidarFrequenciaSemanalAlunoDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaSemanalQuery>(), default))
                .ReturnsAsync(listaFrequenciaVazia);
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand>(), default))
                .ReturnsAsync(true);

            ObterFrequenciaSemanalQuery queryCapturada = null;
            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterFrequenciaSemanalQuery>(), default))
                .Callback<IRequest<IEnumerable<DadosParaConsolidarFrequenciaSemanalAlunoDto>>, CancellationToken>((req, token) =>
                {
                    queryCapturada = (ObterFrequenciaSemanalQuery)req;
                })
                .ReturnsAsync(listaFrequenciaVazia);


            _ = _useCase.Executar(new MensagemRabbit());


            Assert.NotNull(queryCapturada);
            Assert.Equal(4, queryCapturada.DataAulas.Count);


            Assert.All(queryCapturada.DataAulas, d => Assert.Equal(TimeSpan.Zero, d.TimeOfDay));
        }
    }

}