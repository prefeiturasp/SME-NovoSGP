using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Relatorio
{
    public class RelatorioAcompanhamentoAprendizagemUseCaseTeste
    {
        [Fact]
        public async Task Deve_Gerar_Relatorio_Acompanhamento_Aprendizagem()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new RelatorioAcompanhamentoAprendizagemUseCase(mediatorMock.Object);

            var filtro = new FiltroRelatorioAcompanhamentoAprendizagemDto
            {
                TurmaId = 1,
                AlunoCodigo = 12345,
                Semestre = 1,
                ComponenteCurricularId = 10
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
                c.TipoRelatorio == TipoRelatorio.AcompanhamentoAprendizagem &&
                c.Filtros == filtro &&
                c.Formato == TipoFormatoRelatorio.Html &&
                c.RotaRelatorio == RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosRelatorioAcompanhamentoAprendizagem
            ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(resultado);
        }
    }
}
