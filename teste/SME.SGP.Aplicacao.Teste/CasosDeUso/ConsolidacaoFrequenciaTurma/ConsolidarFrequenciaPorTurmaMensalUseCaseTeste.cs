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
    public class ConsolidarFrequenciaPorTurmaMensalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaPorTurmaMensalUseCaseFake useCaseFake;

        public ConsolidarFrequenciaPorTurmaMensalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCaseFake = new ConsolidarFrequenciaPorTurmaMensalUseCaseFake(mediatorMock.Object);
        }

        [Fact]
        public async Task Obter_Frequencia_Consideradas_Retorna_Frequencia_Correta()
        {
            string codigoTurma = "T123";

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "A1", NomeAluno = "Aluno 1" },
                new AlunoPorTurmaResposta { CodigoAluno = "A2", NomeAluno = "Aluno 2" }
            };

            var frequencias = new List<FrequenciaAlunoDto>
            {
                new FrequenciaAlunoDto { AlunoCodigo = "A1", TotalAulas = 20, TotalAusencias = 1, TotalPresencas = 19, TotalRemotos = 0 },
                new FrequenciaAlunoDto { AlunoCodigo = "A2", TotalAulas = 20, TotalAusencias = 3, TotalPresencas = 17, TotalRemotos = 0 }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterAlunosDentroPeriodoQuery>(q => q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterFrequenciaPorTurmaPeriodoQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequencias);

            useCaseFake.SetFiltro(new FiltroConsolidacaoFrequenciaTurma
            {
                Data = new DateTime(2025, 7, 15),
                TurmaCodigo = codigoTurma
            });

            var resultado = await useCaseFake.ObterFrequenciaConsideradasPublic(codigoTurma);

            var lista = resultado.ToList();
            Assert.Equal(2, lista.Count);
            Assert.Contains(lista, f => f.AlunoCodigo == "A1" && f.TotalAusencias == 1 && f.TotalPresencas == 19);
            Assert.Contains(lista, f => f.AlunoCodigo == "A2" && f.TotalAusencias == 3 && f.TotalPresencas == 17);
        }
    }

    public class ConsolidarFrequenciaPorTurmaMensalUseCaseFake : ConsolidarFrequenciaPorTurmaMensalUseCase
    {
        public ConsolidarFrequenciaPorTurmaMensalUseCaseFake(IMediator mediator)
            : base(mediator)
        {
        }

        public Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradasPublic(string codigoTurma)
        {
            return base.ObterFrequenciaConsideradas(codigoTurma);
        }

        public void SetFiltro(FiltroConsolidacaoFrequenciaTurma filtro)
        {
            base.Filtro = filtro;
        }
    }
}
