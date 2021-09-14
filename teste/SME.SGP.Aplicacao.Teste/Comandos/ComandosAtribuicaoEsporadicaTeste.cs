using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosAtribuicaoEsporadicaTeste
    {
        private readonly ComandosAtribuicaoEsporadica comandosAtribuicaoEsporadica;
        private readonly Mock<IRepositorioAtribuicaoEsporadica> repositorioAtribuicaoEsporadica;
        private readonly Mock<IServicoAtribuicaoEsporadica> servicoAtribuicaoEsporadica;

        public ComandosAtribuicaoEsporadicaTeste()
        {
            repositorioAtribuicaoEsporadica = new Mock<IRepositorioAtribuicaoEsporadica>();
            servicoAtribuicaoEsporadica = new Mock<IServicoAtribuicaoEsporadica>();
            comandosAtribuicaoEsporadica = new ComandosAtribuicaoEsporadica(repositorioAtribuicaoEsporadica.Object, servicoAtribuicaoEsporadica.Object);
        }

        [Fact(DisplayName = "Deve Excluir Atribuicao")]
        public async void Deve_Excluir_Atribuicao()
        {
            //ARRANGE
            var atribuicaoEsporadica = new AtribuicaoEsporadica() { Id = 1, Excluido = false };
            repositorioAtribuicaoEsporadica.Setup(a => a.ObterPorId(atribuicaoEsporadica.Id)).Returns(atribuicaoEsporadica);

            //ACT
            //await comandosAtribuicaoEsporadica.Excluir(1);

            //ASSERT
            repositorioAtribuicaoEsporadica.Verify(c => c.SalvarAsync(atribuicaoEsporadica), Times.Once);
        }

        [Fact(DisplayName = "Não Deve Excluir Atribuicao excluida")]
        public async Task Nao_Deve_Excluir_Atribuicao_Excluida()
        {
            //ARRANGE
            var atribuicaoEsporadica = new AtribuicaoEsporadica() { Id = 1, Excluido = true };
            repositorioAtribuicaoEsporadica.Setup(a => a.ObterPorId(atribuicaoEsporadica.Id)).Returns(atribuicaoEsporadica);

            //ACT & ASSERT
            //await Assert.ThrowsAsync<NegocioException>(() => comandosAtribuicaoEsporadica.Excluir(1));
        }
    }
}