using Moq;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasPeriodoEscolarTeste
    {
        private readonly IConsultasPeriodoEscolar consultas;
        private readonly Mock<IConsultasPeriodoFechamento> consultasPeriodoFechamento; 
        private readonly Mock<IConsultasTipoCalendario> consultasTipoCalendario;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorio;

        public ConsultasPeriodoEscolarTeste()
        {
            repositorio = new Mock<IRepositorioPeriodoEscolar>();
            consultasPeriodoFechamento = new Mock<IConsultasPeriodoFechamento>();
            consultasTipoCalendario = new Mock<IConsultasTipoCalendario>();

            consultas = new ConsultasPeriodoEscolar(repositorio.Object, consultasPeriodoFechamento.Object, consultasTipoCalendario.Object);
        }

        [Fact(DisplayName = "Deve_Consultar_Periodo_Escolar")]
        public void Deve_Consultar_Periodo_Escolar()
        {
            repositorio.Setup(r => r.ObterPorTipoCalendario(It.IsAny<long>())).Returns(new List<PeriodoEscolar>());

            consultas.ObterPorTipoCalendario(1);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Se_Instanciar_Sem_Dependencia")]
        public void Deve_Disparar_Excecao_Se_Instanciar_Sem_Dependencia()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasPeriodoEscolar(null, null, null));
        }
    }
}