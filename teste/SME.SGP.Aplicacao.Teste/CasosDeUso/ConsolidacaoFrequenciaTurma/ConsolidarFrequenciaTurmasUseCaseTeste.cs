using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurma
{
    public class ConsolidarFrequenciaTurmasUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaTurmasUseCase useCase;

        public ConsolidarFrequenciaTurmasUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarFrequenciaTurmasUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_Se_Parametro_Execucao_False()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = false });

            var mensagem = new MensagemRabbit();

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<LimparConsolidacaoFrequenciaTurmasPorAnoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Executar_Consolidacao_Quando_Parametro_Ativo()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<LimparConsolidacaoFrequenciaTurmasPorAnoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExecutarConsolidacaoFrequenciaNoAnoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var mensagem = new MensagemRabbit();

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<LimparConsolidacaoFrequenciaTurmasPorAnoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExecutarConsolidacaoFrequenciaNoAnoCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}
