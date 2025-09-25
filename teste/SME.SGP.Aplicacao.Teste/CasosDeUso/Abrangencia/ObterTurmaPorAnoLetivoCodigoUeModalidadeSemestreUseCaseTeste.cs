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
        public async Task Executar_Deve_Retornar_Turmas_Quando_Nao_Eh_Educacao_Infantil()
        {
            var codigoUe = "000001";
            var anoLetivo = 2025;
            var modalidade = Modalidade.Fundamental;
            var semestre = 1;

            var retornoEsperado = new List<OpcaoDropdownDto>
            {
                new OpcaoDropdownDto { Valor = "1", Descricao = "Turma 1" }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Turma 1", ((List<OpcaoDropdownDto>)resultado)[0].Descricao);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Considerar_Filtro_Quando_Educacao_Infantil_Sem_Considerar_Novos_Anos()
        {
            var codigoUe = "000002";
            var anoLetivo = 2025;
            var modalidade = Modalidade.EducacaoInfantil;
            var semestre = 2;
            var consideraNovosAnosInfantil = false;

            string[] anosInfantilDesconsiderar = new[] { "B1", "B2" };

            var retornoEsperado = new List<OpcaoDropdownDto>
            {
                new OpcaoDropdownDto { Valor = "9", Descricao = "Infantil A" }
            };

            mediatorMock
                 .Setup(m => m.Send(It.IsAny<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(anosInfantilDesconsiderar);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre, consideraNovosAnosInfantil);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Infantil A", ((List<OpcaoDropdownDto>)resultado)[0].Descricao);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Nao_Deve_Considerar_Filtro_Quando_Considera_Novos_Anos_Infantil()
        {
            var codigoUe = "000003";
            var anoLetivo = 2025;
            var modalidade = Modalidade.EducacaoInfantil;
            var semestre = 1;
            var consideraNovosAnosInfantil = true;

            var retornoEsperado = new List<OpcaoDropdownDto>
            {
                new OpcaoDropdownDto { Valor = "99", Descricao = "Infantil C" }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await useCase.Executar(codigoUe, anoLetivo, modalidade, semestre, consideraNovosAnosInfantil);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Infantil C", ((List<OpcaoDropdownDto>)resultado)[0].Descricao);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
