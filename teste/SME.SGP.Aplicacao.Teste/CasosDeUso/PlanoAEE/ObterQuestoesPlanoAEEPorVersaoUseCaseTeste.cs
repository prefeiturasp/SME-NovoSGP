using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterQuestoesPlanoAEEPorVersaoUseCaseTeste
    {
        [Fact]
        public async Task Nao_Deve_Buscar_Informacoes_Srm_Ao_Consultar_Versao_Historica()
        {
            // Arrange
            var mediator = new Mock<IMediator>();
            var useCase = new ObterQuestoesPlanoAEEPorVersaoUseCase(mediator.Object);
            var filtro = new FiltroPesquisaQuestoesPlanoAEEDto(1, 2, "3");
            var questoes = new List<QuestaoDto> { new QuestaoDto { Id = 4 } };

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestoesPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.Same(questoes, resultado);
            mediator.Verify(x => x.Send(
                It.IsAny<ObterDadosSrmPaeeColaborativoEolQuery>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
