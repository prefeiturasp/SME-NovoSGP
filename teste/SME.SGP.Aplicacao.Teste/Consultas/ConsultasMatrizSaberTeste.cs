using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasMatrizSaberTeste
    {
        private readonly ConsultasMatrizSaber consultasMatrizSaber;
        private readonly Mock<IRepositorioMatrizSaber> repositorioMatrizSaber;

        public ConsultasMatrizSaberTeste()
        {
            repositorioMatrizSaber = new Mock<IRepositorioMatrizSaber>();

            consultasMatrizSaber = new ConsultasMatrizSaber(repositorioMatrizSaber.Object);
        }

        [Fact(DisplayName = "DeveDispararExcecaoAoInstanciarSemDependencias")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasMatrizSaber(null));
        }

        [Fact(DisplayName = "DeveListarAMatrizDeSaberes")]
        public void DeveListarAMatrizDeSaberes()
        {
            consultasMatrizSaber.Listar();
            repositorioMatrizSaber.Verify(c => c.Listar(), Times.Once);
        }
    }
}