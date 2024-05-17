using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterAlunosSemNotasRecomendacoesUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterAlunosSemNotasRecomendacoesUseCase useCase;

        public ObterAlunosSemNotasRecomendacoesUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterAlunosSemNotasRecomendacoesUseCase(mediator.Object);
        }

        [Fact(DisplayName = "ObterAlunosSemNotasRecomendacoesUseCase - Deve considerar a verificação de alunos dentro do período")]
        public async Task DeveConsiderarVerificacaoAlunosDentroPeriodo()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 05);
            var periodoFim = new DateTime(anoAtual, 04, 30);

            mediator.Setup(x => x.Send(It.Is<ObterTurmaPorIdQuery>(y => y.TurmaId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodoEscolarPorTurmaBimestreQuery>(y => y.Turma.CodigoTurma == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar() { PeriodoInicio = periodoInicio, PeriodoFim = periodoFim });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioQuery>(y => y.TipoCalendarioId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>() { new() { PeriodoInicio = periodoInicio, PeriodoFim = periodoFim } });

            mediator.Setup(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(y => y.CodigoTurma == "1" && y.Periodo.dataInicio == periodoInicio && y.Periodo.dataFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>() { new() { CodigoAluno = "1" }, new() { CodigoAluno = "2" } });

            mediator.Setup(x => x.Send(It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComComponenteDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(y => y.TurmasCodigo.Single() == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.Is<ObterTipoCalendarioIdPorTurmaQuery>(y => y.Turma.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.Is<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(y => y.Bimestre == 1 &&
                                                                                                                                    y.CodigoTurma == "1" &&
                                                                                                                                    y.AnoLetivoTurma == anoAtual &&
                                                                                                                                    y.TipoCalendario == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.Is<ExisteConselhoClasseParaTurmaQuery>(y => y.CodigosTurmas.Single() == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.Is<VerificarSeExisteRecomendacaoPorTurmaQuery>(y => y.TurmasCodigo.Single() == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto>() { new() });

            var resultado = await useCase.Executar(new Interfaces.FiltroInconsistenciasAlunoFamiliaDto(1, 1));

            Assert.NotNull(resultado);

            mediator.Verify(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(y => y.CodigoTurma == "1" &&
                                                                                  y.Periodo.dataInicio == periodoInicio &&
                                                                                  y.Periodo.dataFim == periodoFim &&
                                                                                  !y.ConsideraSomenteAtivos), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
