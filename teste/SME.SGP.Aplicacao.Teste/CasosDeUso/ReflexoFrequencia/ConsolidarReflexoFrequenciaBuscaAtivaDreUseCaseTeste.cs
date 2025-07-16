using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ReflexoFrequencia
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaDreUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Enviar_Comandos_Para_Cada_Ue_E_Retornar_True()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new ConsolidarReflexoFrequenciaBuscaAtivaDreUseCase(mediatorMock.Object);

            var dreId = 123;
            var anoLetivo = 2025;
            var dataBase = new DateTime(anoLetivo, 1, 1);
            var filtro = new FiltroIdAnoLetivoDto(dreId, dataBase);

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var ues = new List<long> { 1, 2, 3 };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUEsIdsPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            var comandosEnviados = new List<PublicarFilaSgpCommand>();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Callback<object, CancellationToken>((request, token) =>
                {
                    if (request is PublicarFilaSgpCommand cmd && cmd.Filtros is FiltroIdAnoLetivoDto filtroOriginal)
                    {
                        var clone = new FiltroIdAnoLetivoDto(filtroOriginal.Id, filtroOriginal.Data);
                        var cmdClone = new PublicarFilaSgpCommand(cmd.Rota, clone, cmd.CodigoCorrelacao);
                        comandosEnviados.Add(cmdClone);
                    }
                });

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            foreach (var ueId in ues)
            {
                Assert.Contains(comandosEnviados, cmd =>
                {
                    var dto = cmd.Filtros as FiltroIdAnoLetivoDto;
                    return cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaUe &&
                           dto != null &&
                           dto.Id == ueId &&
                           dto.Data == dataBase;
                });
            }
        }
    }
}
