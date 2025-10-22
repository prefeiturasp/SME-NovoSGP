using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.Ciclo
{
    public class ExecutarSincronizacaoInstitucionalCicloSyncUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalCicloSyncUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalCicloSyncUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalCicloSyncUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Ciclos_Nulo_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCiclosEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<CicloRetornoDto>)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível localizar tipos de ciclos no EOL para a sincronização instituicional", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Ciclos_Vazio_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCiclosEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CicloRetornoDto>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível localizar tipos de ciclos no EOL para a sincronização instituicional", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Falha_Deve_Salvar_Log_E_Retornar_Verdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };
            var ciclos = new List<CicloRetornoDto>
            {
                new CicloRetornoDto { Id = 1, Descricao = "Ciclo 1" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCiclosEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ciclos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Rota == RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalCicloTratar &&
                c.CodigoCorrelacao == mensagemRabbit.CodigoCorrelacao), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c =>
                c.Mensagem.Contains("Não foi possível inserir o ciclo") &&
                c.Contexto == LogContexto.SincronizacaoInstitucional &&
                c.Nivel == LogNivel.Negocio), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Sucesso_Deve_Retornar_Verdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };
            var ciclos = new List<CicloRetornoDto>
            {
                new CicloRetornoDto { Id = 1, Descricao = "Ciclo 1" },
                new CicloRetornoDto { Id = 2, Descricao = "Ciclo 2" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCiclosEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ciclos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(ciclos.Count));

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
