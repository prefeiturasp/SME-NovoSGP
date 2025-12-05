using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardNaapa
{
    public class ObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase useCase;

        public ObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Retornar_Resultado_Teste()
        {
            var filtro = new FiltroQuantidadeAtendimentoNAAPAPorProfissionalMesDto
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 2,
                Mes = 3,
                Modalidade = Modalidade.Fundamental
            };

            var esperado = new GraficoAtendimentoNAAPADto
            {
                DataUltimaConsolidacao = DateTime.Today,
                TotaEncaminhamento = 10,
                Graficos = new List<GraficoBaseDto>
                {
                    new GraficoBaseDto("Profissional A", 5, "Descricao A"),
                    new GraficoBaseDto("Profissional B", 5, "Descricao B")
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(esperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(esperado.TotaEncaminhamento, resultado.TotaEncaminhamento);
            Assert.Equal(esperado.Graficos.Count, resultado.Graficos.Count);
            mediatorMock.Verify(x => x.Send(It.Is<ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery>(q =>
                q.AnoLetivo == filtro.AnoLetivo &&
                q.DreId == filtro.DreId &&
                q.UeId == filtro.UeId &&
                q.Mes == filtro.Mes &&
                q.Modalidade == filtro.Modalidade
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Vazio_Deve_Retornar_Objeto_Vazio_Teste()
        {
            var filtro = new FiltroQuantidadeAtendimentoNAAPAPorProfissionalMesDto
            {
                AnoLetivo = 2025,
                DreId = 1
            };

            var esperado = new GraficoAtendimentoNAAPADto
            {
                DataUltimaConsolidacao = null,
                TotaEncaminhamento = 0,
                Graficos = new List<GraficoBaseDto>()
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(esperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.Graficos);
            Assert.Equal(0, resultado.TotaEncaminhamento);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Executar_Quando_Mediator_Nulo_Deve_Lancar_Excecao_Teste()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase(null));
        }
    }
}
