using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // arrange
            var ues = new List<string>();
            ues.Add("019375");


            mediator.Setup(a => a.Send(It.IsAny<ObterUesCodigosPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            var turmas = new List<TurmaDTO>();
            turmas.Add(new TurmaDTO() { 
                TurmaCodigo = "2387335",
                TurmaId = 1527109
            });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(turmas);

            mediator.Setup(a => a.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(true);
            // act
            var anotacao = await pendenciaAulaDiarioBordoUseCase.Executar(new MensagemRabbit("{ 'DreId':'4' }"));

            // assert
            Assert.True(anotacao, "Mensagem publicada na fila corretamente");
        }
    }
}
