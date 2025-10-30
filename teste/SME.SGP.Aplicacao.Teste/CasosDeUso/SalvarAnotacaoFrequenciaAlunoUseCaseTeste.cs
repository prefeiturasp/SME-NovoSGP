using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class SalvarAnotacaoFrequenciaAlunoUseCaseTeste
    {
        private readonly SalvarAnotacaoFrequenciaAlunoUseCase salvarAnotacaoFrequenciaAlunoUseCase;
        private readonly Mock<IMediator> mediator;

        public SalvarAnotacaoFrequenciaAlunoUseCaseTeste()
        {

            mediator = new Mock<IMediator>();
            salvarAnotacaoFrequenciaAlunoUseCase = new SalvarAnotacaoFrequenciaAlunoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Salvar_Anotacao_Frequencia_Aluno()
        {
            // arrange
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = new DateTime(hoje.Year, hoje.Month, hoje.Day), TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new AlunoPorTurmaResposta()
              {
                  CodigoAluno = "123",
                  CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                  DataSituacao = hoje
              });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto() { Id = 1 });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma()
                {
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });
            // act
            var auditoria = await salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto);

            // assert
            mediator.Verify(x => x.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(auditoria);
            Assert.True(auditoria.Id == 1);
        }
    }
}
