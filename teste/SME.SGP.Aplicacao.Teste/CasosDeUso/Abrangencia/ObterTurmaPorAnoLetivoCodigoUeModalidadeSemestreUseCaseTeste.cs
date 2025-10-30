using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.Abrangencia;
using SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Abrangencia
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase useCase;

        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Modalidade_Nao_Infantil_Deve_Buscar_Turmas_Sem_Desconsiderar_Anos()
        {
            var codigoUe = "123";
            var anoLetivo = 2023;
            var modalidade = Modalidade.EJA;
            var semestre = 1;
            var consideraNovosAnos = false;

            var turmasEsperadas = new List<OpcaoDropdownDto> { new OpcaoDropdownDto("T1", "Turma 1") };

            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(q =>
                    q.CodigoUe == codigoUe &&
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidade == modalidade &&
                    q.Semestre == semestre &&
                    q.AnosInfantilDesconsiderar == null),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasEsperadas);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre, consideraNovosAnos);

            resultado.Should().BeEquivalentTo(turmasEsperadas);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(q => q.AnosInfantilDesconsiderar == null), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Modalidade_Infantil_E_Considera_Novos_Anos_Deve_Buscar_Turmas_Sem_Desconsiderar_Anos()
        {
            var codigoUe = "123";
            var anoLetivo = 2023;
            var modalidade = Modalidade.EducacaoInfantil;
            var semestre = 1;
            var consideraNovosAnos = true;

            var turmasEsperadas = new List<OpcaoDropdownDto> { new OpcaoDropdownDto("T1", "Turma 1") };

            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(q =>
                    q.CodigoUe == codigoUe &&
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidade == modalidade &&
                    q.Semestre == semestre &&
                    q.AnosInfantilDesconsiderar == null),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasEsperadas);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre, consideraNovosAnos);

            resultado.Should().BeEquivalentTo(turmasEsperadas);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(q => q.AnosInfantilDesconsiderar == null), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Modalidade_Infantil_E_Nao_Considera_Novos_Anos_Deve_Buscar_Turmas_Desconsiderando_Anos()
        {
            var codigoUe = "123";
            var anoLetivo = 2023;
            var modalidade = Modalidade.EducacaoInfantil;
            var semestre = 1;
            var consideraNovosAnos = false;

            var anosDesconsiderar = new string[] { "I1", "I2" };
            var turmasEsperadas = new List<OpcaoDropdownDto> { new OpcaoDropdownDto("T1", "Turma 1") };

            mediatorMock.Setup(m => m.Send(It.Is<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(q =>
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidade == modalidade),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosDesconsiderar);

            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(q =>
                    q.CodigoUe == codigoUe &&
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidade == modalidade &&
                    q.Semestre == semestre &&
                    q.AnosInfantilDesconsiderar == anosDesconsiderar),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasEsperadas);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre, consideraNovosAnos);

            resultado.Should().BeEquivalentTo(turmasEsperadas);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(q => q.AnosInfantilDesconsiderar == anosDesconsiderar), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
