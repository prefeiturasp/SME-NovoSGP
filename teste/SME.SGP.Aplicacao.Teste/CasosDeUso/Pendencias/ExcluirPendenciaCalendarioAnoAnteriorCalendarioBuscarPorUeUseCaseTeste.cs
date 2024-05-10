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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase useCase;

        public ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase(mediator.Object);
        }

        [Fact(DisplayName = "ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase - Deve executar com sucesso acionando a fila seguinte de exclusão de pendências com ano letivo definido")]
        public async Task DeveExecutarExcluirPendenciaAnoAnteriorCalendarioBuscarPorUeAnoLetivoDefinidoComSucessoAcionandoProximaFila()
        {
            var anoLetivoConsiderado = DateTimeExtension.HorarioBrasilia().AddYears(-3).Year;
            var mensagemEsperadaProximaFila = new ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(anoLetivoConsiderado, 1, new long[] { 1 });

            mediator.Setup(x => x.Send(It.Is<ObterPendenciasCalendarioPorUeQuery>(y => y.AnoLetivo == anoLetivoConsiderado && y.UeId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long>() { 1 });

            mediator.Setup(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe &&
                ((ExcluirPendenciaCalendarioAnoAnteriorPorUeDto)y.Filtros).Equals(mensagemEsperadaProximaFila)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var dto = new ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto(anoLetivoConsiderado, 1);
            var param = new MensagemRabbit(JsonConvert.SerializeObject(dto));
            var retorno = await useCase.Executar(param);

            Assert.True(retorno);            

            mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe &&
                ((ExcluirPendenciaCalendarioAnoAnteriorPorUeDto)y.Filtros).Equals(mensagemEsperadaProximaFila)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase - Deve executar com sucesso acionando a fila seguinte de exclusão de pendências com ano letivo não definido definido")]
        public async Task DeveExecutarExcluirPendenciaAnoAnteriorCalendarioBuscarPorUeAnoLetivoNaoDefinidoComSucessoAcionandoProximaFila()
        {
            var anoLetivoConsiderado = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
            var mensagemEsperadaProximaFila = new ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(null, 1, new long[] { 1 });

            mediator.Setup(x => x.Send(It.Is<ObterPendenciasCalendarioPorUeQuery>(y => y.AnoLetivo == anoLetivoConsiderado && y.UeId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long>() { 1 });

            mediator.Setup(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe &&
                ((ExcluirPendenciaCalendarioAnoAnteriorPorUeDto)y.Filtros).Equals(mensagemEsperadaProximaFila)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var dto = new ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto(null, 1);
            var param = new MensagemRabbit(JsonConvert.SerializeObject(dto));
            var retorno = await useCase.Executar(param);

            Assert.True(retorno);

            mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe &&
                ((ExcluirPendenciaCalendarioAnoAnteriorPorUeDto)y.Filtros).Equals(mensagemEsperadaProximaFila)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase - Deve executar com sucesso não acionando a fila seguinte de exclusão de pendências com ano letivo não definido")]
        public async Task DeveExecutarExcluirPendenciaAnoAnteriorCalendarioBuscarPorUeAnoLetivoNaoDefinidoComSucessoNaoAcionandoProximaFila()
        {
            var mensagemEsperadaProximaFila = new ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(null, 1, new long[] { 1 });

            mediator.Setup(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe &&
                ((ExcluirPendenciaCalendarioAnoAnteriorPorUeDto)y.Filtros).Equals(mensagemEsperadaProximaFila)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var dto = new ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto(null, 1);
            var param = new MensagemRabbit(JsonConvert.SerializeObject(dto));
            var retorno = await useCase.Executar(param);

            Assert.True(retorno);

            mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe &&
                ((ExcluirPendenciaCalendarioAnoAnteriorPorUeDto)y.Filtros).Equals(mensagemEsperadaProximaFila)), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase - Deve retornar erro de negócio acionando a fila par salvar log")]
        public async Task DeveRetornarExcluirPendenciaAnoAnteriorCalendarioBuscarPorUeAnoLetivoNaoDefinidoAcionandoFilaSalvarLog()
        {
            var mensagemEsperadaProximaFila = new ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(null, 1, new long[] { 1 });

            mediator.Setup(x => x.Send(It.IsAny<ObterPendenciasCalendarioPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NegocioException(null));

            var dto = new ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto(null, 1);
            var param = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(param));            

            mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe &&
                ((ExcluirPendenciaCalendarioAnoAnteriorPorUeDto)y.Filtros).Equals(mensagemEsperadaProximaFila)), It.IsAny<CancellationToken>()), Times.Never);

            mediator.Verify(x => x.Send(It.Is<SalvarLogViaRabbitCommand>(y => y.Mensagem == "Não foi possível obter as pendências da UE para exclusão após o final do ano - Calendário " &&
                y.Nivel == Dominio.Enumerados.LogNivel.Critico && y.Contexto == Dominio.Enumerados.LogContexto.Calendario && y.Observacao.Contains("UE Id: 1")), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
