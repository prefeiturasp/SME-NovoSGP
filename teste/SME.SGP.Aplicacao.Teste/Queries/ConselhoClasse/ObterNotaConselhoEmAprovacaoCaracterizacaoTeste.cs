using Moq;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ConselhoClasse
{
    public class ObterNotaConselhoEmAprovacaoCaracterizacaoTeste
    {
        [Theory]
        [InlineData(8.5)]
        [InlineData(3.0)]
        [InlineData(0.0)]
        [InlineData(null)]
        [InlineData(-1.0)]
        public async Task Deve_delegar_id_ao_repositorio_uma_vez_sem_transformar_retorno(double? valor)
        {
            var repositorio = new Mock<IRepositorioConselhoClasseNotaConsulta>(MockBehavior.Strict);
            repositorio.Setup(r => r.VerificaNotaConselhoEmAprovacao(1001)).ReturnsAsync(valor);
            var handler = new ObterNotaConselhoEmAprovacaoQueryHandler(repositorio.Object);

            var retorno = await handler.Handle(new ObterNotaConselhoEmAprovacaoQuery(1001), CancellationToken.None);

            Assert.Equal(valor, retorno);
            repositorio.Verify(r => r.VerificaNotaConselhoEmAprovacao(1001), Times.Once);
            repositorio.VerifyNoOtherCalls();
        }
    }
}
