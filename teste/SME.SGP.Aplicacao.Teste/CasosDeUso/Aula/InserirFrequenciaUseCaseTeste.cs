using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class InserirFrequenciaUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveRetornarAuditoria_QuandoNaoHouverErro()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var frequenciaDto = new FrequenciaDto(123)
            {
                CriadoPor = "usuario@teste",
                CriadoRF = "1234567",
                CriadoEm = DateTime.Now,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>
                {
                    new RegistroFrequenciaAlunoDto
                    {
                        CodigoAluno = "999",
                        Aulas = new List<FrequenciaAulaDto>
                        {
                            new FrequenciaAulaDto 
                            {
                                TipoFrequencia = "P",
                                NumeroAula = 1,
                                PossuiCompensacao = true 
                            }
                        }
                    }
                }
            };

            var auditoriaEsperada = new AuditoriaDto
            {
                Id = 1,
                CriadoPor = "usuario@teste",
                CriadoRF = "1234567",
                CriadoEm = frequenciaDto.CriadoEm
            };

            var frequenciaAuditoriaDto = new FrequenciaAuditoriaAulaDto
            {
                AulaIdComErro = null,
                Auditoria = auditoriaEsperada
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<InserirFrequenciasAulaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciaAuditoriaDto);

            var useCase = new InserirFrequenciaUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(frequenciaDto);

            // Assert
            Assert.Equal(auditoriaEsperada, resultado);
        }

        [Fact]
        public async Task Executar_DeveLancarNegocioException_QuandoAulaIdComErroEstiverPreenchido()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var dataErro = new DateTime(2025, 7, 15);
            var frequenciaDto = new FrequenciaDto(123)
            {
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>
                {
                    new RegistroFrequenciaAlunoDto
                    {
                        CodigoAluno = "999",
                        Aulas = new List<FrequenciaAulaDto>
                        {
                            new FrequenciaAulaDto 
                            {
                                TipoFrequencia = "P",
                                NumeroAula = 1,
                                PossuiCompensacao = false 
                            }
                        }
                    }
                }
            };

            var frequenciaAuditoriaDto = new FrequenciaAuditoriaAulaDto
            {
                AulaIdComErro = 888,
                DataAulaComErro = dataErro
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<InserirFrequenciasAulaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciaAuditoriaDto);

            var useCase = new InserirFrequenciaUseCase(mediatorMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(frequenciaDto));
            Assert.Contains(dataErro.ToString("dd/MM/yyyy"), ex.Message);
        }
    }
}
