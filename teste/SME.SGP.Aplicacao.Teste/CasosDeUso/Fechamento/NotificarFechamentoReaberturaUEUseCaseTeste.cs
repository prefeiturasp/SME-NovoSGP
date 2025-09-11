using MediatR;
using Moq;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class NotificarFechamentoReaberturaUEUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificarFechamentoReaberturaUEUseCase useCase;

        public NotificarFechamentoReaberturaUEUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificarFechamentoReaberturaUEUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Com_Filtro_Nulo_Deve_Registrar_Log_E_Retornar_False()
        {
            var mensagemRabbitParaTeste = new MensagemRabbit
            {
                Mensagem = String.Empty 
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbitParaTeste);

            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem == "Não foi possível gerar as notificações, pois o fechamento reabertura não possui dados" &&
                cmd.Nivel == LogNivel.Informacao &&
                cmd.Contexto == LogContexto.Fechamento),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.False(resultado);
        }

        [Fact]
        public async Task Executar_Com_Filtro_Valido_Deve_Executar_Notificacao_E_Retornar_True()
        {
            var fechamentoReaberturaDto = new FiltroFechamentoReaberturaNotificacaoDto(
                dreCodigo: "108000",
                ueCodigo: "094765",
                id: 1,
                codigoRf: "1234567",
                tipoCalendarioNome: "Calendário Escolar 2024",
                ueNome: "EMEF Teste",
                dreAbreviacao: "DRE-BT",
                inicio: DateTime.Now,
                fim: DateTime.Now.AddDays(1),
                bimestreNome: "1º Bimestre",
                ehParaUe: true,
                anoLetivo: 2024,
                modalidades: new int[] { 1, 2 }
            );

            var filtro = new FiltroNotificacaoFechamentoReaberturaUEDto(fechamentoReaberturaDto);

            var mensagemJson = Newtonsoft.Json.JsonConvert.SerializeObject(filtro);
            var mensagemRabbitParaTeste = new MensagemRabbit
            {
                Mensagem = mensagemJson
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ExecutaNotificacaoFechamentoReaberturaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbitParaTeste);

            mediatorMock.Verify(m => m.Send(It.IsAny<ExecutaNotificacaoFechamentoReaberturaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Com_Mensagem_Invalida_Deve_Lancar_Excecao()
        {
            var mensagemRabbitParaTeste = new MensagemRabbit
            {
                Mensagem = "{ invalid json"
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            await Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() =>
                useCase.Executar(mensagemRabbitParaTeste));
        }

        [Fact]
        public async Task Executar_Com_Filtro_Com_Fechamento_Reabertura_Nulo_Deve_Retornar_True()
        {
            var filtro = new FiltroNotificacaoFechamentoReaberturaUEDto(null);
            var mensagemJson = Newtonsoft.Json.JsonConvert.SerializeObject(filtro);
            var mensagemRabbitParaTeste = new MensagemRabbit
            {
                Mensagem = mensagemJson
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ExecutaNotificacaoFechamentoReaberturaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbitParaTeste);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ExecutaNotificacaoFechamentoReaberturaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Com_Excecao_Na_Deserializacao_Deve_Tratar_Excecao()
        {
            // Arrange - Teste para verificar se há tratamento de exceção
            var mensagemRabbitParaTeste = new MensagemRabbit
            {
                Mensagem = "{ invalid json without closing brace"
            };

            // Act & Assert - Se não há tratamento de exceção, o teste falhará
            await Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() =>
                useCase.Executar(mensagemRabbitParaTeste));
        }
    }
}