using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste
{
    public class ObterFechamentoTurmaComPeriodoEscolarQueryTest
    {
        private readonly Mock<IRepositorioFechamentoTurmaConsulta> repositorioFechamentoTurma;
        private readonly Mock<IMediator> mediator;
        private readonly ObterFechamentoTurmaComPeriodoEscolarQueryHandler query;

        public ObterFechamentoTurmaComPeriodoEscolarQueryTest()
        {
            repositorioFechamentoTurma = new Mock<IRepositorioFechamentoTurmaConsulta>();
            mediator = new Mock<IMediator>();

            query = new ObterFechamentoTurmaComPeriodoEscolarQueryHandler(repositorioFechamentoTurma.Object, mediator.Object);
        }

        // TODO: Revisar esse teste
        [Fact]
        public async Task Turma_Sem_Fechamento()
        {
            //Arrange
            FechamentoTurmaPeriodoEscolarDto fechamento = null;
            repositorioFechamentoTurma.Setup(a => a.ObterIdEPeriodoPorTurmaBimestre(It.IsAny<long>(), It.IsAny<int?>())).ReturnsAsync(fechamento);

            //Act
            var retorno = await query.Handle(new ObterFechamentoTurmaComPeriodoEscolarQuery(123, 0), new System.Threading.CancellationToken());

            //Assert
            repositorioFechamentoTurma.Verify(c => c.ObterIdEPeriodoPorTurmaBimestre(It.IsAny<long>(), It.IsAny<int?>()), Times.Once);

            Assert.NotNull(retorno);
            Assert.Equal(0, retorno.FechamentoTurmaId);
            Assert.Equal(0, retorno.PeriodoEscolarId);
            Assert.False(retorno.PossuiAvaliacao);
        }

        [Fact]
        public async Task Turma_Com_Fechamento_Sem_Avaliacao()
        {
            //Arrange
            repositorioFechamentoTurma.Setup(a => a.ObterIdEPeriodoPorTurmaBimestre(It.IsAny<long>(), It.IsAny<int?>()))
                .ReturnsAsync(new FechamentoTurmaPeriodoEscolarDto()
                {
                    FechamentoTurmaId = 123,
                    PeriodoEscolarId = 111
                });

            mediator.Setup(a => a.Send(It.IsAny<TurmaPossuiAvaliacaoNoPeriodoQuery>(), new System.Threading.CancellationToken()))
                .ReturnsAsync(false);

            //Act
            var retorno = await query.Handle(new ObterFechamentoTurmaComPeriodoEscolarQuery(123, 1), new System.Threading.CancellationToken());

            //Assert
            repositorioFechamentoTurma.Verify(c => c.ObterIdEPeriodoPorTurmaBimestre(It.IsAny<long>(), It.IsAny<int?>()), Times.Once);
            mediator.Verify(c => c.Send(It.IsAny<TurmaPossuiAvaliacaoNoPeriodoQuery>(), new System.Threading.CancellationToken()), Times.Once);

            Assert.False(retorno is null, "Turma deve possuir fechamento");
            Assert.False(retorno.PossuiAvaliacao, "Turma não deve possuir avaliação no bimestre");
        }

        [Fact]
        public async Task Turma_Com_Fechamento_Com_Avaliacao()
        {
            //Arrange
            repositorioFechamentoTurma.Setup(a => a.ObterIdEPeriodoPorTurmaBimestre(It.IsAny<long>(), It.IsAny<int?>()))
                .ReturnsAsync(new FechamentoTurmaPeriodoEscolarDto()
                {
                    FechamentoTurmaId = 123,
                    PeriodoEscolarId = 111
                });

            mediator.Setup(a => a.Send(It.IsAny<TurmaPossuiAvaliacaoNoPeriodoQuery>(), new System.Threading.CancellationToken()))
                .ReturnsAsync(true);

            //Act
            var retorno = await query.Handle(new ObterFechamentoTurmaComPeriodoEscolarQuery(123, 1), new System.Threading.CancellationToken());

            //Assert
            repositorioFechamentoTurma.Verify(c => c.ObterIdEPeriodoPorTurmaBimestre(It.IsAny<long>(), It.IsAny<int?>()), Times.Once);
            mediator.Verify(c => c.Send(It.IsAny<TurmaPossuiAvaliacaoNoPeriodoQuery>(), new System.Threading.CancellationToken()), Times.Once);

            Assert.False(retorno is null, "Turma deve possuir fechamento");
            Assert.True(retorno.PossuiAvaliacao, "Turma deve possuir avaliação no bimestre");
        }

        [Fact]
        public async Task Turma_Com_Fechamento_Final()
        {
            //Arrange
            repositorioFechamentoTurma.Setup(a => a.ObterIdEPeriodoPorTurmaBimestre(It.IsAny<long>(), It.IsAny<int?>()))
                .ReturnsAsync(new FechamentoTurmaPeriodoEscolarDto()
                {
                    FechamentoTurmaId = 123,
                    PeriodoEscolarId = null
                });

            //Act
            var retorno = await query.Handle(new ObterFechamentoTurmaComPeriodoEscolarQuery(123, 0), new System.Threading.CancellationToken());

            //Assert
            repositorioFechamentoTurma.Verify(c => c.ObterIdEPeriodoPorTurmaBimestre(It.IsAny<long>(), It.IsAny<int?>()), Times.Once);
            mediator.Verify(c => c.Send(It.IsAny<TurmaPossuiAvaliacaoNoPeriodoQuery>(), new System.Threading.CancellationToken()), Times.Never, "Fechamento final não deve consultar avaliações");

            Assert.False(retorno is null, "Turma deve possuir fechamento final");
            Assert.False(retorno.PossuiAvaliacao, "Turma não deve possuir avaliação para consulta bimestre final");
        }
    }
}
