using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class AlterarDiarioBordoUseCaseTeste
    {
        private readonly AlterarDiarioBordoUseCase inserirDiarioBordoUseCase;
        private readonly Mock<IMediator> mediator;

        public AlterarDiarioBordoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            inserirDiarioBordoUseCase = new AlterarDiarioBordoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Alterar_Diario_De_Bordo()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<AlterarDiarioBordoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Infra.AuditoriaDto()
                {
                    Id = 1
                });

            //Act
            var auditoriaDto = await inserirDiarioBordoUseCase.Executar(new Infra.AlterarDiarioBordoDto()
            {
                AulaId = 1,
                Planejamento = "teste"
            });

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<AlterarDiarioBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }
    }
}
