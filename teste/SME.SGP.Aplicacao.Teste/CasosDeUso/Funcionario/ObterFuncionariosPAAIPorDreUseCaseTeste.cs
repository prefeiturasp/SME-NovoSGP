using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Funcionario
{
    public class ObterFuncionariosPAAIPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterFuncionariosPAAIPorDreUseCase _useCase;

        public ObterFuncionariosPAAIPorDreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterFuncionariosPAAIPorDreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Dre_Nao_Encontrada_Deve_Lancar_Negocio_Exception()
        {
            long dreIdInexistente = 99;
            _mediatorMock.Setup(m => m.Send(It.Is<ObterDREPorIdQuery>(q => q.DreId == dreIdInexistente),
                                           It.IsAny<CancellationToken>()))
                         .ReturnsAsync((SME.SGP.Dominio.Dre)null);


            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(dreIdInexistente));

            Assert.Equal("A DRE informada não foi encontrada", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFuncionariosPorDreECargoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Dre_Encontrada_Deve_Retornar_Funcionarios_Corretamente()
        {
            long dreId = 1;
            var codigoDre = "108300";
            var dreRetorno = new SME.SGP.Dominio.Dre { Id = dreId, CodigoDre = codigoDre };

            var funcionariosRetorno = new List<UsuarioEolRetornoDto>
        {
            new UsuarioEolRetornoDto { CodigoRf = "1234567", NomeServidor = "Funcionario Teste" }
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterDREPorIdQuery>(q => q.DreId == dreId),
                                           It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dreRetorno);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorDreECargoQuery>(q => q.CodigoDRE == codigoDre && q.CodigoCargo == 29),
                                           It.IsAny<CancellationToken>()))
                         .ReturnsAsync(funcionariosRetorno);

            var resultado = await _useCase.Executar(dreId);

            Assert.NotNull(resultado);
            Assert.Equal(funcionariosRetorno, resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDREPorIdQuery>(q => q.DreId == dreId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterFuncionariosPorDreECargoQuery>(q => q.CodigoDRE == codigoDre && q.CodigoCargo == 29), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
