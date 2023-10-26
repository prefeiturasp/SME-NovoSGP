using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardDiarioBordo
{
    public class ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCaseTeste
    {
        private readonly ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase useCase;
        private readonly Mock<IMediator> mediator;
        public ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Trazer_Dados_Usuario_Visao_DRESME()
        {
            // Arrange
            var filtro = new FiltroDasboardDiarioBordoDto()
            {
                AnoLetivo = 2022,
                Modalidade = Dominio.Modalidade.EducacaoInfantil,
                DreId = 12,
                UeId = 019493
            };

            var listaPerfis = new List<PrioridadePerfil>();
            listaPerfis.Add(new PrioridadePerfil() { Tipo = TipoPerfil.DRE });

            var usuario = new Usuario() {CodigoRf = "7909179"};

            usuario.DefinirPerfis(listaPerfis);

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(usuario);

            var dados = new List<GraficoTotalDiariosPreenchidosEPendentesDTO>();
            dados.Add(
                new GraficoTotalDiariosPreenchidosEPendentesDTO() 
                { 
                    TurmaAno = "1",
                    Descricao = "Preenchidos", 
                    Quantidade = 100
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(dados);

            // Act

            var dadosGrafico = await useCase.Executar(filtro);

            // Assert
            Assert.True(dados.Any());
        }

        [Fact]
        public async Task Deve_Trazer_Dados_Usuario_Visao_UE()
        {
            // Arrange
            var filtro = new FiltroDasboardDiarioBordoDto()
            {
                AnoLetivo = 2022,
                Modalidade = Dominio.Modalidade.EducacaoInfantil,
                DreId = 12,
                UeId = 019493
            };

            var listaPerfis = new List<PrioridadePerfil>();
            listaPerfis.Add(new PrioridadePerfil() { Tipo = TipoPerfil.DRE });

            var usuario = new Usuario() { CodigoRf = "8400458" };

            usuario.DefinirPerfis(listaPerfis);

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(usuario);

            var dados = new List<GraficoTotalDiariosPreenchidosEPendentesDTO>();
            dados.Add(
                new GraficoTotalDiariosPreenchidosEPendentesDTO()
                {
                    TurmaAno = "1B",
                    Descricao = "Preenchidos",
                    Quantidade = 100
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(dados);

            // Act

            var dadosGrafico = await useCase.Executar(filtro);

            // Assert
            Assert.True(dadosGrafico.Any());
        }

        [Fact]
        public async Task Nao_Deve_Trazer_Dados_Usuario_Visao_UE()
        {
            // Arrange
            var filtro = new FiltroDasboardDiarioBordoDto()
            {
                AnoLetivo = 2022,
                Modalidade = Dominio.Modalidade.EducacaoInfantil,
                DreId = 12,
                UeId = 019493
            };

            var listaPerfis = new List<PrioridadePerfil>();
            listaPerfis.Add(new PrioridadePerfil() { Tipo = TipoPerfil.UE });

            var usuario = new Usuario() { CodigoRf = "8400458" };

            usuario.DefinirPerfis(listaPerfis);

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(usuario);

            var dados = new List<GraficoTotalDiariosPreenchidosEPendentesDTO>();
            dados.Add(
                new GraficoTotalDiariosPreenchidosEPendentesDTO()
                {
                    TurmaAno = "1B",
                    Descricao = "Pendentes",
                    Quantidade = 100
                });


            mediator.Setup(a => a.Send(It.IsAny<ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(dados);

            // Act
            var dadosGrafico = await useCase.Executar(filtro);

            bool validaComTurma = !UtilRegex.RegexAnoTurma.IsMatch(dadosGrafico?.FirstOrDefault().TurmaAno);
            // Assert
            Assert.False(dadosGrafico?.Where(d => d.TurmaAno.Equals("1")).Any());
        }

        [Fact]
        public async Task Nao_Deve_Trazer_Dados_Usuario_Visao_SME_Com_Turma()
        {
            // Arrange
            var filtro = new FiltroDasboardDiarioBordoDto()
            {
                AnoLetivo = 2022,
                Modalidade = Dominio.Modalidade.EducacaoInfantil,
                DreId = 12,
                UeId = 019493,
                UsuarioRf = "8400458"
            };

            var listaPerfis = new List<PrioridadePerfil>();
            listaPerfis.Add(new PrioridadePerfil() { Tipo = TipoPerfil.UE });

            var usuario = new Usuario() { CodigoRf = "7909179" };

            usuario.DefinirPerfis(listaPerfis);

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(usuario);

            var dados = new List<GraficoTotalDiariosPreenchidosEPendentesDTO>();

            dados.Add(
                new GraficoTotalDiariosPreenchidosEPendentesDTO()
                {
                    TurmaAno = "1",
                    Descricao = "Pendentes",
                    Quantidade = 100
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(dados);

            // Act
            var dadosGrafico = await useCase.Executar(filtro);

            // Assert
            Assert.False(dadosGrafico?.Where(d=> d.TurmaAno.Equals("1B")).Any());
        }
    }
}
