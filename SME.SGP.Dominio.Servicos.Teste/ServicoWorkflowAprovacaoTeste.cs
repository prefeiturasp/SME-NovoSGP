using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoWorkflowAprovacaoTeste
    {
        private readonly Mock<IRepositorioNotificacao> repositorioNotificacao;
        private readonly Mock<IRepositorioWorkflowAprovacaoNivel> repositorioWorkflowAprovacaoNivel;
        private readonly Mock<IRepositorioWorkflowAprovacaoNivelNotificacao> repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly Mock<IServicoEOL> servicoEOL;
        private readonly Mock<IServicoNotificacao> servicoNotificacao;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly ServicoWorkflowAprovacao servicoWorkflowAprovacao;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ServicoWorkflowAprovacaoTeste()
        {
            repositorioNotificacao = new Mock<IRepositorioNotificacao>();
            repositorioWorkflowAprovacaoNivelNotificacao = new Mock<IRepositorioWorkflowAprovacaoNivelNotificacao>();
            servicoEOL = new Mock<IServicoEOL>();
            servicoNotificacao = new Mock<IServicoNotificacao>();
            servicoUsuario = new Mock<IServicoUsuario>();
            unitOfWork = new Mock<IUnitOfWork>();
            repositorioWorkflowAprovacaoNivel = new Mock<IRepositorioWorkflowAprovacaoNivel>();

            servicoWorkflowAprovacao = new ServicoWorkflowAprovacao(unitOfWork.Object, repositorioNotificacao.Object, repositorioWorkflowAprovacaoNivelNotificacao.Object,
                servicoEOL.Object, servicoUsuario.Object, servicoNotificacao.Object, repositorioWorkflowAprovacaoNivel.Object);
        }

        [Fact]
        public void Deve_()
        {
        }
    }
}