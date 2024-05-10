using FluentValidation.TestHelper;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class InserirDevolutivaCommandHandlerTeste
    {
        private const string textoDescricao = "teste de inclusão de devolutiva... teste de inclusão de devolutiva... teste de inclusão de devolutiva... teste de inclusão de devolutiva... teste de inclusão de devolutiva.....";
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioDevolutiva> repositorioDevolutiva;
        private readonly InserirDevolutivaCommandHandler inserirDevolutivaCommandHandler;

        public InserirDevolutivaCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioDevolutiva = new Mock<IRepositorioDevolutiva>();
            inserirDevolutivaCommandHandler = new InserirDevolutivaCommandHandler(mediator.Object, repositorioDevolutiva.Object);
        }

        [Fact]
        public Task Deve_Inserir_Devolutiva()
        {
            // Arrange
            mediator.Setup(a => a.Send(It.IsAny<AulaExisteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositorioDevolutiva.Setup(a => a.SalvarAsync(It.IsAny<Devolutiva>()))
                .ReturnsAsync(1);

            // Act
            var auditoriaDto = inserirDevolutivaCommandHandler.Handle(new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.Today.AddDays(15), textoDescricao, 1), new CancellationToken());

            // Assert
            repositorioDevolutiva.Verify(x => x.SalvarAsync(It.IsAny<Devolutiva>()), Times.Once);
            Assert.True(auditoriaDto.Id > 0);

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Obrigar_Descricao()
        {
            var command = new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.Today.AddDays(15), "", 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Descricao);

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Obrigar_PeriodoInicio()
        {
            var command = new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.MinValue, DateTime.Today.AddDays(15), textoDescricao, 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.PeriodoInicio);

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Obrigar_PeriodoFim()
        {
            var command = new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.MinValue, textoDescricao, 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.PeriodoFim);

            return Task.CompletedTask;
        }

        private TestValidationResult<InserirDevolutivaCommand> ValidarCommand(InserirDevolutivaCommand command)
        {
            var validator = new InserirDevolutivaCommandValidator();
            return validator.TestValidate(command);
        }
    }
}
