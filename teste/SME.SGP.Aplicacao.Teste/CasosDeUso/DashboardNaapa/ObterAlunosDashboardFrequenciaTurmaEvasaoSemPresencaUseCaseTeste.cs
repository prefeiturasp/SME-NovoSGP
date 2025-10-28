using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardNaapa
{
    public class ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase useCase;

        public ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Retornar_Resultado_Teste()
        {
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto
            {
                AnoLetivo = 2025,
                DreCodigo = "D1",
                UeCodigo = "U1",
                TurmaCodigo = "T1",
                Modalidade = Modalidade.EducacaoInfantil,
                Semestre = 1,
                Mes = 9
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
                        PercentualFrequencia = 0
                    }
                },
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            mediatorMock.Verify(m => m.Send(It.Is<ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQuery>(q =>
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
        public async Task Executar_Quando_Mediator_Retornar_Vazio_Deve_Retornar_Resultado_Vazio_Teste()
        {
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil,
                Mes = 5
            };

            var resultadoEsperado = new PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>
            {
                Items = new List<AlunoFrequenciaTurmaEvasaoDto>(),
                TotalPaginas = 0,
                TotalRegistros = 0
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.Items);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Executar_Quando_Mediator_Nulo_Deve_Lancar_Excecao_Teste()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase(null));
        }
    }
}
