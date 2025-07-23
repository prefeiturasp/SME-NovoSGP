using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ParecerConclusivo
{
    public class ReprocessarParecerConclusivoPorDreUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveEnviarComandoParaCadaUeERetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var comandosRecebidos = new List<PublicarFilaSgpCommand>();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUesCodigosPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<string> { "UE1", "UE2" });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((request, token) =>
                {
                    if (request is PublicarFilaSgpCommand cmd)
                        comandosRecebidos.Add(cmd);
                })
                .ReturnsAsync(true);

            var useCase = new ReprocessarParecerConclusivoPorDreUseCase(mediatorMock.Object);

            var anoLetivo = 2025;
            var dreId = 1;

            var filtro = new FiltroDreUeTurmaDto(anoLetivo, dreId, null);

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            foreach (var ueCodigo in new List<string> { "UE1", "UE2" })
            {
                var cmd = comandosRecebidos.FirstOrDefault(c =>
                    c.Rota == RotasRabbitSgpFechamento.RotaAtualizarParecerConclusivoAlunoPorUe &&
                    c.Filtros is FiltroDreUeTurmaDto filtroDto &&
                    filtroDto.AnoLetivo == anoLetivo &&
                    filtroDto.DreId == dreId &&
                    filtroDto.UeCodigo == ueCodigo
                );
                Assert.NotNull(cmd);
            }
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}
