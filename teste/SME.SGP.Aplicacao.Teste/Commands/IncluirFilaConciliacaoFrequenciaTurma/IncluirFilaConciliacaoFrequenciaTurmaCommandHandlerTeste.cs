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

namespace SME.SGP.Aplicacao.Teste.Commands.IncluirFilaConciliacaoFrequenciaTurma
{
    public class IncluirFilaConciliacaoFrequenciaTurmaCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly IncluirFilaConciliacaoFrequenciaTurmaCommandHandler commandHandler;

        public IncluirFilaConciliacaoFrequenciaTurmaCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            commandHandler = new IncluirFilaConciliacaoFrequenciaTurmaCommandHandler(mediator.Object);
        }

        [Fact(DisplayName = "IncluirFilaConciliacaoFrequenciaTurmaCommand - Deve listar os componentes da turma ao incluir conciliação frequência turma")]
        public async Task DeveListarComponentesEolPorTurmasAoIncluirFilaConciliacaoFrequenciaTurmaCommand()
        {
            var codigoTurma = "1";
            var periodo = (new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 05), new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 30));
            var listaAlunos = new List<AlunoPorTurmaResposta>() { new() { CodigoAluno = "1" } };
            var listaComponentes = new List<ComponenteCurricularEol>() { new() { Codigo = 1 }, new() { Codigo = 2 } };

            mediator.Setup(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(y => y.CodigoTurma == codigoTurma && y.Periodo.Equals(periodo) && y.TempoArmazenamentoCache == 5), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos);

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesEOLPorTurmasCodigoQuery>(y => y.CodigosDeTurmas.Contains("1")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaComponentes);

            var comando = new IncluirFilaConciliacaoFrequenciaTurmaCommand("1", 1, null, periodo.Item1, periodo.Item2);
            var resultado = await commandHandler.Handle(comando, It.IsAny<CancellationToken>());

            Assert.True(resultado);

            mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpFrequencia.RotaConciliacaoCalculoFrequenciaPorTurmaComponente &&
                                                                           y.Filtros is CalcularFrequenciaPorTurmaCommand &&
                                                                           y.Usuario == null), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}
