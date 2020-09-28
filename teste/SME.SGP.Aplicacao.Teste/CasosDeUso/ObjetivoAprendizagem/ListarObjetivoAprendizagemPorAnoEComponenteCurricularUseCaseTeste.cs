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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase useCase;

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Listar()
        {
            //Arrange
            var mockRetorno = new List<ObjetivoAprendizagem> {
                    new ObjetivoAprendizagem
                    {
                        AtualizadoEm = DateTime.Now,
                        CodigoCompleto = "(EF06C11)",
                        ComponenteCurricularId = 2,
                        AnoTurma = "first",
                        CriadoEm  = DateTime.Now,
                        Descricao = "",
                        Excluido = false,
                        Id = 1
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetorno);

            //Act
            var retorno = await useCase.Executar(1,2);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }
    }
}
