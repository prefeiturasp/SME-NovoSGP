using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Aplicacao.CasosDeUso.Abrangencia;
using SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade;
using MediatR;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Abrangencia
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Opcoes_Dropdown_Corretamente()
        {
            var mediatorMock = new Mock<IMediator>();

            var codigoUe = "000001";
            var anoLetivo = 2025;
            var modalidade = Modalidade.Fundamental;
            var semestre = 1;
            var anos = new List<string> { "1", "2", "3" };

            var retornoEsperado = new List<OpcaoDropdownDto>
            {
                new OpcaoDropdownDto { Valor = "101", Descricao = "Turma 101" },
                new OpcaoDropdownDto { Valor = "102", Descricao = "Turma 102" }
            };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery>(
                        q => q.CodigoUe == codigoUe
                             && q.AnoLetivo == anoLetivo
                             && q.Modalidade == modalidade
                             && q.Semestre == semestre
                             && q.Anos == anos),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var useCase = new ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre, anos);

            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => Assert.Equal("Turma 101", item.Descricao),
                item => Assert.Equal("Turma 102", item.Descricao));

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
