using FluentAssertions;
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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurma
{
    public class ConsolidarFrequenciaTurmasPorDREUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaTurmasPorDREUseCase useCase;
        public ConsolidarFrequenciaTurmasPorDREUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarFrequenciaTurmasPorDREUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comandos_Para_Todas_UEs_Da_DRE()
        {
            var dreId = 1L;
            var data = new DateTime(2025, 7, 1);
            var tipoConsolidado = TipoConsolidadoFrequencia.Mensal;
            var percentualMinimo = 75;
            var percentualMinimoInfantil = 60;

            var filtro = new FiltroConsolidacaoFrequenciaTurmaPorDre(data, tipoConsolidado, dreId, percentualMinimo, percentualMinimoInfantil);

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var uesIds = new List<long> { 101, 102 };

            mediatorMock.Setup(m => m.Send(It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == dreId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(uesIds);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            foreach (var ueId in uesIds)
            {
                mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd =>
                    FiltroEhValido(cmd, ueId, data, tipoConsolidado, percentualMinimo, percentualMinimoInfantil)),
                It.IsAny<CancellationToken>()), Times.Once);
            }
        }
        private bool FiltroEhValido(PublicarFilaSgpCommand cmd, long ueId, DateTime data, TipoConsolidadoFrequencia tipo, int percMin, int percMinInf)
        {
            var f = cmd.Filtros as FiltroConsolidacaoFrequenciaTurmaPorUe;
            return cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorUe &&
                   f != null &&
                   f.Data == data &&
                   f.TipoConsolidado == tipo &&
                   f.UeId == ueId &&
                   f.PercentualMinimo == percMin &&
                   f.PercentualMinimoInfantil == percMinInf;
        }
    }
}
