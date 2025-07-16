using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class ObterAulasPorTurmaComponenteDataUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveRetornarListaDeAulas()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var filtro = new FiltroObterAulasPorTurmaComponenteDataDto("T123", 456, new DateTime(2025, 7, 15));

            var retorno = new List<AulaConsultaDto>
        {
            new AulaConsultaDto { Id = 1, Quantidade = 2, TipoAula = TipoAula.Normal, AulaCJ = false },
            new AulaConsultaDto { Id = 2, Quantidade = 1, TipoAula = TipoAula.Reposicao, AulaCJ = true }
        };

            mediatorMock.Setup(m => m.Send(
                It.Is<ObterAulasPorDataTurmaComponenteCurricularQuery>(q =>
                    q.CodigoTurma == filtro.TurmaCodigo &&
                    q.CodigoComponenteCurricular == filtro.ComponenteCurricular &&
                    q.DataAula == filtro.DataAula),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retorno);

            var useCase = new ObterAulasPorTurmaComponenteDataUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            var lista = new List<AulaQuantidadeTipoDto>(resultado);
            Assert.Equal(2, lista.Count);

            Assert.Equal(1, lista[0].Id);
            Assert.Equal(2, lista[0].Quantidade);
            Assert.Equal((int)TipoAula.Normal, lista[0].Tipo);
            Assert.False(lista[0].EhCj);

            Assert.Equal(2, lista[1].Id);
            Assert.Equal(1, lista[1].Quantidade);
            Assert.Equal((int)TipoAula.Reposicao, lista[1].Tipo);
            Assert.True(lista[1].EhCj);
        }
    }
}
