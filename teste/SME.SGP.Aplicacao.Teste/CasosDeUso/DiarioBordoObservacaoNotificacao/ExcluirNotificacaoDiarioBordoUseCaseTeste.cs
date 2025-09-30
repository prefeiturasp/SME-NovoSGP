using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiarioBordoObservacaoNotificacao
{
    public class ExcluirNotificacaoDiarioBordoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirNotificacaoDiarioBordoUseCase _useCase;

        public ExcluirNotificacaoDiarioBordoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirNotificacaoDiarioBordoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Enviar_Comando_Para_Excluir_Notificacao_Com_Sucesso()
        {
            var dto = new ExcluirNotificacaoDiarioBordoDto(9876L);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            _mediatorMock.Setup(m => m.Send(It.Is<ExcluirNotificacaoDiarioBordoCommand>(c => c.ObservacaoId == dto.ObservacaoId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ExcluirNotificacaoDiarioBordoCommand>(c => c.ObservacaoId == dto.ObservacaoId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }
    }
}
