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
    public class ConsolidarFrequenciaTurmasPorUEUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaTurmasPorUEUseCase useCase;

        public ConsolidarFrequenciaTurmasPorUEUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarFrequenciaTurmasPorUEUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comando_Para_Cada_Turma()
        {
            var data = new DateTime(2025, 7, 1);
            var ueId = 1234;
            var percentualMinimo = 75;
            var percentualMinimoInfantil = 80;

            var filtro = new FiltroConsolidacaoFrequenciaTurmaPorUe(data,
                TipoConsolidadoFrequencia.Mensal,
                ueId,
                percentualMinimo,
                percentualMinimoInfantil);

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var turmas = new List<TurmaModalidadeDto>
            {
                new TurmaModalidadeDto { TurmaId = 1, TurmaCodigo = "T1", Modalidade = Modalidade.EducacaoInfantil },
                new TurmaModalidadeDto { TurmaId = 2, TurmaCodigo = "T2", Modalidade = Modalidade.EducacaoInfantil }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComModalidadePorAnoUEQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turmas);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(
                It.IsAny<ObterTurmasComModalidadePorAnoUEQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd => FiltroEhValido(cmd, 1, percentualMinimoInfantil)),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(cmd => FiltroEhValido(cmd, 2, percentualMinimoInfantil)),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        private bool FiltroEhValido(PublicarFilaSgpCommand cmd, long turmaIdEsperado, int percentualEsperado)
        {
            if (cmd.Filtros == null || cmd.Filtros.GetType() != typeof(FiltroConsolidacaoFrequenciaTurma))
                return false;

            var f = (FiltroConsolidacaoFrequenciaTurma)cmd.Filtros;

            return f.TurmaId == turmaIdEsperado &&
                   f.PercentualFrequenciaMinimo == percentualEsperado;
        }
    }

}
