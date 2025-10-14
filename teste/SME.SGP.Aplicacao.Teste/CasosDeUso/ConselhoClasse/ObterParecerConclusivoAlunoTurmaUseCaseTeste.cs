using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterParecerConclusivoAlunoTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterParecerConclusivoAlunoTurmaUseCase _useCase;

        public ObterParecerConclusivoAlunoTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterParecerConclusivoAlunoTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Executar_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterParecerConclusivoAlunoTurmaUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_ConselhoClasseAluno_Nao_Encontrado_Deve_Retornar_Dto_Vazio()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Turma());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ConselhoClasseAluno)null);

            var resultado = await _useCase.Executar("turma", "aluno");

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.Id);
            Assert.Null(resultado.Nome);
            Assert.False(resultado.EmAprovacao);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Atual_Sem_Parecer_Deve_Gerar_Novo_Parecer()
        {
            var turma = new Turma { AnoLetivo = DateTime.Now.Year };
            var conselhoAluno = new ConselhoClasseAluno
            {
                Id = 1,
                ConselhoClasseParecerId = null,
                ConselhoClasse = new SME.SGP.Dominio.ConselhoClasse { FechamentoTurmaId = 99 }
            };
            var parecerGerado = new ParecerConclusivoDto { Id = 10, Nome = "Aprovado" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(conselhoAluno);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(parecerGerado);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterSePossuiParecerEmAprovacaoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((WFAprovacaoParecerConclusivo)null);

            var resultado = await _useCase.Executar("turma", "aluno");

            Assert.Equal(parecerGerado.Id, resultado.Id);
            Assert.Equal(parecerGerado.Nome, resultado.Nome);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Turma_De_Ano_Anterior_Deve_Retornar_Parecer_Existente()
        {
            var turma = new Turma { AnoLetivo = DateTime.Now.Year - 1 };
            var conselhoAluno = new ConselhoClasseAluno
            {
                Id = 1,
                ConselhoClasseParecerId = 20,
                ConselhoClasseParecer = new ConselhoClasseParecerConclusivo { Nome = "Retido" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(conselhoAluno);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterSePossuiParecerEmAprovacaoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((WFAprovacaoParecerConclusivo)null);

            var resultado = await _useCase.Executar("turma", "aluno");

            Assert.Equal(conselhoAluno.ConselhoClasseParecerId, resultado.Id);
            Assert.Equal(conselhoAluno.ConselhoClasseParecer.Nome, resultado.Nome);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Atual_Com_Parecer_Deve_Retornar_Parecer_Existente()
        {
            var turma = new Turma { AnoLetivo = DateTime.Now.Year };
            var conselhoAluno = new ConselhoClasseAluno
            {
                Id = 1,
                ConselhoClasseParecerId = 30,
                ConselhoClasseParecer = new ConselhoClasseParecerConclusivo { Nome = "Promovido" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(conselhoAluno);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterSePossuiParecerEmAprovacaoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((WFAprovacaoParecerConclusivo)null);

            var resultado = await _useCase.Executar("turma", "aluno");

            Assert.Equal(conselhoAluno.ConselhoClasseParecerId, resultado.Id);
            Assert.Equal(conselhoAluno.ConselhoClasseParecer.Nome, resultado.Nome);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Existe_Parecer_Em_Aprovacao_Deve_Retornar_Parecer_Em_Aprovacao()
        {
            var turma = new Turma { AnoLetivo = DateTime.Now.Year };
            var conselhoAluno = new ConselhoClasseAluno { Id = 1, ConselhoClasseParecerId = 30 };
            var wfAprovacao = new WFAprovacaoParecerConclusivo
            {
                ConselhoClasseParecerId = 40,
                ConselhoClasseParecer = new ConselhoClasseParecerConclusivo { Nome = "Aprovado pelo Coordenador" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(conselhoAluno);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterSePossuiParecerEmAprovacaoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(wfAprovacao);

            var resultado = await _useCase.Executar("turma", "aluno");

            Assert.Equal(wfAprovacao.ConselhoClasseParecerId, resultado.Id);
            Assert.Equal(wfAprovacao.ConselhoClasseParecer.Nome, resultado.Nome);
            Assert.True(resultado.EmAprovacao);
        }
    }
}
