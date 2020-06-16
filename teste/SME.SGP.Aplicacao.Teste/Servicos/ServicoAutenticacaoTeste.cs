using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Servicos
{
    public class ServicoAutenticacaoTeste
    {
        private readonly ServicoAutenticacao servicoAutenticacao;
        private readonly Mock<IServicoEol> servicoEol;

        public ServicoAutenticacaoTeste()
        {
            servicoEol = new Mock<IServicoEol>();
            servicoAutenticacao = new ServicoAutenticacao(servicoEol.Object);
        }

        [Fact]
        public async Task DeveAlterarSenhaComSucesso()
        {
            servicoEol.Setup(c => c.Autenticar(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UsuarioEolAutenticacaoRetornoDto
                {
                    CodigoRf = "123",
                    Status = AutenticacaoStatusEol.Ok
                }));

            servicoEol.Setup(c => c.AlterarSenha(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new AlterarSenhaRespostaDto
                {
                    StatusRetorno = 200,
                    SenhaAlterada = true
                }));

            await servicoAutenticacao.AlterarSenha("123", "456", "789");
            servicoEol.Verify(c => c.Autenticar(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            servicoEol.Verify(c => c.AlterarSenha(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(true);
        }

        [Fact]
        public async Task NaoDeveAlterarSenhaComSenhaAnteriorReutilizada()
        {
            servicoEol.Setup(c => c.Autenticar(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UsuarioEolAutenticacaoRetornoDto
                {
                    CodigoRf = "123",
                    Status = AutenticacaoStatusEol.Ok,
                }));

            servicoEol.Setup(c => c.AlterarSenha(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new AlterarSenhaRespostaDto
                {
                    StatusRetorno = 200,
                    SenhaAlterada = false,
                    Mensagem = "Senha já utilizada"
                }));

            var erro = await Assert.ThrowsAsync<NegocioException>(async () => await servicoAutenticacao.AlterarSenha("123", "456", "789"));

            Assert.Equal("Senha já utilizada", erro.Message);
            servicoEol.Verify(c => c.Autenticar(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            servicoEol.Verify(c => c.AlterarSenha(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task NaoDeveAlterarSenhaComSenhaAtualIncorreta()
        {
            servicoEol.Setup(c => c.Autenticar(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UsuarioEolAutenticacaoRetornoDto
                {
                    CodigoRf = "123",
                    Status = AutenticacaoStatusEol.SenhaPadrao,
                }));

            servicoEol.Setup(c => c.AlterarSenha(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new AlterarSenhaRespostaDto
                {
                    StatusRetorno = 200,
                    SenhaAlterada = true
                }));

            await Assert.ThrowsAsync<NegocioException>(async () => await servicoAutenticacao.AlterarSenha("123", "456", "789"));
            servicoEol.Verify(c => c.Autenticar(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            servicoEol.Verify(c => c.AlterarSenha(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}