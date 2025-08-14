using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Relatorio
{
    public class GerarRelatorioRegistroIndividualUseCaseTeste
    {
        [Fact]
        public async Task Deve_Gerar_Relatorio_Registro_Individual()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new RelatorioRegistroIndividualUseCase(mediatorMock.Object);

            var filtro = new FiltroRelatorioRegistroIndividualDto
            {
                TurmaId = 1,
                AlunoCodigo = 12345,
                DataInicio = new DateTime(2024, 1, 1),
                DataFim = new DateTime(2024, 6, 30)
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
                c.TipoRelatorio == TipoRelatorio.RegistroIndividual &&
                c.Filtros == filtro &&
                c.RotaRelatorio == RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosRegistroIndividual
            ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(usuarioLogado.Nome, filtro.UsuarioNome);
            Assert.Equal(usuarioLogado.CodigoRf, filtro.UsuarioRF);
            Assert.True(resultado);
        }
    }
}
