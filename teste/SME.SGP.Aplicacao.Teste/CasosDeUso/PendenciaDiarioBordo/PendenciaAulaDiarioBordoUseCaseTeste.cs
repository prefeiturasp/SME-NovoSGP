using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class PendenciaAulaDiarioBordoUseCaseTeste
    {
        private readonly PendenciaAulaDiarioBordoUseCase pendenciaAulaDiarioBordoUseCase;
        private readonly Mock<IMediator> mediator;

        public PendenciaAulaDiarioBordoUseCaseTeste()
        {

            mediator = new Mock<IMediator>();
            pendenciaAulaDiarioBordoUseCase = new PendenciaAulaDiarioBordoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Publicar_Fila_Por_Turma()
        {
            var turmas = new List<TurmaDTO>
            {
                new TurmaDTO()
                {
                    TurmaCodigo = "123123",
                    TurmaId = 111
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(turmas);

            mediator.Setup(a => a.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(true);
            // act
            var anotacao = await pendenciaAulaDiarioBordoUseCase.Executar(new MensagemRabbit("{ 'mensagem': { 'dreId': 4, 'ueId': 101,'CodigoUe':'1'} }"));

        // assert
        Assert.True(anotacao, "Mensagem publicada na fila corretamente");
        }
    }
}
