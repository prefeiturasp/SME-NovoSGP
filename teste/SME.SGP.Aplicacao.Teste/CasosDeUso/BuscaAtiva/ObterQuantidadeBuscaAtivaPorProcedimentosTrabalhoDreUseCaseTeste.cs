using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.BuscaAtiva
{
    public class ObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Grafico_Com_Grupo_Preenchido_Quando_Dre_Id_Nao_Informado()
        {
            var filtro = new FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto
            {
                TipoProcedimentoTrabalho = EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica,
                AnoLetivo = 2025,
                Modalidade = Modalidade.Fundamental,
                UeId = 10,
                DreId = null, 
                Semestre = 1
            };

            var dadosRetorno = new List<DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto>
            {
                new DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto
                {
                    Quantidade = 15,
                    RealizouProcedimentoTrabalho = "Sim",
                    Dre = "DRE Campo Limpo"
                },
                new DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto
                {
                    Quantidade = 8,
                    RealizouProcedimentoTrabalho = "Não",
                    Dre = "DRE Pirituba"
                }
            };

            var repositorioMock = new Mock<IRepositorioDashBoardBuscaAtiva>();
            repositorioMock
                .Setup(r => r.ObterDadosGraficoProcedimentoTrabalho(filtro.TipoProcedimentoTrabalho, filtro.AnoLetivo, filtro.Modalidade, filtro.UeId, filtro.DreId, filtro.Semestre))
                .ReturnsAsync(dadosRetorno);

            var mediatorMock = new Mock<IMediator>();
            var useCase = new ObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase(repositorioMock.Object, mediatorMock.Object);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Graficos.Count);

            Assert.Contains(resultado.Graficos, g => g.Descricao == "Sim" && g.Quantidade == 15 && g.Grupo == "DRE Campo Limpo");
            Assert.Contains(resultado.Graficos, g => g.Descricao == "Não" && g.Quantidade == 8 && g.Grupo == "DRE Pirituba");

            repositorioMock.Verify(r => r.ObterDadosGraficoProcedimentoTrabalho(filtro.TipoProcedimentoTrabalho, filtro.AnoLetivo, filtro.Modalidade, filtro.UeId, filtro.DreId, filtro.Semestre), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Grafico_Com_Grupo_Vazio_Quando_Dre_Id_Informado()
        {
            var filtro = new FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto
            {
                TipoProcedimentoTrabalho = EnumProcedimentoTrabalhoBuscaAtiva.LigacaoTelefonica,
                AnoLetivo = 2025,
                Modalidade = Modalidade.Fundamental,
                UeId = 10,
                DreId = 777, 
                Semestre = 2
            };

            var dadosRetorno = new List<DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto>
            {
                new DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto
                {
                    Quantidade = 4,
                    RealizouProcedimentoTrabalho = "Sim",
                    Dre = "DRE Ipiranga"
                }
            };

            var repositorioMock = new Mock<IRepositorioDashBoardBuscaAtiva>();
            repositorioMock
                .Setup(r => r.ObterDadosGraficoProcedimentoTrabalho(filtro.TipoProcedimentoTrabalho, filtro.AnoLetivo, filtro.Modalidade, filtro.UeId, filtro.DreId, filtro.Semestre))
                .ReturnsAsync(dadosRetorno);

            var mediatorMock = new Mock<IMediator>();
            var useCase = new ObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase(repositorioMock.Object, mediatorMock.Object);


            var resultado = await useCase.Executar(filtro);


            Assert.NotNull(resultado);
            Assert.Single(resultado.Graficos);
            var item = resultado.Graficos[0];

            Assert.Equal("Sim", item.Descricao);
            Assert.Equal(4, item.Quantidade);
            Assert.Equal(string.Empty, item.Grupo);

            repositorioMock.Verify(r => r.ObterDadosGraficoProcedimentoTrabalho(filtro.TipoProcedimentoTrabalho, filtro.AnoLetivo, filtro.Modalidade, filtro.UeId, filtro.DreId, filtro.Semestre), Times.Once);
        }
    }
}
