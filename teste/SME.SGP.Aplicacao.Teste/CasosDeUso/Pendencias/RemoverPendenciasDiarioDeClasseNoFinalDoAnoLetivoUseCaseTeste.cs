using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase _useCase;

        public RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Se_Mensagem_For_Nula()
        {
            var mensagem = new MensagemRabbit { Mensagem = null };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Se_Codigo_Ue_For_Nulo_Ou_Vazio()
        {
            var dto = new FiltroRemoverPendenciaFinalAnoLetivoDto(2024, 1, null);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Executar_Consultas_Mas_Nao_Publicar_Se_Nao_Houver_Pendencias()
        {
            var dto = new FiltroRemoverPendenciaFinalAnoLetivoDto(2024, 1, "UE123");
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<IEnumerable<long>>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<long>());

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => true), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Mensagem_SeHouver_Pendencias()
        {
            var dto = new FiltroRemoverPendenciaFinalAnoLetivoDto(2024, 1, "UE123");
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdsPendenciaAulaPorAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<long> { 1 });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdsPendenciaDiarioBordoPorAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<long> { 2 });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdsPendenciaIndividualPorAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<long> { 3 });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdsPendenciaDevolutivaPorAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<long> { 4 });

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                c => c.Rota == RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivo
                     && ((List<long>)c.Filtros).Count == 4
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
