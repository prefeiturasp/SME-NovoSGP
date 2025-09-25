using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.Turma;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterTurmasPapPorAnoLetivoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IContextoAplicacao> contextoAplicacaoMock;
        private readonly ObterTurmasPapPorAnoLetivoUseCase useCase;

        public ObterTurmasPapPorAnoLetivoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            contextoAplicacaoMock = new Mock<IContextoAplicacao>();

            useCase = new ObterTurmasPapPorAnoLetivoUseCase(contextoAplicacaoMock.Object, mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Turmas_Pap()
        {
            var anoLetivo = 2025;
            var codigoUe = "123456";
            var turmasEsperadas = new List<TurmasPapDto>
            {
                new TurmasPapDto { CodigoTurma = "TURMA1", TurmaNome = "Turma 1" },
                new TurmasPapDto { CodigoTurma = "TURMA2", TurmaNome = "Turma 2" }
            };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<ObterTurmasPapPorAnoLetivoQuery>(q => q.AnoLetivo == anoLetivo && q.CodigoUe == codigoUe),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasEsperadas);

            var resultado = await useCase.Executar(anoLetivo, codigoUe);

            Assert.NotNull(resultado);
            var lista = new List<TurmasPapDto>(resultado);
            Assert.Equal(2, lista.Count);
            Assert.Equal("TURMA1", lista[0].CodigoTurma);
            Assert.Equal("TURMA2", lista[1].CodigoTurma);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {
            var anoLetivo = 2024;
            var codigoUe = "UE9876";

            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<ObterTurmasPapPorAnoLetivoQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TurmasPapDto>());

            await useCase.Executar(anoLetivo, codigoUe);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterTurmasPapPorAnoLetivoQuery>(
                    q => q.AnoLetivo == anoLetivo && q.CodigoUe == codigoUe),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
