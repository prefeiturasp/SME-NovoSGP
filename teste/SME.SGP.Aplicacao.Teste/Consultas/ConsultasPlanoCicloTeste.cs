using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasPlanoCicloTeste
    {
        private readonly ConsultasPlanoCiclo consultasPlanoCiclo;
        private readonly Mock<IRepositorioPlanoCiclo> repositorioPlanoCiclo;

        public ConsultasPlanoCicloTeste()
        {
            repositorioPlanoCiclo = new Mock<IRepositorioPlanoCiclo>();

            consultasPlanoCiclo = new ConsultasPlanoCiclo(repositorioPlanoCiclo.Object);
        }

        [Fact(DisplayName = "DeveDispararExcecaoAoInstanciarSemDependencias")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasPlanoCiclo(null));
        }

        [Fact(DisplayName = "DeveObterPlanoCicloPorAnoCicloEEscola")]
        public void DeveObterPlanoCicloPorAnoCicloEEscola()
        {
            consultasPlanoCiclo.ObterPorAnoCicloEEscola(2019, 1, "1");
            repositorioPlanoCiclo.Verify(c => c.ObterPlanoCicloComMatrizesEObjetivos(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }
    }
}