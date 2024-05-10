using MediatR;
using Moq;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class AlterarCartaIntencoesObservacaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly AlterarCartaIntencoesObservacaoUseCase useCase;

        public AlterarCartaIntencoesObservacaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new AlterarCartaIntencoesObservacaoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Alterar_Observacao_Carta_de_Intencoes()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<AlterarCartaIntencoesObservacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto()
                {
                    Id = 1
                });

            var dto = new AlterarCartaIntencoesObservacaoDto();
            dto.Observacao = "observacao";
            //Act
            var auditoriaDto = await useCase.Executar(1, dto);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<AlterarCartaIntencoesObservacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }
    }
}
