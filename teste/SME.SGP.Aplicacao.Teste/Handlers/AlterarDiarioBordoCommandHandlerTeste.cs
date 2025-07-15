using FluentValidation.TestHelper;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class AlterarDiarioBordoCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioDiarioBordo> repositorioDiarioBordo;
        private readonly Mock<IConsultasDisciplina> consultaDisciplina;
        private readonly AlterarDiarioBordoCommandHandler inserirDiarioBordoCommandHandler;

        public AlterarDiarioBordoCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioDiarioBordo = new Mock<IRepositorioDiarioBordo>();
            consultaDisciplina = new Mock<IConsultasDisciplina>();
            inserirDiarioBordoCommandHandler = new AlterarDiarioBordoCommandHandler(mediator.Object, repositorioDiarioBordo.Object, consultaDisciplina.Object);
        }

        [Fact]
        public Task Deve_Alterar_Diario_De_Bordo()
        {
            // Arrange
            var mockEntity = new Dominio.DiarioBordo
            {
                Id = 1,
                AulaId = 1,
                Planejamento = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
            };

            repositorioDiarioBordo.Setup(a => a.ObterPorAulaId(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(mockEntity);

            mediator.Setup(a => a.Send(It.IsAny<AulaExisteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Aula { Id = 1 });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { Id = 1 , Ue = new Ue { Id = 1, CodigoUe = "101011", Dre = new Dre { Id = 1, CodigoDre = "101100" } } });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "123", PerfilAtual = Dominio.Perfis.PERFIL_PROFESSOR });

            mediator.Setup(a => a.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Teste");

            mediator.Setup(a => a.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.Turma { CodigoTurma = "1" });

            mediator.Setup(x => x.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.Aula { Id = 1, TurmaId = "1" });

            var disciplinaDto = RetornaDisciplinaDto();

            consultaDisciplina.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma("1", false, false, false)).Returns(disciplinaDto);

            repositorioDiarioBordo.Setup(a => a.SalvarAsync(It.IsAny<Dominio.DiarioBordo>()))
                .ReturnsAsync(1);
            // Act
            var auditoriaDto = inserirDiarioBordoCommandHandler.Handle(new AlterarDiarioBordoCommand(1, 1, "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",1), new System.Threading.CancellationToken());

            // Assert
            repositorioDiarioBordo.Verify(x => x.SalvarAsync(It.IsAny<Dominio.DiarioBordo>()), Times.Once);
            Assert.True(auditoriaDto.Id > 0);

            return Task.CompletedTask;
        }
        private Task<List<DisciplinaDto>> RetornaDisciplinaDto()
        {
            var listaDisciplinaDto = new List<DisciplinaDto>();
            var disciplinaDto = new DisciplinaDto() { Id = 1, CodigoComponenteCurricular = 1, CdComponenteCurricularPai = 1, Nome = "Matematica", TurmaCodigo = "1" };

            listaDisciplinaDto.Add(disciplinaDto);

            return Task.FromResult(listaDisciplinaDto);
        }

        [Fact]
        public Task Deve_Obrigar_Id()
        {
            var command = new AlterarDiarioBordoCommand(0, 1, "",1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Id);

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Obrigar_Planejamento()
        {
            var command = new AlterarDiarioBordoCommand(1, 1, "", 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Planejamento);

            return Task.CompletedTask;
        }

        [Fact]
        public Task Deve_Exigir_Planejamento_Com_200_Caracteres()
        {
            var command = new AlterarDiarioBordoCommand(1, 1, "0123456789", 1);
            var result = ValidarCommand(command);

            result.ShouldHaveValidationErrorFor(a => a.Planejamento);

            return Task.CompletedTask;
        }

        private TestValidationResult<AlterarDiarioBordoCommand> ValidarCommand(AlterarDiarioBordoCommand command)
        {
            var validator = new AlterarDiarioBordoCommandValidator();
            return validator.TestValidate(command);
        }
    }
}
