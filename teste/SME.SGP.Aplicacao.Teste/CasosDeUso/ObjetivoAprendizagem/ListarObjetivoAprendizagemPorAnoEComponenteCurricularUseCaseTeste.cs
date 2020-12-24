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
        public async Task Deve_Listar_Sem_Ensino_Especial()
        {
            //Arrange
            var mockObjetivos = new List<ObjetivoAprendizagemDto> {
                    new ObjetivoAprendizagemDto
                    {
                        Codigo = "(EF06C11)",
                        Ano = "fifth",
                        Descricao = "",
                        Id = 1475,
                        ComponenteCurricularEolId = 139,
                        IdComponenteCurricular = 1
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockObjetivos);

            var mockJurema = new long[] { 1 };

            mediator.Setup(a => a.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockJurema);

            //Act
            var retorno = await useCase.Executar("5", 139, false);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
                       

            Assert.True(retorno.Any());
        }

        [Fact]
        public async Task Deve_Listar_Com_Ensino_Especial()
        {
            //Arrange
            var mockObjetivos = new List<ObjetivoAprendizagemDto> {
                    new ObjetivoAprendizagemDto
                    {
                        Codigo = "(EF06C11)",
                        Ano = "fifth",
                        Descricao = "",
                        Id = 1475,
                        ComponenteCurricularEolId = 138,
                        IdComponenteCurricular = 1
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockObjetivos);

            var mockJurema = new long[] { 6 };

            mediator.Setup(a => a.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockJurema);

            //Act
            var retorno = await useCase.Executar("5", 138, true);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterJuremaIdsPorComponentesCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);


            Assert.True(retorno.Any());
        }
    }
}
