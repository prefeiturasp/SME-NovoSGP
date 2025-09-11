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
    public class ConsolidarFrequenciaTurmasPorAnoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaTurmasPorAnoUseCase useCase;

        public ConsolidarFrequenciaTurmasPorAnoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarFrequenciaTurmasPorAnoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Obter_Parametros_IdsDres_E_Publicar_Comandos()
        {
            var dataReferencia = new DateTime(2025, 4, 1);

            var filtroAno = new FiltroAnoDto
            {
                Data = dataReferencia,
                TipoConsolidado = TipoConsolidadoFrequencia.Mensal
            };

            var filtroFrequencia = new FiltroConsolidacaoFrequenciaTurmaPorDre(filtroAno.Data, filtroAno.TipoConsolidado, 1234, 40, 40);

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtroFrequencia)
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IRequest<ParametrosSistema> query, CancellationToken _) =>
                {
                    var tipo = ((ObterParametroSistemaPorTipoEAnoQuery)query).TipoParametroSistema;
                    var valor = tipo == TipoParametroSistema.PercentualFrequenciaCritico ? "75" : "60";
                    return new ParametrosSistema { Valor = valor };
                });

            var idsDres = new List<long> { 10, 20 };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdsDresQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(idsDres);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaCritico &&
                q.Ano == dataReferencia.Year
            ), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaMinimaInfantil &&
                q.Ano == dataReferencia.Year
            ), It.IsAny<CancellationToken>()), Times.Once);

            foreach (var dreId in idsDres)
            {
                mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorDre &&
                cmd.Filtros != null &&
                (cmd.Filtros as FiltroConsolidacaoFrequenciaTurmaPorDre) != null &&
                ((FiltroConsolidacaoFrequenciaTurmaPorDre)cmd.Filtros).Data.Date == dataReferencia
            ), It.IsAny<CancellationToken>()), Times.Exactly(idsDres.Count));
            }
        }
    }
}
