using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterPlanosAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterPlanosAEEUseCase useCase;

        public ObterPlanosAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterPlanosAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Query_Correta()
        {
            // Arrange
            var filtro = new FiltroPlanosAEEDto
            {
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                AlunoCodigo = "1",
                Situacao = Dominio.Enumerados.SituacaoPlanoAEE.AtribuicaoPAAI
            };

            var resultadoEsperado = new PaginacaoResultadoDto<PlanoAEEResumoDto>
            {
                Items = new[]
                {
                    new PlanoAEEResumoDto { Id = 1, CodigoAluno = "1" }
                },
                TotalRegistros = 1,
                TotalPaginas = 1
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanosAEEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);

            mediator.Verify(x => x.Send(
                It.Is<ObterPlanosAEEQuery>(q =>
                    q.DreId == filtro.DreId &&
                    q.UeId == filtro.UeId &&
                    q.TurmaId == filtro.TurmaId &&
                    q.AlunoCodigo == filtro.AlunoCodigo &&
                    q.Situacao == filtro.Situacao),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

