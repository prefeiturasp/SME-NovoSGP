using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtividadeAvaliativa
{
    public class ImportarNotaAtividadeGsaCommandHandlerTeste
    {

        public readonly Mock<IMediator> mediatorMock;
        public readonly ImportarNotaAtividadeGsaCommandHandler handler;

        public ImportarNotaAtividadeGsaCommandHandlerTeste()
        {
            mediatorMock = new Mock<IMediator>();
            handler = new ImportarNotaAtividadeGsaCommandHandler(mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Turma_Nao_Infantil_Deve_Executar_Fluxo_De_Nota()
        {
            var turma = new Turma { Ano = "2024", ModalidadeCodigo = Modalidade.Medio };

            var notaAtividade = new NotaAtividadeGsaDto
            {
                TurmaId = 1,
                ComponenteCurricularId = 2,
                AtividadeGoogleClassroomId = 123,
                CodigoAluno = 456,
                Nota = 9.5,
                StatusGsa = StatusGSA.Entregue,
                DataInclusao = DateTime.Today,
                Titulo = "Avaliação de História"
            };

            var atividadeAvaliativa = new SME.SGP.Dominio.AtividadeAvaliativa { Id = 10 };
            var notaConceito = new NotaConceito { Id = 20 };
            var notaTipo = new NotaTipoValor { TipoNota = TipoNota.Nota };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadeAvaliativaPorGoogleClassroomIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(atividadeAvaliativa);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaPorAtividadeGoogleClassIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(notaConceito);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaTipoPorAnoModalidadeDataReferenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(notaTipo);

            var command = new ImportarNotaAtividadeGsaCommand(notaAtividade);
            var handlerFake = new ImportarNotaAtividadeGsaCommandHandlerFake(mediatorMock.Object);

            await handlerFake.Executar(command);

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarNotaAtividadeAvaliativaGsaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Componente_Nao_Lanca_Nota_Deve_Encerrar_Execucao()
        {
            var dto = new NotaAtividadeGsaDto { ComponenteCurricularId = 1 };
            var command = new ImportarNotaAtividadeGsaCommand(dto);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handlerFake = new ImportarNotaAtividadeGsaCommandHandlerFake(mediatorMock.Object);

            await handlerFake.Executar(command);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Turma_Nao_Encontrada_Deve_Encerrar_Execucao()
        {
            var dto = new NotaAtividadeGsaDto { ComponenteCurricularId = 1, TurmaId = 123 };
            var command = new ImportarNotaAtividadeGsaCommand(dto);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            var handlerFake = new ImportarNotaAtividadeGsaCommandHandlerFake(mediatorMock.Object);

            await handlerFake.Executar(command);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAtividadeAvaliativaPorGoogleClassroomIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Turma_Infantil_Sem_Registro_Deve_Inserir_Registro_Individual()
        {
            var dto = new NotaAtividadeGsaDto
            {
                TurmaId = 1,
                CodigoAluno = 123,
                ComponenteCurricularId = 456,
                DataInclusao = DateTime.Today,
                Titulo = "Brincadeira com Letras"
            };

            var command = new ImportarNotaAtividadeGsaCommand(dto);
            var turma = new Turma { ModalidadeCodigo = Modalidade.EducacaoInfantil };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterRegistroIndividualPorAlunoDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RegistroIndividualDto)null);

            var handlerFake = new ImportarNotaAtividadeGsaCommandHandlerFake(mediatorMock.Object);

            await handlerFake.Executar(command);

            mediatorMock.Verify(m => m.Send(It.Is<InserirRegistroIndividualCommand>(x =>
                x.TurmaId == dto.TurmaId &&
                x.AlunoCodigo == dto.CodigoAluno &&
                x.ComponenteCurricularId == dto.ComponenteCurricularId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Turma_Infantil_Com_Registro_Deve_Alterar_Registro_Individual()
        {
            var dto = new NotaAtividadeGsaDto
            {
                TurmaId = 1,
                CodigoAluno = 123,
                ComponenteCurricularId = 456,
                DataInclusao = DateTime.Today,
                Titulo = "Atividade de cores"
            };

            var command = new ImportarNotaAtividadeGsaCommand(dto);
            var turma = new Turma { ModalidadeCodigo = Modalidade.EducacaoInfantil };
            var registroExistente = new RegistroIndividualDto { Id = 99 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterRegistroIndividualPorAlunoDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registroExistente);

            var handlerFake = new ImportarNotaAtividadeGsaCommandHandlerFake(mediatorMock.Object);

            await handlerFake.Executar(command);

            mediatorMock.Verify(m => m.Send(It.Is<AlterarRegistroIndividualCommand>(x =>
                x.Id == registroExistente.Id &&
                x.TurmaId == dto.TurmaId &&
                x.AlunoCodigo == dto.CodigoAluno &&
                x.ComponenteCurricularId == dto.ComponenteCurricularId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Atividade_Nao_Encontrada_Deve_Publicar_Fila_Sincronizacao()
        {
            var dto = new NotaAtividadeGsaDto
            {
                TurmaId = 1,
                ComponenteCurricularId = 2,
                AtividadeGoogleClassroomId = 999,
                CodigoAluno = 456,
                Nota = 8.0,
                StatusGsa = StatusGSA.Criado
            };

            var turma = new Turma { Ano = "2024", ModalidadeCodigo = Modalidade.Fundamental };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadeAvaliativaPorGoogleClassroomIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((SME.SGP.Dominio.AtividadeAvaliativa)null);

            var command = new ImportarNotaAtividadeGsaCommand(dto);

            var handlerFake = new ImportarNotaAtividadeGsaCommandHandlerFake(mediatorMock.Object);

            await handlerFake.Executar(command);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Rota == RotasRabbitSgp.RotaNotaAtividadesSyncAgendado), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
    public class ImportarNotaAtividadeGsaCommandHandlerFake : ImportarNotaAtividadeGsaCommandHandler
    {
        public ImportarNotaAtividadeGsaCommandHandlerFake(IMediator mediator) : base(mediator) { }

        public async Task Executar(ImportarNotaAtividadeGsaCommand command)
        {
            await base.Handle(command, CancellationToken.None);
        }
    }

}
