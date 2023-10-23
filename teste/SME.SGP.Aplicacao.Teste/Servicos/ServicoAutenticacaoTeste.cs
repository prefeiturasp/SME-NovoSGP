using System.Threading;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;
using MediatR;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Servicos
{
    public class ServicoAutenticacaoTeste
    {
        private readonly ServicoAutenticacao servicoAutenticacao;
        private readonly Mock<IMediator> mediator;

        public ServicoAutenticacaoTeste()
        {
            mediator = new Mock<IMediator>();
            servicoAutenticacao = new ServicoAutenticacao(mediator.Object);
        }

        [Fact]
        public async Task DeveAlterarSenhaComSucesso()
        {
            mediator.Setup(x => x.Send(It.IsAny<AutenticarQuery>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AutenticacaoApiEolDto
                {
                    CodigoRf = "123",
                    Status = AutenticacaoStatusEol.Ok
                });
            
            mediator.Setup(x => x.Send(It.IsAny<AlterarSenhaUsuarioCommand>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AlterarSenhaRespostaDto
                {
                    StatusRetorno = 200,
                    SenhaAlterada = true
                });

            await servicoAutenticacao.AlterarSenha("123", "456", "789");
            Assert.True(true);
        }

        [Fact]
        public async Task NaoDeveAlterarSenhaComSenhaAnteriorReutilizada()
        {
            mediator.Setup(x => x.Send(It.IsAny<AutenticarQuery>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AutenticacaoApiEolDto
                {
                    CodigoRf = "123",
                    Status = AutenticacaoStatusEol.Ok,
                });

            mediator.Setup(x => x.Send(It.IsAny<AlterarSenhaUsuarioCommand>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AlterarSenhaRespostaDto
                {
                    StatusRetorno = 200,
                    SenhaAlterada = false,
                    Mensagem = "Senha já utilizada"
                });

            var erro = await Assert.ThrowsAsync<NegocioException>(async () => await servicoAutenticacao.AlterarSenha("123", "456", "789"));
            Assert.Equal("Senha já utilizada", erro.Message);
        }

        [Fact]
        public async Task NaoDeveAlterarSenhaComSenhaAtualIncorreta()
        {
            mediator.Setup(x => x.Send(It.IsAny<AutenticarQuery>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AutenticacaoApiEolDto
                {
                    CodigoRf = "123",
                    Status = AutenticacaoStatusEol.SenhaPadrao,
                });

            mediator.Setup(x => x.Send(It.IsAny<AlterarSenhaUsuarioCommand>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AlterarSenhaRespostaDto
                {
                    StatusRetorno = 200,
                    SenhaAlterada = true
                });

            await Assert.ThrowsAsync<NegocioException>(async () => await servicoAutenticacao.AlterarSenha("123", "456", "789"));
        }
    }
}