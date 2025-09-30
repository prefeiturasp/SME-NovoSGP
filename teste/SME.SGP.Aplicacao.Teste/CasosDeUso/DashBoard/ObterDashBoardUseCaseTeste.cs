using MediatR;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashBoard
{
    public class ObterDashBoardUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDashBoardUseCase _useCase;

        public ObterDashBoardUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDashBoardUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Query_E_Retornar_Dados_Do_Dashboard()
        {
            var dashboardData = new List<SME.SGP.Dominio.DashBoard>
            {
                new SME.SGP.Dominio.DashBoard { Descricao = "Dashboard Teste", Rota = "/teste" }
            };

            _mediatorMock.Setup(m => m.Send(ObterDashBoardPorPerfilQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dashboardData);

            var resultado = await _useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(dashboardData.First().Descricao, resultado.First().Descricao);
            _mediatorMock.Verify(m => m.Send(ObterDashBoardPorPerfilQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
