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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurmaEvasao
{
    public class ConsolidarFrequenciaTurmaEvasaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ConsolidarFrequenciaTurmaEvasaoUseCase _useCase;

        public ConsolidarFrequenciaTurmaEvasaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _useCase = new ConsolidarFrequenciaTurmaEvasaoUseCase(_mediatorMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Executar_DeveConsolidarFrequenciaCorretamente()
        {
            // Arrange
            long turmaId = 123;
            int mes = 5;
            var filtro = new FiltroConsolidacaoFrequenciaTurmaEvasao(turmaId, mes);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var consolidacoesMensaisMock = new List<ConsolidacaoFrequenciaAlunoMensalDto>
        {
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A001", Percentual = 75.0M, TurmaId = turmaId, Mes = mes },
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A002", Percentual = 40.0M, TurmaId = turmaId, Mes = mes }, 
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A003", Percentual = 0.0M, TurmaId = turmaId, Mes = mes }, 
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A004", Percentual = 49.9M, TurmaId = turmaId, Mes = mes }, 
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A001", Percentual = 75.0M, TurmaId = turmaId, Mes = mes } 
        };

            var alunosEolMock = new List<AlunoPorTurmaResposta>
        {
            new AlunoPorTurmaResposta { CodigoAluno = "A001", NomeAluno = "Aluno Um" },
            new AlunoPorTurmaResposta { CodigoAluno = "A002", NomeAluno = "Aluno Dois" },
            new AlunoPorTurmaResposta { CodigoAluno = "A003", NomeAluno = "Aluno Tres" },
            new AlunoPorTurmaResposta { CodigoAluno = "A004", NomeAluno = "Aluno Quatro" }
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery>(q => q.TurmaId == turmaId && q.Mes == mes), It.IsAny<CancellationToken>()))
                .ReturnsAsync(consolidacoesMensaisMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaCodigoPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync("COD_TURMA_123");

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosEolPorTurmaQuery>(q => q.TurmaId == "COD_TURMA_123" && q.ConsideraInativos == true), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosEolMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(100L);

            // Act
            var result = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(result);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery>(q => q.TurmaId == turmaId && q.Mes == mes), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(c => c.TurmasIds.Contains(turmaId) && c.Meses.Contains(mes)), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(c => c.TurmasIds.Contains(turmaId) && c.Meses.Contains(mes)), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoCommand>(c =>
                c.TurmaId == turmaId &&
                c.Mes == mes &&
                c.QuantidadeAlunosAbaixo50Porcento == 2 &&
                c.QuantidadeAlunox0Porcento == 1), 
                It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(c =>
                c.FrequenciaTurmaEvasaoId == 100L &&
                c.AlunoCodigo == "A002" &&
                c.AlunoNome == "Aluno Dois" &&
                c.PercentualFrequencia == 40.0), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(c =>
                c.FrequenciaTurmaEvasaoId == 100L &&
                c.AlunoCodigo == "A003" &&
                c.AlunoNome == "Aluno Tres" &&
                c.PercentualFrequencia == 0.0), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(c =>
                c.FrequenciaTurmaEvasaoId == 100L &&
                c.AlunoCodigo == "A004" &&
                c.AlunoNome == "Aluno Quatro" &&
                c.PercentualFrequencia == 49.9), It.IsAny<CancellationToken>()), Times.Once);


            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveFazerRollbackEmCasoDeErro()
        {
            // Arrange
            long turmaId = 123;
            int mes = 5;
            var filtro = new FiltroConsolidacaoFrequenciaTurmaEvasao(turmaId, mes);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var consolidacoesMensaisMock = new List<ConsolidacaoFrequenciaAlunoMensalDto>
        {
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A001", Percentual = 75.0M, TurmaId = turmaId, Mes = mes }
        };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(consolidacoesMensaisMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro simulado no banco de dados."));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagem));

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveLidarComConsolidacoesVazias()
        {
            // Arrange
            long turmaId = 456;
            int mes = 6;
            var filtro = new FiltroConsolidacaoFrequenciaTurmaEvasao(turmaId, mes);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var consolidacoesMensaisMock = new List<ConsolidacaoFrequenciaAlunoMensalDto>();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery>(q => q.TurmaId == turmaId && q.Mes == mes), It.IsAny<CancellationToken>()))
                .ReturnsAsync(consolidacoesMensaisMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(200L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            // Act
            var result = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(result);

            _mediatorMock.Verify(m => m.Send(It.Is<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(c => c.TurmasIds.Contains(turmaId) && c.Meses.Contains(mes)), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(c => c.TurmasIds.Contains(turmaId) && c.Meses.Contains(mes)), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoCommand>(c =>
                c.TurmaId == turmaId &&
                c.Mes == mes &&
                c.QuantidadeAlunosAbaixo50Porcento == 0 &&
                c.QuantidadeAlunox0Porcento == 0),
                It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveExcluirDuplicatasPorCodigoAluno()
        {
            // Arrange
            long turmaId = 789;
            int mes = 7;
            var filtro = new FiltroConsolidacaoFrequenciaTurmaEvasao(turmaId, mes);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var consolidacoesMensaisMock = new List<ConsolidacaoFrequenciaAlunoMensalDto>
        {
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A005", Percentual = 30.0M, TurmaId = turmaId, Mes = mes },
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A005", Percentual = 20.0M, TurmaId = turmaId, Mes = mes },
            new ConsolidacaoFrequenciaAlunoMensalDto { AlunoCodigo = "A006", Percentual = 0.0M, TurmaId = turmaId, Mes = mes }
        };

            var alunosEolMock = new List<AlunoPorTurmaResposta>
        {
            new AlunoPorTurmaResposta { CodigoAluno = "A005", NomeAluno = "Aluno Cinco" },
            new AlunoPorTurmaResposta { CodigoAluno = "A006", NomeAluno = "Aluno Seis" }
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery>(q => q.TurmaId == turmaId && q.Mes == mes), It.IsAny<CancellationToken>()))
                .ReturnsAsync(consolidacoesMensaisMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaCodigoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("COD_TURMA_789");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosEolPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosEolMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(300L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            // Act
            await _useCase.Executar(mensagem);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoCommand>(c =>
                c.QuantidadeAlunosAbaixo50Porcento == 1 && 
                c.QuantidadeAlunox0Porcento == 1), 
                It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(c =>
                c.AlunoCodigo == "A005" &&
                (c.PercentualFrequencia == 30.0 || c.PercentualFrequencia == 20.0)), 
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
