using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterRecomendacoesAlunoFamiliaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterRecomendacoesAlunoFamiliaUseCase useCase;

        public ObterRecomendacoesAlunoFamiliaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterRecomendacoesAlunoFamiliaUseCase(mediator.Object);
        }

        [Fact(DisplayName = "ObterRecomendacoesAlunoFamiliaUseCase - Deve retornar recomendações quando existirem")]
        public async Task DeveRetornarRecomendacoesQuandoExistirem()
        {
            var recomendacoesEsperadas = new List<RecomendacoesAlunoFamiliaDto>
            {
                new RecomendacoesAlunoFamiliaDto { Id = 1, Recomendacao = "Recomendação 1", Tipo = 1 }
            };

            mediator.Setup(m => m.Send(It.IsAny<IRequest<IEnumerable<RecomendacoesAlunoFamiliaDto>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(recomendacoesEsperadas);

            var resultado = await useCase.Executar();

            Assert.NotEmpty(resultado);
        }

        [Fact(DisplayName = "ObterRecomendacoesAlunoFamiliaUseCase - Deve retornar lista vazia quando não houver recomendações")]
        public async Task DeveRetornarListaVaziaQuandoNaoHouverRecomendacoes()
        {
            mediator.Setup(m => m.Send(It.IsAny<IRequest<IEnumerable<RecomendacoesAlunoFamiliaDto>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<RecomendacoesAlunoFamiliaDto>());

            var resultado = await useCase.Executar();

            Assert.Empty(resultado);
        }
    }
}

