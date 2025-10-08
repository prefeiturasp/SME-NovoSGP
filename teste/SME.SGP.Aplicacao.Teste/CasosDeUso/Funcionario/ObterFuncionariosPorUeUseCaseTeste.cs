using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Funcionario
{
    public class ObterFuncionariosPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterFuncionariosPorUeUseCase _useCase;

        public ObterFuncionariosPorUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterFuncionariosPorUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ObterFuncionariosPorUeUseCase(null));
            Assert.Equal("mediator", exception.ParamName);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Invocar_Mediator_Corretamente()
        {
            var codigoUe = "123456";
            var filtro = "algumFiltro";
            var funcionariosEsperados = new List<UsuarioEolRetornoDto>
        {
            new UsuarioEolRetornoDto { NomeServidor = "Funcionario 1" }
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorUeQuery>(q => q.CodigoUe == codigoUe && q.Filtro == filtro),
                                           It.IsAny<CancellationToken>()))
                         .ReturnsAsync(funcionariosEsperados);

            var resultado = await _useCase.Executar(codigoUe, filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(funcionariosEsperados.First().NomeServidor, resultado.First().NomeServidor);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterFuncionariosPorUeQuery>(q => q.CodigoUe == codigoUe && q.Filtro == filtro),
                                            It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
