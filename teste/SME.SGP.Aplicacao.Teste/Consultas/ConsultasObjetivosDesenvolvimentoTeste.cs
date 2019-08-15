using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasObjetivosDesenvolvimentoTeste
    {
        private readonly ConsultasObjetivoDesenvolvimento consultasObjetivoDesenvolvimento;
        private readonly Mock<IRepositorioObjetivoDesenvolvimento> repositorioObjetivosDesenvolvimento;

        public ConsultasObjetivosDesenvolvimentoTeste()
        {
            repositorioObjetivosDesenvolvimento = new Mock<IRepositorioObjetivoDesenvolvimento>();

            consultasObjetivoDesenvolvimento = new ConsultasObjetivoDesenvolvimento(repositorioObjetivosDesenvolvimento.Object);
        }

        [Fact(DisplayName = "DeveDispararExcecaoAoInstanciarSemDependencias")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasObjetivoDesenvolvimento(null));
        }

        [Fact(DisplayName = "DeveListarAMatrizDeSaberes")]
        public void DeveListarAMatrizDeSaberes()
        {
            consultasObjetivoDesenvolvimento.Listar();
            repositorioObjetivosDesenvolvimento.Verify(c => c.Listar(), Times.Once);
        }
    }
}