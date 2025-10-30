using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.Abrangencia;
using SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Abrangencia
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase useCase;

        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Query_Correta_E_Retornar_Resultado()
        {
            var codigoUe = "123456";
            var anoLetivo = 2023;
            var modalidade = Modalidade.EducacaoInfantil;
            var semestre = 1;
            var anos = new List<string> { "1", "2" };

            var respostaEsperada = new List<OpcaoDropdownDto>
            {
                new OpcaoDropdownDto("T1", "Turma 1"),
                new OpcaoDropdownDto("T2", "Turma 2")
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery>(q =>
                    q.CodigoUe == codigoUe &&
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidade == modalidade &&
                    q.Semestre == semestre &&
                    q.Anos == anos),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaEsperada);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre, anos);

            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(respostaEsperada);
            resultado.Count().Should().Be(2);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Parametros_Nulos_Ou_Vazios_Deve_Enviar_Query_E_Retornar_Resultado_Esperado()
        {
            var codigoUe = "654321";
            var anoLetivo = 2024;
            Modalidade? modalidade = null;
            var semestre = 2;
            IList<string> anos = null;

            var respostaEsperada = new List<OpcaoDropdownDto>
            {
                new OpcaoDropdownDto("T3", "Turma 3")
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery>(q =>
                    q.CodigoUe == codigoUe &&
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidade == modalidade &&
                    q.Semestre == semestre &&
                    q.Anos == anos),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaEsperada);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre, anos);

            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(respostaEsperada);
            resultado.Count().Should().Be(1);

            mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery>(q => q.Modalidade == null && q.Anos == null), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
