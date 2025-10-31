using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;
using static SME.SGP.Aplicacao.AlterarRecorrenciaEventosUseCase;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Evento
{
    public class AlterarRecorrenciaEventosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IServicoEvento> _servicoEventoMock;
        private readonly AlterarRecorrenciaEventosUseCase _useCase;

        public AlterarRecorrenciaEventosUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _servicoEventoMock = new Mock<IServicoEvento>();
            _useCase = new AlterarRecorrenciaEventosUseCase(_mediatorMock.Object, _servicoEventoMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Servico_De_Alteracao_Com_Dados_Da_Mensagem()
        {
            var evento = new SME.SGP.Dominio.Evento { Id = 99 };
            var parametro = new AlterarRecorrenciaEventosParametro
            {
                Evento = evento,
                AlterarRecorrenciaCompleta = true
            };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(parametro) };

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);

            _servicoEventoMock.Verify(s => s.AlterarRecorrenciaEventos(
                It.Is<SME.SGP.Dominio.Evento>(e => e.Id == evento.Id),
                parametro.AlterarRecorrenciaCompleta),
            Times.Once);
        }
    }
}
