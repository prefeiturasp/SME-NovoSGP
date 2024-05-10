using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterNotasFrequenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IConsultasPeriodoFechamento> consultasPeriodoFechamento;
        private readonly ObterNotasFrequenciaUseCase useCase;

        public ObterNotasFrequenciaUseCaseTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.consultasPeriodoFechamento = new Mock<IConsultasPeriodoFechamento>();
            this.useCase = new ObterNotasFrequenciaUseCase(mediator.Object, consultasPeriodoFechamento.Object);
        }

        [Fact(DisplayName = "ObterNotasFrequenciaUseCase - Deve garantir a busca de códigos de turmas ativos informando o fim do período escolar")]
        public async Task ExecutarObterNotasFrequenciaObtendoCodigosTurmasAtivasInformandoFimPeriodo()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var turma = new Turma() { CodigoTurma = "1", AnoLetivo = anoAtual, TipoTurma = Dominio.Enumerados.TipoTurma.Regular };
            var periodoEscolar = new PeriodoEscolar() { Bimestre = 1, PeriodoInicio = new DateTime(anoAtual, 2, 5), PeriodoFim = new DateTime(anoAtual, 4, 30) };
            var fechamentoTurma = new FechamentoTurma(turma, periodoEscolar);

            mediator.Setup(x => x.Send(It.Is<ObterTurmaPorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.Is<ObterFechamentoTurmaPorIdAlunoCodigoQuery>(y => y.FechamentoTurmaId == 1 && y.AlunoCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentoTurma);

            mediator.Setup(x => x.Send(It.Is<ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery>(y => y.AnoLetivo == anoAtual &&
                                                                                                          y.Modalidade == ModalidadeTipoCalendario.FundamentalMedio &&
                                                                                                          y.Semestre == 0), It.IsAny<CancellationToken>())).ReturnsAsync(new PeriodoEscolar());

            mediator.Setup(x => x.Send(It.Is<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(y => y.AnoLetivo == anoAtual &&
                                                                                                   y.Modalidade == ModalidadeTipoCalendario.FundamentalMedio &&
                                                                                                   y.Semestre == 0), It.IsAny<CancellationToken>())).ReturnsAsync(new Dominio.TipoCalendario() { Id = 1 });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioQuery>(y => y.TipoCalendarioId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>() { new () });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(y => y.TipoCalendarioId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>() { periodoEscolar });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasConsideradasNoConselhoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<string> { "1" });

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterDadosAlunosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoDadosBasicosDto>() { new() { CodigoEOL = "1", SituacaoCodigo = SituacaoMatriculaAluno.Ativo } });

            var retorno = await useCase.Executar(new ConselhoClasseNotasFrequenciaDto(1, 1, "1", "1", 1, false));

            Assert.NotNull(retorno);
            mediator.Verify(x => x.Send(It.Is<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(y => y.AnoLetivo == anoAtual &&
                                                                                                          y.CodigoAluno == "1" &&
                                                                                                          y.DataReferencia == periodoEscolar.PeriodoFim &&
                                                                                                          !y.Semestre.HasValue), It.IsAny<CancellationToken>()), Times.Once);            
        }
    }
}
