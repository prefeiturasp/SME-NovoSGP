using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.EOL.Aluno.Turmas.ObterTurmasComMatriculasValidasPeriodoFechamento
{
    public class ObterTurmasComMatriculasValidasPeriodoQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterTurmasComMatriculasValidasPeriodoQueryHandler queryHandler;

        public ObterTurmasComMatriculasValidasPeriodoQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            queryHandler = new ObterTurmasComMatriculasValidasPeriodoQueryHandler(mediator.Object);
        }

        [Fact(DisplayName = "ObterTurmasComMatriculasValidasPeriodoQueryHandler - Obter turmas com matriculas validas no periodo considerando somente período escolar")]
        public async Task Obter_Turmas_Com_Matriculas_Validas_No_Periodo_Considerando_Somente_Periodo_Escolar()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 2, 5);
            var periodoFim = new DateTime(anoAtual, 4, 30);
            var turmas = new List<string>() { "1234567", "7654321", "1234567" };

            mediator.Setup(x => x.Send(It.Is<ObterTurmasComMatriculasValidasQuery>(y => y.AlunoCodigo == "1" &&
                                                                                        y.TurmasCodigos.Intersect(turmas).Count() == turmas.Distinct().Count() &&
                                                                                        y.PeriodoInicio == periodoInicio &&
                                                                                        y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>())).ReturnsAsync(turmas);

            var retorno = await queryHandler
                .Handle(new ObterTurmasComMatriculasValidasPeriodoQuery("1", false, 1, 1, turmas.ToArray(), periodoInicio, periodoFim, false), It.IsAny<CancellationToken>());

            Assert.Equal(turmas.Distinct(), retorno);

            mediator.Verify(x => x.Send(It.Is<ObterTurmasComMatriculasValidasQuery>(y => y.AlunoCodigo == "1" &&
                                                                                         y.TurmasCodigos.Intersect(turmas).Count() == turmas.Distinct().Count() &&
                                                                                         y.PeriodoInicio == periodoInicio &&
                                                                                         y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>()), Times.Once);

            mediator.Verify(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "ObterTurmasComMatriculasValidasPeriodoQueryHandler - Obter turmas com matriculas validas no periodo considerando o período de fechamento")]
        public async Task Obter_Turmas_Com_Matriculas_Validas_No_Periodo_Considerando_Periodo_Fechamento()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 2, 5);
            var periodoFim = new DateTime(anoAtual, 4, 30);
            var turmas = new List<string>() { "1234567", "7654321", "1234567" };

            mediator.Setup(x => x.Send(It.Is<ObterTurmasComMatriculasValidasQuery>(y => y.AlunoCodigo == "1" &&
                                                                                        y.TurmasCodigos.Intersect(turmas).Count() == turmas.Distinct().Count() &&
                                                                                        y.PeriodoInicio == periodoInicio &&
                                                                                        y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>())).ReturnsAsync(turmas);

            mediator.Setup(x => x.Send(It.Is<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(y => y.TipoCandarioId == 1 &&
                                                                                                      !y.EhTurmaInfantil &&
                                                                                                       y.Bimestre == 1), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new PeriodoFechamentoBimestre(1, new PeriodoEscolar() { Id = 1 }, periodoInicio, periodoFim));

            var retorno = await queryHandler
                .Handle(new ObterTurmasComMatriculasValidasPeriodoQuery("1", false, 1, 1, turmas.ToArray(), periodoInicio, periodoFim), It.IsAny<CancellationToken>());

            Assert.Equal(turmas.Distinct(), retorno);

            mediator.Verify(x => x.Send(It.Is<ObterTurmasComMatriculasValidasQuery>(y => y.AlunoCodigo == "1" &&
                                                                                         y.TurmasCodigos.Intersect(turmas).Count() == turmas.Distinct().Count() &&
                                                                                         y.PeriodoInicio == periodoInicio &&
                                                                                         y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>()), Times.Exactly(2));

            mediator.Verify(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
