using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosPeriodoEscolarTeste
    {
        private readonly ComandosPeriodoEscolar comandosPeriodoEscolar;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodo;
        private readonly Mock<IServicoPeriodoEscolar> servicoPeriodoEscolar;

        public ComandosPeriodoEscolarTeste()
        {
            servicoPeriodoEscolar = new Mock<IServicoPeriodoEscolar>();
            repositorioPeriodo = new Mock<IRepositorioPeriodoEscolar>();

            comandosPeriodoEscolar = new ComandosPeriodoEscolar(repositorioPeriodo.Object, servicoPeriodoEscolar.Object);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias")]
        public void Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(null, servicoPeriodoEscolar.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(repositorioPeriodo.Object, null));
        }

        [Fact(DisplayName = "Deve_Salvar_Periodo_Escolar")]
        public async Task Deve_Salvar_Periodo_Escolar()
        {
            servicoPeriodoEscolar.Setup(x => x.SalvarPeriodoEscolar(It.IsAny<IEnumerable<PeriodoEscolar>>(), It.IsAny<long>()));

            await comandosPeriodoEscolar.Salvar(new PeriodoEscolarListaDto
            {
                TipoCalendario = 1,
                Periodos = new List<PeriodoEscolarDto>
                {
                    new PeriodoEscolarDto
                    {
                        Bimestre = 1,
                        PeriodoInicio = DateTime.Now,
                        PeriodoFim = DateTime.Now.AddMinutes(1),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 2,
                        PeriodoInicio = DateTime.Now.AddMinutes(2),
                        PeriodoFim = DateTime.Now.AddMinutes(3),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 3,
                        PeriodoInicio = DateTime.Now.AddMinutes(4),
                        PeriodoFim = DateTime.Now.AddMinutes(5),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 4,
                        PeriodoInicio = DateTime.Now.AddMinutes(6),
                        PeriodoFim = DateTime.Now.AddMinutes(7),
                    }
                }
            });
        }

        [Fact(DisplayName = "Nao_Deve_Salvar_Sem_Tipo_Calendario")]
        public async Task Nao_Deve_Salvar_Sem_Tipo_Calendario()
        {
            servicoPeriodoEscolar.Setup(x => x.SalvarPeriodoEscolar(It.IsAny<IEnumerable<PeriodoEscolar>>(), It.IsAny<long>()));

            await Assert.ThrowsAsync<NegocioException>(() =>
             comandosPeriodoEscolar.Salvar(new PeriodoEscolarListaDto
             {
                 TipoCalendario = 0,
                 Periodos = new List<PeriodoEscolarDto>
                 {
                    new PeriodoEscolarDto
                    {
                        Bimestre = 1,
                        PeriodoInicio = DateTime.Now,
                        PeriodoFim = DateTime.Now.AddMinutes(1),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 2,
                        PeriodoInicio = DateTime.Now.AddMinutes(2),
                        PeriodoFim = DateTime.Now.AddMinutes(3),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 3,
                        PeriodoInicio = DateTime.Now.AddMinutes(4),
                        PeriodoFim = DateTime.Now.AddMinutes(5),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 4,
                        PeriodoInicio = DateTime.Now.AddMinutes(6),
                        PeriodoFim = DateTime.Now.AddMinutes(7),
                    }
                 }
             }));
        }
    }
}