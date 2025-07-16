using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoAcompanhamentoAprendizagemAluno
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase _useCase;

        public ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase(_mediatorMock.Object);
        }

        [Theory]
        [InlineData(TipoEscola.CEIDIRET)]
        [InlineData(TipoEscola.CEIINDIR)]
        [InlineData(TipoEscola.CEUCEI)]
        public async Task Executar_DeveRetornarTrueParaTiposDeEscolaEspecificos(TipoEscola tipoEscola)
        {
            var filtro = new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(1, 2024, 1, 30);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var ueMock = new Ue { TipoEscola = tipoEscola };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUEPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterUEPorTurmaIdQuery>(q => q.TurmaId == filtro.TurmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls(); 
        }

        [Fact]
        public async Task Executar_DeveConsolidarDadosComSucesso()
        {
            var filtro = new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(1, 2024, 1, 30);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var ueMock = new Ue { TipoEscola = TipoEscola.EMEBS };
            var turmaMock = new Turma { Id = 1, CodigoTurma = "T001" };
            var alunosMock = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "A1", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta { CodigoAluno = "A2", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Rematriculado },
                new AlunoPorTurmaResposta { CodigoAluno = "A3", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUEPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosPorTurmaCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAlunosComAcompanhamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2); // Mock 2 alunos com acompanhamento
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistraConsolidacaoAcompanhamentoAprendizagemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // Mock command success

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterUEPorTurmaIdQuery>(q => q.TurmaId == filtro.TurmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == filtro.TurmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAlunosAtivosPorTurmaCodigoQuery>(q => q.TurmaCodigo == turmaMock.CodigoTurma), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTotalAlunosComAcompanhamentoQuery>(q =>
                q.TurmaId == filtro.TurmaId &&
                q.AnoLetivo == filtro.AnoLetivo &&
                q.Semestre == filtro.Semestre &&
                q.CodigosAlunos.Contains("A1") && q.CodigosAlunos.Contains("A2") && q.CodigosAlunos.Contains("A3")
            ), It.IsAny<CancellationToken>()), Times.Once);

            var expectedTotalAlunosSemAcompanhamento = (filtro.QuantidadeAlunosTurma - 2) < 0 ? 0 : (filtro.QuantidadeAlunosTurma - 2);

            _mediatorMock.Verify(m => m.Send(It.Is<RegistraConsolidacaoAcompanhamentoAprendizagemCommand>(c =>
                c.TurmaId == filtro.TurmaId &&
                c.QuantidadeComAcompanhamento == 2 &&
                c.QuantidadeSemAcompanhamento == expectedTotalAlunosSemAcompanhamento &&
                c.Semestre == filtro.Semestre
            ), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarNegocioExceptionQuandoAlunosNaoSaoEncontrados()
        {
            var filtro = new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(1, 2024, 1, 30);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var ueMock = new Ue { TipoEscola = TipoEscola.EMEBS };
            var turmaMock = new Turma { Id = 1, CodigoTurma = "T001" };
            var alunosMock = new List<AlunoPorTurmaResposta>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUEPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosPorTurmaCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagem));

            Assert.Contains($"Não foram encontrados alunos para a turma {turmaMock.CodigoTurma} no Eol", ex.Message);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterUEPorTurmaIdQuery>(q => q.TurmaId == filtro.TurmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == filtro.TurmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAlunosAtivosPorTurmaCodigoQuery>(q => q.TurmaCodigo == turmaMock.CodigoTurma), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveCalcularAlunosSemAcompanhamentoCorretamenteQuandoResultadoNegativo()
        {
            var filtro = new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(1, 2024, 1, 10); // Only 10 students in class
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var ueMock = new Ue { TipoEscola = TipoEscola.EMEBS };
            var turmaMock = new Turma { Id = 1, CodigoTurma = "T001" };
            var alunosMock = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "A1", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta { CodigoAluno = "A2", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUEPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosPorTurmaCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAlunosComAcompanhamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(15);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistraConsolidacaoAcompanhamentoAprendizagemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<RegistraConsolidacaoAcompanhamentoAprendizagemCommand>(c =>
                c.QuantidadeComAcompanhamento == 15 &&
                c.QuantidadeSemAcompanhamento == 0),
            It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveFiltrarAlunosPorSituacaoMatriculaCorretamente()
        {
            var filtro = new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(1, 2024, 1, 5);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var ueMock = new Ue { TipoEscola = TipoEscola.EMEBS };
            var turmaMock = new Turma { Id = 1, CodigoTurma = "T001" };
            var allStudents = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "A1", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta { CodigoAluno = "A2", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente },
                new AlunoPorTurmaResposta { CodigoAluno = "A3", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido },
                new AlunoPorTurmaResposta { CodigoAluno = "A4", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUEPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosPorTurmaCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(allStudents);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAlunosComAcompanhamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistraConsolidacaoAcompanhamentoAprendizagemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTotalAlunosComAcompanhamentoQuery>(q =>
                q.CodigosAlunos.Length == 2 &&
                q.CodigosAlunos.Contains("A1") &&
                q.CodigosAlunos.Contains("A3") &&
                !q.CodigosAlunos.Contains("A2") &&
                !q.CodigosAlunos.Contains("A4")
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}