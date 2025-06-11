using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EncaminhamentoNAAPA
{
    public class AtualizarTurmaDoEncaminhamentoNAAPAUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AtualizarTurmaDoEncaminhamentoNAAPAUseCase _useCase;

        public AtualizarTurmaDoEncaminhamentoNAAPAUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AtualizarTurmaDoEncaminhamentoNAAPAUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveAtualizarTurma_QuandoAlunoPossuiNovaTurmaValida()
        {
            // Arrange
            var alunoCodigo = "123456";
            var encaminhamento = new EncaminhamentoNAAPADto
            {
                Id = 1,
                AlunoCodigo = alunoCodigo,
                TurmaId = 99, // diferente do que será retornado
                SituacaoMatriculaAluno = SituacaoMatriculaAluno.Transferido // diferente da turma
            };

            var mensagem = new MensagemRabbit
            {
                // Simula serialização da mensagem
                Mensagem = System.Text.Json.JsonSerializer.Serialize(encaminhamento)
            };

            var turmaAluno = new TurmasDoAlunoDto
            {
                CodigoTurma = 1234,
                CodigoTipoTurma = (int)TipoTurma.Regular,
                AnoLetivo = DateTime.Now.Year,
                DataSituacao = DateTime.Now.AddDays(-1),
                CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosEolPorCodigosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TurmasDoAlunoDto> { turmaAluno });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaIdPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterEncaminhamentoNAAPAComTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new SME.SGP.Dominio.EncaminhamentoNAAPA());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Turma());

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarEncaminhamentoNAAPACommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosEolPorCodigosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaIdPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarEncaminhamentoNAAPACommand>(cmd =>
                cmd.EncaminhamentoNAAPA.TurmaId == 1
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
