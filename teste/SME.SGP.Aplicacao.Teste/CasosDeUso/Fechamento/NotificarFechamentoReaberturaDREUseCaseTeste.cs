using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class NotificarFechamentoReaberturaDREUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificarFechamentoReaberturaDREUseCase useCase;

        public NotificarFechamentoReaberturaDREUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificarFechamentoReaberturaDREUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Com_FiltroValido_Deve_Enviar_Notificacoes_E_Retornar_True()
        {
            var fechamentoReabertura = new FiltroFechamentoReaberturaNotificacaoDto(
                dreCodigo: "1",
                ueCodigo: null,
                id: 123,
                codigoRf: null,
                tipoCalendarioNome: "Calendário A",
                ueNome: "Unidade Escolar 1",
                dreAbreviacao: "DRE1",
                inicio: DateTime.Today.AddDays(-10),
                fim: DateTime.Today,
                bimestreNome: "1º Bimestre",
                ehParaUe: true,
                anoLetivo: 2025,
                modalidades: new int[] { 1, 2 }
            );

            var filtro = new FiltroNotificacaoFechamentoReaberturaDREDto(
                dre: "1",
                ues: new List<string> { "10", "20" },
                fechamentoReabertura: fechamentoReabertura
            );

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var admins = new List<string> { "admin1", "admin2" };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAdministradoresPorUEQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(admins.ToArray());

            mediatorMock.Setup(m => m.Send(It.IsAny<ExecutaNotificacaoCadastroFechamentoReaberturaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAdministradoresPorUEQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExecutaNotificacaoCadastroFechamentoReaberturaCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(admins.Count));
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(filtro.Ues.Count()));
            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_ComFiltroNulo_Deve_RegistrarLog_E_Retornar_False()
        {
            var mensagemRabbitParaTeste = new MensagemRabbit
            {
                Mensagem = string.Empty
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbitParaTeste);

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.False(resultado);
        }
    }
}
