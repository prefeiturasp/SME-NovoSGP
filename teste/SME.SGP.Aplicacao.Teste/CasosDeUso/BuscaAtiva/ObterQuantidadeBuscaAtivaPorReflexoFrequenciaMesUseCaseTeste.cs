using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.BuscaAtiva
{
    public class ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Grafico_Com_Dados_Esperados_Quando_Ue_Id_Nao_Informado()
        {
            var filtro = new FiltroGraficoReflexoFrequenciaBuscaAtivaDto
            {
                Mes = 6,
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil,
                UeId = null,
                DreId = 10,
                Semestre = 1
            };

            var dadosRetorno = new List<DadosGraficoReflexoFrequenciaAnoTurmaBuscaAtivaDto>
            {
                new DadosGraficoReflexoFrequenciaAnoTurmaBuscaAtivaDto
                {
                    Quantidade = 12,
                    ReflexoFrequencia = "Baixa Frequência",
                    Ano = "3",
                    Modalidade = Modalidade.Fundamental,
                    Turma = "T1"
                },
                new DadosGraficoReflexoFrequenciaAnoTurmaBuscaAtivaDto
                {
                    Quantidade = 7,
                    ReflexoFrequencia = "Alta Frequência",
                    Ano = "2",
                    Modalidade = Modalidade.Medio,
                    Turma = "T2"
                }
            };

            var dataConsolidacao = new DateTime(2025, 7, 1);

            var repositorioMock = new Mock<IRepositorioDashBoardBuscaAtiva>();
            repositorioMock
                .Setup(r => r.ObterDadosGraficoReflexoFrequencia(filtro.Mes, filtro.AnoLetivo, filtro.Modalidade, filtro.UeId, filtro.DreId, filtro.Semestre))
                .ReturnsAsync(dadosRetorno);

            repositorioMock
                .Setup(r => r.ObterDataUltimaConsolidacaoReflexoFrequencia())
                .ReturnsAsync(dataConsolidacao);

            var mediatorMock = new Mock<IMediator>();
            var useCase = new ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase(repositorioMock.Object, mediatorMock.Object);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(dataConsolidacao, resultado.DataUltimaConsolidacao);
            Assert.Equal(2, resultado.Graficos.Count);

            Assert.Contains(resultado.Graficos, g =>
                g.Descricao == "Baixa Frequência" &&
                g.Quantidade == 12 &&
                g.Grupo == "3º ano");

            Assert.Contains(resultado.Graficos, g =>
                g.Descricao == "Alta Frequência" &&
                g.Quantidade == 7 &&
                g.Grupo == "2º ano");

            repositorioMock.Verify(r => r.ObterDadosGraficoReflexoFrequencia(filtro.Mes, filtro.AnoLetivo, filtro.Modalidade, filtro.UeId, filtro.DreId, filtro.Semestre), Times.Once);
            repositorioMock.Verify(r => r.ObterDataUltimaConsolidacaoReflexoFrequencia(), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Grupo_Com_Modalidade_E_Turma_Quando_Ue_Id_Informado()
        {
            var filtro = new FiltroGraficoReflexoFrequenciaBuscaAtivaDto
            {
                Mes = 6,
                AnoLetivo = 2025,
                Modalidade = Modalidade.Medio,
                UeId = 999, 
                DreId = 11,
                Semestre = 2
            };

            var dadosRetorno = new List<DadosGraficoReflexoFrequenciaAnoTurmaBuscaAtivaDto>
            {
                new DadosGraficoReflexoFrequenciaAnoTurmaBuscaAtivaDto
                {
                    Quantidade = 5,
                    ReflexoFrequencia = "Frequência Regular",
                    Modalidade = Modalidade.Medio,
                    Turma = "Médio-3B"
                }
            };

            var dataConsolidacao = new DateTime(2025, 6, 30);

            var repositorioMock = new Mock<IRepositorioDashBoardBuscaAtiva>();
            repositorioMock
                .Setup(r => r.ObterDadosGraficoReflexoFrequencia(filtro.Mes, filtro.AnoLetivo, filtro.Modalidade, filtro.UeId, filtro.DreId, filtro.Semestre))
                .ReturnsAsync(dadosRetorno);

            repositorioMock
                .Setup(r => r.ObterDataUltimaConsolidacaoReflexoFrequencia())
                .ReturnsAsync(dataConsolidacao);

            var mediatorMock = new Mock<IMediator>();
            var useCase = new ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase(repositorioMock.Object, mediatorMock.Object);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Graficos);
            var item = resultado.Graficos[0];

            Assert.Equal("Frequência Regular", item.Descricao);
            Assert.Equal(5, item.Quantidade);
            Assert.Equal("EM-Médio-3B", item.Grupo);

            Assert.Equal(dataConsolidacao, resultado.DataUltimaConsolidacao);
        }
    }
}
