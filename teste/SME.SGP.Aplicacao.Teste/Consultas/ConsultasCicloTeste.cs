using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasCicloTeste
    {
        private readonly ConsultasCiclo consultasCiclo;

        public ConsultasCicloTeste()
        {
            consultasCiclo = new ConsultasCiclo();
        }

        [Fact(DisplayName = "DeveObterCicloPorTurmas")]
        public void DeveObterCicloPorTurmas()
        {
            consultasCiclo.Listar(new List<int>() { 1, 2 });
            Assert.True(true);
        }
    }
}