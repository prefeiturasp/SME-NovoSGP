using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class RemoverPendenciasNoFinalDoAnoLetivoPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly RemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase useCase;

        public RemoverPendenciasNoFinalDoAnoLetivoPorUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new RemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Se_Mensagem_For_Nula()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = null };

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                useCase.Executar(mensagemRabbit));
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Sem_Executar_Nada_Se_DreId_For_Menor_Ou_Igual_A_Zero()
        {
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(2024, 0);
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Uma_Mensagem_Por_Ue_Se_DreId_Valido()
        {
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(2024, 1);
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var ues = new List<Ue>
            {
                new Ue { CodigoUe = "UE001" },
                new Ue { CodigoUe = "UE002" }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterUesPorDreCodigoQuery>(q => q.DreId == filtro.DreId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUesPorDreCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Query_Mas_Nao_Publicar_Mensagem_Se_Nao_Houver_Ues()
        {
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(2024, 1);
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterUesPorDreCodigoQuery>(q => q.DreId == filtro.DreId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Ue>());

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUesPorDreCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
