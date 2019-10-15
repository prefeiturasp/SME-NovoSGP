using Moq;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Aplicacao.Interfaces.Consultas;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasPeriodoEscolarTeste
    {
        private readonly IConsultasPeriodoEscolar consultas;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorio;

        public ConsultasPeriodoEscolarTeste()
        {
            repositorio = new Mock<IRepositorioPeriodoEscolar>();

            consultas = new ConsultasPeriodoEscolar(repositorio.Object);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Se_Instanciar_Sem_Dependencia")]
        public void Deve_Disparar_Excecao_Se_Instanciar_Sem_Dependencia()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasPeriodoEscolar(null));
        }

        [Fact(DisplayName ="Deve_Consultar_Periodo_Escolar")]
        public void Deve_Consultar_Periodo_Escolar()
        {
            consultas.ObterPorTipoCalendario(1);
            repositorio.Verify(r => r.ObterPorTipoCalendario(It.IsAny<long>()));
        }
    }
}
