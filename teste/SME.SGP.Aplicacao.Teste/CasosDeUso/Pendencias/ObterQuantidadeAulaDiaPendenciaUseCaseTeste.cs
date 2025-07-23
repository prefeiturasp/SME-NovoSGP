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
    public class ObterQuantidadeAulaDiaPendenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterQuantidadeAulaDiaPendenciaUseCase useCase;

        public ObterQuantidadeAulaDiaPendenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterQuantidadeAulaDiaPendenciaUseCase(mediatorMock.Object);
        }

        private MensagemRabbit CriarMensagem(object payload)
        {
            return new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(payload)
            };
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comandos_Para_Cada_UE_Com_AnoLetivo()
        {
            var mensagem = new MensagemRabbit
            {
                Mensagem = "2024"
            };

            var ues = new List<long> { 1, 2, 3 };

            mediatorMock
                .Setup(m => m.Send(ObterTodasUesIdsQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(ues.Count));
        }

        [Fact]
        public async Task Executar_Deve_Obter_Pendencias_E_Publicar_Fila_Para_Cada_Pendencia()
        {
            var mensagem = CriarMensagem(2024);

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTodasUesIdsQuery>(q => q == ObterTodasUesIdsQuery.Instance), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long> { 10, 20 });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd => VerificaComandoComAnoLetivoPreenchido(cmd)),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Deve_Salvar_Log_E_Retornar_False_Se_Ocorre_Excecao()
        {
            var mensagem = CriarMensagem(2024); 

            var excecao = new Exception("Erro simulado");

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTodasUesIdsQuery>(q => q == ObterTodasUesIdsQuery.Instance), It.IsAny<CancellationToken>()))
                .ThrowsAsync(excecao);

            SalvarLogViaRabbitCommand? comandoLogEnviado = null;

            mediatorMock
                .Setup(m => m.Send<bool>(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((request, ct) =>
                {
                    comandoLogEnviado = request as SalvarLogViaRabbitCommand;
                })
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);

            Assert.NotNull(comandoLogEnviado);
            Assert.Contains("Erro ao realizar a carga", comandoLogEnviado.Mensagem);
            Assert.Equal(LogContexto.Pendencia, comandoLogEnviado.Contexto);
            Assert.Equal(LogNivel.Negocio, comandoLogEnviado.Nivel);
            Assert.Equal(excecao.Message, comandoLogEnviado.Observacao);

            mediatorMock.Verify(m => m.Send<bool>(
                It.Is<SalvarLogViaRabbitCommand>(cmd =>
                    cmd.Mensagem.Contains("Erro ao realizar a carga") &&
                    cmd.Contexto == LogContexto.Pendencia &&
                    cmd.Nivel == LogNivel.Negocio &&
                    cmd.Observacao == excecao.Message),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Funcionar_Se_Mensagem_Rabbit_Mensagem_For_Nulo()
        {
            var mensagem = new MensagemRabbit { Mensagem = null };
            var ues = new List<long> { 7 };

            mediatorMock
                .Setup(m => m.Send(ObterTodasUesIdsQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
            It.Is<PublicarFilaSgpCommand>(cmd => VerificaComandoComAnoLetivoNulo(cmd)),
            It.IsAny<CancellationToken>()), Times.Once);
        }
        private static bool VerificaComandoComAnoLetivoNulo(PublicarFilaSgpCommand cmd)
        {
            var dto = cmd.Filtros as ObterQuantidadeAulaDiaPendenciaDto;
            return dto != null && dto.AnoLetivo == null;
        }
        private static bool VerificaComandoComAnoLetivoPreenchido(PublicarFilaSgpCommand cmd)
        {
            var dto = cmd.Filtros as ObterQuantidadeAulaDiaPendenciaDto;
            return dto != null && dto.AnoLetivo == 2024;
        }
    }
}
