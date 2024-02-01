using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;

        public ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCaseTeste()
        {
            this.mediator = new Mock<IMediator>();
        }

        //[Fact(DisplayName = "Frequência - DeveFrequenciasPorBimestreAlunoTurmaComponenteCurricularComTerritorioAtrelado")]
        public async Task DeveFrequenciasPorBimestreAlunoTurmaComponenteCurricularComTerritorioAtrelado()
        {
            var frequencias = new List<FrequenciaAluno>()
            {
                new FrequenciaAluno()
                {
                    Bimestre = 1,
                    CodigoAluno = "1",
                    DisciplinaId = "2",
                    TurmaId = "1"
                }
            };

            mediator.Setup(x => x.Send(It.Is<ObterTurmaPorCodigoQuery>(x => x.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1" });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.Is<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery>(x => x.ComponenteCurricularId == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<FrequenciaAluno>);

            mediator.Setup(x => x.Send(It.Is<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery>(x => x.ComponenteCurricularId == "2"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequencias);

            var useCase = new ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase(mediator.Object);

            var resultado = await useCase.Executar(new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto("1", "1", new int[] { 1 }, "1"));

            Assert.Equal(frequencias.Count, resultado.Count());
            Assert.Equal(frequencias.First().Bimestre, resultado.First().Bimestre);
            Assert.Equal(frequencias.First().CodigoAluno, resultado.First().CodigoAluno);
            Assert.Equal(frequencias.First().DisciplinaId, resultado.First().DisciplinaId);
            Assert.Equal(frequencias.First().TurmaId, resultado.First().TurmaId);
        }
    }
}
