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
    public class ObterPareceresConclusivosAnoLetivoModalidadeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPareceresConclusivosAnoLetivoModalidadeUseCase _useCase;

        public ObterPareceresConclusivosAnoLetivoModalidadeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPareceresConclusivosAnoLetivoModalidadeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterPareceresConclusivosAnoLetivoModalidadeUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Parametros_Validos_Deve_Enviar_Query_E_Retornar_Resultado()
        {
            var anoLetivo = 2025;
            var modalidade = Modalidade.Fundamental;
            var retornoEsperado = new List<ParecerConclusivoDto>
            {
                new ParecerConclusivoDto { Id = 1, Nome = "Aprovado" },
                new ParecerConclusivoDto { Id = 2, Nome = "Retido" }
            };

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterPareceresConclusivosAnoLetivoModalidadeQuery>(q => q.AnoLetivo == anoLetivo && q.Modalidade == modalidade),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(anoLetivo, modalidade);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterPareceresConclusivosAnoLetivoModalidadeQuery>(q => q.AnoLetivo == anoLetivo && q.Modalidade == modalidade),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
