using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SME.SGP.Infra;
using SME.SGP.Aplicacao.Interfaces;
using MediatR;
using SME.SGP.Infra.Dtos;
using System;

namespace SME.SGP.WebApi.Tests.Controllers
{
    public class AbrangenciaControllerTests
    {
        private readonly Mock<IConsultasAbrangencia> _mockConsultasAbrangencia;
        private readonly Mock<IServicoAbrangencia> _mockServicoAbrangencia;
        private readonly AbrangenciaController _controller;

        public AbrangenciaControllerTests()
        {
            _mockConsultasAbrangencia = new Mock<IConsultasAbrangencia>();
            _mockServicoAbrangencia = new Mock<IServicoAbrangencia>();

            _controller = new AbrangenciaController(_mockConsultasAbrangencia.Object, _mockServicoAbrangencia.Object);

            // Simular RouteData com consideraHistorico = true
            var routeData = new RouteData();
            routeData.Values["consideraHistorico"] = "true";

            var controllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = routeData
            };

            _controller.ControllerContext = controllerContext;
        }

        [Fact]
        public async Task ObterAbrangenciaAutoComplete_DeveRetornarOk_ComDadosCompletos()
        {
            // Arrange
            var filtro = "turma";
            var consideraAnosTurmasInfantil = false;

            var abrangencias = new List<AbrangenciaFiltroRetorno>
            {
                new AbrangenciaFiltroRetorno
                {
                    Ano = "3º",
                    AnoLetivo = 2025,
                    CodigoDre = "dre01",
                    CodigoTurma = "turma01",
                    CodigoUe = "ue01",
                    Modalidade = Modalidade.Fundamental,
                    NomeDre = "DRE Norte",
                    NomeTurma = "Turma A",
                    NomeUe = "EMEF Exemplo",
                    QtDuracaoAula = 50,
                    Semestre = 1,
                    TipoEscola = TipoEscola.EMEF,
                    TipoTurma = TipoTurma.Regular,
                    TurmaId = 1234,
                    TipoTurno = 1,
                    EnsinoEspecial = false
                }
            };

            _mockConsultasAbrangencia.Setup(x =>
                x.ObterAbrangenciaPorfiltro(filtro, true, consideraAnosTurmasInfantil))
                .ReturnsAsync(abrangencias);

            // Act
            var resultado = await _controller.ObterAbrangenciaAutoComplete(filtro, consideraAnosTurmasInfantil);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsAssignableFrom<IEnumerable<AbrangenciaFiltroRetorno>>(okResult.Value);
            var abrangencia = valor.First();

            Assert.Equal("3º", abrangencia.Ano);
            Assert.Equal("EMEF Exemplo", abrangencia.NomeUe);
            Assert.Equal("Ensino Fundamental", abrangencia.NomeModalidade);
            Assert.Equal("EMEF", abrangencia.NomeTipoEscola);
            Assert.Equal(1234, abrangencia.TurmaId);
        }


