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
    public class AlterarDevolutivaCommandHandlerTeste
    {
        private const string textoDescricao = "teste de alteração de devolutiva... teste de alteração de devolutiva... teste de alteração de devolutiva... teste de alteração de devolutiva... teste de alteração de devolutiva.....";
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioDevolutiva> repositorioDevolutiva;
        private readonly AlterarDevolutivaCommandHandler inserirDevolutivaCommandHandler;

        public AlterarDevolutivaCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioDevolutiva = new Mock<IRepositorioDevolutiva>();
            inserirDevolutivaCommandHandler = new AlterarDevolutivaCommandHandler(mediator.Object, repositorioDevolutiva.Object);
        }

        [Fact]
        public async Task Deve_Alterar_Devolutiva()
        {
            // Arrange
            mediator.Setup(a => a.Send(It.IsAny<AulaExisteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositorioDevolutiva.Setup(a => a.SalvarAsync(It.IsAny<Devolutiva>()))
                .ReturnsAsync(1);

            var devolutiva = GerarDevolutiva();

            // Act
            var auditoriaDto = inserirDevolutivaCommandHandler.Handle(new AlterarDevolutivaCommand(devolutiva, 1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.Today.AddDays(15), textoDescricao), new CancellationToken());

            // Assert
            repositorioDevolutiva.Verify(x => x.SalvarAsync(It.IsAny<Devolutiva>()), Times.Once);
            Assert.True(auditoriaDto.Id > 0);
        }

        [Fact]
        public async Task Deve_Obrigar_Descricao()
        {
            var devolutiva = GerarDevolutiva();

            var command = new AlterarDevolutivaCommand(devolutiva, 1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.Today.AddDays(15), "");
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Descricao);
        }

        [Fact]
        public async Task Deve_Obrigar_PeriodoInicio()
        {
            var devolutiva = GerarDevolutiva();

            var command = new AlterarDevolutivaCommand(devolutiva, 1, new List<long> { 1, 2, 3, 4 }, DateTime.MinValue, DateTime.Today.AddDays(15), textoDescricao);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.PeriodoInicio);
        }

        [Fact]
        public async Task Deve_Obrigar_PeriodoFim()
        {
            var devolutiva = GerarDevolutiva();

            var command = new AlterarDevolutivaCommand(devolutiva, 1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.MinValue, textoDescricao);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.PeriodoFim);
        }

        private TestValidationResult<AlterarDevolutivaCommand, AlterarDevolutivaCommand> ValidarCommand(AlterarDevolutivaCommand command)
        {
            var validator = new AlterarDevolutivaCommandValidator();
            return validator.TestValidate(command);
        }

        private Devolutiva GerarDevolutiva()
        {
            return new Devolutiva
            {
                Id = 1,
                CodigoComponenteCurricular = 1,
                Descricao = "teste",
                PeriodoInicio = DateTime.Today.AddDays(-15),
                PeriodoFim = DateTime.Today.AddDays(15)
            };
        }
    }
}
