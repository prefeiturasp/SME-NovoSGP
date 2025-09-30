using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiarioBordoObservacaoNotificacao
{
    public class NotificarDiarioBordoObservacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioDiarioBordoObservacaoNotificacao> _repositorioMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly NotificarDiarioBordoObservacaoUseCase _useCase;

        public NotificarDiarioBordoObservacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioMock = new Mock<IRepositorioDiarioBordoObservacaoNotificacao>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _useCase = new NotificarDiarioBordoObservacaoUseCase(_mediatorMock.Object, _repositorioMock.Object, _unitOfWorkMock.Object);
        }

        private MensagemRabbit CriarMensagemRabbit(string usuarioLogadoRf, IEnumerable<string> usuariosParaNotificar = null)
        {
            var dto = new NotificarDiarioBordoObservacaoDto(1, "obs", new Usuario { CodigoRf = usuarioLogadoRf, Nome = "Logado" }, 10, usuariosParaNotificar);
            return new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };
        }

        private SME.SGP.Dominio.DiarioBordo CriarDiarioBordo()
        {
            return new SME.SGP.Dominio.DiarioBordo { Aula = new SME.SGP.Dominio.Aula { Turma = new Turma { Nome = "1A", CodigoTurma = "T1", Ue = new Ue { Nome = "Escola", TipoEscola = TipoEscola.EMEF, Dre = new SME.SGP.Dominio.Dre { Abreviacao = "DRE-CL" } } } } };
        }

        [Fact]
        public async Task Executar_Quando_Diario_Bordo_Nao_Encontrado_Deve_Retornar_False()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiarioBordoComAulaETurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((SME.SGP.Dominio.DiarioBordo)null);
            var resultado = await _useCase.Executar(CriarMensagemRabbit("RF1"));
            Assert.False(resultado);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Contem_Usuarios_Deve_Notificar_Apenas_Eles()
        {
            var mensagem = CriarMensagemRabbit("RF-LOGADO", new List<string> { "RF-LOGADO", "RF-ALVO" });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiarioBordoComAulaETurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(CriarDiarioBordo());
            _mediatorMock.Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(1L);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<NotificarUsuarioCommand>(c => c.UsuarioRf == "RF-ALVO"), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Executar_Quando_Nao_Contem_Usuarios_Deve_Buscar_E_Notificar_Professores(bool ehStringUnica)
        {
            var professores = ehStringUnica ?
                new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { ProfessorRf = "RF-LOGADO, RF-ALVO1, RF-ALVO2" } } :
                new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { ProfessorRf = "RF-LOGADO" }, new ProfessorTitularDisciplinaEol { ProfessorRf = "RF-ALVO1" } };

            var mensagem = CriarMensagemRabbit("RF-LOGADO");
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiarioBordoComAulaETurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(CriarDiarioBordo());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(professores);
            _mediatorMock.Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(1L);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            var totalNotificacoes = ehStringUnica ? 2 : 1;
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(totalNotificacoes));
        }

        [Fact]
        public async Task Executar_Quando_Nao_Ha_Usuarios_Ou_Professores_Deve_Retornar_False()
        {
            var mensagem = CriarMensagemRabbit("RF-LOGADO");
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiarioBordoComAulaETurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(CriarDiarioBordo());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProfessorTitularDisciplinaEol>());

            var resultado = await _useCase.Executar(mensagem);

            Assert.False(resultado);
        }

        [Fact]
        public async Task Executar_Quando_Ocorre_Erro_Na_Notificacao_Deve_Fazer_Rollback()
        {
            var mensagem = CriarMensagemRabbit("RF-LOGADO", new List<string> { "RF-ALVO" });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiarioBordoComAulaETurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(CriarDiarioBordo());
            _mediatorMock.Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Erro de BD"));

            await _useCase.Executar(mensagem);

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Never);
        }
    }
}
