using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste
{
    public class ObterDatasAulasPorTurmaEComponenteUseCaseTeste
    {
        private readonly ObterDatasAulasPorTurmaEComponenteUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public ObterDatasAulasPorTurmaEComponenteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDatasAulasPorTurmaEComponenteUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Consultar_Datas_Aulas_Por_Turma_E_Componente()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterDatasAulasPorProfessorEComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DatasAulasDto>());

            // Act
            var datasAulas = await useCase.Executar(new ConsultaDatasAulasDto("123", "1105"));

            // Assert
            Assert.NotNull(datasAulas);
        }
    }
}
