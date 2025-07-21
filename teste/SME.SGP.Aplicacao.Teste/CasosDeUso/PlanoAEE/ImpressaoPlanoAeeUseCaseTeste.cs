using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ImpressaoPlanoAeeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ImpressaoPlanoAeeUseCase useCase;

        public ImpressaoPlanoAeeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ImpressaoPlanoAeeUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Disparar_Excecao_Quando_Usuario_Nao_Encontrado()
        {
            // Arrange
            var filtro = new FiltroRelatorioPlanoAeeDto();
            mediator.Setup(x => x.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Usuario)null);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
        }

        [Fact]
        public async Task Deve_Chamar_Mediator_Para_Gerar_Relatorio()
        {
            // Arrange
            var filtro = new FiltroRelatorioPlanoAeeDto();
            var usuarioLogado = new Usuario { Id = 1, Login = "teste", Nome = "Usuário Teste" };

            mediator.Setup(x => x.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            mediator.Setup(x => x.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.True(resultado);
            mediator.Verify(x => x.Send(
                It.Is<GerarRelatorioCommand>(c =>
                    c.TipoRelatorio == TipoRelatorio.PlanoAee &&
                    c.IdUsuarioLogado == usuarioLogado.Id &&
                    c.RotaRelatorio == RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosPlanoAee),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Falha_Ao_Gerar_Relatorio()
        {
            // Arrange
            var filtro = new FiltroRelatorioPlanoAeeDto();
            var usuarioLogado = new Usuario { Id = 1, Login = "teste", Nome = "Usuário Teste" };

            mediator.Setup(x => x.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            mediator.Setup(x => x.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.False(resultado);
        }
    }
}
