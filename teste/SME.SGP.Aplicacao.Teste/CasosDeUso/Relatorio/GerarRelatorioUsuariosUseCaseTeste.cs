using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Relatorio
{
    public class GerarRelatorioUsuariosUseCaseTeste
    {
        [Fact]
        public async Task Deve_Gerar_Relatorio_Usuarios()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new RelatorioUsuariosUseCase(mediatorMock.Object);

            var filtro = new FiltroRelatorioUsuarios
            {
                CodigoDre = "DRE01",
                CodigoUe = "UE01",
                UsuarioRf = "123456",
                Perfis = new[] { "Perfil1", "Perfil2" },
                Situacoes = new[] { 1, 2 },
                DiasSemAcesso = 10,
                ExibirHistorico = true
            };

            var usuarioLogado = new Usuario
            {
                Nome = "Usuário Teste",
                CodigoRf = "654321"
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(filtro);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.Is<GerarRelatorioCommand>(c =>
                c.TipoRelatorio == TipoRelatorio.Usuarios &&
                c.Filtros == filtro &&
                c.RotaRelatorio == RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosUsuarios
            ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(usuarioLogado.Nome, filtro.NomeUsuario);
            Assert.True(resultado);
        }
    }
}
