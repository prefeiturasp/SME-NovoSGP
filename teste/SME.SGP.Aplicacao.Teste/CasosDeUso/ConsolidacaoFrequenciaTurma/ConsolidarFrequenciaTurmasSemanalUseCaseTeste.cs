using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurma
{
    public class ConsolidarFrequenciaTurmasSemanalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaTurmasSemanalUseCase useCase;

        public ConsolidarFrequenciaTurmasSemanalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarFrequenciaTurmasSemanalUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comando_Com_Filtro_Semanal()
        {
            var mensagem = new MensagemRabbit();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd => ComandoEhValido(cmd)),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        private bool ComandoEhValido(PublicarFilaSgpCommand cmd)
        {
            var filtro = cmd.Filtros as FiltroAnoDto;
            return cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasNoAno
                && filtro != null
                && filtro.TipoConsolidado == TipoConsolidadoFrequencia.Semanal;
        }
    }
}
