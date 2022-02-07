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
    public class InserirDiarioBordoUseCaseTeste
    {
        private readonly InserirDiarioBordoUseCase inserirDiarioBordoUseCase;
        private readonly Mock<IMediator> mediator;

        public InserirDiarioBordoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            inserirDiarioBordoUseCase = new InserirDiarioBordoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Diario_De_Bordo()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<InserirDiarioBordoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Infra.AuditoriaDto()
                {
                    Id = 1
                });

            //Act
            var auditoriaDto = await inserirDiarioBordoUseCase.Executar(new Infra.InserirDiarioBordoDto()
            {
                AulaId = 1,
                Planejamento = "teste"
            });

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<InserirDiarioBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }
    }
}
