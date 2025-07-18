using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AulaInfantilControllerTeste
    {
        private readonly AulaInfantilController _controller;
        private readonly Mock<IMediator> _mediator = new();
        private readonly Faker _faker;

        public AulaInfantilControllerTeste()
        {
            _controller = new AulaInfantilController(_mediator.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task DeveRetornarOk_QuandoSincronizarAulasTurmaForChamado()
        {
            // Arrange
            PublicarFilaSgpCommand comandoCapturado = null;

            _mediator
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, _) => comandoCapturado = (PublicarFilaSgpCommand)cmd)
                .ReturnsAsync(true);

            long? codigoTurma = 456;

            // Act
            var resultado = await _controller.SincronizarAulasTurma(codigoTurma);

            // Assert
            Assert.IsType<OkResult>(resultado);
            Assert.NotNull(comandoCapturado);
            Assert.Equal(RotasRabbitSgpAula.RotaSincronizarAulasInfantil, comandoCapturado.Rota);
            Assert.Equal(codigoTurma.ToString(), ((DadosCriacaoAulasAutomaticasCarregamentoDto)comandoCapturado.Filtros).CodigoTurma);
        }
    }
}
