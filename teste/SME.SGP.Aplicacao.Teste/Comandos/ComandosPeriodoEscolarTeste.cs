using Moq;
using SME.SGP.Aplicacao.Comandos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosPeriodoEscolarTeste
    {
        private readonly ComandosPeriodoEscolar comandosPeriodoEscolar;
        private readonly Mock<IConsultasTipoCalendario> consultasTipoCalendario;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodo;

        public ComandosPeriodoEscolarTeste()
        {
            consultasTipoCalendario = new Mock<IConsultasTipoCalendario>();
            unitOfWork = new Mock<IUnitOfWork>();
            repositorioPeriodo = new Mock<IRepositorioPeriodoEscolar>();

            comandosPeriodoEscolar = new ComandosPeriodoEscolar(unitOfWork.Object, consultasTipoCalendario.Object, repositorioPeriodo.Object);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias")]
        public void Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(null, consultasTipoCalendario.Object, repositorioPeriodo.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(unitOfWork.Object, null, repositorioPeriodo.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPeriodoEscolar(unitOfWork.Object, consultasTipoCalendario.Object, null));
        }

        [Fact(DisplayName = "Deve_Salvar_Periodo_Escolar")]
        public void Deve_Salvar_Periodo_Escolar()
        {
            consultasTipoCalendario.Setup(x => x.BuscarPorId(It.IsAny<long>())).Returns(new TipoCalendarioCompletoDto
            {
                Id = 1,
                Modalidade = Modalidade.Fundamental,
                Situacao = true,
                Periodo = Periodo.Anual,
                Nome = "Teste",
                AnoLetivo = 2019
            });

            

            comandosPeriodoEscolar.Salvar(new PeriodoEscolarListaDto
            {
                AnoBase = 2019,
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
    }
}
