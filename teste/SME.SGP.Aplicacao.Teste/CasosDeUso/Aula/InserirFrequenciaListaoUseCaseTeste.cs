using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class InserirFrequenciaListaoUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveInserirFrequenciaERetornarAuditoria()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var useCase = new InserirFrequenciaListaoUseCase(mediatorMock.Object);

            var aulaId = 123;
            var codigoAluno = "111111";
            var dataAula = new DateTime(2025, 7, 15);

            var frequencias = new List<FrequenciaSalvarAulaAlunosDto>
            {
                new FrequenciaSalvarAulaAlunosDto
                {
                    AulaId = aulaId,
                    Alunos = new List<FrequenciaSalvarAlunoDto>
                    {
                        new FrequenciaSalvarAlunoDto
                        {
                            CodigoAluno = codigoAluno,
                            Desabilitado = false,
                            Frequencias = new List<FrequenciaAulaDto>
                            {
                                new FrequenciaAulaDto()
                            }
                        }
                    }
                }
            };

            var auditoriaDto = new FrequenciaAuditoriaAulaDto
            {
                DataAula = dataAula,
                TurmaId = "TURMA1",
                DisciplinaId = "DISC1",
                Auditoria = new AuditoriaDto()
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<InserirFrequenciasAulaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(auditoriaDto);

            mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(frequencias);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Auditoria);
            mediatorMock.Verify(m => m.Send(It.IsAny<InserirFrequenciasAulaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
