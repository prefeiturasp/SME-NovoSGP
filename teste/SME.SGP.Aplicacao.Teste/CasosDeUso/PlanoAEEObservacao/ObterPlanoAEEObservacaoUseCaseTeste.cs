using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEEObservacao
{
    public class ObterPlanoAEEObservacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPlanoAEEObservacaoUseCase _useCase;

        public ObterPlanoAEEObservacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPlanoAEEObservacaoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Id_Valido_Deve_Obter_Usuario_E_Retornar_Observacoes_()
        {
            long planoAEEId = 123;
            var usuario = new Usuario { CodigoRf = "RF123" };
            var observacoes = new List<PlanoAEEObservacaoDto>
            {
                new PlanoAEEObservacaoDto(1, "Obs 1", true)
            };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterObservacoesPlanoAEEPorIdQuery>(q =>
                    q.Id == planoAEEId &&
                    q.UsuarioRF == usuario.CodigoRf),
                It.IsAny<CancellationToken>()))
                         .ReturnsAsync(observacoes);

            var resultado = await _useCase.Executar(planoAEEId);

            Assert.NotNull(resultado);
            Assert.Same(observacoes, resultado);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterObservacoesPlanoAEEPorIdQuery>(q =>
                    q.Id == planoAEEId &&
                    q.UsuarioRF == usuario.CodigoRf),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Nulo_Deve_Lancar_Null_Reference_Exception_()
        {
            long planoAEEId = 123;

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Usuario)null);

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(planoAEEId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterObservacoesPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
