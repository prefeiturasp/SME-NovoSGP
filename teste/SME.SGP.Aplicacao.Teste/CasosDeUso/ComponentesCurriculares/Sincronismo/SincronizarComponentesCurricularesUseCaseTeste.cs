using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class SincronizarComponentesCurricularesUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly SincronizarComponentesCurricularesUseCase sincronizarComponentesCurricularesUseCase;

        public SincronizarComponentesCurricularesUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            sincronizarComponentesCurricularesUseCase = new SincronizarComponentesCurricularesUseCase(mediator.Object);
        }

        [Fact]
        public async Task Nao_Deve_Sincronizar()
        {
            var mockComponentesEol = new List<ComponenteCurricularDto>()
            {
                new ComponenteCurricularDto()
                {
                    Codigo = "1",
                    Descricao = "Componente 1"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "2",
                    Descricao = "Componente 2"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "3",
                    Descricao = "Componente 3"
                },
            };


            var mockComponentesSgp = new List<ComponenteCurricularDto>()
            {
                new ComponenteCurricularDto()
                {
                    Codigo = "1",
                    Descricao = "Componente 1"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "2",
                    Descricao = "Componente 2"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "3",
                    Descricao = "Componente 3"
                },
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesEolQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockComponentesEol);
            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockComponentesSgp);

            var retorno = await sincronizarComponentesCurricularesUseCase.Executar(null);
            mediator.Verify(x => x.Send(It.IsAny<InserirVariosComponentesCurricularesCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.True(retorno);
        }

        [Fact]
        public async Task Deve_Sincronizar()
        {
            var mockComponentesEol = new List<ComponenteCurricularDto>()
            {
                new ComponenteCurricularDto()
                {
                    Codigo = "1",
                    Descricao = "Componente 1"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "2",
                    Descricao = "Componente 2"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "3",
                    Descricao = "Componente 3"
                },
            };


            var mockComponentesSgp = new List<ComponenteCurricularDto>()
            {
                new ComponenteCurricularDto()
                {
                    Codigo = "1",
                    Descricao = "Componente 1"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "2",
                    Descricao = "Componente 2"
                }
            };

            var mockInserir = new List<ComponenteCurricularDto>()
            {
                new ComponenteCurricularDto()
                {
                    Codigo = "1",
                    Descricao = "Componente 1"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "2",
                    Descricao = "Componente 2"
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "3",
                    Descricao = "Componente 3"
                },
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesEolQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockComponentesEol);
            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockComponentesSgp);
            mediator.Setup(a => a.Send(It.IsAny<InserirVariosComponentesCurricularesCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var retorno = await sincronizarComponentesCurricularesUseCase.Executar(null);
            mediator.Verify(x => x.Send(It.IsAny<InserirVariosComponentesCurricularesCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(retorno);
        }

    }
}
