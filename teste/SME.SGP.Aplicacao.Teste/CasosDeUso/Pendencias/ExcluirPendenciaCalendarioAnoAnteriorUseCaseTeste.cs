using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class ExcluirPendenciaCalendarioAnoAnteriorUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExcluirPendenciaCalendarioAnoAnteriorUseCase useCase;

        public ExcluirPendenciaCalendarioAnoAnteriorUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ExcluirPendenciaCalendarioAnoAnteriorUseCase(mediator.Object);
        }

        [Fact(DisplayName = "ExcluirPendenciaCalendarioAnoAnteriorUseCase - Deve executar com excluir pendência calendário ano anterior mesmo com a mensagem rabbit nula acionando a próxima fila")]
        public async Task Deve_Executar_Com_Sucesso_Excluir_Pendencia_Calendario_Ano_Anterior_Com_Mensagem_Rabbit_Nula_Acionando_Proxima_Fila()
        {
            var anoLetivoConsiderado = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
            var idsUe = new List<long>() { 1, 2 };

            mediator.Setup(x => x.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idsUe);

            var mensagensEsperadaProximaFilaUE = new List<ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto>()
            {
                new (anoLetivoConsiderado, 1), new(anoLetivoConsiderado, 2)
            };

            foreach (var mensagemEsperada in mensagensEsperadaProximaFilaUE)
            {
                mediator.Setup(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUes &&
                    ((ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto)y.Filtros).Equals(mensagemEsperada)), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);
            }

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);

            foreach (var mensagemEsperada in mensagensEsperadaProximaFilaUE)
            {
                mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUes &&
                    ((ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto)y.Filtros).Equals(mensagemEsperada)), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact(DisplayName = "ExcluirPendenciaCalendarioAnoAnteriorUseCase - Deve salvar log e lançar exceção ao falhar na exclusão")]
        public async Task Deve_Salvar_Log_E_Lancar_Excecao_Ao_Falhar_Na_Exclusao()
        {
            var param = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year)
            };

            var exceptionToThrow = new Exception("Falha simulada");

            mediator.Setup(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exceptionToThrow);

            var ex = await Assert.ThrowsAsync<Exception>(() => useCase.Executar(param));

            Assert.Equal("Falha simulada", ex.Message);

            mediator.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Não foi possível realizar a exclusão das pendências após o final do ano") &&
                cmd.Nivel == LogNivel.Critico &&
                cmd.Contexto == LogContexto.Calendario &&
                (exceptionToThrow.InnerException == null || cmd.InnerException.Contains(exceptionToThrow.InnerException.ToString())) &&
                cmd.Observacao == exceptionToThrow.Message
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "ExcluirPendenciaCalendarioAnoAnteriorUseCase - Deve salvar log e lançar exceção ao falhar mesmo com MensagemRabbit.Mensagem nulo")]
        public async Task Deve_Salvar_Log_E_Lancar_Excecao_Ao_Falhar_Com_Mensagem_Rabbit_Mensagem_Nulo()
        {
            var param = new MensagemRabbit
            {
                Mensagem = null
            };

            var exceptionToThrow = new Exception("Falha simulada");

            mediator.Setup(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exceptionToThrow);

            var ex = await Assert.ThrowsAsync<Exception>(() => useCase.Executar(param));

            Assert.Equal("Falha simulada", ex.Message);

            mediator.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Não foi possível realizar a exclusão das pendências após o final do ano") &&
                cmd.Nivel == LogNivel.Critico &&
                cmd.Contexto == LogContexto.Calendario &&
                (exceptionToThrow.InnerException == null || cmd.InnerException.Contains(exceptionToThrow.InnerException.ToString())) &&
                cmd.Observacao == exceptionToThrow.Message
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
