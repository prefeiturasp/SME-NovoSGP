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
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase _useCase;

        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Nula_Deve_Processar_Todas_Dres()
        {
            var dresIds = new List<long> { 1, 2, 3 };
            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dresIds);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(null);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(dresIds.Count));
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Id_Zerado_Deve_Processar_Todas_Dres()
        {
            var dresIds = new List<long> { 10, 20 };
            var filtro = new FiltroIdAnoLetivoDto(0, DateTime.Now);
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dresIds);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(dresIds.Count));
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Id_Especifico_Deve_Processar_Apenas_Dre_Correspondente()
        {
            var dresIds = new List<long> { 15, 25, 35 };
            long dreIdEspecifica = 25;

            var filtro = new FiltroIdAnoLetivoDto(dreIdEspecifica, DateTime.Now);
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            PublicarFilaSgpCommand comandoPublicado = null;

            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dresIds);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, token) =>
                         {
                             comandoPublicado = cmd as PublicarFilaSgpCommand;
                         })
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(comandoPublicado);
            var filtroPublicado = comandoPublicado.Filtros as FiltroIdAnoLetivoDto;
            Assert.NotNull(filtroPublicado);
            Assert.Equal(dreIdEspecifica, filtroPublicado.Id);
        }
    }
}
