using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class InserirFechamentoTurmaDisciplinaUseCaseTeste
    {
        [Fact]
        public async Task Deve_Executar_E_Publicar_Consolidacao_Quando_Nao_Estiver_Em_Aprovacao()
        {
            var fechamentoTurma = new FechamentoFinalTurmaDisciplinaDto
            {
                TurmaId = "1234"
            };

            var auditoriaRetorno = new AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto
            {
                EmAprovacao = false
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auditoriaRetorno);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            var useCase = new InserirFechamentoTurmaDisciplinaUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(fechamentoTurma);

            Assert.NotNull(resultado);
            Assert.False(resultado.EmAprovacao);

            mediatorMock.Verify(m => m.Send(
                It.Is<SalvarFechamentoCommand>(cmd => cmd.FechamentoFinalTurmaDisciplina == fechamentoTurma),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd => VerificarPublicarFilaSgpCommand(cmd)),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Nao_Deve_Publicar_Consolidacao_Quando_Estiver_Em_Aprovacao()
        {
            var fechamentoTurma = new FechamentoFinalTurmaDisciplinaDto
            {
                TurmaId = "1234"
            };

            var auditoriaRetorno = new AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto
            {
                EmAprovacao = true
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auditoriaRetorno);

            var useCase = new InserirFechamentoTurmaDisciplinaUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(fechamentoTurma);

            Assert.NotNull(resultado);
            Assert.True(resultado.EmAprovacao);

            mediatorMock.Verify(m => m.Send(
                It.IsAny<PublicarFilaSgpCommand>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }
        private static bool VerificarPublicarFilaSgpCommand(PublicarFilaSgpCommand cmd)
        {
            if (cmd == null)
                return false;

            var dto = cmd.Filtros as ConsolidacaoTurmaDto;
            if (dto == null)
                return false;

            return dto.TurmaId == 1234 && cmd.Rota == RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync;
        }
    }
}
