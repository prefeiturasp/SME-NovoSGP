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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Nota
{
    public class NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase _useCase;

        public NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Com_Bimestre_Nao_Nulo_Deve_Chamar_Adicionar_Nivel_Apenas_CP()
        {
            var dados = new List<WfAprovacaoNotaFechamentoTurmaDto>
            {
                new WfAprovacaoNotaFechamentoTurmaDto
                {
                    FechamentoTurmaDisciplinaId = 1,
                    TurmaId = 10,
                    Bimestre = 1,
                    WfAprovacao = new WfAprovacaoNotaFechamento { ConceitoId = null, Id = 100 }
                }
            };

            var turmaFake = CriarTurmaFake();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaFake);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarWorkflowAprovacaoNivelUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(999);

            _mediatorMock.Setup(m => m.Send(It.IsAny<AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagemRabbit = new MensagemRabbit();
            mensagemRabbit.Mensagem = JsonConvert.SerializeObject(dados);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarWorkflowAprovacaoNivelUsuarioCommand>(cmd =>
                cmd != null
                && cmd.WorkflowAprovacao != null
                && cmd.WorkflowAprovacao.Niveis.Any(n => n.Cargo == Cargo.CP)
                && !cmd.WorkflowAprovacao.Niveis.Any(n => n.Cargo == Cargo.Supervisor)
            ), It.IsAny<CancellationToken>()), Moq.Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand>(cmd =>
                cmd.WorkflowAprovacaoFechamentoNotaIds.SequenceEqual(new long[] { 100 })
            ), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Executar_Com_Bimestre_Nulo_Deve_Chamar_Adicionar_Nivel_CP_E_Supervisor()
        {
            var dados = new List<WfAprovacaoNotaFechamentoTurmaDto>
            {
                new WfAprovacaoNotaFechamentoTurmaDto
                {
                    FechamentoTurmaDisciplinaId = 1,
                    TurmaId = 10,
                    Bimestre = null,
                    WfAprovacao = new WfAprovacaoNotaFechamento { ConceitoId = null, Id = 100 }
                }
            };

            var turmaFake = CriarTurmaFake();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaFake);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarWorkflowAprovacaoNivelUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(999);

            _mediatorMock.Setup(m => m.Send(It.IsAny<AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagemRabbit = new MensagemRabbit();
            mensagemRabbit.Mensagem = JsonConvert.SerializeObject(dados);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarWorkflowAprovacaoNivelUsuarioCommand>(cmd =>
                cmd != null
                && cmd.WorkflowAprovacao != null
                && cmd.WorkflowAprovacao.Niveis.Any(c => c.Cargo == Cargo.CP)
                && cmd.WorkflowAprovacao.Niveis.Any(c => c.Cargo == Cargo.Supervisor)
            ), It.IsAny<CancellationToken>()), Moq.Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand>(cmd =>
                cmd.WorkflowAprovacaoFechamentoNotaIds.SequenceEqual(new long[] { 100 })
            ), It.IsAny<CancellationToken>()), Moq.Times.Once);
        }
        private Turma CriarTurmaFake()
        {
            return new Turma
            {
                CodigoTurma = "10",
                AnoLetivo = 2025,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Nome = "Turma Teste",
                Ue = new Ue
                {
                    CodigoUe = "UE1",
                    Nome = "Escola Teste",
                    Dre = new SME.SGP.Dominio.Dre { CodigoDre = "DRE1", Abreviacao = "DRE" },
                    TipoEscola = new TipoEscola {  }
                }
            };
        }
    }
}
