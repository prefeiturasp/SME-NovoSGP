using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.NotaAtividadeAvaliativa
{
    public class ImportarNotaAtividadeAvaliativaGsaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ImportarNotaAtividadeAvaliativaGsaUseCase useCase;

        public ImportarNotaAtividadeAvaliativaGsaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ImportarNotaAtividadeAvaliativaGsaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var notaDto = new NotaAtividadeGsaDto
            {
                TurmaId = 1,
                ComponenteCurricularId = 2,
                AtividadeGoogleClassroomId = 3,
                CodigoAluno = 4,
                Nota = 10,
                StatusGsa = StatusGSA.Entregue
            };

            var mensagemJson = JsonConvert.SerializeObject(notaDto);
            var mensagemRabbit = new MensagemRabbit(mensagemJson);

            mediatorMock.Setup(m => m.Send(It.IsAny<ImportarNotaAtividadeGsaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(
                It.Is<ImportarNotaAtividadeGsaCommand>(cmd =>
                    cmd.NotaAtividadeGsaDto.TurmaId == notaDto.TurmaId &&
                    cmd.NotaAtividadeGsaDto.CodigoAluno == notaDto.CodigoAluno &&
                    cmd.NotaAtividadeGsaDto.Nota == notaDto.Nota),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>("mediator", () => new ImportarNotaAtividadeAvaliativaGsaUseCase(null));
        }
    }
}
