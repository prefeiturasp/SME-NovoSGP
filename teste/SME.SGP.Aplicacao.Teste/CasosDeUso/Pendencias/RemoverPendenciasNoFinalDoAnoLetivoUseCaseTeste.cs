using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class RemoverPendenciasNoFinalDoAnoLetivoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly RemoverPendenciasNoFinalDoAnoLetivoUseCase _useCase;

        public RemoverPendenciasNoFinalDoAnoLetivoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new RemoverPendenciasNoFinalDoAnoLetivoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Enviar_Comando_Excluir_Pendencias_Quando_Existirem_Ids()
        {
            var ids = new List<long> { 1, 2, 3 };
            var mensagem = new MensagemRabbit
            {
                Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(ids)
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m =>
                m.Send(It.Is<ExcluirPendenciasPorIdsCommand>(cmd =>
                    cmd.PendenciasIds.Length == 3 &&
                    cmd.PendenciasIds[0] == 1 &&
                    cmd.PendenciasIds[1] == 2 &&
                    cmd.PendenciasIds[2] == 3),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Executar_Nao_Deve_Enviar_Comando_Se_Lista_Estiver_Vazia()
        {
            var ids = new List<long>(); 
            var mensagem = new MensagemRabbit
            {
                Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(ids)
            };

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m =>
                m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Mesmo_Se_Nao_Tiver_Pendencias()
        {
            var ids = new List<long>();
            var mensagem = new MensagemRabbit
            {
                Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(ids)
            };

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Se_Mensagem_Nao_Contiver_Lista()
        {
            var mensagem = new MensagemRabbit
            {
                Mensagem = "valor_invalido"
            };

            Func<Task> act = async () => await _useCase.Executar(mensagem);

            await act.Should().ThrowAsync<Newtonsoft.Json.JsonReaderException>();
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Se_Mensagem_For_Nula()
        {
            var mensagem = new MensagemRabbit
            {
                Mensagem = null
            };

            Func<Task> act = async () => await _useCase.Executar(mensagem);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
