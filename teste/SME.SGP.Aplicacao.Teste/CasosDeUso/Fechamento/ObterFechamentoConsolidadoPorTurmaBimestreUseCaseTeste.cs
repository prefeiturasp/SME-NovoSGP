using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class ObterFechamentoConsolidadoPorTurmaBimestreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterFechamentoConsolidadoPorTurmaBimestreUseCase _useCase;

        public ObterFechamentoConsolidadoPorTurmaBimestreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterFechamentoConsolidadoPorTurmaBimestreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Status_Total_Fechamento_Dto_Corretamente()
        {
            var filtro = new FiltroFechamentoConsolidadoTurmaBimestreDto(
                turmaId: 1234,
                bimestre: 1,
                situacaoFechamento: (int)SituacaoFechamento.NaoIniciado
            );

            var fechamentos = new List<FechamentoConsolidadoComponenteTurma>
            {
                new FechamentoConsolidadoComponenteTurma { Status = SituacaoFechamento.NaoIniciado },
                new FechamentoConsolidadoComponenteTurma { Status = SituacaoFechamento.EmProcessamento },
                new FechamentoConsolidadoComponenteTurma { Status = SituacaoFechamento.ProcessadoComSucesso }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFechamentoConsolidadoPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentos);

            var resultado = (await _useCase.Executar(filtro)).ToList();

            Assert.NotNull(resultado);
            Assert.Equal(4, resultado.Count); 
            Assert.Contains(resultado, r => r.Status == (int)SituacaoFechamento.NaoIniciado && r.Quantidade == 2); 
            Assert.Contains(resultado, r => r.Status == (int)SituacaoFechamento.ProcessadoComSucesso && r.Quantidade == 1);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Fechamentos()
        {
            var filtro = new FiltroFechamentoConsolidadoTurmaBimestreDto(
                turmaId: 1234,
                bimestre: 1,
                situacaoFechamento: (int)SituacaoFechamento.ProcessadoComSucesso
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFechamentoConsolidadoPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoConsolidadoComponenteTurma>());

            var resultado = await _useCase.Executar(filtro);

            Assert.Empty(resultado);
        }
    }
}
