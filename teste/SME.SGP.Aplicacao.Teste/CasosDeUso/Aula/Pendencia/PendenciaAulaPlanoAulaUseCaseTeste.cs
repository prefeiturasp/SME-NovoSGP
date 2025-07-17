using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.Pendencia
{
    public class PendenciaAulaPlanoAulaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly PendenciaAulaPlanoAulaUseCase useCase;

        public PendenciaAulaPlanoAulaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new PendenciaAulaPlanoAulaUseCase(mediatorMock.Object);
        }

        private MensagemRabbit CriarMensagemRabbitComObjeto<T>(T obj)
        {
            var mensagem = new MensagemRabbit();
            mensagem.Mensagem = JsonConvert.SerializeObject(obj);
            return mensagem;
        }

        [Fact]
        public async Task Executar_DeveProcessarPendenciasComAulas()
        {
            // Arrange
            var filtro = new DreUeDto(dreId: 1, ueId: 2);
            var aulas = new List<SME.SGP.Dominio.Aula> { new SME.SGP.Dominio.Aula(), new SME.SGP.Dominio.Aula() };

            var mensagemRabbit = CriarMensagemRabbitComObjeto(filtro);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(aulas);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(aulas);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<long>());

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPendenciaAulasPorTipoCommand>(), It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(Unit.Value));

            mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPendenciaAulasPorTipoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveExcluirPendenciasQuandoExistiremParaInfantil()
        {
            // Arrange
            var filtro = new DreUeDto(dreId: 1, ueId: 2);
            var aulas = new List<SME.SGP.Dominio.Aula>();

            var mensagemRabbit = CriarMensagemRabbitComObjeto(filtro);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(aulas);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(aulas);

            // Simula pendências para infantil
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<long> { 1, 2 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        
    }
}
