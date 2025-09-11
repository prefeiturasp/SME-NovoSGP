using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class RemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly RemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase useCase;

        public RemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new RemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Excluir_Pendencias_Quando_Lista_Nao_Esta_Vazia()
        {
            var pendenciasIds = new List<long> { 1, 2, 3 };
            var mensagemJson = JsonConvert.SerializeObject(pendenciasIds);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<ExcluirPendenciasPorIdsCommand>(cmd =>
                    cmd.PendenciasIds.Length == pendenciasIds.Count &&
                    cmd.PendenciasIds[0] == 1 &&
                    cmd.PendenciasIds[1] == 2 &&
                    cmd.PendenciasIds[2] == 3),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Executar_Nao_Deve_Excluir_Pendencias_Quando_Lista_Esta_Vazia()
        {
            var pendenciasIds = new List<long>();
            var mensagemJson = JsonConvert.SerializeObject(pendenciasIds);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Salvar_Log_E_Propagar_Excecao_Quando_Ocorre_Erro()
        {
            var pendenciasIds = new List<long> { 10, 20 };
            var mensagemJson = JsonConvert.SerializeObject(pendenciasIds);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            var excecao = new Exception("Erro simulado");

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirPendenciasPorIdsCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(excecao);

            var ex = await Assert.ThrowsAsync<Exception>(() => useCase.Executar(mensagemRabbit));

            Assert.Equal("Erro simulado", ex.Message);

            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Não foi possível realizar a exclusão das pendências após o final do ano") &&
                cmd.Nivel == LogNivel.Critico &&
                cmd.Contexto == LogContexto.Calendario &&
                cmd.Observacao.Contains("10") &&
                cmd.Observacao.Contains("20") &&
                cmd.Rastreamento != null &&
                cmd.InnerException != null),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Se_Mensagem_For_Nulo_Ou_Invalido()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = null };

            await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.Executar(mensagemRabbit));
        }
    }
}
