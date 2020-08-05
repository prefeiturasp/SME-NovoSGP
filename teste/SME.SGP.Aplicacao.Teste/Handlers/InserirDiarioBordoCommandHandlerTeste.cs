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

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class InserirDiarioBordoCommandHandlerTeste
    {
        private readonly Mock<IRepositorioDiarioBordo> repositorioDiarioBordo;
        private readonly InserirDiarioBordoCommandHandler inserirDiarioBordoCommandHandler;

        public InserirDiarioBordoCommandHandlerTeste()
        {
            repositorioDiarioBordo = new Mock<IRepositorioDiarioBordo>();
            inserirDiarioBordoCommandHandler = new InserirDiarioBordoCommandHandler(repositorioDiarioBordo.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Diario_De_Bordo()
        {
            // Arrange
            repositorioDiarioBordo.Setup(a => a.SalvarAsync(It.IsAny<DiarioBordo>()))
                .ReturnsAsync(1);

            // Act
            var auditoriaDto = inserirDiarioBordoCommandHandler.Handle(new InserirDiarioBordoCommand(1, "teste de inclusão de diário de bordo... teste de inclusão de diário de bordo... teste de inclusão de diário de bordo... teste de inclusão de diário de bordo... teste de inclusão de diário de bordo.....", ""), new System.Threading.CancellationToken());

            // Assert
            repositorioDiarioBordo.Verify(x => x.SalvarAsync(It.IsAny<DiarioBordo>()), Times.Once);
            Assert.True(auditoriaDto.Id == 1);
        }

        [Fact]
        public async Task Deve_Obrigar_Planejamento()
        {
            var command = new InserirDiarioBordoCommand(1, "", "");
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Planejamento);
        }

        [Fact]
        public async Task Deve_Exigir_Planejamento_Com_200_Caracteres()
        {
            var command = new InserirDiarioBordoCommand(1, "teste de limite de caracteres", "");
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Planejamento);
        }

        private TestValidationResult<InserirDiarioBordoCommand, InserirDiarioBordoCommand> ValidarCommand(InserirDiarioBordoCommand command)
        {
            var validator = new InserirDiarioBordoCommandValidator();
            return validator.TestValidate(command);
        }
    }
}
