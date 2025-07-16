using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.ConsolidacaoAcompanhamentoAprendizagemAluno
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase _useCase;

        public ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveConsolidarDadosComSucesso()
        {
            var ueCodigo = "UE001";
            var anoLetivo = DateTime.Now.Year;
            var filtro = new FiltroUEDto(ueCodigo, anoLetivo);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var turmasMock = new List<TurmaDTO>
            {
                new TurmaDTO { TurmaId = 1, TurmaCodigo = "T001" },
                new TurmaDTO { TurmaId = 2, TurmaCodigo = "T002" }
            };

            var quantidadesAlunosMock = new List<AlunosPorTurmaDto>
            {
                new AlunosPorTurmaDto { TurmaCodigo = "T001", Quantidade = 25 },
                new AlunosPorTurmaDto { TurmaCodigo = "T002", Quantidade = 30 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuantidadeAlunosPorTurmaNaUEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(quantidadesAlunosMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasInfantilPorUEQuery>(q => q.AnoLetivo == anoLetivo && q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterQuantidadeAlunosPorTurmaNaUEQuery>(q => q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(command =>
                command.Rota == RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).TurmaId == 1 &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).AnoLetivo == anoLetivo &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).Semestre == 1 &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).QuantidadeAlunosTurma == 25), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(command =>
                command.Rota == RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).TurmaId == 1 &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).AnoLetivo == anoLetivo &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).Semestre == 2 &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).QuantidadeAlunosTurma == 25), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(command =>
                command.Rota == RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).TurmaId == 2 &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).AnoLetivo == anoLetivo &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).Semestre == 1 &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).QuantidadeAlunosTurma == 30), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(command =>
                command.Rota == RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).TurmaId == 2 &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).AnoLetivo == anoLetivo &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).Semestre == 2 &&
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).QuantidadeAlunosTurma == 30), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoNaoEncontrarTurmas()
        {
            var ueCodigo = "UE002";
            var anoLetivo = DateTime.Now.Year;
            var filtro = new FiltroUEDto(ueCodigo, anoLetivo);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<TurmaDTO>)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagem));
            Assert.Contains("Não foi possível localizar turmas para consolidar dados de Média de Registros Individuais", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasInfantilPorUEQuery>(q => q.AnoLetivo == anoLetivo && q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoObterTurmasInfantilFalha()
        {
            var ueCodigo = "UE003";
            var anoLetivo = DateTime.Now.Year;
            var filtro = new FiltroUEDto(ueCodigo, anoLetivo);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter turmas infantis"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagem));
            Assert.Contains("Erro ao obter turmas infantis", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasInfantilPorUEQuery>(q => q.AnoLetivo == anoLetivo && q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoObterQuantidadeAlunosPorTurmaNaUEFalha()
        {
            var ueCodigo = "UE004";
            var anoLetivo = DateTime.Now.Year;
            var filtro = new FiltroUEDto(ueCodigo, anoLetivo);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var turmasMock = new List<TurmaDTO>
            {
                new TurmaDTO { TurmaId = 1, TurmaCodigo = "T001" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuantidadeAlunosPorTurmaNaUEQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter quantidade de alunos por turma"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagem));
            Assert.Contains("Erro ao obter quantidade de alunos por turma", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasInfantilPorUEQuery>(q => q.AnoLetivo == anoLetivo && q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterQuantidadeAlunosPorTurmaNaUEQuery>(q => q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveUsarAnoAtualQuandoAnoLetivoNaoInformado()
        {
            var ueCodigo = "UE006";
            var filtro = new FiltroUEDto(ueCodigo, 0);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;

            var turmasMock = new List<TurmaDTO>
            {
                new TurmaDTO { TurmaId = 1, TurmaCodigo = "T001" }
            };

            var quantidadesAlunosMock = new List<AlunosPorTurmaDto>
            {
                new AlunosPorTurmaDto { TurmaCodigo = "T001", Quantidade = 20 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuantidadeAlunosPorTurmaNaUEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(quantidadesAlunosMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _useCase.Executar(mensagem);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasInfantilPorUEQuery>(q => q.AnoLetivo == anoAtual && q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterQuantidadeAlunosPorTurmaNaUEQuery>(q => q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(command =>
                ((FiltroAcompanhamentoAprendizagemAlunoTurmaDTO)command.Filtros).AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Exactly(2));

            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoTurmasVazias()
        {
            var ueCodigo = "UE007";
            var anoLetivo = DateTime.Now.Year;
            var filtro = new FiltroUEDto(ueCodigo, anoLetivo);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TurmaDTO>());

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagem));
            Assert.Contains("Não foi possível localizar turmas para consolidar dados de Média de Registros Individuais", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasInfantilPorUEQuery>(q => q.AnoLetivo == anoLetivo && q.UeCodigo == ueCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}