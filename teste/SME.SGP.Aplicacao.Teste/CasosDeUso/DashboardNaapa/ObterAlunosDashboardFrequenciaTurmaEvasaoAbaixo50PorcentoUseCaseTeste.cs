using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardNaapa
{
    public class ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase useCase;

        public ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Retornar_Resultados()
        {
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto
            {
                AnoLetivo = 2025,
                DreCodigo = "D1",
                UeCodigo = "U1",
                TurmaCodigo = "T1",
                Modalidade = Modalidade.EducacaoInfantil,
                Semestre = 1,
                Mes = 10
            };

            var resultadoEsperado = new PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>
            {
                Items = new List<AlunoFrequenciaTurmaEvasaoDto>
                {
                    new AlunoFrequenciaTurmaEvasaoDto
                    {
                        Dre = "D1",
                        Ue = "U1",
                        Turma = "T1",
                        Aluno = "Aluno 1",
                        PercentualFrequencia = 45.2
                    }
                },
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            mediatorMock.Verify(m => m.Send(It.Is<ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>(q =>
                q.FiltroAbrangencia.AnoLetivo == filtro.AnoLetivo &&
                q.FiltroAbrangencia.DreCodigo == filtro.DreCodigo &&
                q.FiltroAbrangencia.UeCodigo == filtro.UeCodigo &&
                q.FiltroAbrangencia.TurmaCodigo == filtro.TurmaCodigo &&
                q.FiltroAbrangencia.Modalidade == filtro.Modalidade &&
                q.FiltroAbrangencia.Semestre == filtro.Semestre &&
                q.Mes == filtro.Mes
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retornar_Vazio_Deve_Retornar_Resultado_Vazio()
        {
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil,
                Mes = 8
            };

            var resultadoEsperado = new PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>
            {
                Items = new List<AlunoFrequenciaTurmaEvasaoDto>(),
                TotalPaginas = 0,
                TotalRegistros = 0
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.Items);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_Excecao()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase(null));
        }
    }
}
