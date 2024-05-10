using MediatR;
using Moq;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ConsultasAtribuicaoEsporadicaTeste
    {
        private readonly ConsultasAtribuicaoEsporadica consultasAtribuicaoEsporadica;
        private readonly Mock<IContextoAplicacao> contextoAplicacao;
        private readonly Mock<IRepositorioAtribuicaoEsporadica> repositorioAtribuicaoEsporadica;
        private readonly Mock<IServicoAtribuicaoEsporadica> servicoAtribuicaoEsporadica;
        private readonly Mock<IMediator> mediator;

        public ConsultasAtribuicaoEsporadicaTeste()
        {
            repositorioAtribuicaoEsporadica = new Mock<IRepositorioAtribuicaoEsporadica>();
            servicoAtribuicaoEsporadica = new Mock<IServicoAtribuicaoEsporadica>();
            mediator = new Mock<IMediator>();
            contextoAplicacao = new Mock<IContextoAplicacao>();
            consultasAtribuicaoEsporadica = new ConsultasAtribuicaoEsporadica(repositorioAtribuicaoEsporadica.Object, contextoAplicacao.Object,mediator.Object);
        }

        [Fact(DisplayName = "Deve Consultar Atribuicao")]
        public void Deve_Consultar_Atribuicao()
        {
            //ARRANGE
            var atribuicaoEsporadica = new AtribuicaoEsporadica() { Id = 1, Excluido = false };
            repositorioAtribuicaoEsporadica.Setup(a => a.ObterPorIdAsync(atribuicaoEsporadica.Id)).Returns(Task.FromResult(atribuicaoEsporadica));

            //ASSERT && //ACT
            Assert.NotNull(consultasAtribuicaoEsporadica.ObterPorId(1));
            repositorioAtribuicaoEsporadica.Verify(c => c.ObterPorIdAsync(1), Times.Once);
        }

        [Fact(DisplayName = "Não Deve Consultar Atribuição não existe")]
        public async Task Nao_Deve_Consultar_Atribuicao_Nao_Existe()
        {
            //ARRANGE
            var atribuicaoEsporadica = new AtribuicaoEsporadica() { Id = 1, Excluido = false };
            repositorioAtribuicaoEsporadica.Setup(a => a.ObterPorIdAsync(atribuicaoEsporadica.Id)).Returns(Task.FromResult(atribuicaoEsporadica));

            //ASSERT && //ACT
            var resultado = await consultasAtribuicaoEsporadica.ObterPorId(2);
            Assert.Null(resultado);
            repositorioAtribuicaoEsporadica.Verify(c => c.ObterPorIdAsync(2).Result, Times.Once);
        }
    }
}