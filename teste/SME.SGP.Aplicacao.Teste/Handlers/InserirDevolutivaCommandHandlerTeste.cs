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
    public class InserirDevolutivaCommandHandlerTeste
    {
        private const string textoDescricao = "teste de inclusão de devolutiva... teste de inclusão de devolutiva... teste de inclusão de devolutiva... teste de inclusão de devolutiva... teste de inclusão de devolutiva.....";
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioDevolutiva> repositorioDevolutiva;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly InserirDevolutivaCommandHandler inserirDevolutivaCommandHandler;

        public InserirDevolutivaCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioDevolutiva = new Mock<IRepositorioDevolutiva>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            inserirDevolutivaCommandHandler = new InserirDevolutivaCommandHandler(mediator.Object, repositorioDevolutiva.Object, repositorioTurma.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Devolutiva()
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
        }

        [Fact]
        public async Task Deve_Obrigar_Descricao()
        {
            var command = new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.Today.AddDays(15), "", 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Descricao);
        }

        [Fact]
        public async Task Deve_Obrigar_PeriodoInicio()
        {
            var command = new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.MinValue, DateTime.Today.AddDays(15), textoDescricao, 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.PeriodoInicio);
        }

        [Fact]
        public async Task Deve_Obrigar_PeriodoFim()
        {
            var command = new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.MinValue, textoDescricao, 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.PeriodoFim);
        }

        private TestValidationResult<InserirDevolutivaCommand> ValidarCommand(InserirDevolutivaCommand command)
        {
            var validator = new InserirDevolutivaCommandValidator();
            return validator.TestValidate(command);
        }
    }
}
