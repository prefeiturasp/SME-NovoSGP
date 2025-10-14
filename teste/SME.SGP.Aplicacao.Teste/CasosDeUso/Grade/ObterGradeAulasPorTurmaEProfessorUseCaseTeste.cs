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
        [Fact]
        public async Task Executar_Quando_Valido_Deve_Chamar_Queries_E_Retornar_Grade()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new ObterGradeAulasPorTurmaEProfessorUseCase();

            var turmaCodigo = "TURMA-123";
            var componenteCurricular = 50L;
            var dataAula = DateTime.Now.Date;
            var codigoRf = "RF-456";
            var ehRegencia = true;
            var gradeRetorno = new GradeComponenteTurmaAulasDto { PodeEditar = true, QuantidadeAulasGrade = 2 };

            mediatorMock.Setup(m => m.Send(It.Is<AulaDeExperienciaPedagogicaQuery>(q => q.ComponenteCurricular == componenteCurricular), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            mediatorMock.Setup(m => m.Send(
                            It.Is<ObterGradeAulasPorTurmaEProfessorQuery>(q =>
                                q.TurmaCodigo == turmaCodigo &&
                                q.ComponentesCurriculares.Contains(componenteCurricular) &&
                                q.DataAula == dataAula &&
                                q.CodigoRf == codigoRf &&
                                q.EhRegencia == ehRegencia), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(gradeRetorno);

            var resultado = await useCase.Executar(mediatorMock.Object, turmaCodigo, componenteCurricular, dataAula, codigoRf, ehRegencia);

            Assert.NotNull(resultado);
            Assert.Equal(gradeRetorno, resultado);

            mediatorMock.Verify(m => m.Send(It.Is<AulaDeExperienciaPedagogicaQuery>(q => q.ComponenteCurricular == componenteCurricular), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterGradeAulasPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
