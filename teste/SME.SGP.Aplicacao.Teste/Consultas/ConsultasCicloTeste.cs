using Moq;
using SME.SGP.Dominio.Interfaces;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasCicloTeste
    {
        private readonly ConsultasCiclo consultasCiclo;
        private readonly Mock<IRepositorioCiclo> repositorioCiclo;

        public ConsultasCicloTeste()
        {
            repositorioCiclo = new Mock<IRepositorioCiclo>();

            consultasCiclo = new ConsultasCiclo(repositorioCiclo.Object);
        }

        [Fact(DisplayName = "DeveObterCicloPorAno")]
        public void DeveObterCicloPorTurmas()
        {
            consultasCiclo.Selecionar(1);
            repositorioCiclo.Verify(c => c.ObterCicloPorAno(1), Times.Once);
        }
    }
}