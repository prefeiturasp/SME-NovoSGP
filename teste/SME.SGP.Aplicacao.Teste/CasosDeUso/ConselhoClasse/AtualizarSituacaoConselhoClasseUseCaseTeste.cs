using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class AtualizarSituacaoConselhoClasseUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IAtualizarSituacaoConselhoClasseUseCase _useCase;

        public AtualizarSituacaoConselhoClasseUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AtualizarSituacaoConselhoClasseUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Atualizar_Situacao_Com_Sucesso()
        {
            var command = new AtualizaSituacaoConselhoClasseCommand(123, "12345");

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<AtualizaSituacaoConselhoClasseCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(x => x.Send<bool>(
               It.Is<AtualizaSituacaoConselhoClasseCommand>(
                   c => c.ConselhoClasseId == 123 && c.CodigoTurma == "12345"
               ),
               It.IsAny<CancellationToken>()
           ), Times.Once);
        }
    }
}
