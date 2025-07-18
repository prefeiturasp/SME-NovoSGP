using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtividadeAvaliativaTeste
{
    public class ImportarAtividadesGsaUseCaseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ImportarAtividadesGsaUseCase _useCase;

        public ImportarAtividadesGsaUseCaseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ImportarAtividadesGsaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_ShouldSendImportarAtividadeGsaCommand_WhenMensagemRabbitIsValid()
        {
            // Arrange
            var atividadeGsaDto = new AtividadeGsaDto
            {
                AtividadeClassroomId = 123,
                TurmaId = "TurmaA",
                ComponenteCurricularId = 456,
                UsuarioRf = "rf123",
                Titulo = "Atividade Teste",
                Descricao = "Descrição da atividade teste",
                DataCriacao = DateTime.Now,
                Email = "teste@email.com"
            };

            var mensagemJson = JsonConvert.SerializeObject(atividadeGsaDto);
            var mensagemRabbit = new MensagemRabbit(mensagemJson);

            // Act
            var result = await _useCase.Executar(mensagemRabbit);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<ImportarAtividadeGsaCommand>(cmd =>
                cmd.AtividadeGsa.AtividadeClassroomId == atividadeGsaDto.AtividadeClassroomId &&
                cmd.AtividadeGsa.TurmaId == atividadeGsaDto.TurmaId &&
                cmd.AtividadeGsa.ComponenteCurricularId == atividadeGsaDto.ComponenteCurricularId &&
                cmd.AtividadeGsa.UsuarioRf == atividadeGsaDto.UsuarioRf &&
                cmd.AtividadeGsa.Titulo == atividadeGsaDto.Titulo &&
                cmd.AtividadeGsa.Descricao == atividadeGsaDto.Descricao &&
                cmd.AtividadeGsa.Email == atividadeGsaDto.Email
            ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(result);
        }

        [Fact]
        public async Task Executar_ShouldReturnTrue_EvenIfAtividadeGsaDtoIsNull()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit("null");

            // Act
            var result = await _useCase.Executar(mensagemRabbit);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<ImportarAtividadeGsaCommand>(cmd =>
                cmd.AtividadeGsa == null
            ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(result);
        }
    }
}
