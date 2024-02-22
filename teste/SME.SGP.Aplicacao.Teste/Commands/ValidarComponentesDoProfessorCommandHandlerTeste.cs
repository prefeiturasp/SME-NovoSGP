using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
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

        [Fact(DisplayName = "ValidarComponentesDoProfessorCommand - Deve exibir a mensagem de negocio quando houver componentes sem atribuição para a data selecionada")]
        public async Task Ao_validar_componentes_do_professor_territorio_do_saber_sem_atribuicao()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();
            var listacomponentesCurricularesDoProfessor = new List<ComponenteCurricularEol>() {
                new ComponenteCurricularEol() { TurmaCodigo = "1",Codigo = 1214,TerritorioSaber = true, Descricao = "1214",LancaNota = false, RegistraFrequencia = true }
            };
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { ModalidadeCodigo = Dominio.Modalidade.Fundamental });

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesDoProfessorNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listacomponentesCurricularesDoProfessor);

            mediator.Setup(x => x.Send(It.IsAny<VerificaPodePersistirTurmaDisciplinaEOLQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);


            var commandHanlder = new ValidarComponentesDoProfessorCommandHandler(mediator.Object);


            var command = new ValidarComponentesDoProfessorCommand(new Usuario(), "1", 1214, dataAtual);
            var retorno = await commandHanlder.Handle(command, It.IsAny<CancellationToken>());

            Assert.False(retorno.resultado);
            Assert.Equal(MensagemNegocioComuns.VOCE_NAO_PODE_CRIAR_AULAS_PARA_COMPONENTES_SEM_ATRIBUICAO_NA_DATA_SELECIONADA, retorno.mensagem);
        }
    }
}
