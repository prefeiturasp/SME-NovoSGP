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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaAluno
{
    public class ConsolidarFrequenciaAlunoPorTurmaEMesUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ConsolidarFrequenciaAlunoPorTurmaEMesUseCase _useCase;

        public ConsolidarFrequenciaAlunoPorTurmaEMesUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _useCase = new ConsolidarFrequenciaAlunoPorTurmaEMesUseCase(_mediatorMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            var filtro = new FiltroConsolidacaoFrequenciaAlunoMensal("TURMA-1", 9);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == filtro.TurmaCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Turma)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Executar_Quando_Ocorrer_Erro_Na_Persistencia_Deve_Executar_Rollback_E_Rethrow()
        {
            var filtro = new FiltroConsolidacaoFrequenciaAlunoMensal("TURMA-1", 9);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var turma = new Turma { Id = 1, CodigoTurma = "TURMA-1", AnoLetivo = 2025 };

            var frequenciasAtuais = new List<RegistroFrequenciaAlunoPorTurmaEMesDto>
            {
                new RegistroFrequenciaAlunoPorTurmaEMesDto { AlunoCodigo = "ALUNO-NOVO" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunosPorTurmaEMesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(frequenciasAtuais);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ConsolidacaoFrequenciaAlunoMensalDto>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarConsolidacaoFrequenciaAlunoMensalCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Erro de BD"));

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagem));

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Dados_Validos_Deve_Inserir_Alterar_E_Remover_Corretamente()
        {
            var filtro = new FiltroConsolidacaoFrequenciaAlunoMensal("TURMA-COMPLETA", 9);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var turma = new Turma { Id = 99, CodigoTurma = "TURMA-COMPLETA", AnoLetivo = 2025 };

            var consolidacoesExistentes = new List<ConsolidacaoFrequenciaAlunoMensalDto>
            {
                new ConsolidacaoFrequenciaAlunoMensalDto { Id = 1, AlunoCodigo = "ALUNO-ALTERAR", QuantidadeAulas = 10 },
                new ConsolidacaoFrequenciaAlunoMensalDto { Id = 2, AlunoCodigo = "ALUNO-REMOVER" },
                new ConsolidacaoFrequenciaAlunoMensalDto { Id = 3, AlunoCodigo = "ALUNO-MANTER", QuantidadeAulas = 20 }
            };

            var frequenciasAtuais = new List<RegistroFrequenciaAlunoPorTurmaEMesDto>
            {
                new RegistroFrequenciaAlunoPorTurmaEMesDto { AlunoCodigo = "ALUNO-ALTERAR", QuantidadeAulas = 15 },
                new RegistroFrequenciaAlunoPorTurmaEMesDto { AlunoCodigo = "ALUNO-INSERIR" },
                new RegistroFrequenciaAlunoPorTurmaEMesDto { AlunoCodigo = "ALUNO-MANTER", QuantidadeAulas = 20 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunosPorTurmaEMesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(frequenciasAtuais);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(consolidacoesExistentes);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<RegistrarConsolidacaoFrequenciaAlunoMensalCommand>(c => c.AlunoCodigo == "ALUNO-INSERIR"), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<AlterarConsolidacaoFrequenciaAlunoMensalCommand>(c => c.ConsolidacaoId == 1 && c.QuantidadeAulas == 15), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<RemoverConsolidacaoAlunoFrequenciaMensalCommand>(c => c.ConsolidacaoId == 2), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RegistrarConsolidacaoFrequenciaAlunoMensalCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AlterarConsolidacaoFrequenciaAlunoMensalCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverConsolidacaoAlunoFrequenciaMensalCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Never);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaTurmaEvasao), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaTurmaEvasaoAcumulado), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
