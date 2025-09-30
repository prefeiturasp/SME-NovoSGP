using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoMatriculaTurma
{
    public class CarregarMatriculaTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CarregarMatriculaTurmaUseCase _useCase;

        public CarregarMatriculaTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new CarregarMatriculaTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Apenas_Ano_Atual_Deve_Consolidar_Corretamente()
        {
            var filtro = new FiltroConsolidacaoMatriculaUeDto("UE-CODIGO", Enumerable.Empty<int>());
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            var matriculasAnoAtual = new List<ConsolidacaoMatriculaTurmaDto>
            {
                new ConsolidacaoMatriculaTurmaDto("TURMA-VALIDA-ATUAL", 30)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMatriculasConsolidacaoPorAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(matriculasAnoAtual);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaIdPorCodigoQuery>(q => q.TurmaCodigo == "TURMA-VALIDA-ATUAL"), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1L);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterMatriculasConsolidacaoAnosAnterioresQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpInstitucional.ConsolidacaoMatriculasTurmasSync), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Tiver_Ano_Atual_E_Historico_Deve_Consolidar_Ambos()
        {
            var anosAnteriores = new List<int> { DateTime.Now.Year - 1 };
            var filtro = new FiltroConsolidacaoMatriculaUeDto("UE-CODIGO", anosAnteriores);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            var matriculasAnoAtual = new List<ConsolidacaoMatriculaTurmaDto>
            {
                new ConsolidacaoMatriculaTurmaDto("TURMA-VALIDA-ATUAL", 25)
            };
            var matriculasAnoHistorico = new List<ConsolidacaoMatriculaTurmaDto>
            {
                new ConsolidacaoMatriculaTurmaDto("TURMA-VALIDA-HIST", 28)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMatriculasConsolidacaoPorAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(matriculasAnoAtual);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMatriculasConsolidacaoAnosAnterioresQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(matriculasAnoHistorico);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaIdPorCodigoQuery>(q => q.TurmaCodigo == "TURMA-VALIDA-ATUAL"), It.IsAny<CancellationToken>())).ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaIdPorCodigoQuery>(q => q.TurmaCodigo == "TURMA-VALIDA-HIST"), It.IsAny<CancellationToken>())).ReturnsAsync(2L);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterMatriculasConsolidacaoAnosAnterioresQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpInstitucional.ConsolidacaoMatriculasTurmasSync), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Deve_Ignorar_Matriculas_De_Turmas_Inexistentes_No_SGP()
        {
            var anosAnteriores = new List<int> { DateTime.Now.Year - 1 };
            var filtro = new FiltroConsolidacaoMatriculaUeDto("UE-CODIGO", anosAnteriores);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            var matriculasAnoAtual = new List<ConsolidacaoMatriculaTurmaDto>
            {
                new ConsolidacaoMatriculaTurmaDto("TURMA-INVALIDA-ATUAL", 10)
            };
            var matriculasAnoHistorico = new List<ConsolidacaoMatriculaTurmaDto>
            {
                new ConsolidacaoMatriculaTurmaDto("TURMA-INVALIDA-HIST", 15)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMatriculasConsolidacaoPorAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(matriculasAnoAtual);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMatriculasConsolidacaoAnosAnterioresQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(matriculasAnoHistorico);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaIdPorCodigoQuery>(q => q.TurmaCodigo.StartsWith("TURMA-INVALIDA")), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0L);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpInstitucional.ConsolidacaoMatriculasTurmasSync), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
