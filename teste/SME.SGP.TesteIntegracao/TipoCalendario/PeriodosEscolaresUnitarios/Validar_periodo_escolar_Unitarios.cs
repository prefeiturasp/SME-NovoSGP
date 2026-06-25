using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TipoCalendario.PeriodosEscolaresUnitarios
{
    public class Validar_periodo_escolar_Unitarios
    {
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodo;
        private readonly Mock<IServicoPeriodoEscolar> servicoPeriodoEscolar;
        private readonly Mock<IMediator> mediatorMock;
        private readonly ComandosPeriodoEscolar comandosPeriodoEscolar;

        public Validar_periodo_escolar_Unitarios()
        {
            servicoPeriodoEscolar = new Mock<IServicoPeriodoEscolar>();
            repositorioPeriodo = new Mock<IRepositorioPeriodoEscolar>();
            mediatorMock = new Mock<IMediator>();

            comandosPeriodoEscolar = new ComandosPeriodoEscolar(
                repositorioPeriodo.Object,
                servicoPeriodoEscolar.Object,
                mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias")]
        public void Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(null, servicoPeriodoEscolar.Object, mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(repositorioPeriodo.Object, null, mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(repositorioPeriodo.Object, servicoPeriodoEscolar.Object, null));
        }

        [Fact(DisplayName = "Deve Salvar Periodo Escolar")]
        public async Task Deve_Salvar_Periodo_Escolar()
        {
            // Arrange
            servicoPeriodoEscolar
                .Setup(x => x.SalvarPeriodoEscolar(It.IsAny<IEnumerable<PeriodoEscolar>>(), It.IsAny<long>()));

            var dto = new PeriodoEscolarListaDto
            {
                TipoCalendario = 1,
                Periodos = new List<PeriodoEscolarDto>
                {
                    new PeriodoEscolarDto { Bimestre = 1, PeriodoInicio = DateTimeExtension.HorarioBrasilia(), PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(1) },
                    new PeriodoEscolarDto { Bimestre = 2, PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(2), PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(3) },
                    new PeriodoEscolarDto { Bimestre = 3, PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(4), PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(5) },
                    new PeriodoEscolarDto { Bimestre = 4, PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(6), PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(7) }
                }
            };

            // Act
            await comandosPeriodoEscolar.Salvar(dto);

            // Assert
            // Adicionando a asserção para verificar se o método SalvarPeriodoEscolar foi chamado.
            servicoPeriodoEscolar.Verify(x => x.SalvarPeriodoEscolar(It.IsAny<IEnumerable<PeriodoEscolar>>(), It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "Nao_Deve_Salvar_Sem_Tipo_Calendario")]
        public async Task Nao_Deve_Salvar_Sem_Tipo_Calendario()
        {
            // Arrange
            servicoPeriodoEscolar
                .Setup(x => x.SalvarPeriodoEscolar(It.IsAny<IEnumerable<PeriodoEscolar>>(), It.IsAny<long>()));

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() =>
                comandosPeriodoEscolar.Salvar(new PeriodoEscolarListaDto
                {
                    TipoCalendario = 0,
                    Periodos = new List<PeriodoEscolarDto>
                    {
                        new PeriodoEscolarDto { Bimestre = 1, PeriodoInicio = DateTimeExtension.HorarioBrasilia(), PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(1) }
                    }
                }));
        }
    }
}