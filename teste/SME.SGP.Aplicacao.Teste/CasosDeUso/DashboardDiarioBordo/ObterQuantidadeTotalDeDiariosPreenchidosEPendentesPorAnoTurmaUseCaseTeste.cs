using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
                UeId = 019493,
                UsuarioRf = "7909179"
            };

            var listaPerfis = new List<PrioridadePerfil>();
            listaPerfis.Add(new PrioridadePerfil() { Tipo = TipoPerfil.DRE });

            var usuario = new Usuario() {CodigoRf = "7909179"};

            usuario.DefinirPerfis(listaPerfis);

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPorCodigoRfLoginQuery>(), It.IsAny<CancellationToken>()))
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
            bool verificaSePossuiSoNumeros = Regex.IsMatch(dadosGrafico?.FirstOrDefault().TurmaAno, @"^[0-9]+$");

            // Assert
            Assert.True(verificaSePossuiSoNumeros);
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
                UeId = 019493,
                UsuarioRf = "7909179"
            };

            var listaPerfis = new List<PrioridadePerfil>();
            listaPerfis.Add(new PrioridadePerfil() { Tipo = TipoPerfil.DRE });

            var usuario = new Usuario() { CodigoRf = "7909179" };

            usuario.DefinirPerfis(listaPerfis);

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPorCodigoRfLoginQuery>(), It.IsAny<CancellationToken>()))
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
            bool verificaSePossuiSoNumeros = !Regex.IsMatch(dadosGrafico?.FirstOrDefault().TurmaAno, @"^[0-9]+$");
            bool verificaSeETurma = dadosGrafico?.FirstOrDefault().TurmaAno.Length > 1;

            // Assert
            Assert.True(verificaSePossuiSoNumeros & verificaSeETurma);
        }

        [Fact]
        public async Task Nao_Deve_Trazer_Dados_Usuario_Visao_UE_Somente_Ano()
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

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPorCodigoRfLoginQuery>(), It.IsAny<CancellationToken>()))
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
            bool validaComAno = !Regex.IsMatch(dadosGrafico?.FirstOrDefault().TurmaAno, @"^[0-9]+$");
            // Assert
            Assert.False(validaComAno);
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

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPorCodigoRfLoginQuery>(), It.IsAny<CancellationToken>()))
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
            bool validaComTurma = !Regex.IsMatch(dadosGrafico?.FirstOrDefault().TurmaAno, @"^[0-9]+$");
            // Assert
            Assert.False(validaComTurma);
        }
    }
}
