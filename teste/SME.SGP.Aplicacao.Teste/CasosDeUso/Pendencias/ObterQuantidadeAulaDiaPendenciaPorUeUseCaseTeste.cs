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
    public class ObterQuantidadeAulaDiaPendenciaPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterQuantidadeAulaDiaPendenciaPorUeUseCase useCase;

        public ObterQuantidadeAulaDiaPendenciaPorUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterQuantidadeAulaDiaPendenciaPorUeUseCase(mediatorMock.Object);
        }

        private MensagemRabbit CriarMensagem(object payload)
        {
            return new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(payload)
            };
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Mensagens_Para_Cada_Pendencia()
        {
            var filtro = new ObterQuantidadeAulaDiaPendenciaDto { AnoLetivo = 2024, UeId = 123 };
            var mensagem = CriarMensagem(filtro);

            var pendencias = new List<AulasDiasPendenciaDto>
            {
                new AulasDiasPendenciaDto { PendenciaId = 1, QuantidadeDias = 5, QuantidadeAulas = 20 },
                new AulasDiasPendenciaDto { PendenciaId = 2, QuantidadeDias = 10, QuantidadeAulas = 40 }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPendenciasParaInserirAulasEDiasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendencias);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(pendencias.Count));
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Se_Nao_Houver_Pendencias()
        {
            var filtro = new ObterQuantidadeAulaDiaPendenciaDto { AnoLetivo = 2024, UeId = 456 };
            var mensagem = CriarMensagem(filtro);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPendenciasParaInserirAulasEDiasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AulasDiasPendenciaDto>());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Salvar_Log_E_Retornar_False_Em_Erro()
        {
            var filtro = new ObterQuantidadeAulaDiaPendenciaDto { AnoLetivo = 2024, UeId = 789 };
            var mensagem = CriarMensagem(filtro);

            var excecao = new Exception("Erro simulado");

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPendenciasParaInserirAulasEDiasQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(excecao);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Erro ao realizar a carga") &&
                cmd.Nivel == LogNivel.Negocio &&
                cmd.Contexto == LogContexto.Pendencia &&
                cmd.Observacao == excecao.Message
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
