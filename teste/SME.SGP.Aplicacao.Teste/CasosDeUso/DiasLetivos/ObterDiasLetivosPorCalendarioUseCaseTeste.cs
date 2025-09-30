using MediatR;
using Moq;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiasLetivos
{
    public class ObterDiasLetivosPorCalendarioUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDiasLetivosPorCalendarioUseCase _useCase;

        public ObterDiasLetivosPorCalendarioUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDiasLetivosPorCalendarioUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Repassar_Filtro_Para_Query_E_Retornar_Resultado()
        {
            var filtro = new FiltroDiasLetivosDTO
            {
                TipoCalendarioId = 1,
                DreId = "DRE-CL",
                UeId = "UE-12345"
            };

            var dadosRetorno = new DiasLetivosDto { Dias = 200, EstaAbaixoPermitido = false };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterQuantidadeDiasLetivosPorCalendarioQuery>(q =>
                                     q.TipoCalendarioId == filtro.TipoCalendarioId &&
                                     q.DreCodigo == filtro.DreId &&
                                     q.UeCodigo == filtro.UeId),
                                 It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosRetorno);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(dadosRetorno.Dias, resultado.Dias);
            Assert.Equal(dadosRetorno.EstaAbaixoPermitido, resultado.EstaAbaixoPermitido);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterQuantidadeDiasLetivosPorCalendarioQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
