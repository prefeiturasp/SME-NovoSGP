using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class SalvarAnotacaoFechamentoAlunoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SalvarAnotacaoFechamentoAlunoUseCase _useCase;

        public SalvarAnotacaoFechamentoAlunoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new SalvarAnotacaoFechamentoAlunoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Excluir_Anotacao_Quando_Anotacao_Vazia()
        {
            var fechamentoId = 1L;
            var alunoCodigo = "123456";
            var fechamentoAlunoId = 10L;

            var anotacaoDto = new AnotacaoAlunoDto
            {
                FechamentoId = fechamentoId,
                CodigoAluno = alunoCodigo,
                Anotacao = string.Empty
            };

            var anotacaoExistente = new AnotacaoFechamentoAluno
            {
                Id = 1,
                Anotacao = "teste",
                FechamentoAlunoId = fechamentoAlunoId
            };

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(anotacaoExistente);

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterFechamentoAlunoPorTurmaIdQuery>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new FechamentoAluno { Id = fechamentoAlunoId });

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<RemoverArquivosExcluidosCommand>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(
                It.Is<ExcluirAnotacaoFechamentoAlunoCommand>(cmd =>
                    cmd.AnotacaoFechamentoAluno != null &&
                    cmd.AnotacaoFechamentoAluno.Id == anotacaoExistente.Id &&
                    cmd.AnotacaoFechamentoAluno.FechamentoAlunoId == fechamentoAlunoId
                ),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(Unit.Value);

            var resultado = await _useCase.Executar(anotacaoDto);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirAnotacaoFechamentoAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            var auditoria = Assert.IsType<AuditoriaPersistenciaDto>(resultado);
            Assert.True(auditoria.Sucesso);
            Assert.Equal(anotacaoExistente.Id, auditoria.Id);
        }

        [Fact]
        public async Task Deve_Salvar_Anotacao_Quando_Anotacao_Preenchida()
        {
            var fechamentoId = 1L;
            var alunoCodigo = "123456";
            var fechamentoAlunoId = 20L;
            var novaAnotacao = "Anotação salva";

            var anotacaoDto = new AnotacaoAlunoDto
            {
                FechamentoId = fechamentoId,
                CodigoAluno = alunoCodigo,
                Anotacao = novaAnotacao
            };

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync((AnotacaoFechamentoAluno)null);

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterFechamentoAlunoPorTurmaIdQuery>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new FechamentoAluno { Id = fechamentoAlunoId });

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<MoverArquivosTemporariosCommand>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(novaAnotacao);

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<RemoverArquivosExcluidosCommand>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(
                It.Is<SalvarAnotacaoFechamentoAlunoCommand>(cmd =>
                    cmd.AnotacaoFechamentoAluno != null &&
                    cmd.AnotacaoFechamentoAluno.FechamentoAlunoId == fechamentoAlunoId &&
                    cmd.AnotacaoFechamentoAluno.Anotacao == novaAnotacao),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(Unit.Value);

            var resultado = await _useCase.Executar(anotacaoDto);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarAnotacaoFechamentoAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            var auditoria = Assert.IsType<AuditoriaPersistenciaDto>(resultado);
            Assert.True(auditoria.Sucesso);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarAnotacaoFechamentoAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
