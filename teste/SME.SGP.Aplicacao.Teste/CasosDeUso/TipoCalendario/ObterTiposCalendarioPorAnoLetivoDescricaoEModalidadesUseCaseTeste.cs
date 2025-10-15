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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.TipoCalendario
{
    public class ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCase _useCase;

        public ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametros_Validos_Deve_Enviar_Query_E_Retornar_Tipos_Calendario()
        {
            var anoLetivo = 2025;
            var modalidades = new List<int> { (int)ModalidadeTipoCalendario.FundamentalMedio };
            var descricao = "Calendario Fundamental";
            var retornoEsperado = new List<TipoCalendarioRetornoDto>
            {
                new TipoCalendarioRetornoDto { Id = 1, AnoLetivo = anoLetivo, Descricao = descricao, Modalidade = ModalidadeTipoCalendario.FundamentalMedio, Nome = "Calendario 2025" }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery>(q =>
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidades.SequenceEqual(modalidades) &&
                    q.Descricao == descricao),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(anoLetivo, modalidades, descricao);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery>(q =>
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidades.SequenceEqual(modalidades) &&
                    q.Descricao == descricao),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado.Count(), resultado.Count());
            Assert.Equal(retornoEsperado, resultado);
        }
    }
}