        [Fact]
        public async Task ObterAbrangenciaAutoComplete_DeveRetornar204_QuandoRetornoVazio()
        {
            // Arrange
            var filtro = "ab";
            _mockConsultasAbrangencia.Setup(x => x.ObterAbrangenciaPorfiltro(filtro, true, false))
                .ReturnsAsync(new List<AbrangenciaFiltroRetorno>());

            // Act
            var resultado = await _controller.ObterAbrangenciaAutoComplete(filtro);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        public async Task ObterAbrangenciaAutoComplete_DeveRetornar204_QuandoFiltroForMenorQueDoisCaracteres(string filtro)
        {
            // Act
            var resultado = await _controller.ObterAbrangenciaAutoComplete(filtro);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ObterAnosLetivosPorUe_DeveRetornarOk_QuandoHouverDados()
        {
            // Arrange
            var resultadoEsperado = new List<OpcaoDropdownDto>
            {
                new OpcaoDropdownDto("2025", "Ano Letivo 2025")
            };

            _mockConsultasAbrangencia.Setup(x =>
                x.ObterAnosTurmasPorUeModalidade("UE01", Modalidade.Fundamental, true, 2025))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.ObterAnosLetivos("UE01", (int)Modalidade.Fundamental, 2025);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<OpcaoDropdownDto>>(ok.Value);
            Assert.Single(retorno);

            var item = retorno.First();
            Assert.Equal("2025", item.Valor);
            Assert.Equal("Ano Letivo 2025", item.Descricao);
        }

        [Fact]
        public async Task ObterAnosLetivosPorUe_DeveRetornarNoContent_QuandoNaoHouverDados()
        {
            // Arrange
            _mockConsultasAbrangencia.Setup(x =>
                x.ObterAnosTurmasPorUeModalidade("UE01", Modalidade.Fundamental, true, 2025))
                .ReturnsAsync(new List<OpcaoDropdownDto>());

            // Act
            var resultado = await _controller.ObterAnosLetivos("UE01", (int)Modalidade.Fundamental, 2025);

            // Assert
            var result = resultado as StatusCodeResult;
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);

        }



        [Fact]
        public async Task ObterAnosLetivos_DeveRetornarOk_QuandoHouverDados()
        {
            // Arrange
            var anos = new[] { 2023, 2024 };

            _mockConsultasAbrangencia.Setup(x =>
                x.ObterAnosLetivos(true, 2020)).ReturnsAsync(anos);

            // Act
            var resultado = await _controller.ObterAnosLetivos(2020);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<int[]>(ok.Value);
            Assert.Equal(2, retorno.Length);
        }

        [Fact]
        public async Task ObterAnosLetivosTodos_DeveRetornarOk_QuandoHouverDados()
        {
            // Arrange
            var anos = new[] { 2021, 2022, 2023 };
            _mockConsultasAbrangencia.Setup(x => x.ObterAnosLetivosTodos())
                .ReturnsAsync(anos);

            // Act
            var resultado = await _controller.ObterAnosLetivosTodos();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<int[]>(ok.Value);
            Assert.Contains(2022, retorno);
        }

        [Fact]
        public async Task ObterDres_DeveRetornarOk_QuandoHouverDres()
        {
            // Arrange
            var mockUseCase = new Mock<IObterAbrangenciaDresUseCase>();

            var dres = new List<AbrangenciaDreRetornoDto>
            {
                new AbrangenciaDreRetornoDto
                {
                    Id = 1,
                    Codigo = "123",
                    Nome = "DRE Leste",
                    Abreviacao = "LST"
                }
            };

            mockUseCase.Setup(x =>
                x.Executar(It.IsAny<Modalidade?>(), 0, true, 2024, "DRE"))
                .ReturnsAsync(dres);

            _controller.ControllerContext.RouteData.Values["consideraHistorico"] = "true";

            // Act
            var resultado = await _controller.ObterDres(mockUseCase.Object, null, 0, 2024, "DRE");

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<AbrangenciaDreRetornoDto>>(ok.Value);
            Assert.Single(retorno);
        }


        [Fact]
        public async Task ObterModalidades_DeveRetornarOk_QuandoExistiremModalidades()
        {
            // Arrange
            var mockUseCase = new Mock<IObterModalidadesPorAnoUseCase>();

            var modalidades = new List<EnumeradoRetornoDto>
            {
                new EnumeradoRetornoDto { Id = 1, Descricao = "Educação Infantil" },
                new EnumeradoRetornoDto { Id = 2, Descricao = "Ensino Fundamental" }
            };

            mockUseCase.Setup(x => x.Executar(2025, true, false))
                .ReturnsAsync(modalidades);

            _controller.ControllerContext.RouteData.Values["consideraHistorico"] = "true";

            // Act
            var resultado = await _controller.ObterModalidades(mockUseCase.Object, 2025);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<EnumeradoRetornoDto>>(ok.Value);
            Assert.Equal(2, retorno.Count());

            Assert.Contains(retorno, m => m.Descricao == "Educação Infantil");
            Assert.Contains(retorno, m => m.Id == 2);
        }

        [Fact]
        public async Task ObterModalidades_DeveRetornarNoContent_QuandoNaoExistiremModalidades()
        {
            // Arrange
            var mockUseCase = new Mock<IObterModalidadesPorAnoUseCase>();

            mockUseCase.Setup(x => x.Executar(2025, true, false))
                .ReturnsAsync(new List<EnumeradoRetornoDto>());

            _controller.ControllerContext.RouteData.Values["consideraHistorico"] = "true";

            // Act
            var resultado = await _controller.ObterModalidades(mockUseCase.Object, 2025);

            // Assert
            var statusCode = (resultado as StatusCodeResult)?.StatusCode;
            Assert.Equal(204, statusCode);
        }

        [Fact]
        public async Task ObterSemestres_DeveRetornarOk_QuandoHouverSemestres()
        {
            // Arrange
            var filtro = new FiltroSemestreDto
            {
                Modalidade = Modalidade.Fundamental,
                AnoLetivo = 2024,
                DreCodigo = "123",
                UeCodigo = "456"
            };

            var semestres = new[] { 1, 2 };

            _mockConsultasAbrangencia.Setup(x =>
                x.ObterSemestres(filtro.Modalidade, true, filtro.AnoLetivo, filtro.DreCodigo, filtro.UeCodigo))
                .ReturnsAsync(semestres);

            // Act
            var resultado = await _controller.ObterSemestres(filtro);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<int[]>(ok.Value);
            Assert.Equal(2, retorno.Length);
        }

        [Fact]
        public async Task ObterTurmas_DeveRetornarOk_QuandoHouverTurmas()
        {
            // Arrange
            var mediator = new Mock<IMediator>();
            var turmas = new List<AbrangenciaTurmaRetorno>
            {
                new AbrangenciaTurmaRetorno { Codigo = "123", Nome = "Turma A" }
            };

            mediator.Setup(x => x.Send(It.IsAny<IRequest<IEnumerable<AbrangenciaTurmaRetorno>>>(), default))
                .ReturnsAsync(turmas);

            // Act
            var resultado = await _controller.ObterTurmas(mediator.Object, "UE01", Modalidade.Fundamental, 0, 2024, null, false);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<AbrangenciaTurmaRetorno>>(ok.Value);
            Assert.Single(retorno);
        }

        [Fact]
        public async Task ObterTurmasMesmoComponenteCurricular_DeveRetornarOk_QuandoHouverTurmas()
        {
            // Arrange
            var mediator = new Mock<IMediator>();
            var turmasIniciais = new List<AbrangenciaTurmaRetorno>
            {
                new AbrangenciaTurmaRetorno { Codigo = "001", Nome = "Turma X" }
            };
            var turmasFinal = new List<AbrangenciaTurmaRetorno>
            {
                new AbrangenciaTurmaRetorno { Codigo = "001", Nome = "Turma X - C/Disciplina" }
            };

            _mockConsultasAbrangencia.Setup(x =>
                x.ObterTurmasRegulares("UE01", Modalidade.Fundamental, 0, true, 2024))
                .ReturnsAsync(turmasIniciais);

            _mockConsultasAbrangencia.Setup(x =>
                x.ObterTurmasAbrangenciaMesmoComponente(turmasIniciais, "MAT"))
                .ReturnsAsync(turmasFinal);

            // Act
            var resultado = await _controller.ObterTurmasMesmoComponenteCurricular(
                mediator.Object, "UE01", "MAT", true, Modalidade.Fundamental, 0, 2024, null, false);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<AbrangenciaTurmaRetorno>>(ok.Value);
            Assert.Single(retorno);
        }

        [Fact]
        public async Task ObterTurmasRegulares_DeveRetornarOk_QuandoHouverTurmas()
        {
            // Arrange
            var turmas = new List<AbrangenciaTurmaRetorno>
            {
                new AbrangenciaTurmaRetorno { Codigo = "A", Nome = "Turma A" }
            };

            _mockConsultasAbrangencia.Setup(x =>
                x.ObterTurmasRegulares("UE01", Modalidade.Fundamental, 0, true, 2024))
                .ReturnsAsync(turmas);

            // Act
            var resultado = await _controller.ObterTurmasRegulares("UE01", Modalidade.Fundamental, 0, 2024);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<AbrangenciaTurmaRetorno>>(ok.Value);
            Assert.Single(retorno);
        }

        [Fact]
        public async Task ObterUes_DeveRetornarOk_QuandoHouverUes()
        {
            // Arrange
            var mockUseCase = new Mock<IObterUEsPorDreUseCase>();

            var ues = new List<AbrangenciaUeRetorno>
            {
                new AbrangenciaUeRetorno
                {
                    Codigo = "UE123",
                    NomeSimples = "EMEF Brasil",
                    TipoEscola = TipoEscola.EMEF,
                    Id = 1
                },
                new AbrangenciaUeRetorno
                {
                    Codigo = "UE456",
                    NomeSimples = "CEI São Paulo",
                    TipoEscola = TipoEscola.CEIINDIR,
                    Id = 2
                }
            };

            mockUseCase
                .Setup(x => x.Executar(It.IsAny<UEsPorDreDto>()))
                .ReturnsAsync(ues);

            _controller.ControllerContext.RouteData.Values["consideraHistorico"] = "true";

            // Act
            var resultado = await _controller.ObterUes(
                mockUseCase.Object,
                codigoDre: "DRE123",
                modalidade: Modalidade.Fundamental,
                periodo: 0,
                anoLetivo: 2024,
                consideraNovasUEs: false,
                filtrarTipoEscolaPorAnoLetivo: false,
                filtro: "EME"
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<AbrangenciaUeRetorno>>(ok.Value);
            Assert.Equal(2, retorno.Count());

            var lista = retorno.ToList();
            Assert.Equal("EMEF Brasil", lista[0].NomeSimples);
            Assert.True(lista[1].EhInfantil);
        }


        [Fact]
        public async Task UsuarioAdm_DeveRetornarTrue_QuandoUsuarioForAdm()
        {
            // Arrange
            var mockUseCase = new Mock<IUsuarioPossuiAbrangenciaAdmUseCase>();
            mockUseCase.Setup(x => x.Executar()).ReturnsAsync(true);

            // Act
            var resultado = await _controller.UsuarioAdm(mockUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            Assert.True((bool)ok.Value);
        }

        [Fact]
        public async Task ObterTurmasNaoHistoricas_DeveRetornarOk_QuandoHouverDados()
        {
            // Arrange
            var mockUseCase = new Mock<IObterTurmasNaoHistoricasUseCase>();
            var turmas = new List<TurmaNaoHistoricaDto>
            {
                new TurmaNaoHistoricaDto { Codigo = "001", Nome = "Turma Atual" }
            };

            mockUseCase.Setup(x => x.Executar()).ReturnsAsync(turmas);

            // Act
            var resultado = await _controller.ObterTurmasNaoHistoricas(mockUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<TurmaNaoHistoricaDto>>(ok.Value);
            Assert.Single(retorno);
        }

        [Fact]
        public async Task CarregarAbrangencia_DeveRetornarOk_QuandoExecutado()
        {
            // Arrange
            var mockUseCase = new Mock<ICarregarAbrangenciaUsusarioUseCase>();
            mockUseCase.Setup(x => x.Executar(It.IsAny<UsuarioPerfilDto>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _controller.CarregarAbrangencia("usuario.login", Guid.NewGuid(), mockUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            Assert.True((bool)ok.Value);
        }
    }
}
