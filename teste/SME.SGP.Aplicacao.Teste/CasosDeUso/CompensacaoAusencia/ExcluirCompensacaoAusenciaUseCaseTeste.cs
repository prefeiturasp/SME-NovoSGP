using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class ExcluirCompensacaoAusenciaUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveExcluirCompensacaoAusencia_ComFluxoCompleto()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var compensacaoId = 1L;
            var compensacaoIds = new[] { compensacaoId };

            // Compensação principal
            var compensacao = new SME.SGP.Dominio.CompensacaoAusencia
            {
                Id = compensacaoId,
                TurmaId = 10,
                Bimestre = 2,
                Descricao = "Compensação Teste",
                DisciplinaId = "MAT",
                AnoLetivo = 2025
            };

            // Turma
            var turma = new Turma
            {
                Id = 10,
                AnoLetivo = 2025,
                ModalidadeCodigo = Modalidade.EJA,
                Semestre = 1,
                CodigoTurma = "TURMA1"
            };

            // Periodo escolar
            var periodoEscolar = new PeriodoEscolarDto
            {
                Bimestre = 2,
                Id = 99,
                PeriodoInicio = new DateTime(2025, 2, 1),
                PeriodoFim = new DateTime(2025, 4, 30),
                Migrado = false
            };

            var periodoEscolarListaDto = new PeriodoEscolarListaDto
            {
                TipoCalendario = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = "RF123",
                AlteradoEm = DateTime.Now,
                AlteradoPor = "Teste",
                AlteradoRF = "RF123",
                Periodos = new List<PeriodoEscolarDto> { periodoEscolar }
            };

            // Compensação aluno
            var compensacaoAluno = new CompensacaoAusenciaAluno
            {
                Id = 100,
                CompensacaoAusenciaId = compensacaoId,
                CodigoAluno = "A1"
            };

            // Compensação disciplina regência
            var compensacaoDisciplinaRegencia = new CompensacaoAusenciaDisciplinaRegencia
            {
                Id = 200,
                CompensacaoAusenciaId = compensacaoId,
                DisciplinaId = "MAT"
            };

            // Compensação aluno aula
            var compensacaoAlunoAula = new CompensacaoAusenciaAlunoAula
            {
                Id = 300,
                CompensacaoAusenciaAlunoId = compensacaoAluno.Id,
                NumeroAula = 1,
                DataAula = DateTime.Today
            };

            // Mock dos métodos privados (queries)
            mediatorMock.Setup(m => m.Send(It.Is<ObterCompensacaoAusenciaPorIdQuery>(q => q.Id == compensacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(compensacao);

            mediatorMock.Setup(m => m.Send(It.Is<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery>(q => q.CompensacaoId == compensacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CompensacaoAusenciaAluno> { compensacaoAluno });

            mediatorMock.Setup(m => m.Send(It.Is<ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery>(q => q.CompensacaoId == compensacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CompensacaoAusenciaDisciplinaRegencia> { compensacaoDisciplinaRegencia });

            mediatorMock.Setup(m => m.Send(It.Is<ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery>(q => q.CompensacaoId == compensacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CompensacaoAusenciaAlunoAula> { compensacaoAlunoAula });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.Is<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(
                q => q.AnoLetivo == turma.AnoLetivo && q.Modalidade == turma.ModalidadeCodigo.ObterModalidadeTipoCalendario() && q.Semestre == turma.Semestre),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SME.SGP.Dominio.TipoCalendario { Id = 1, Nome = "Calendário Teste", AnoLetivo = turma.AnoLetivo, Modalidade = turma.ModalidadeCodigo.ObterModalidadeTipoCalendario() });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });
                       
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarListaPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolarListaDto);

            // Mock dos comandos de persistência
            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaDiciplinaRegenciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirNotificacaoCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            mediatorMock.Setup(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Mock transação
            unitOfWorkMock.Setup(u => u.IniciarTransacao());
            unitOfWorkMock.Setup(u => u.PersistirTransacao());
            unitOfWorkMock.Setup(u => u.Rollback());

            var useCase = new ExcluirCompensacaoAusenciaUseCase(mediatorMock.Object, unitOfWorkMock.Object);

            // Act
            var resultado = await useCase.Executar(compensacaoIds);

            // Assert
            Assert.True(resultado);

            // Verificações detalhadas dos mocks
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaDiciplinaRegenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirNotificacaoCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
        }
    }
}
