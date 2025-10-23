using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ProdutividadeFrequencia
{
    public class ConsolidarInformacoesProdutividadeFrequenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarInformacoesProdutividadeFrequenciaUseCase _useCase;

        public ConsolidarInformacoesProdutividadeFrequenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarInformacoesProdutividadeFrequenciaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Rabbit_Nula_Deve_Publicar_Para_Todas_Dres()
        {
            var dres = new long[] { 1L, 2L };
            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dres);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(null);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == 1L), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == 2L), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Quando_Propriedade_Mensagem_Nula_Deve_Publicar_Para_Todas_Dres()
        {
            var dres = new long[] { 1L, 2L };
            var mensagemRabbit = new MensagemRabbit(null);
            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dres);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == 1L), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == 2L), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Com_Filtro_Id_Zero_Deve_Publicar_Para_Todas_Dres()
        {
            var dres = new long[] { 1L, 2L, 3L };
            var filtroDto = new FiltroIdAnoLetivoDto(0, DateTime.Now.Date);
            var mensagemPayload = JsonConvert.SerializeObject(filtroDto);
            var mensagemRabbit = new MensagemRabbit(mensagemPayload);
            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dres);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == 1L), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == 2L), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == 3L), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Com_Filtro_Id_Especifico_Deve_Publicar_Apenas_Para_Dre_Especifica()
        {
            var dres = new long[] { 1L, 2L, 3L };
            long dreEspecificaId = 2L;
            var filtroDto = new FiltroIdAnoLetivoDto(dreEspecificaId, DateTime.Now.Date);
            var mensagemPayload = JsonConvert.SerializeObject(filtroDto);
            var mensagemRabbit = new MensagemRabbit(mensagemPayload);
            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dres);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == dreEspecificaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Lista_De_Dres_Vazia_Deve_Retornar_True_Sem_Publicar()
        {
            var dres = Array.Empty<long>();
            var filtroDto = new FiltroIdAnoLetivoDto(0, DateTime.Now.Date);
            var mensagemPayload = JsonConvert.SerializeObject(filtroDto);
            var mensagemRabbit = new MensagemRabbit(mensagemPayload);
            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dres);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
