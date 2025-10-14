using MediatR;
using Moq;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ComponentesCurriculares
{
    public class ObterComponentesCurricularesPorTurmaECodigoUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterComponentesCurricularesPorTurmaECodigoUeUseCase _useCase;

        public ObterComponentesCurricularesPorTurmaECodigoUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterComponentesCurricularesPorTurmaECodigoUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterComponentesCurricularesPorTurmaECodigoUeUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Chamar_Query_E_Retornar_Componentes()
        {
            var filtro = new FiltroComponentesCurricularesPorTurmaECodigoUeDto
            {
                CodigoUe = "123456",
                CodigosDeTurmas = new string[] { "1", "2" }
            };

            var componentesEsperados = new List<ComponenteCurricularDto>
            {
                new ComponenteCurricularDto { Codigo = "1", Descricao = "Matemática" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery>(), default))
                         .ReturnsAsync(componentesEsperados);

            var resultado = await _useCase.Executar(filtro);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery>(
                    q => q.CodigoUe == filtro.CodigoUe && q.CodigosDeTurmas == filtro.CodigosDeTurmas),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Same(componentesEsperados, resultado);
        }
    }
}

