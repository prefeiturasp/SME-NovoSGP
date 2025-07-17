using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoRegistrosPedagogicos
{
    public class ConsolidarRegistrosPedagogicosUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Publicar_Comandos_Para_Cada_Ue_E_Atualizar_Parametro()
        {
            var mediatorMock = new Mock<IMediator>();

            var anoParametro = DateTime.Now.Year - 1;
            var parametro = new ParametrosSistema
            {
                Ano = anoParametro,
                Tipo = TipoParametroSistema.ExecucaoConsolidacaoRegistrosPedagogicos,
                Valor = null
            };

            var parametros = new List<ParametrosSistema> { parametro };
            var ues = new List<long> { 1, 2 };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametrosSistemaPorTiposQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExisteConsolidacaoRegistroPedagogicoPorAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1234);

            var useCase = new ConsolidarRegistrosPedagogicosUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametrosSistemaPorTiposQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExisteConsolidacaoRegistroPedagogicoPorAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgp.ConsolidarRegistrosPedagogicosPorUeTratar), It.IsAny<CancellationToken>()), Times.Exactly(ues.Count));
            mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
