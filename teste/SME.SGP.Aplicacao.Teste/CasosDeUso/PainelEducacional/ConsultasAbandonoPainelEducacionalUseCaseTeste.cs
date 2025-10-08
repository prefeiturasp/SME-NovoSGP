using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasAbandonoPainelEducacionalUseCaseTeste
    {
        [Fact]
        public async Task ObterAbandonoVisaoSmeDre_DeveChamarMediatorERetornarResultado()
        {
            var mediatorMock = new Mock<IMediator>();
            var resultadoEsperado = new List<PainelEducacionalAbandono> { new PainelEducacionalAbandono { Id = 1 } };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAbandonoVisaoSmeDreQuery>(), default))
                .ReturnsAsync(resultadoEsperado);
            var useCase = new ConsultasAbandonoPainelEducacionalUseCase(mediatorMock.Object);

            var resultado = await useCase.ObterAbandonoVisaoSmeDre(2024, "dre", "ue");

            Assert.Equal(resultadoEsperado, resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAbandonoVisaoSmeDreQuery>(), default), Times.Once);
        }
    }
}
