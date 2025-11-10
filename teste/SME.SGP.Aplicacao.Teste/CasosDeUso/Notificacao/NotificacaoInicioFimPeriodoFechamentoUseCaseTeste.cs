using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoInicioFimPeriodoFechamentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificacaoInicioFimPeriodoFechamentoUseCase useCase;

        public NotificacaoInicioFimPeriodoFechamentoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificacaoInicioFimPeriodoFechamentoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametros_Validos_Deve_Executar_Todas_As_Notificacoes_Teste()
        {
            var parametroInicio = new ParametrosSistema { Valor = "1" };
            var parametroFim = new ParametrosSistema { Valor = "2" };
            var periodo1 = new PeriodoFechamentoBimestre
            {
                PeriodoFechamento = new SME.SGP.Dominio.PeriodoFechamento(),
                PeriodoEscolar = new PeriodoEscolar { Id = 1 }
            };
            var periodo2 = new PeriodoFechamentoBimestre
            {
                PeriodoFechamento = new SME.SGP.Dominio.PeriodoFechamento(),
                PeriodoEscolar = new PeriodoEscolar { Id = 2 }
            };
            var ue1 = new Ue { Id = 1, Nome = "UE1" };
            var ue2 = new Ue { Id = 2, Nome = "UE2" };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroInicio);
            mediatorMock.SetupSequence(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroInicio)
                .ReturnsAsync(parametroFim);
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataInicioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoFechamentoBimestre> { periodo1 });
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoFechamentoBimestre> { periodo2 });
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterUesComDrePorModalidadeTurmasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Ue> { ue1, ue2 });
            mediatorMock.Setup(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataInicioQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterUesComDrePorModalidadeTurmasQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediatorMock.Verify(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Existirem_Periodos_Deve_Continuar_Sem_Erro_Teste()
        {
            var parametroInicio = new ParametrosSistema { Valor = "1" };
            var parametroFim = new ParametrosSistema { Valor = "2" };

            mediatorMock.SetupSequence(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroInicio)
                .ReturnsAsync(parametroFim);
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataInicioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoFechamentoBimestre>());
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoFechamentoBimestre>());
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterUesComDrePorModalidadeTurmasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Ue>());
            mediatorMock.Setup(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
