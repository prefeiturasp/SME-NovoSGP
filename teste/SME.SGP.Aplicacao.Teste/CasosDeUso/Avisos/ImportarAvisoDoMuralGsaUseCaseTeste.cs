using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Avisos
{
    public class ImportarAvisoDoMuralGsaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ImportarAvisoDoMuralGsaUseCase useCase;

        public ImportarAvisoDoMuralGsaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ImportarAvisoDoMuralGsaUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Executar deve enviar comando para importar aviso e retornar true")]
        public async Task Executar_Deve_Chamar_Mediator_Enviar_Comando()
        {
            var aviso = new AvisoMuralGsaDto
            {
                AvisoClassroomId = 123,
                TurmaId = "T1",
                ComponenteCurricularId = 456,
                UsuarioRf = "123456",
                DataCriacao = DateTime.Now,
                DataAlteracao = DateTime.Now,
                Mensagem = "Aviso teste",
                Email = "teste@teste.com"
            };

            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(aviso));

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ImportarAvisoDoMuralGsaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ImportarAvisoDoMuralGsaCommand>(c => c.AvisoDto.Mensagem == "Aviso teste"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Construtores de MensagemRabbit devem setar propriedades corretamente")]
        public void Mensagem_Rabbit_Deve_Criar_Instancias()
        {
            var objeto = new { Nome = "Teste" };
            var guid = Guid.NewGuid();

            var msg1 = new MensagemRabbit("acao", objeto, guid, "123456", true, "perfil", "admin");
            Assert.Equal("acao", msg1.Action);
            Assert.Equal(objeto, msg1.Mensagem);
            Assert.Equal(guid, msg1.CodigoCorrelacao);
            Assert.Equal("123456", msg1.UsuarioLogadoRF);
            Assert.True(msg1.NotificarErroUsuario);
            Assert.Equal("perfil", msg1.PerfilUsuario);
            Assert.Equal("admin", msg1.Administrador);

            var msg2 = new MensagemRabbit(objeto, guid, "Nome Completo", "654321", guid, false, "adm", "acao2");
            Assert.Equal("Nome Completo", msg2.UsuarioLogadoNomeCompleto);
            Assert.Equal("654321", msg2.UsuarioLogadoRF);
            Assert.Equal("acao2", msg2.Action);
            Assert.Equal("adm", msg2.Administrador);

            var msg3 = new MensagemRabbit(objeto);
            Assert.Equal(objeto, msg3.Mensagem);

            var msg4 = new MensagemRabbit();
            Assert.Null(msg4.Mensagem);
        }

        [Fact(DisplayName = "ObterObjetoMensagem deve desserializar corretamente")]
        public void Obter_Objeto_Mensagem_Deve_Desserializar_Objeto()
        {
            var aviso = new AvisoMuralGsaDto
            {
                AvisoClassroomId = 999,
                TurmaId = "Turma123",
                ComponenteCurricularId = 888,
                UsuarioRf = "RF999",
                DataCriacao = DateTime.Today,
                Mensagem = "Mensagem Teste",
                Email = "teste@email.com"
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(aviso);
            var mensagem = new MensagemRabbit(json);

            var resultado = mensagem.ObterObjetoMensagem<AvisoMuralGsaDto>();

            Assert.Equal(999, resultado.AvisoClassroomId);
            Assert.Equal("Turma123", resultado.TurmaId);
            Assert.Equal("Mensagem Teste", resultado.Mensagem);
        }
    }
}

