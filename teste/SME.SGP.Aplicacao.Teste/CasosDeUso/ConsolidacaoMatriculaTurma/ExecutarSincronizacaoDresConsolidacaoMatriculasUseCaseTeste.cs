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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoMatriculaTurma
{
    public class ExecutarSincronizacaoDresConsolidacaoMatriculasUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutarSincronizacaoDresConsolidacaoMatriculasUseCase _useCase;

        public ExecutarSincronizacaoDresConsolidacaoMatriculasUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoDresConsolidacaoMatriculasUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Ocorre_Com_Sucesso_Deve_Finalizar_Sem_Log()
        {
            var filtro = new FiltroConsolidacaoMatriculaDreDto(1, new List<int> { 2024 });
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var uesCodigos = new List<string> { "UE1", "UE2" };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUesCodigosPorDreQuery>(q => q.DreId == filtro.Id), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(uesCodigos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(uesCodigos.Count));
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Retorna_False_Deve_Salvar_Log_De_Negocio()
        {
            var filtro = new FiltroConsolidacaoMatriculaDreDto(2, new List<int> { 2024 });
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var uesCodigos = new List<string> { "UE3" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesCodigosPorDreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(uesCodigos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Nivel == LogNivel.Negocio), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Lanca_Excecao_Deve_Salvar_Log_Critico()
        {
            var filtro = new FiltroConsolidacaoMatriculaDreDto(3, new List<int> { 2024 });
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var uesCodigos = new List<string> { "UE4" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesCodigosPorDreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(uesCodigos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Erro de conexão com a fila"));

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Nivel == LogNivel.Critico), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
