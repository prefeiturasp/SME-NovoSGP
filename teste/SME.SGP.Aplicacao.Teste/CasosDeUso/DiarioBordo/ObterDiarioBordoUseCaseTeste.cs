using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiarioBordo
{
    public class ObterDiarioBordoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IConsultasDisciplina> consultasDisciplinaMock;
        private readonly ObterDiarioBordoUseCase useCase;

        public ObterDiarioBordoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            consultasDisciplinaMock = new Mock<IConsultasDisciplina>();
            useCase = new ObterDiarioBordoUseCase(mediatorMock.Object, consultasDisciplinaMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarDto_QuandoDadosForemValidos()
        {
            // Arrange
            long aulaId = 1;
            long componenteCurricularId = 10;
            string turmaId = "TURMA-1";
            DateTime dataAula = DateTime.Today;

            var aula = new Aula
            {
                Id = aulaId,
                TurmaId = turmaId,
                DataAula = dataAula,
                Excluido = false
            };

            var diario = new SME.SGP.Dominio.DiarioBordo
            {
                AulaId = aulaId,
                ComponenteCurricularId = componenteCurricularId,
                Planejamento = "Plano",
                Excluido = false,
                Migrado = false,
                InseridoCJ = true
            };

            var disciplinas = new List<DisciplinaDto>
            {
                new DisciplinaDto
                {
                    CodigoComponenteCurricular = componenteCurricularId,
                    NomeComponenteInfantil = "Matemática",
                    CdComponenteCurricularPai = componenteCurricularId
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(aula);

            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Turma());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestreAtualQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiariosDeBordosPorAulaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<SME.SGP.Dominio.DiarioBordo> { diario });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(disciplinas);

            consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(It.IsAny<string>(), false, false, false))
                                   .ReturnsAsync(new List<DisciplinaDto>
                                   {
                                       new DisciplinaDto
                                       {
                                           CodigoComponenteCurricular = componenteCurricularId
                                       }
                                   });

            // Act
            var resultado = await useCase.Executar(aulaId, componenteCurricularId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(aulaId, resultado.AulaId);
            Assert.Equal("Plano", resultado.Planejamento);
            Assert.Equal("Matemática", resultado.NomeComponente);
            Assert.True(resultado.TemPeriodoAberto);
        }
    }
}
