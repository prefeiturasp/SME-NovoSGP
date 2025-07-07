using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.Turma;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterPeriodoLetivoTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterPeriodoLetivoTurmaUseCase useCase;

        public ObterPeriodoLetivoTurmaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterPeriodoLetivoTurmaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Periodo_Letivo_Correto_Quando_Turma_E_Periodos_Forem_Encontrados()
        {
            var codigoTurma = "123456";

            var turma = new Turma
            {
                CodigoTurma = codigoTurma,
                ModalidadeCodigo = Modalidade.Medio,
                AnoLetivo = 2025,
                Semestre = 1
            };

            var periodos = new List<PeriodoEscolar>
            {
                new PeriodoEscolar { Bimestre = 1, PeriodoInicio = new DateTime(2025, 2, 1), PeriodoFim = new DateTime(2025, 3, 31) },
                new PeriodoEscolar { Bimestre = 2, PeriodoInicio = new DateTime(2025, 4, 1), PeriodoFim = new DateTime(2025, 6, 30) }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodos);

            var resultado = await useCase.Executar(codigoTurma);

            Assert.NotNull(resultado);
            Assert.Equal(codigoTurma, resultado.TurmaCodigo);
            Assert.Equal(new DateTime(2025, 2, 1), resultado.PeriodoInicio);
            Assert.Equal(new DateTime(2025, 6, 30), resultado.PeriodoFim);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Turma_Nao_For_Encontrada()
        {
            var codigoTurma = "TURMA_INEXISTENTE";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(codigoTurma));
            Assert.Equal($"A turma com o código {codigoTurma} não foi localizada.", ex.Message);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Null_Quando_Nao_Houver_Periodos_Escolares()
        {
            var codigoTurma = "TURMA02";
            var turma = new Turma
            {
                CodigoTurma = codigoTurma,
                ModalidadeCodigo = Modalidade.Medio,
                AnoLetivo = 2025,
                Semestre = 1
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>());

            var resultado = await useCase.Executar(codigoTurma);

            Assert.Null(resultado);
        }
    }
}
