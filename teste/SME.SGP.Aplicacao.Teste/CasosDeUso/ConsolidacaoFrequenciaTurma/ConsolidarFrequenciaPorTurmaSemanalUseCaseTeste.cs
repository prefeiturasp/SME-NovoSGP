using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurma
{
    public class ConsolidarFrequenciaPorTurmaSemanalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaPorTurmaSemanalUseCaseFake useCaseFake;

        public ConsolidarFrequenciaPorTurmaSemanalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCaseFake = new ConsolidarFrequenciaPorTurmaSemanalUseCaseFake(mediatorMock.Object);
        }

        [Fact]
        public async Task Obter_Frequencia_Consideradas_Retorna_Frequencia_Semanal_Correta()
        {
            string codigoTurma = "TURMA456";
            var data = new DateTime(2025, 7, 15); 

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "AL1", NomeAluno = "João" },
                new AlunoPorTurmaResposta { CodigoAluno = "AL2", NomeAluno = "Maria" }
            };

            var frequencias = new List<FrequenciaAlunoDto>
            {
                new FrequenciaAlunoDto { AlunoCodigo = "AL1", TotalAulas = 5, TotalPresencas = 4, TotalAusencias = 1, TotalRemotos = 0 },
                new FrequenciaAlunoDto { AlunoCodigo = "AL2", TotalAulas = 5, TotalPresencas = 3, TotalAusencias = 2, TotalRemotos = 0 }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFrequenciaPorTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequencias);

            useCaseFake.SetFiltro(new FiltroConsolidacaoFrequenciaTurma
            {
                Data = data,
                TurmaCodigo = codigoTurma
            });

            var resultado = await useCaseFake.ObterFrequenciaConsideradasPublic(codigoTurma);

            var lista = resultado.ToList();
            Assert.Equal(2, lista.Count);
            Assert.Contains(lista, f => f.AlunoCodigo == "AL1" && f.TotalPresencas == 4 && f.TotalAusencias == 1);
            Assert.Contains(lista, f => f.AlunoCodigo == "AL2" && f.TotalPresencas == 3 && f.TotalAusencias == 2);
        }
    }

    public class ConsolidarFrequenciaPorTurmaSemanalUseCaseFake : ConsolidarFrequenciaPorTurmaSemanalUseCase
    {
        public ConsolidarFrequenciaPorTurmaSemanalUseCaseFake(IMediator mediator) : base(mediator) { }

        public void SetFiltro(FiltroConsolidacaoFrequenciaTurma filtro)
        {
            base.Filtro = filtro;
        }

        public Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradasPublic(string codigoTurma)
        {
            return base.ObterFrequenciaConsideradas(codigoTurma);
        }
    }
}
