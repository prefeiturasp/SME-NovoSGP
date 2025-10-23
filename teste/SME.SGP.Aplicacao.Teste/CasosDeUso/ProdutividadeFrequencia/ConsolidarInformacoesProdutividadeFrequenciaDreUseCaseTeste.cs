using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ProdutividadeFrequencia
{
    public class ConsolidarInformacoesProdutividadeFrequenciaDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarInformacoesProdutividadeFrequenciaDreUseCase useCase;

        public ConsolidarInformacoesProdutividadeFrequenciaDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarInformacoesProdutividadeFrequenciaDreUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator", () => new ConsolidarInformacoesProdutividadeFrequenciaDreUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Dre_Possui_Ues_Deve_Publicar_Na_Fila_Para_Cada_Ue()
        {
            var filtroInicial = new FiltroIdAnoLetivoDto(10, DateTime.Now);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtroInicial));
            var uesIds = new List<long> { 101, 102 };

            mediatorMock.Setup(m => m.Send(
                It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == filtroInicial.Id),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesIds);

            mediatorMock.Setup(m => m.Send(
                It.IsAny<PublicarFilaSgpCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == filtroInicial.Id),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(c =>
                    c.Rota == RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaUe),
                It.IsAny<CancellationToken>()), Times.Exactly(uesIds.Count));
        }

        [Fact]
        public async Task Executar_Quando_Dre_Nao_Possui_Ues_Deve_Nao_Publicar_Na_Fila_E_Retornar_True()
        {
            var filtroInicial = new FiltroIdAnoLetivoDto(20, DateTime.Now);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtroInicial));
            var uesIds = new List<long>();

            mediatorMock.Setup(m => m.Send(
                It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == filtroInicial.Id),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesIds);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == filtroInicial.Id),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.IsAny<PublicarFilaSgpCommand>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
