using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Usuarios
{
    public class ObterUsuarioFuncionarioUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterUsuarioFuncionarioUseCase _useCase;

        public ObterUsuarioFuncionarioUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterUsuarioFuncionarioUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterUsuarioFuncionarioUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_ObterUsuarioFuncionarioQuery_Corretamente()
        {
            var filtro = new FiltroFuncionarioDto("DRE1", "UE1", "RF1234", "Nome Teste");
            var retornoEsperado = new List<UsuarioEolRetornoDto>
            {
                new UsuarioEolRetornoDto { CodigoRf = "RF1234", NomeServidor = "Nome Teste", UsuarioId = 1 }
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.Is<ObterUsuarioFuncionarioQuery>(q =>
                        q.CodigoDre == filtro.CodigoDRE &&
                        q.CodigoUe == filtro.CodigoUE &&
                        q.CodigoRf == filtro.CodigoRF &&
                        q.NomeServidor == filtro.NomeServidor),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(filtro);

            _mediatorMock.Verify(x => x.Send(
                It.IsAny<ObterUsuarioFuncionarioQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(retornoEsperado, resultado);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Com_Filtro_Vazio_Deve_Enviar_ObterUsuarioFuncionarioQuery_Corretamente()
        {
            var filtro = new FiltroFuncionarioDto();
            var retornoEsperado = new List<UsuarioEolRetornoDto>();

            _mediatorMock
                .Setup(x => x.Send(
                    It.Is<ObterUsuarioFuncionarioQuery>(q =>
                        q.CodigoDre == null &&
                        q.CodigoUe == null &&
                        q.CodigoRf == null &&
                        q.NomeServidor == null),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(filtro);

            _mediatorMock.Verify(x => x.Send(
                It.IsAny<ObterUsuarioFuncionarioQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Empty(resultado);
        }
    }
}
