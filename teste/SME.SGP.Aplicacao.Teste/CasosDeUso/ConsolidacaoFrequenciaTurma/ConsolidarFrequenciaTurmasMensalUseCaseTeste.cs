using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurma
{
    public class ConsolidarFrequenciaTurmasMensalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaTurmasMensalUseCase useCase;

        public ConsolidarFrequenciaTurmasMensalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarFrequenciaTurmasMensalUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comando_De_Consolidacao_Mensal()
        {

            var filtro = new FiltroAnoDto(DateTime.Now, TipoConsolidadoFrequencia.Mensal);

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            mediatorMock
                .Setup(x => x.Send(It.IsAny<IRequest<Unit>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(x => x.Send(
            It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasNoAno &&
                cmd.Filtros != null &&
                (cmd.Filtros as FiltroAnoDto) != null &&
                ((FiltroAnoDto)cmd.Filtros).Data.Date == filtro.Data.Date
            ), It.IsAny<CancellationToken>()),
            Times.Once);
        }
    }
}
