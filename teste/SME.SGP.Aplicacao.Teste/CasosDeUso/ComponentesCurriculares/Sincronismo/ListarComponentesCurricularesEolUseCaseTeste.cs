using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ComponentesCurriculares.Sincronismo
{
    public class ListarComponentesCurricularesEolUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ListarComponentesCurricularesEolUseCase _useCase;

        public ListarComponentesCurricularesEolUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ListarComponentesCurricularesEolUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ListarComponentesCurricularesEolUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Retornar_Componentes()
        {
            var componentesEsperados = new List<ComponenteCurricularDto>
            {
                new ComponenteCurricularDto { Codigo = "1", Descricao = "Matemática" }
            };

            _mediatorMock.Setup(m => m.Send(ObterComponentesCurricularesEolQuery.Instance, default))
                         .ReturnsAsync(componentesEsperados);

            var resultado = await _useCase.Executar();

            _mediatorMock.Verify(m => m.Send(ObterComponentesCurricularesEolQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Same(componentesEsperados, resultado);
        }
    }
}
