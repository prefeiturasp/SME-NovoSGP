using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ParametrosAnoAnterior
{
    public class ReplicarParametrosAnoAnteriorUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ReplicarParametrosAnoAnteriorUseCase _useCase;

        public ReplicarParametrosAnoAnteriorUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ReplicarParametrosAnoAnteriorUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Replicacao_Falha_Deve_Lancar_Negocio_Exception()
        {
            var filtro = new FiltroInclusaoParametrosAnoAnterior
            {
                AnoLetivo = 2023,
                ModalidadeTipoCalendario = ModalidadeTipoCalendario.EJA
            };

            var mensagemJson = JsonConvert.SerializeObject(filtro);
            var mensagemRabbit = new MensagemRabbit(mensagemJson);

            _mediatorMock.Setup(m => m.Send(It.Is<ReplicarParametrosAnoAnteriorCommand>(c =>
                c.AnoLetivo == filtro.AnoLetivo &&
                c.ModalidadeTipoCalendario == filtro.ModalidadeTipoCalendario), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal($"Não foi possível replicar para o ano {filtro.AnoLetivo} e a modalidade {filtro.ModalidadeTipoCalendario.Name()}", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ReplicarParametrosAnoAnteriorCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Replicacao_Sucesso_Deve_Retornar_True()
        {
            var filtro = new FiltroInclusaoParametrosAnoAnterior
            {
                AnoLetivo = 2024,
                ModalidadeTipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            var mensagemJson = JsonConvert.SerializeObject(filtro);
            var mensagemRabbit = new MensagemRabbit(mensagemJson);

            _mediatorMock.Setup(m => m.Send(It.Is<ReplicarParametrosAnoAnteriorCommand>(c =>
                c.AnoLetivo == filtro.AnoLetivo &&
                c.ModalidadeTipoCalendario == filtro.ModalidadeTipoCalendario), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ReplicarParametrosAnoAnteriorCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
