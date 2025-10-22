using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional.Frequencia;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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
    public class ConsolidarFrequenciaDiariaPainelEducacionalUseCaseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarFrequenciaDiariaPainelEducacionalUseCase _useCase;

        public ConsolidarFrequenciaDiariaPainelEducacionalUseCaseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarFrequenciaDiariaPainelEducacionalUseCase(_mediatorMock.Object);
        }

        private static IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto> ObterListaFrequenciaMock()
        {
            var dataAula = new DateTime(2025, 10, 22);
            return new List<DadosParaConsolidarFrequenciaDiariaAlunoDto>
            {
                new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "DRE-A", UeId = 1, TurmaId = 101, NomeTurma = "T1A", AnoLetivo = 2025, DataAula = dataAula, TotalPresentes = 95, TotalRemotos = 0, TotalAusentes = 5 }, // 95%
                new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "DRE-A", UeId = 1, TurmaId = 101, NomeTurma = "T1A", AnoLetivo = 2025, DataAula = dataAula, TotalPresentes = 95, TotalRemotos = 0, TotalAusentes = 5 }, // 95%

                new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "DRE-A", UeId = 1, TurmaId = 102, NomeTurma = "T2B", AnoLetivo = 2025, DataAula = dataAula, TotalPresentes = 88, TotalRemotos = 0, TotalAusentes = 12 }, // 88%

                new DadosParaConsolidarFrequenciaDiariaAlunoDto { CodigoDre = "DRE-B", UeId = 2, TurmaId = 201, NomeTurma = "T3C", AnoLetivo = 2025, DataAula = dataAula, TotalPresentes = 80, TotalRemotos = 0, TotalAusentes = 20 }, // 80%
            };
        }

        private static IEnumerable<Ue> ObterListaUeMock()
        {
            return new List<Ue>
            {
                new Ue { Id = 1, CodigoUe = "000001", Nome = "Escola A", TipoEscola = TipoEscola.EMEBS } ,
                new Ue { Id = 2, CodigoUe = "000002", Nome = "Escola B", TipoEscola = TipoEscola.EMEI }
            };
        }


        [Fact]
        public async Task Executar_DeveRetornarTrue_EChamarComandosCorretamente()
        {
            var listaFrequencia = ObterListaFrequenciaMock();
            var listaUe = ObterListaUeMock();
            var param = new MensagemRabbit();

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterFrequenciaDiariaQuery>(), default))
                .ReturnsAsync(listaFrequencia);

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterTodasUesQuery>(), default))
                .ReturnsAsync(listaUe);

            var resultado = await _useCase.Executar(param);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand>(
                    c => c.Indicadores.Count() == 3),
                default),
                Times.Once);

            _mediatorMock.Verify(m => m.Send(
               It.Is<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand>(
                   c => c.Indicadores.Count() == 2),
               default),
               Times.Once);
        }

        [Fact]
        public async Task Executar_ComFrequenciaVazia_DeveChamarComandosComListaVazia()
        {
            var listaFrequenciaVazia = new List<DadosParaConsolidarFrequenciaDiariaAlunoDto>();
            var listaUe = ObterListaUeMock();
            var param = new MensagemRabbit();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaDiariaQuery>(), default))
                .ReturnsAsync(listaFrequenciaVazia);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesQuery>(), default))
                .ReturnsAsync(listaUe);

            var resultado = await _useCase.Executar(param);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand>(
                    c => !c.Indicadores.Any()),
                default),
                Times.Once);

            _mediatorMock.Verify(m => m.Send(
                It.Is<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand>(
                    c => !c.Indicadores.Any()),
                default),
                Times.Once);
        }

        [Theory]
        [InlineData(84.99, NivelFrequenciaEnum.Baixa)]
        [InlineData(0, NivelFrequenciaEnum.Baixa)]
        [InlineData(85.00, NivelFrequenciaEnum.Media)]
        [InlineData(89.99, NivelFrequenciaEnum.Media)]
        [InlineData(90.00, NivelFrequenciaEnum.Alta)]
        [InlineData(100.00, NivelFrequenciaEnum.Alta)]
        public void ObterNivelFrequencia_DeveRetornarNivelCorreto(decimal percentual, NivelFrequenciaEnum esperado)
        {
            var resultado = ConsolidarFrequenciaDiariaPainelEducacionalUseCase.ObterNivelFrequencia(percentual);

            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public async Task Executar_DeveCalcularCorretamenteConsolidacaoTurma()
        {
            // Arrange
            var listaFrequencia = ObterListaFrequenciaMock();
            var listaUe = ObterListaUeMock();
            var param = new MensagemRabbit();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaDiariaQuery>(), default))
                .ReturnsAsync(listaFrequencia);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesQuery>(), default))
                .ReturnsAsync(listaUe);

            SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand commandTurmaCapturado = null;
            _mediatorMock.Setup(m => m.Send(
                It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand>(), default))
                .Callback<IRequest<bool>, CancellationToken>((req, token) =>
                {
                    commandTurmaCapturado = (SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand)req;
                })
                .ReturnsAsync(true);

            // Act
            await _useCase.Executar(param);

            // Assert
            var consolidacaoT1A = commandTurmaCapturado.Indicadores.First(x => x.Turma == "T1A");

            Assert.Equal(1, listaUe.First(u => u.CodigoUe == consolidacaoT1A.CodigoUe).Id);
            Assert.Equal(200, consolidacaoT1A.TotalEstudantes);
            Assert.Equal(190, consolidacaoT1A.TotalPresentes);
            Assert.Equal(95m, consolidacaoT1A.PercentualFrequencia); 
            Assert.Equal(NivelFrequenciaEnum.Alta, consolidacaoT1A.NivelFrequencia);
            Assert.Equal("T1A", consolidacaoT1A.Turma);
        }

        [Fact]
        public async Task Executar_DeveCalcularCorretamenteConsolidacaoDre()
        {
            var listaFrequencia = ObterListaFrequenciaMock();
            var listaUe = ObterListaUeMock();
            var param = new MensagemRabbit();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaDiariaQuery>(), default))
                .ReturnsAsync(listaFrequencia);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesQuery>(), default))
                .ReturnsAsync(listaUe);

            SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand commandDreCapturado = null;
            _mediatorMock.Setup(m => m.Send(
                It.IsAny<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand>(), default))
                .Callback<IRequest<bool>, CancellationToken>((req, token) =>
                {
                    commandDreCapturado = (SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand)req;
                })
                .ReturnsAsync(true);

            await _useCase.Executar(param);

            var consolidacaoUE1 = commandDreCapturado.Indicadores.First(x => x.CodigoUe == "000001");

            Assert.Equal("DRE-A", consolidacaoUE1.CodigoDre);
            Assert.Equal(300, consolidacaoUE1.TotalEstudantes);
            Assert.Equal(278, consolidacaoUE1.TotalPresentes);
            Assert.Equal(92.66666666666666666666666667m, consolidacaoUE1.PercentualFrequencia);
            Assert.Equal(NivelFrequenciaEnum.Alta, consolidacaoUE1.NivelFrequencia);

            var consolidacaoUE2 = commandDreCapturado.Indicadores.First(x => x.CodigoUe == "000002");

            Assert.Equal("DRE-B", consolidacaoUE2.CodigoDre);
            Assert.Equal(100, consolidacaoUE2.TotalEstudantes);
            Assert.Equal(80, consolidacaoUE2.TotalPresentes);
            Assert.Equal(80m, consolidacaoUE2.PercentualFrequencia);
            Assert.Equal(NivelFrequenciaEnum.Baixa, consolidacaoUE2.NivelFrequencia);
        }
    }
}