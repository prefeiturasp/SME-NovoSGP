using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAula
{
    public class MigrarPlanoAulaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly MigrarPlanoAulaUseCase _useCase;

        public MigrarPlanoAulaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new MigrarPlanoAulaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Valido_Deve_Obter_Usuario_E_Enviar_Comando_()
        {
            var usuario = new Usuario();
            var dto = new MigrarPlanoAulaDto
            {
                PlanoAulaId = 1,
                DisciplinaId = "123",
                IdsPlanoTurmasDestino = new List<DataPlanoAulaTurmaDto> { new DataPlanoAulaTurmaDto() }
            };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.Is<MigrarPlanoAulaCommand>(c => c.PlanoAulaMigrar == dto && c.Usuario == usuario), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(dto);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<MigrarPlanoAulaCommand>(c => c.PlanoAulaMigrar == dto && c.Usuario == usuario), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Dto_Nulo_Deve_Enviar_Comando_Com_Dto_Nulo_()
        {
            var usuario = new Usuario();
            MigrarPlanoAulaDto dto = null;

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.Is<MigrarPlanoAulaCommand>(c => c.PlanoAulaMigrar == null && c.Usuario == usuario), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            var resultado = await _useCase.Executar(dto);

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<MigrarPlanoAulaCommand>(c => c.PlanoAulaMigrar == null && c.Usuario == usuario), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Nulo_Deve_Enviar_Comando_Com_Usuario_Nulo_()
        {
            Usuario usuario = null;
            var dto = new MigrarPlanoAulaDto
            {
                PlanoAulaId = 1,
                DisciplinaId = "123",
                IdsPlanoTurmasDestino = new List<DataPlanoAulaTurmaDto> { new DataPlanoAulaTurmaDto() }
            };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.Is<MigrarPlanoAulaCommand>(c => c.PlanoAulaMigrar == dto && c.Usuario == null), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(dto);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<MigrarPlanoAulaCommand>(c => c.PlanoAulaMigrar == dto && c.Usuario == null), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
