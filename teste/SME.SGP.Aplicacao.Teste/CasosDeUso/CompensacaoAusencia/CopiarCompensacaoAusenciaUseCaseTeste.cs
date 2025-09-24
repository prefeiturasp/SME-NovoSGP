using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class CopiarCompensacaoAusenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioCompensacaoAusencia> _repositorioCompensacaoAusenciaMock;
        private readonly Mock<IRepositorioCompensacaoAusenciaDisciplinaRegencia> _repositorioDisciplinaRegenciaMock;
        private readonly Mock<ISalvarCompensacaoAusenciaUseCase> _salvarCompensacaoAusenciaUseCaseMock;
        private readonly CopiarCompensacaoAusenciaUseCase _useCase;

        public CopiarCompensacaoAusenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioCompensacaoAusenciaMock = new Mock<IRepositorioCompensacaoAusencia>();
            _repositorioDisciplinaRegenciaMock = new Mock<IRepositorioCompensacaoAusenciaDisciplinaRegencia>();
            _salvarCompensacaoAusenciaUseCaseMock = new Mock<ISalvarCompensacaoAusenciaUseCase>();

            _useCase = new CopiarCompensacaoAusenciaUseCase(
                _mediatorMock.Object,
                _repositorioCompensacaoAusenciaMock.Object,
                _repositorioDisciplinaRegenciaMock.Object,
                _salvarCompensacaoAusenciaUseCaseMock.Object
            );
        }

        [Fact]
        public async Task Executar_DeveCopiarParaTodasTurmasERetornarMensagemDeSucesso()
        {
            // Arrange
            var param = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = 1,
                TurmasIds = new List<string> { "T1", "T2" },
                Bimestre = 2
            };

            var compensacaoOrigem = new SME.SGP.Dominio.CompensacaoAusencia
            {
                Id = 1,
                DisciplinaId = "D1",
                Nome = "Atividade Teste",
                Descricao = "Descrição Teste"
            };

            var disciplinasRegencia = new List<CompensacaoAusenciaDisciplinaRegencia>
            {
                new CompensacaoAusenciaDisciplinaRegencia { DisciplinaId = "DR1" },
                new CompensacaoAusenciaDisciplinaRegencia { DisciplinaId = "DR2" }
            };

            _repositorioCompensacaoAusenciaMock.Setup(r => r.ObterPorId(param.CompensacaoOrigemId))
                .Returns(compensacaoOrigem);

            _repositorioDisciplinaRegenciaMock.Setup(r => r.ObterPorCompensacao(compensacaoOrigem.Id))
                .ReturnsAsync(disciplinasRegencia);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ObterTurmaPorCodigoQuery query, CancellationToken token) =>
                    new Turma { Nome = $"Turma {query.TurmaCodigo}" });

            _salvarCompensacaoAusenciaUseCaseMock.Setup(s => s.Executar(It.IsAny<long>(), It.IsAny<CompensacaoAusenciaDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _useCase.Executar(param);

            // Assert
            Assert.Contains("A cópia para as turmas Turma T1, Turma T2 foi realizada com sucesso", resultado);

            _repositorioCompensacaoAusenciaMock.Verify(r => r.ObterPorId(param.CompensacaoOrigemId), Times.Once);
            _repositorioDisciplinaRegenciaMock.Verify(r => r.ObterPorCompensacao(compensacaoOrigem.Id), Times.Exactly(2));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _salvarCompensacaoAusenciaUseCaseMock.Verify(s => s.Executar(0, It.IsAny<CompensacaoAusenciaDto>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoCompensacaoOrigemNaoEncontrada()
        {
            // Arrange
            var param = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = 99,
                TurmasIds = new List<string> { "T1" },
                Bimestre = 1
            };

            _repositorioCompensacaoAusenciaMock.Setup(r => r.ObterPorId(param.CompensacaoOrigemId))
                .Returns((Dominio.CompensacaoAusencia)(object)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(param));
            Assert.Contains("Compensação de origem não localizada", ex.Message);
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoSalvarCompensacaoFalhaEmUmaTurma()
        {
            // Arrange
            var param = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = 1,
                TurmasIds = new List<string> { "T1", "T2" },
                Bimestre = 2
            };

            var compensacaoOrigem = new SME.SGP.Dominio.CompensacaoAusencia
            {
                Id = 1,
                DisciplinaId = "D1",
                Nome = "Atividade Teste",
                Descricao = "Descrição Teste"
            };

            _repositorioCompensacaoAusenciaMock.Setup(r => r.ObterPorId(param.CompensacaoOrigemId))
                .Returns(compensacaoOrigem);

            _repositorioDisciplinaRegenciaMock.Setup(r => r.ObterPorCompensacao(compensacaoOrigem.Id))
                .ReturnsAsync(new List<CompensacaoAusenciaDisciplinaRegencia>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ObterTurmaPorCodigoQuery query, CancellationToken token) =>
                    new Turma { Nome = $"Turma {query.TurmaCodigo}" });

            _salvarCompensacaoAusenciaUseCaseMock.SetupSequence(s => s.Executar(0, It.IsAny<CompensacaoAusenciaDto>()))
                .Returns(Task.CompletedTask)
                .ThrowsAsync(new Exception("Falha ao salvar"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(param));
            Assert.Contains("A cópia para a turma Turma T2 não foi realizada: Falha ao salvar", ex.Message);
            Assert.Contains("A cópia para a turma Turma T1 foi realizada com sucesso", ex.Message);
        }
    }
}
