using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class AlterarObservacaoDiarioBordoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly AlterarObservacaoDiarioBordoUseCase alterarObservacaoDiarioBordoUseCase;

        public AlterarObservacaoDiarioBordoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            alterarObservacaoDiarioBordoUseCase = new AlterarObservacaoDiarioBordoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Alterar_Observacao_Diario_De_Bordo()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<AlterarObservacaoDiarioBordoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto()
                {
                    Id = 1
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaDiarioBordoAulaPorObservacaoIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma()
                {
                    Id = 1,
                    CodigoTurma = "123456"
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterProfessoresTitularesDaTurmaCompletosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProfessorTitularDisciplinaEol>()
                {
                    new ProfessorTitularDisciplinaEol()
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioNotificarDiarioBordoObservacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UsuarioNotificarDiarioBordoObservacaoDto>()
                {
                    new UsuarioNotificarDiarioBordoObservacaoDto()
                });

            //Act
            var auditoriaDto = await alterarObservacaoDiarioBordoUseCase.Executar("observacao", 1, null);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<AlterarObservacaoDiarioBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }
    }
}
