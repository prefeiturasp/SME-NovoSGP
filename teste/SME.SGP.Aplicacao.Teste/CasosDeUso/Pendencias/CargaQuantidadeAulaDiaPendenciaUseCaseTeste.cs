using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class CargaQuantidadeAulaDiaPendenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly CargaQuantidadeAulaDiaPendenciaUseCase useCase;

        public CargaQuantidadeAulaDiaPendenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new CargaQuantidadeAulaDiaPendenciaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Enviar_Comando_E_Criar_Pendencia_Com_Sucesso()
        {
            var dto = new AulasDiasPendenciaDto
            {
                PendenciaId = 1,
                QuantidadeDias = 5,
                QuantidadeAulas = 10
            };

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(dto)
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CargaPendenciasQuantidadeDiasQuantidadeAulasCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<CargaPendenciasQuantidadeDiasQuantidadeAulasCommand>(cmd =>
                cmd.Carga.PendenciaId == dto.PendenciaId &&
                cmd.Carga.QuantidadeDias == dto.QuantidadeDias &&
                cmd.Carga.QuantidadeAulas == dto.QuantidadeAulas
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Enviar_Log_E_Retornar_False_Quando_Ocorre_Excecao()
        {
            var dto = new AulasDiasPendenciaDto
            {
                PendenciaId = 1,
                QuantidadeDias = 5,
                QuantidadeAulas = 10
            };

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(dto)
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CargaPendenciasQuantidadeDiasQuantidadeAulasCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro esperado"));

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Erro ao realizar a carga de dias e aulas") &&
                cmd.Nivel == LogNivel.Negocio &&
                cmd.Contexto == LogContexto.Pendencia &&
                cmd.Observacao == "Erro esperado"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
