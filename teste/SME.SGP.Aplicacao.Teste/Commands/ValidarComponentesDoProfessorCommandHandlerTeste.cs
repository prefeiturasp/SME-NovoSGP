using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands
{
    public class ValidarComponentesDoProfessorCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;

        public ValidarComponentesDoProfessorCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
        }

        [Fact(DisplayName = "ValidarComponentesDoProfessorCommand - Validar componentes do professor, considerando infantil agrupado")]
        public async Task Ao_validar_componentes_do_professor_infantil_agrupados()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { ModalidadeCodigo = Dominio.Modalidade.EducacaoInfantil });

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesDoProfessorNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComponenteCurricularEol>().AsEnumerable());

            var commandHanlder = new ValidarComponentesDoProfessorCommandHandler(mediator.Object);

            var dataAtual = DateTimeExtension.HorarioBrasilia();
            var command = new ValidarComponentesDoProfessorCommand(new Usuario(), "1", 1, dataAtual);            
            await commandHanlder.Handle(command, It.IsAny<CancellationToken>());

            mediator.Verify(x => x.Send(It.Is<ObterComponentesCurricularesDoProfessorNaTurmaQuery>(x => x.RealizarAgrupamentoComponente), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
