using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ReflexoFrequencia
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioRegistroAcaoBuscaAtiva> repositorioMock;
        private readonly ConsolidarReflexoFrequenciaBuscaAtivaUeUseCase useCase;
        public ConsolidarReflexoFrequenciaBuscaAtivaUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioMock = new Mock<IRepositorioRegistroAcaoBuscaAtiva>();
            useCase = new ConsolidarReflexoFrequenciaBuscaAtivaUeUseCase(mediatorMock.Object, repositorioMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Excluir_Consolidacoes_Publicar_Comandos_E_Retornar_True()
        {
            var filtro = new FiltroIdAnoLetivoDto(id: 123, data: new DateTime(2025, 7, 16));

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var ueCodigo = "UE123";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DreUeCodigoDto { UeCodigo = ueCodigo });

            var dataAtual = new DateTime(2025, 7, 16);           

            var registrosBuscaAtiva = new List<long> { 1001, 1002 };
            repositorioMock
                .Setup(r => r.ObterIdsRegistrosAlunoResponsavelContatado(filtro.Data.Date, filtro.Id, filtro.Data.Year))
                .ReturnsAsync(registrosBuscaAtiva);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommand>(
                    cmd => cmd.UeCodigo == ueCodigo
                        && cmd.Mes == filtro.Data.Month
                        && cmd.AnoLetivo == filtro.Data.Year),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd => cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaAluno),
                It.IsAny<CancellationToken>()), Times.Exactly(registrosBuscaAtiva.Count));
        }
    }
}
