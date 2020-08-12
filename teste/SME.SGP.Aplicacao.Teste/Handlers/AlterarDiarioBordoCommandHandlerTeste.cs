using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Xunit;
using MediatR;
using System.Threading;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class AlterarDiarioBordoCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioDiarioBordo> repositorioDiarioBordo;
        private readonly AlterarDiarioBordoCommandHandler inserirDiarioBordoCommandHandler;

        public AlterarDiarioBordoCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioDiarioBordo = new Mock<IRepositorioDiarioBordo>();
            inserirDiarioBordoCommandHandler = new AlterarDiarioBordoCommandHandler(mediator.Object, repositorioDiarioBordo.Object);
        }

        [Fact]
        public async Task Deve_Alterar_Diario_De_Bordo()
        {
            // Arrange
            var mockEntity = new DiarioBordo
            {
                Id = 1,
                AulaId = 1,
                Planejamento = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
            };

            mediator.Setup(a => a.Send(It.IsAny<AulaExisteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositorioDiarioBordo.Setup(a => a.ObterPorAulaId(1))
                .ReturnsAsync(mockEntity);
            repositorioDiarioBordo.Setup(a => a.SalvarAsync(It.IsAny<DiarioBordo>()))
                .ReturnsAsync(1);

            // Act
            var auditoriaDto = inserirDiarioBordoCommandHandler.Handle(new AlterarDiarioBordoCommand(1, 1, "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", ""), new System.Threading.CancellationToken());

            // Assert
            repositorioDiarioBordo.Verify(x => x.SalvarAsync(It.IsAny<DiarioBordo>()), Times.Once);
            Assert.True(auditoriaDto.Id > 0);
        }

        [Fact]
        public async Task Deve_Obrigar_Id()
        {
            var command = new AlterarDiarioBordoCommand(0, 1, "", "");
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Id);
        }

        [Fact]
        public async Task Deve_Obrigar_Planejamento()
        {
            var command = new AlterarDiarioBordoCommand(1, 1, "", "");
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Planejamento);
        }

        [Fact]
        public async Task Deve_Exigir_Planejamento_Com_200_Caracteres()
        {
            var command = new AlterarDiarioBordoCommand(1, 1, "0123456789", "");
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Planejamento);
        }

        private TestValidationResult<AlterarDiarioBordoCommand, AlterarDiarioBordoCommand> ValidarCommand(AlterarDiarioBordoCommand command)
        {
            var validator = new AlterarDiarioBordoCommandValidator();
            return validator.TestValidate(command);
        }
    }
}
