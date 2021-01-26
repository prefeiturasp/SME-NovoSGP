using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class AdicionarObservacaoDiarioBordoUseCaseTeste
    {
        private readonly AdicionarObservacaoDiarioBordoUseCase adicionarObservacaoDiarioBordoUseCase;
        private readonly Mock<IMediator> mediator;

        public AdicionarObservacaoDiarioBordoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            adicionarObservacaoDiarioBordoUseCase = new AdicionarObservacaoDiarioBordoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Adicionar_Observacao_Diario_De_Bordo()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<AdicionarObservacaoDiarioBordoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto()
                {
                    Id = 1
                });

            //Act
            var auditoriaDto = await adicionarObservacaoDiarioBordoUseCase.Executar("observacao", 1, null);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<AdicionarObservacaoDiarioBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }
    }
}
