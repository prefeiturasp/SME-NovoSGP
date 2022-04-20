using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.ConselhoClasse;
using SME.SGP.Aplicacao.Queries.ConselhoClasse.ObterTotalAlunosSemFrequenciaPorTurmaQuery;
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
    public class ObterTotalAulasSemFrequenciaPorTurmaUseCaseTeste
    {
        private readonly ObterTotalAulasSemFrequenciaPorTurmaUseCase useCase;
        private readonly Mock<IMediator> mediator;
        public ObterTotalAulasSemFrequenciaPorTurmaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterTotalAulasSemFrequenciaPorTurmaUseCase(mediator.Object);
        }
        [Fact]
        public async Task Deve_Obter_Total_Aulas_SemFrequencia_Por_Turma()
        {
            //Arrange
            var codigoTurma = "2370993";

            mediator.Setup(x => x.Send(It.IsAny<ObterTotalAulasSemFrequenciaPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TotalAulasPorAlunoTurmaDto>());

            //Act
            var totalAulasSemFrequencia = await useCase.Executar(codigoTurma);

            //Assert
            Assert.NotNull(totalAulasSemFrequencia);

        }
    }
}
