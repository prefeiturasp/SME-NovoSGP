using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.Dre
{
    public class ExecutarSincronizacaoInstitucionalDreSyncUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalDreSyncUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalDreSyncUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalDreSyncUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Codigos_Dre_Nulo_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string[]?)null!); 

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível localizar as Dres no EOL para a sincronização instituicional", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Codigos_Dre_Vazio_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<string>()); 
            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível localizar as Dres no EOL para a sincronização instituicional", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Falha_Deve_Salvar_Log_E_Retornar_Verdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };

            var codigosDre = new string[] { "012345" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codigosDre);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Rota == RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalDreTratar &&
                (string)c.Filtros == codigosDre.First()), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c =>
                c.Mensagem.Contains("Não foi possível inserir a Dre") &&
                c.Contexto == LogContexto.SincronizacaoInstitucional &&
                c.Nivel == LogNivel.Negocio), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Sucesso_Deve_Retornar_Verdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };
            var codigosDre = new List<string> { "012345" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codigosDre.ToArray()); 

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(codigosDre.Count));

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
