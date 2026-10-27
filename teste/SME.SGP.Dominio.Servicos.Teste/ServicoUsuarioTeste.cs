using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoUsuarioTeste
    {
        private readonly ServicoUsuario servicoUsuario;
        private readonly Mock<IMediator> mediator;

        public ServicoUsuarioTeste()
        {
            mediator = new Mock<IMediator>();

            servicoUsuario = new ServicoUsuario(new Mock<IRepositorioUsuario>().Object, new Mock<IRepositorioPrioridadePerfil>().Object,
                new Mock<IUnitOfWork>().Object, new Mock<IContextoAplicacao>().Object, new Mock<IRepositorioCache>().Object,
                new Mock<IRepositorioAtribuicaoCJ>().Object, mediator.Object);
        }

        [Fact(DisplayName = "ServicoUsuario - Codigo RF nulo deve buscar usuário pelo login sem lançar exceção")]
        public async Task Codigo_rf_nulo_deve_buscar_usuario_pelo_login()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterUsuarioPorCodigoRfLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { Login = "45773413876", Nome = "Usuario" });

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(null, "45773413876");

            Assert.NotNull(usuario);
            mediator.Verify(m => m.Send(It.Is<ObterUsuarioPorCodigoRfLoginQuery>(q => q.CodigoRf == null && q.Login == "45773413876"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "ServicoUsuario - CPF formatado deve ser considerado sem pontuação")]
        public async Task Cpf_formatado_deve_ser_considerado_sem_pontuacao()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterUsuarioPorCodigoRfLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "45773413876", Login = "45773413876", Nome = "Usuario" });

            await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona("457.734.138-76", "45773413876");

            mediator.Verify(m => m.Send(It.Is<ObterUsuarioPorCodigoRfLoginQuery>(q => q.CodigoRf == "45773413876"),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
