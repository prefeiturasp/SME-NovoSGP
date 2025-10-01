using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia.BuscaAtiva
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase _useCase;

        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Dre_Possui_Ues_Deve_Publicar_Comando_Para_Cada_Ue()
        {
            var dreId = 10L;
            var filtroInicial = new FiltroIdAnoLetivoDto(dreId, new DateTime(2025, 9, 30));
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtroInicial) };
            var uesIds = new List<long> { 101, 102 };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == dreId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(uesIds);

            var filtrosCapturados = new List<FiltroIdAnoLetivoDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, token) =>
                         {
                             var publicarCmd = (PublicarFilaSgpCommand)cmd;
                             var filtroCmd = (FiltroIdAnoLetivoDto)publicarCmd.Filtros;
                             filtrosCapturados.Add(new FiltroIdAnoLetivoDto(filtroCmd.Id, filtroCmd.Data));
                         })
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(uesIds.Count));

            Assert.Equal(2, filtrosCapturados.Count);
            Assert.Contains(filtrosCapturados, f => f.Id == 101);
            Assert.Contains(filtrosCapturados, f => f.Id == 102);
        }

        [Fact]
        public async Task Executar_Quando_Dre_Nao_Possui_Ues_Nao_Deve_Publicar_Comandos()
        {
            var dreId = 20L;
            var filtro = new FiltroIdAnoLetivoDto(dreId, new DateTime(2025, 9, 30));
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == dreId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<long>());

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
