using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.ConselhoClasse;
using SME.SGP.Aplicacao.Queries.ConselhoClasse.ObterTotalAulasPorAlunoTurmaQuery;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhodeClasse
{
    public class ObterTotalAulasPorAlunoTurmaUseCaseTeste
    {
        private readonly ObterTotalAulasPorAlunoTurmaUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public ObterTotalAulasPorAlunoTurmaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterTotalAulasPorAlunoTurmaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Total_Aulas_Por_AlunoTurma()
        {
            //Arrange
            var codigoAluno = "5854736";
            var codigoTurma = "2370993";

            mediator.Setup(x => x.Send(It.IsAny<ObterTotalAulasPorAlunoTurmaQuery>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(new List<TotalAulasPorAlunoTurmaDto>());

            //Act
            var totalAulas = await useCase.Executar(codigoAluno, codigoTurma);

            //Assert
            Assert.NotNull(totalAulas);

        }
    }
}
