using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ComponentesCurriculares
{
    public class ObterComponentesCurricularesPorAnoEscolarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterComponentesCurricularesPorAnoEscolarUseCase _useCase;

        public ObterComponentesCurricularesPorAnoEscolarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterComponentesCurricularesPorAnoEscolarUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterComponentesCurricularesPorAnoEscolarUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Repassar_Parametros_E_Retornar_Resultado()
        {
            var codigoUe = "012345";
            var modalidade = Modalidade.Fundamental;
            var anoLetivo = 2025;
            var anosEscolares = new string[] { "1", "2" };
            var turmaPrograma = false;

            var componentesEsperados = new List<ComponenteCurricularEol>
            {
                new ComponenteCurricularEol { Codigo = 1, Descricao = "Matemática" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorAnosEModalidadeQuery>(), default))
                         .ReturnsAsync(componentesEsperados);

            var resultado = await _useCase.Executar(codigoUe, modalidade, anoLetivo, anosEscolares, turmaPrograma);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterComponentesCurricularesPorAnosEModalidadeQuery>(q =>
                    q.CodigoUe == codigoUe &&
                    q.Modalidade == modalidade &&
                    q.AnoLetivo == anoLetivo &&
                    q.AnosEscolares == anosEscolares &&
                    q.TurmaPrograma == turmaPrograma),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Same(componentesEsperados, resultado);
        }
    }
}
