using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterQuestoesPlanoAEEPorVersaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterQuestoesPlanoAEEPorVersaoUseCase useCase;

        public ObterQuestoesPlanoAEEPorVersaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterQuestoesPlanoAEEPorVersaoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Questoes_Do_Plano_AEE()
        {
            // Arrange
            var filtro = CriarFiltro();

            var questoesEsperadas = new List<QuestaoDto>
            {
                new QuestaoDto { Id = 1, Nome = "Questão 1" },
                new QuestaoDto { Id = 2, Nome = "Questão 2" }
            };

            mediator.Setup(x => x.Send(
                It.Is<ObterQuestoesPlanoAEEPorVersaoQuery>(q =>
                    q.QuestionarioId == filtro.QuestionarioId &&
                    q.VersaoPlanoId == filtro.VersaoPlanoId &&
                    q.TurmaCodigo == filtro.TurmaCodigo),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoesEsperadas);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Questão 1", resultado.First().Nome);
            Assert.Equal("Questão 2", resultado.Last().Nome);
        }

        [Fact]
        public async Task Lista_Vazia_Deve_Retornar_Exception()
        {
            // Arrange
            mediator.Setup(x => x.Send(
                It.Is<ObterQuestoesPlanoAEEPorVersaoQuery>(q =>
                    q.QuestionarioId == 0 &&
                    q.VersaoPlanoId == 0 &&
                    q.TurmaCodigo == null),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<QuestaoDto>());

            // Assert & Act
            await Assert.ThrowsAsync<NullReferenceException>(() => useCase.Executar(null));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {
            // Arrange
            var filtro = CriarFiltro();

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestoesPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<QuestaoDto>());

            // Act
            await useCase.Executar(filtro);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<ObterQuestoesPlanoAEEPorVersaoQuery>(q =>
                    q.QuestionarioId == filtro.QuestionarioId &&
                    q.VersaoPlanoId == filtro.VersaoPlanoId &&
                    q.TurmaCodigo == filtro.TurmaCodigo),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        private static FiltroPesquisaQuestoesPlanoAEEDto CriarFiltro(long questionarioId = 1, long versaoPlanoId = 1, string turmaCodigo = "1")
           => new FiltroPesquisaQuestoesPlanoAEEDto(questionarioId, versaoPlanoId, turmaCodigo);
    }
}