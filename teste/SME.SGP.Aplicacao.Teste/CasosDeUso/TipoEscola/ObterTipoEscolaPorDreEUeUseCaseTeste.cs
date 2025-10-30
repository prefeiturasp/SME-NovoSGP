using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.TipoEscola
{
    public class ObterTipoEscolaPorDreEUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTipoEscolaPorDreEUeUseCase _useCase;

        public ObterTipoEscolaPorDreEUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTipoEscolaPorDreEUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametros_Validos_Deve_Enviar_Query_E_Retornar_Tipos_Escola()
        {
            var dreCodigo = "108300";
            var ueCodigo = "019402";
            var modalidades = new int[] { 3, 4 };
            var tiposEscolaRetorno = new List<TipoEscolaDto>
            {
                new TipoEscolaDto { Id = 1, CodTipoEscola = 10, Descricao = "Escola de Teste 1" },
                new TipoEscolaDto { Id = 2, CodTipoEscola = 12, Descricao = "Escola de Teste 2" }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoEscolaPorDreEUeQuery>(q =>
                    q.DreCodigo == dreCodigo &&
                    q.UeCodigo == ueCodigo &&
                    q.Modalidades == modalidades),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(tiposEscolaRetorno);

            var resultado = await _useCase.Executar(dreCodigo, ueCodigo, modalidades);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTipoEscolaPorDreEUeQuery>(q =>
                    q.DreCodigo == dreCodigo &&
                    q.UeCodigo == ueCodigo &&
                    q.Modalidades == modalidades),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal(tiposEscolaRetorno.Count(), resultado.Count());
            Assert.Equal(tiposEscolaRetorno, resultado);
        }
    }
}
