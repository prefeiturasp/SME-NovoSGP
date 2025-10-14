using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Grade
{
    public class ObterGradeAulasPorTurmaEProfessorUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterGradeAulasPorTurmaEProfessorUseCase _useCase;

        public ObterGradeAulasPorTurmaEProfessorUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterGradeAulasPorTurmaEProfessorUseCase();
        }

        [Fact]
        public async Task Executar_QuandoChamado_DeveInvocarMediatorCorretamente()
        {
            var turmaCodigo = "123";
            long componenteCurricularId = 456;
            var dataAula = DateTime.Now;
            var codigoRf = "789";
            var ehRegencia = true;

            var retornoEsperado = new GradeComponenteTurmaAulasDto { PodeEditar = true, QuantidadeAulasGrade = 2 };

            _mediatorMock.Setup(m => m.Send(It.Is<AulaDeExperienciaPedagogicaQuery>(q => q.ComponenteCurricular == componenteCurricularId),
                                           It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterGradeAulasPorTurmaEProfessorQuery>(q => q.TurmaCodigo == turmaCodigo &&
                                                                                                q.ComponentesCurriculares.Contains(componenteCurricularId) &&
                                                                                                q.DataAula == dataAula &&
                                                                                                q.CodigoRf == codigoRf &&
                                                                                                q.EhRegencia == ehRegencia), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(_mediatorMock.Object, turmaCodigo, componenteCurricularId, dataAula, codigoRf, ehRegencia);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado.PodeEditar, resultado.PodeEditar);
            Assert.Equal(retornoEsperado.QuantidadeAulasGrade, resultado.QuantidadeAulasGrade);

            _mediatorMock.Verify(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterGradeAulasPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
