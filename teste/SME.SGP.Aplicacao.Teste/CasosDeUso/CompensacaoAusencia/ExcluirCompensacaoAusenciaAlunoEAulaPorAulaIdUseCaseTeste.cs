using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;

        public ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
        }

        private ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase CriarUseCase()
            => new ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase(mediatorMock.Object);

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Command_Correto_E_Retornar_True()
        {
            var filtro = new FiltroIdDto(123);
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = CriarUseCase();

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand>(c => c.AulaId == 123), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Mensagem_Rabbit_Obter_Objeto_Mensagem_Deve_Desserializar_Corretamente()
        {
            var filtro = new FiltroIdDto(456);
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));
            var objeto = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            Assert.NotNull(objeto);
            Assert.Equal(456, objeto.Id);
        }

        [Fact]
        public void Mensagem_Rabbit_Construtores_Deve_Popular_Propriedades()
        {
            var id = Guid.NewGuid();

            var m1 = new MensagemRabbit("acao", "teste", id, "123");
            Assert.Equal("acao", m1.Action);
            Assert.Equal("teste", m1.Mensagem);
            Assert.Equal("123", m1.UsuarioLogadoRF);

            var m2 = new MensagemRabbit("teste2", id, "Fulano", "456", Guid.NewGuid());
            Assert.Equal("teste2", m2.Mensagem);
            Assert.Equal("Fulano", m2.UsuarioLogadoNomeCompleto);
            Assert.Equal("456", m2.UsuarioLogadoRF);

            var m3 = new MensagemRabbit("teste3");
            Assert.Equal("teste3", m3.Mensagem);

            var m4 = new MensagemRabbit();
            Assert.Null(m4.Mensagem);
        }

        [Fact]
        public void Validator_Deve_Ser_Invalido_Quando_Id_ForZero()
        {
            var validator = new ExcluirCompensacaoAusenciaPorAulaIdCommandValidator();
            var command = new ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand(0);

            var resultado = validator.Validate(command);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage.Contains("O id da aula deve ser informado"));
        }

        [Fact]
        public void Validator_DeveSerValido_QuandoIdForMaiorQueZero()
        {
            var validator = new ExcluirCompensacaoAusenciaPorAulaIdCommandValidator();
            var command = new ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand(789);

            var resultado = validator.Validate(command);

            Assert.True(resultado.IsValid);
        }
    }
}
