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
    public class ConsolidarFrequenciaTurmaEvasaoAcumuladoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase _useCase;

        public ConsolidarFrequenciaTurmaEvasaoAcumuladoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _useCase = new ConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase(_mediatorMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Executar_DeveProcessarTodasAsTurmasQuandoTurmaIdForZero()
        {
            var anoAtual = DateTime.Now.Year;
            var filtro = new FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado(anoAtual, 0);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));
            var dataConsultaEsperada = DateTime.Today;

            var dresMock = new List<long> { 1, 2 };
            var uesDre1Mock = new List<long> { 101, 102 };
            var uesDre2Mock = new List<long> { 201 };

            var turmasUe101Mock = new List<TurmaModalidadeDto> { new TurmaModalidadeDto { TurmaCodigo = "T101A", TurmaId = 10101 } };
            var turmasUe102Mock = new List<TurmaModalidadeDto> { new TurmaModalidadeDto { TurmaCodigo = "T102A", TurmaId = 10201 } };
            var turmasUe201Mock = new List<TurmaModalidadeDto> { new TurmaModalidadeDto { TurmaCodigo = "T201A", TurmaId = 20101 } };

            var alunosMock = new List<AlunoPorTurmaResposta>
        {
            new AlunoPorTurmaResposta { CodigoAluno = "A1", NomeAluno = "Aluno A", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, DataMatricula = DateTime.Now },
            new AlunoPorTurmaResposta { CodigoAluno = "A2", NomeAluno = "Aluno B", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, DataMatricula = DateTime.Now }
        };

            var frequenciasMock = new List<FrequenciaAluno>
        {
            new FrequenciaAluno { CodigoAluno = "A1", TotalAulas = 100, TotalAusencias = 30, TotalCompensacoes = 0 }, // 70%
            new FrequenciaAluno { CodigoAluno = "A2", TotalAulas = 100, TotalAusencias = 60, TotalCompensacoes = 0 }  // 40%
        };

            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesDre1Mock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == 2), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesDre2Mock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasComModalidadePorAnoUEQuery>(q => q.UeId == 101 && q.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasUe101Mock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasComModalidadePorAnoUEQuery>(q => q.UeId == 102 && q.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasUe102Mock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasComModalidadePorAnoUEQuery>(q => q.UeId == 201 && q.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasUe201Mock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosPorTurmaCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralPorAlunosTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciasMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == 1), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == 2), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasComModalidadePorAnoUEQuery>(q => q.UeId == 101 && q.Ano == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasComModalidadePorAnoUEQuery>(q => q.UeId == 102 && q.Ano == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasComModalidadePorAnoUEQuery>(q => q.UeId == 201 && q.Ano == anoAtual), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosAtivosPorTurmaCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaGeralPorAlunosTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            _mediatorMock.Verify(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            _mediatorMock.Verify(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoCommand>(c => c.QuantidadeAlunosAbaixo50Porcento == 1 && c.QuantidadeAlunox0Porcento == 0), It.IsAny<CancellationToken>()), Times.Exactly(3));
            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(c => c.AlunoCodigo == "A2" && c.PercentualFrequencia == 40.0), It.IsAny<CancellationToken>()), Times.Exactly(3));

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Exactly(3));
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Exactly(3));
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveProcessarApenasUmaTurmaQuandoTurmaIdForFornecido()
        {
            var anoAtual = DateTime.Now.Year;
            long turmaId = 123;
            var filtro = new FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado(anoAtual, turmaId);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));
            var dataConsultaEsperada = DateTime.Today;

            var turmaMock = new Turma { Id = turmaId, CodigoTurma = "TURMA123" };
            var alunosMock = new List<AlunoPorTurmaResposta>
        {
            new AlunoPorTurmaResposta { CodigoAluno = "A1", NomeAluno = "Aluno A", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, DataMatricula = DateTime.Today },
            new AlunoPorTurmaResposta { CodigoAluno = "A3", NomeAluno = "Aluno C", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, DataMatricula = DateTime.Today }
        };
            var frequenciasMock = new List<FrequenciaAluno>
        {
            new FrequenciaAluno { CodigoAluno = "A1", TotalAulas = 100, TotalAusencias = 90, TotalCompensacoes = 0 }, // 10%
            new FrequenciaAluno { CodigoAluno = "A3", TotalAulas = 100, TotalAusencias = 100, TotalCompensacoes = 0 }  // 0%
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosAtivosPorTurmaCodigoQuery>(q => q.TurmaCodigo == turmaMock.CodigoTurma && q.DataAula == dataConsultaEsperada), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaGeralPorAlunosTurmaQuery>(q => q.CodigoTurma == turmaMock.CodigoTurma && q.CodigosAlunos.Contains("A1")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciasMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAlunosAtivosPorTurmaCodigoQuery>(q => q.TurmaCodigo == turmaMock.CodigoTurma && q.DataAula == dataConsultaEsperada), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciaGeralPorAlunosTurmaQuery>(q => q.CodigoTurma == turmaMock.CodigoTurma && q.CodigosAlunos.Contains("A1")), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(c => c.TurmasIds.Contains(turmaId)), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(c => c.TurmasIds.Contains(turmaId)), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoCommand>(c => c.TurmaId == turmaId && c.QuantidadeAlunosAbaixo50Porcento == 2 && c.QuantidadeAlunox0Porcento == 1), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(c => c.AlunoCodigo == "A1" && c.PercentualFrequencia == 10.0), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(c => c.AlunoCodigo == "A3" && c.PercentualFrequencia == 0.0), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveDefinirDataDeConsultaPara31DeDezembroQuandoAnoNaoForAtual()
        {
            var anoAnterior = DateTime.Now.Year - 1;
            var filtro = new FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado(anoAnterior, 0);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));
            var dataConsultaEsperada = new DateTime(anoAnterior, 12, 31);

            var dresMock = new List<long> { 1 };
            var uesDre1Mock = new List<long> { 101 };
            var turmasUe101Mock = new List<TurmaModalidadeDto> { new TurmaModalidadeDto { TurmaCodigo = "T101A", TurmaId = 10101 } };

            var alunosMock = new List<AlunoPorTurmaResposta>
        {
            new AlunoPorTurmaResposta { CodigoAluno = "A1", NomeAluno = "Aluno A", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, DataMatricula = DateTime.Now }
        };
            var frequenciasMock = new List<FrequenciaAluno>
        {
            new FrequenciaAluno { CodigoAluno = "A1", TotalAulas = 100, TotalAusencias = 40, TotalCompensacoes = 0 } // 60%
        };

            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterUEsIdsPorDreQuery>(q => q.DreId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesDre1Mock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasComModalidadePorAnoUEQuery>(q => q.UeId == 101 && q.Ano == anoAnterior), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasUe101Mock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosAtivosPorTurmaCodigoQuery>(q => q.DataAula == dataConsultaEsperada), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralPorAlunosTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciasMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarFrequenciaTurmaEvasaoAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAlunosAtivosPorTurmaCodigoQuery>(q => q.DataAula == dataConsultaEsperada), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveFazerRollbackDaTransacaoEmCasoDeExcecao()
        {
            var anoAtual = DateTime.Now.Year;
            long turmaId = 123;
            var filtro = new FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado(anoAtual, turmaId);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));
            var turmaMock = new Turma { Id = turmaId, CodigoTurma = "TURMA123" };
            var alunosMock = new List<AlunoPorTurmaResposta>
        {
            new AlunoPorTurmaResposta { CodigoAluno = "A1", NomeAluno = "Aluno A", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, DataMatricula = DateTime.Now }
        };
            var frequenciasMock = new List<FrequenciaAluno>
        {
            new FrequenciaAluno { CodigoAluno = "A1", TotalAulas = 100, TotalAusencias = 90, TotalCompensacoes = 0 } // 10%
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosPorTurmaCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralPorAlunosTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciasMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro no banco de dados"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagem));

            Assert.Equal("Erro no banco de dados", exception.Message);

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Never);
        }
    }
}