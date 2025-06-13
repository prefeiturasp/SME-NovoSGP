using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterPeriodoEscolarPorTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterPeriodoEscolarPorTurmaUseCase useCase;

        public ObterPeriodoEscolarPorTurmaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterPeriodoEscolarPorTurmaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Turma_Nao_For_Encontrada()
        {
            long turmaId = 1;
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Dominio.Turma)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(turmaId));
            Assert.Contains("não localizada", ex.Message);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Periodos()
        {
            var turma = new Dominio.Turma { AnoLetivo = 2025, Semestre = 1 };

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediator.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<Dominio.PeriodoEscolar>());

            var resultado = await useCase.Executar(1);

            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_De_Periodos_Convertidos_Para_Dto()
        {
            var turma = new Dominio.Turma { AnoLetivo = DateTime.Today.Year, Semestre = 1 };

            var periodos = new List<Dominio.PeriodoEscolar>
        {
            new Dominio.PeriodoEscolar { Id = 1, Bimestre = 1, TipoCalendarioId = 99, Migrado = false },
            new Dominio.PeriodoEscolar { Id = 2, Bimestre = 2, TipoCalendarioId = 99, Migrado = true }
        };

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediator.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(periodos);

            mediator.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(1);

            Assert.Equal(2, resultado.Count());
            Assert.All(resultado, r => Assert.True(r.PeriodoAberto));
        }

        [Fact]
        public void Deve_Filtrar_Periodos_Quando_Houver_Mais_De_Um_TipoCalendario()
        {
            var periodos = new List<Dominio.PeriodoEscolar>
        {
            new Dominio.PeriodoEscolar { Id = 1, TipoCalendarioId = 1 },
            new Dominio.PeriodoEscolar { Id = 2, TipoCalendarioId = 2 },
            new Dominio.PeriodoEscolar { Id = 3, TipoCalendarioId = 2 }
        };

            var resultado = useCase.FiltrarPeriodosCorretos(periodos);

            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, p => Assert.Equal(2, p.TipoCalendarioId));
        }
    }
}
