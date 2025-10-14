using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ComponentesCurriculares.Sincronismo
{
    public class ExecutarSincronismoComponentesCurricularesUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutarSincronismoComponentesCurricularesUseCase _useCase;

        public ExecutarSincronismoComponentesCurricularesUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronismoComponentesCurricularesUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Existem_Novos_Componentes_Nao_Deve_Chamar_Comando_Inserir()
        {
            var componentesExistentes = new List<ComponenteCurricularDto>
            {
                new ComponenteCurricularDto { Codigo = "1", Descricao = "Matemática" }
            };

            _mediatorMock.Setup(m => m.Send(ObterComponentesCurricularesEolQuery.Instance, default))
                         .ReturnsAsync(componentesExistentes);

            _mediatorMock.Setup(m => m.Send(ObterComponentesCurricularesQuery.Instance, default))
                         .ReturnsAsync(componentesExistentes);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<InserirVariosComponentesCurricularesCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Existem_Novos_Componentes_Deve_Chamar_Comando_Inserir()
        {
            var componentesEol = new List<ComponenteCurricularDto>
            {
                new ComponenteCurricularDto { Codigo = "1", Descricao = "Matemática" },
                new ComponenteCurricularDto { Codigo = "2", Descricao = "Português" }
            };

            var componentesSgp = new List<ComponenteCurricularDto>
            {
                new ComponenteCurricularDto { Codigo = "1", Descricao = "Matemática" }
            };

            _mediatorMock.Setup(m => m.Send(ObterComponentesCurricularesEolQuery.Instance, default))
                         .ReturnsAsync(componentesEol);

            _mediatorMock.Setup(m => m.Send(ObterComponentesCurricularesQuery.Instance, default))
                         .ReturnsAsync(componentesSgp);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<InserirVariosComponentesCurricularesCommand>(
                    c => c.ComponentesCurriculares.Count() == 1 && c.ComponentesCurriculares.First().Codigo == "2"),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
