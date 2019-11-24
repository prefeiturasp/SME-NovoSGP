using Moq;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ConsultasAtribuicaoEsporadicaTeste
    {
        private readonly ConsultasAtribuicaoEsporadica consultasAtribuicaoEsporadica;
        private readonly Mock<IContextoAplicacao> contextoAplicacao;
        private readonly Mock<IRepositorioAtribuicaoEsporadica> repositorioAtribuicaoEsporadica;
        private readonly Mock<IServicoAtribuicaoEsporadica> servicoAtribuicaoEsporadica;
        private readonly Mock<IServicoEOL> servicoEol;

        public ConsultasAtribuicaoEsporadicaTeste()
        {
            repositorioAtribuicaoEsporadica = new Mock<IRepositorioAtribuicaoEsporadica>();
            servicoAtribuicaoEsporadica = new Mock<IServicoAtribuicaoEsporadica>();
            servicoEol = new Mock<IServicoEOL>();
            contextoAplicacao = new Mock<IContextoAplicacao>();
            consultasAtribuicaoEsporadica = new ConsultasAtribuicaoEsporadica(repositorioAtribuicaoEsporadica.Object, servicoEol.Object, contextoAplicacao.Object);
        }

        [Fact(DisplayName = "Deve Consultar Atribuicao")]
        public void Deve_Consultar_Atribuicao()
        {
            //ARRANGE
            var atribuicaoEsporadica = new AtribuicaoEsporadica() { Id = 1, Excluido = false };
            repositorioAtribuicaoEsporadica.Setup(a => a.ObterPorId(atribuicaoEsporadica.Id)).Returns(atribuicaoEsporadica);

            //ASSERT && //ACT
            Assert.NotNull(consultasAtribuicaoEsporadica.ObterPorId(1));
            repositorioAtribuicaoEsporadica.Verify(c => c.ObterPorId(1), Times.Once);
        }

        [Fact(DisplayName = "Não Deve Consultar Atribuição não existe")]
        public void Nao_Deve_Consultar_Atribuicao_Nao_Existe()
        {
            //ARRANGE
            var atribuicaoEsporadica = new AtribuicaoEsporadica() { Id = 1, Excluido = false };
            repositorioAtribuicaoEsporadica.Setup(a => a.ObterPorId(atribuicaoEsporadica.Id)).Returns(atribuicaoEsporadica);

            //ASSERT && //ACT
            Assert.Null(consultasAtribuicaoEsporadica.ObterPorId(2));
            repositorioAtribuicaoEsporadica.Verify(c => c.ObterPorId(2), Times.Once);
        }
    }
}