using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AulasPrevistas
{
    public class NotificacaoAulasPrevistrasUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IConfiguration> configurationMock;
        private readonly NotificacaoAulasPrevistrasUseCase useCase;

        public NotificacaoAulasPrevistrasUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(x => x["UrlFrontEnd"]).Returns("http://localhost/");

            useCase = new NotificacaoAulasPrevistrasUseCase(mediatorMock.Object, configurationMock.Object);
        }

        private RegistroAulaPrevistaDivergenteDto CriarDto()
        {
            return new RegistroAulaPrevistaDivergenteDto
            {
                Bimestre = 2,
                CodigoDre = "DRE1",
                CodigoTurma = "T1",
                CodigoUe = "UE1",
                DisciplinaId = "MAT",
                NomeDre = "DRE NORTE",
                NomeTurma = "Turma Teste",
                NomeUe = "Escola X",
                ProfessorRf = "123456"
            };
        }

        [Fact]
        public async Task Executar_Deve_Notificar_Quando_Usuario_Nao_Notificado()
        {

            var dto = CriarDto();
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(dto));

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(10); 

            mediatorMock.Setup(m => m.Send(It.IsAny<UsuarioNotificadoAulaPrevistaDivergenteQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false); 

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarNotificacaoAulaPrevistaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarNotificacaoAulaPrevistaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_NaoDeve_Notificar_Quando_Usuario_Ja_Notificado()
        {
            var dto = CriarDto();
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(dto));

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(10); 

            mediatorMock.Setup(m => m.Send(It.IsAny<UsuarioNotificadoAulaPrevistaDivergenteQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true); 

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarNotificacaoAulaPrevistaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_NaoDeve_Notificar_Quando_Usuario_Id_Invalido()
        {
            var dto = CriarDto();
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(dto));

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(0); 

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarNotificacaoAulaPrevistaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Configuration_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoAulasPrevistrasUseCase(mediatorMock.Object, null));
        }

        [Fact]
        public void SalvarNotificacaoAulaPrevistaCommand_Deve_Mapear_Corretamente()
        {
            var dto = CriarDto();
            var cmd = new SalvarNotificacaoAulaPrevistaCommand(dto, "titulo", "mensagem", 99);

            Assert.Equal("titulo", cmd.Titulo);
            Assert.Equal("mensagem", cmd.Mensagem);
            Assert.Equal(dto.ProfessorRf, cmd.ProfessorRF);
            Assert.Equal(dto.CodigoDre, cmd.DreCodigo);
            Assert.Equal(dto.CodigoUe, cmd.UeCodigo);
            Assert.Equal(dto.CodigoTurma, cmd.TurmaCodigo);
            Assert.Equal(99, cmd.UsuarioId);
            Assert.Equal(dto.Bimestre, cmd.Bimestre);
            Assert.Equal(dto.DisciplinaId, cmd.ComponenteCurricularId);
        }

        [Fact]
        public void ObterUsuarioIdPorRfOuCriaQuery_Deve_Criar_Corretamente()
        {
            var query = new ObterUsuarioIdPorRfOuCriaQuery("123", "Fulano");
            Assert.Equal("123", query.UsuarioRf);
            Assert.Equal("Fulano", query.UsuarioNome);
        }

        [Fact]
        public void MensagemRabbit_Deve_Construir_Com_Diferentes_Construtores()
        {
            var dto = CriarDto();

            var msg1 = new MensagemRabbit("acao", dto, Guid.NewGuid(), "RF1");
            Assert.NotNull(msg1.Action);

            var msg2 = new MensagemRabbit(dto, Guid.NewGuid(), "Nome", "RF2", Guid.NewGuid());
            Assert.NotNull(msg2.UsuarioLogadoNomeCompleto);

            var msg3 = new MensagemRabbit(dto);
            Assert.NotNull(msg3.Mensagem);

            var msg4 = new MensagemRabbit();
            Assert.Null(msg4.Mensagem);

            var serializado = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
            var msg5 = new MensagemRabbit(serializado);
            var obj = msg5.ObterObjetoMensagem<RegistroAulaPrevistaDivergenteDto>();
            Assert.Equal(dto.ProfessorRf, obj.ProfessorRf);
        }
    }
}
