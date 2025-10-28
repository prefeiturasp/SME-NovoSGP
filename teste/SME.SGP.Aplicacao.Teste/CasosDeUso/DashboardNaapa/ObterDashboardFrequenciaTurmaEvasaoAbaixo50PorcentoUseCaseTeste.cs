using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardNaapa
{
    public class ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase useCase;

        public ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Retornar_Resultado_Teste()
        {
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto
            {
                AnoLetivo = 2025,
                DreCodigo = "D1",
                UeCodigo = "U1",
                Modalidade = Modalidade.Fundamental,
                Semestre = 1,
                Mes = 8
            };

            var esperado = new FrequenciaTurmaEvasaoDto
            {
                TotalEstudantes = 10,
                GraficosFrequencia = new List<GraficoFrequenciaTurmaEvasaoDto>
                {
                    new GraficoFrequenciaTurmaEvasaoDto
                    {
                        Grupo = "G1",
                        Quantidade = 5,
                        Descricao = "Teste",
                        DreCodigo = "D1",
                        UeCodigo = "U1",
                        TurmaCodigo = "T1"
                    }
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(esperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(esperado.TotalEstudantes, resultado.TotalEstudantes);
            Assert.Single(resultado.GraficosFrequencia);
            mediatorMock.Verify(x => x.Send(It.Is<ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>(q =>
                q.AnoLetivo == filtro.AnoLetivo &&
                q.DreCodigo == filtro.DreCodigo &&
                q.UeCodigo == filtro.UeCodigo &&
                q.Modalidade == filtro.Modalidade &&
                q.Semestre == filtro.Semestre &&
                q.Mes == filtro.Mes
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Vazio_Deve_Retornar_Objeto_Vazio_Teste()
        {
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil
            };

            var esperado = new FrequenciaTurmaEvasaoDto
            {
                TotalEstudantes = 0,
                GraficosFrequencia = new List<GraficoFrequenciaTurmaEvasaoDto>()
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(esperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.GraficosFrequencia);
            Assert.Equal(0, resultado.TotalEstudantes);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Executar_Quando_Mediator_Nulo_Deve_Lancar_Excecao_Teste()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase(null));
        }
    }
}
