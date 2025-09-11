using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.BuscaAtiva
{
    public class ObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCaseTeste
    {

        [Fact]
        public async Task Executar_Deve_Retornar_Graficos_Com_Dados_Esperados()
        {
            var anoLetivo = 2025;
            var modalidade = Modalidade.Fundamental;
            var ueId = 123;
            var dreId = 456;
            var semestre = 1;

            var filtro = new FiltroGraficoBuscaAtivaDto
            {
                AnoLetivo = anoLetivo,
                Modalidade = modalidade,
                UeId = ueId,
                DreId = dreId,
                Semestre = semestre
            };

            var dadosRetorno = new List<DadosGraficoMotivoAusenciaBuscaAtivaDto>
            {
                new DadosGraficoMotivoAusenciaBuscaAtivaDto { Quantidade = 10, MotivoAusencia = "Doença" },
                new DadosGraficoMotivoAusenciaBuscaAtivaDto { Quantidade = 5, MotivoAusencia = "Problemas Familiares" }
            };

            var repositorioMock = new Mock<IRepositorioDashBoardBuscaAtiva>();
            repositorioMock.Setup(r => r.ObterDadosGraficoMotivoAusencia(anoLetivo, modalidade, ueId, dreId, semestre))
                           .ReturnsAsync(dadosRetorno);

            var mediatorMock = new Mock<IMediator>();

            var useCase = new ObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase(repositorioMock.Object, mediatorMock.Object);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Graficos.Count);

            Assert.Contains(resultado.Graficos, g => g.Descricao == "Doença" && g.Quantidade == 10);
            Assert.Contains(resultado.Graficos, g => g.Descricao == "Problemas Familiares" && g.Quantidade == 5);

            repositorioMock.Verify(r => r.ObterDadosGraficoMotivoAusencia(anoLetivo, modalidade, ueId, dreId, semestre), Times.Once);
        }
    }
}
