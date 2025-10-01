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
        public async Task Executar_Quando_Dre_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            long dreIdInexistente = 99;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterDREPorIdQuery>(q => q.DreId == dreIdInexistente), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((SME.SGP.Dominio.Dre)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(dreIdInexistente));

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFuncionariosPorDreECargoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Dre_Encontrada_Deve_Retornar_Funcionarios()
        {
            long dreId = 10;
            var dreCodigo = "DRE-CODIGO";
            var dre = new SME.SGP.Dominio.Dre { Id = dreId, CodigoDre = dreCodigo };

            var funcionarios = new List<UsuarioEolRetornoDto>
        {
            new UsuarioEolRetornoDto { CodigoRf = "123", NomeServidor = "Funcionario 1" },
            new UsuarioEolRetornoDto { CodigoRf = "456", NomeServidor = "Funcionario 2" }
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterDREPorIdQuery>(q => q.DreId == dreId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dre);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorDreECargoQuery>(q => q.CodigoDRE == dreCodigo && q.CodigoCargo == 29), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(funcionarios);

            var resultado = await _useCase.Executar(dreId);

            Assert.NotNull(resultado);
            Assert.Equal(funcionarios.Count, (resultado as List<UsuarioEolRetornoDto>).Count);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDREPorIdQuery>(q => q.DreId == dreId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterFuncionariosPorDreECargoQuery>(q => q.CodigoDRE == dreCodigo && q.CodigoCargo == 29), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
