using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterConselhoClasseConsolidadoPorTurmaBimestreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterConselhoClasseConsolidadoPorTurmaBimestreUseCase _useCase;

        public ObterConselhoClasseConsolidadoPorTurmaBimestreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterConselhoClasseConsolidadoPorTurmaBimestreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Obter_Conselho_Classe_Consolidado_Quando_Mediator_Nulo_Deve_Lancar_Excecao()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterConselhoClasseConsolidadoPorTurmaBimestreUseCase(null));
        }

        [Fact]
        public async Task Obter_Conselho_Classe_Consolidado_Quando_Retorno_Nulo_Do_Mediator_Deve_Retornar_Enumerable_Vazio()
        {
            var filtro = new FiltroConselhoClasseConsolidadoTurmaBimestreDto(1, 1, 1);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((List<ConselhoClasseAlunoDto>)null);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Obter_Conselho_Classe_Consolidado_Quando_Retorno_Vazio_Do_Mediator_Deve_Retornar_Enumerable_Vazio()
        {
            var filtro = new FiltroConselhoClasseConsolidadoTurmaBimestreDto(1, 1, 1);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<ConselhoClasseAlunoDto>());

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Obter_Conselho_Classe_Consolidado_Quando_Retorna_Alunos_Com_Status_Parciais_Deve_Mapear_E_Retornar_Todos_Status()
        {
            var filtro = new FiltroConselhoClasseConsolidadoTurmaBimestreDto(1, 1, 1);
            var listaAlunos = new List<ConselhoClasseAlunoDto>
            {
                new ConselhoClasseAlunoDto { SituacaoFechamentoCodigo = (int)SituacaoConselhoClasse.NaoIniciado },
                new ConselhoClasseAlunoDto { SituacaoFechamentoCodigo = (int)SituacaoConselhoClasse.NaoIniciado },
                new ConselhoClasseAlunoDto { SituacaoFechamentoCodigo = (int)SituacaoConselhoClasse.Concluido }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaAlunos);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());

            Assert.Equal(2, resultado.First(r => r.Status == (int)SituacaoConselhoClasse.NaoIniciado).Quantidade);
            Assert.Equal(0, resultado.First(r => r.Status == (int)SituacaoConselhoClasse.EmAndamento).Quantidade);
            Assert.Equal(1, resultado.First(r => r.Status == (int)SituacaoConselhoClasse.Concluido).Quantidade);

            Assert.Equal(SituacaoConselhoClasse.NaoIniciado.Name(), resultado.First(r => r.Status == (int)SituacaoConselhoClasse.NaoIniciado).Descricao);
            Assert.Equal(SituacaoConselhoClasse.EmAndamento.Name(), resultado.First(r => r.Status == (int)SituacaoConselhoClasse.EmAndamento).Descricao);
            Assert.Equal(SituacaoConselhoClasse.Concluido.Name(), resultado.First(r => r.Status == (int)SituacaoConselhoClasse.Concluido).Descricao);
        }

        [Fact]
        public async Task Obter_Conselho_Classe_Consolidado_Quando_Retorna_Alunos_Com_Todos_Status_Deve_Mapear_Corretamente()
        {
            var filtro = new FiltroConselhoClasseConsolidadoTurmaBimestreDto(1, 1, 1);
            var listaAlunos = new List<ConselhoClasseAlunoDto>
            {
                new ConselhoClasseAlunoDto { SituacaoFechamentoCodigo = (int)SituacaoConselhoClasse.NaoIniciado },
                new ConselhoClasseAlunoDto { SituacaoFechamentoCodigo = (int)SituacaoConselhoClasse.EmAndamento },
                new ConselhoClasseAlunoDto { SituacaoFechamentoCodigo = (int)SituacaoConselhoClasse.Concluido }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaAlunos);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());
            Assert.True(resultado.All(r => r.Quantidade == 1));
        }

        [Fact]
        public async Task Obter_Conselho_Classe_Consolidado_Quando_Retorna_Status_Invalido_Deve_Mapear_Para_NaoIniciado()
        {
            var filtro = new FiltroConselhoClasseConsolidadoTurmaBimestreDto(1, 1, 1);
            var listaAlunos = new List<ConselhoClasseAlunoDto>
            {
                new ConselhoClasseAlunoDto { SituacaoFechamentoCodigo = 99 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaAlunos);

            var resultado = await _useCase.Executar(filtro);
            var statusInvalido = resultado.FirstOrDefault(r => r.Status == 99);

            Assert.NotNull(statusInvalido);
            Assert.Equal(SituacaoConselhoClasse.NaoIniciado.Name(), statusInvalido.Descricao);
        }
    }
}
