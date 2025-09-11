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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiarioBordoPendenciaDevolutiva
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase useCase;

        public ReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Parametros_Ativos_E_Executar_Com_Sucesso()
        {
            var parametro1 = new ParametrosSistema { Ativo = true };
            var parametro2 = new ParametrosSistema { Ativo = true };

            var filtro = new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo: 2024);

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var dres = new List<long> { 1, 2, 3 };

            var sequence = new Queue<ParametrosSistema>();
            sequence.Enqueue(parametro1);
            sequence.Enqueue(parametro2);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(() => sequence.Dequeue());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdsDresQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(dres);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(dres.Count));
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_Quando_Parametro_Nao_Ativo()
        {
            var parametroInativo = new ParametrosSistema { Ativo = false };

            var sequence = new Queue<ParametrosSistema>();
            sequence.Enqueue(parametroInativo);
            sequence.Enqueue(parametroInativo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(() => sequence.Dequeue());

            var filtro = new FiltroDiarioBordoPendenciaDevolutivaDto(2024);

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_Quando_Lancar_Excecao()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new Exception("Erro inesperado"));

            var filtro = new FiltroDiarioBordoPendenciaDevolutivaDto(2024);

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd => cmd.Mensagem.Contains("Não foi possível executar")), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
