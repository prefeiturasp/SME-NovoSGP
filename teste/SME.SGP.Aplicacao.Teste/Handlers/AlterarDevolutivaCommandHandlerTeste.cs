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
        private readonly Mock<IRepositorioDevolutiva> repositorioDevolutiva;
        private readonly AlterarDevolutivaCommandHandler inserirDevolutivaCommandHandler;

        public AlterarDevolutivaCommandHandlerTeste()
        {
            repositorioDevolutiva = new Mock<IRepositorioDevolutiva>();
            inserirDevolutivaCommandHandler = new AlterarDevolutivaCommandHandler(repositorioDevolutiva.Object);
        }

        [Fact]
        public async Task Deve_Alterar_Devolutiva()
        {
            // Arrange
            repositorioDevolutiva.Setup(a => a.SalvarAsync(It.IsAny<Devolutiva>()))
                .ReturnsAsync(1);

            var devolutiva = GerarDevolutiva();

            // Act
            var auditoriaDto = inserirDevolutivaCommandHandler.Handle(new AlterarDevolutivaCommand(devolutiva), new CancellationToken());

            // Assert
            repositorioDevolutiva.Verify(x => x.SalvarAsync(It.IsAny<Devolutiva>()), Times.Once);
            Assert.True(auditoriaDto.Id > 0);
        }

        [Fact]
        public async Task Deve_Obrigar_Devolutiva()
        {
            var command = new AlterarDevolutivaCommand(null);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Devolutiva);
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
