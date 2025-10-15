using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.UE
{
    public class ExecutarSincronizacaoInstitucionalUeTratarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalUeTratarUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalUeTratarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalUeTratarUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Codigo_Ue_Vazio_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = string.Empty };

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível localizar o código da Ue para tratar o Sync.", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Tratamento_Falha_Deve_Lancar_Negocio_Exception()
        {
            var ueCodigo = "123456";
            var mensagemRabbit = new MensagemRabbit { Mensagem = ueCodigo };
            var ueEol = new UeDetalhesParaSincronizacaoInstituicionalDto();
            var ueSgp = new Ue();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUeDetalhesParaSincronizacaoInstitucionalQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(ueEol);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUeComDrePorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(ueSgp);
            _mediatorMock.Setup(m => m.Send(It.IsAny<TrataSincronizacaoInstitucionalUeCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal($"Não foi possível sincronizar a UE de código {ueCodigo}", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Tratamento_Sucesso_Deve_Publicar_Fila_E_Retornar_Resultado()
        {
            var ueCodigo = "123456";
            var mensagemRabbit = new MensagemRabbit { Mensagem = ueCodigo, CodigoCorrelacao = Guid.NewGuid() };
            var ueEol = new UeDetalhesParaSincronizacaoInstituicionalDto();
            var ueSgp = new Ue();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUeDetalhesParaSincronizacaoInstitucionalQuery>(q => q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>())).ReturnsAsync(ueEol);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterUeComDrePorCodigoQuery>(q => q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>())).ReturnsAsync(ueSgp);
            _mediatorMock.Setup(m => m.Send(It.Is<TrataSincronizacaoInstitucionalUeCommand>(c => c.UeEOL == ueEol && c.UeSGP == ueSgp), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                c => c.Rota == RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTurmasSync),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
