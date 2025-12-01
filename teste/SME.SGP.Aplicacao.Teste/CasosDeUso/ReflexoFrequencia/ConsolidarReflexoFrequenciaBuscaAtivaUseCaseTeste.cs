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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ReflexoFrequencia
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarReflexoFrequenciaBuscaAtivaUseCase useCase;

        public ConsolidarReflexoFrequenciaBuscaAtivaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarReflexoFrequenciaBuscaAtivaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Com_Mensagem_Valida_Deve_Publicar_Para_Cada_Dre()
        {
            var filtro = new FiltroIdAnoLetivoDto(10, new DateTime(2025, 7, 16));

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var dres = new List<long> { 10, 20, 30 };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<IEnumerable<long>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd => cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaDre && ((FiltroIdAnoLetivoDto)cmd.Filtros).Id == 10),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd => cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaDre && ((FiltroIdAnoLetivoDto)cmd.Filtros).Id != 10),
                It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact]
        public async Task Executar_Com_Mensagem_Nula_Deve_Usar_Filtro_Padrao_E_Publicar_Para_Todos_Dres()
        {
            var mensagem = new MensagemRabbit(null); // Mensagem será tratada como nula

            var dres = new List<long> { 1, 2, 3 };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<IEnumerable<long>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            var comandosEnviados = new List<PublicarFilaSgpCommand>();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Callback<object, CancellationToken>((request, token) =>
                {
                    if (request is PublicarFilaSgpCommand cmd && cmd.Filtros is FiltroIdAnoLetivoDto filtro)
                    {
                        var filtroClonado = new FiltroIdAnoLetivoDto(filtro.Id, filtro.Data);
                        var cmdClone = new PublicarFilaSgpCommand(cmd.Rota, filtroClonado, cmd.CodigoCorrelacao);
                        comandosEnviados.Add(cmdClone);
                    }
                });

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            foreach (var dreId in dres)
            {
                var encontrou = comandosEnviados.Any(cmd =>
                    cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaDre &&
                    cmd.Filtros is FiltroIdAnoLetivoDto filtro &&
                    filtro.Id == dreId);

                Assert.True(encontrou, $"Não encontrou comando para DRE {dreId}");
            }
        }
    }
}
