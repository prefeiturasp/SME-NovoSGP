using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class LancarFrequenciaAulaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly LancarFrequenciaAulaUseCase useCase;

        public LancarFrequenciaAulaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new LancarFrequenciaAulaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Retornar_True()
        {
            var frequenciaDto = new FrequenciaDto(1);
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(frequenciaDto))
            {
                UsuarioLogadoRF = "123456"
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<InserirFrequenciasAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new FrequenciaAuditoriaAulaDto());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.Is<InserirFrequenciasAulaCommand>(c =>
                c.Frequencia.AulaId == frequenciaDto.AulaId &&
                c.UsuarioLogin == "123456"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_UsuarioLogadoRF_Nulo_Deve_Usar_Sistema()
        {
            var frequenciaDto = new FrequenciaDto(2);
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(frequenciaDto))
            {
                UsuarioLogadoRF = null
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<InserirFrequenciasAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new FrequenciaAuditoriaAulaDto());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.Is<InserirFrequenciasAulaCommand>(c =>
                c.Frequencia.AulaId == frequenciaDto.AulaId &&
                c.UsuarioLogin == "Sistema"), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
