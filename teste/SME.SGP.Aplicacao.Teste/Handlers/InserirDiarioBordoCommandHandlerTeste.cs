using FluentValidation.TestHelper;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class InserirDiarioBordoCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioDiarioBordo> repositorioDiarioBordo;
        private readonly InserirDiarioBordoCommandHandler inserirDiarioBordoCommandHandler;

        public InserirDiarioBordoCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioDiarioBordo = new Mock<IRepositorioDiarioBordo>();
            inserirDiarioBordoCommandHandler = new InserirDiarioBordoCommandHandler(mediator.Object, repositorioDiarioBordo.Object);
        }

        [Fact]
        public Task Deve_Inserir_Diario_De_Bordo()
        {
            // Arrange
            mediator.Setup(a => a.Send(It.IsAny<AulaExisteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Aula { Id = 1 });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { Id = 1, Ue = new Ue { Id = 1, CodigoUe = "101011", Dre = new Dre { Id = 1, CodigoDre = "101100" } } });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "123", PerfilAtual = Dominio.Perfis.PERFIL_PROFESSOR });

            mediator.Setup(a => a.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Teste");

            mediator.Setup(a => a.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositorioDiarioBordo.Setup(a => a.SalvarAsync(It.IsAny<DiarioBordo>()))
                .ReturnsAsync(1);

            // Act
            var auditoriaDto = inserirDiarioBordoCommandHandler.Handle(new InserirDiarioBordoCommand(1, "teste de inclusão de diário de bordo... teste de inclusão de diário de bordo... teste de inclusão de diário de bordo... teste de inclusão de diário de bordo... teste de inclusão de diário de bordo.....", 1), new System.Threading.CancellationToken());

            // Assert
            repositorioDiarioBordo.Verify(x => x.SalvarAsync(It.IsAny<DiarioBordo>()), Times.Once);
            Assert.True(auditoriaDto.Id > 0);

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Obrigar_Planejamento()
        {
            var command = new InserirDiarioBordoCommand(1, "", 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Planejamento);

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Exigir_Planejamento_Com_200_Caracteres()
        {
            var command = new InserirDiarioBordoCommand(1, "teste de limite de caracteres", 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Planejamento);

            return Task.CompletedTask;
        }

        private TestValidationResult<InserirDiarioBordoCommand> ValidarCommand(InserirDiarioBordoCommand command)
        {
            var validator = new InserirDiarioBordoCommandValidator();
            return validator.TestValidate(command);
        }
    }
}
