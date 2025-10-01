using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoMatriculaTurma
{
    public class ExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase _useCase;

        public ExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Enviar_Comando_Para_Registrar_Consolidacao_Com_Sucesso()
        {
            var matriculaDto = new ConsolidacaoMatriculaTurmaDto("TURMA-CODIGO", 25)
            {
                TurmaId = 999L
            };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(matriculaDto) };

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<RegistraConsolidacaoMatriculaTurmaCommand>(c =>
                    c.TurmaId == matriculaDto.TurmaId &&
                    c.Quantidade == matriculaDto.Quantidade),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }
    }
}
