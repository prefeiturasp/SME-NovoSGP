using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterPareceresConclusivosTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPareceresConclusivosTurmaUseCase _useCase;

        public ObterPareceresConclusivosTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPareceresConclusivosTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_Excecao()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterPareceresConclusivosTurmaUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Turma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(1L, false));

            Assert.Equal("Turma não encontrada.", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Encontrada_Deve_Retornar_Pareceres_Conclusivos()
        {
            var turmaId = 123L;
            var anoLetivoAnterior = false;
            var turma = new Turma { Id = turmaId };
            var pareceresEsperados = new List<ParecerConclusivoDto>
            {
                new ParecerConclusivoDto { Id = 1, Nome = "Aprovado" }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPareceresConclusivosTurmaQuery>(q => q.Turma == turma && q.AnoLetivoAnterior == anoLetivoAnterior), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(pareceresEsperados);

            var resultado = await _useCase.Executar(turmaId, anoLetivoAnterior);

            Assert.NotNull(resultado);
            Assert.Equal(pareceresEsperados, resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPareceresConclusivosTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
