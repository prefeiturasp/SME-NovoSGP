using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Ocorrencia
{
    public class ListarTiposOcorrenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IListarTiposOcorrenciaUseCase _useCase;

        public ListarTiposOcorrenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ListarTiposOcorrenciaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Query_E_Retornar_Lista()
        {
            var listaRetorno = new List<OcorrenciaTipoDto>
            {
                new OcorrenciaTipoDto { Id = 1, Descricao = "Advertência" },
                new OcorrenciaTipoDto { Id = 2, Descricao = "Suspensão" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ListarTiposOcorrenciaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaRetorno);

            var resultado = await _useCase.Executar();

            resultado.Should().BeEquivalentTo(listaRetorno);
            _mediatorMock.Verify(m => m.Send(ListarTiposOcorrenciaQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
