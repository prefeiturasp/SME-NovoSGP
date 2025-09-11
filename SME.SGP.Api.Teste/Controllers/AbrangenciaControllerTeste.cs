using Bogus;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class AbrangenciaControllerTeste
    {
        private readonly Mock<IConsultasAbrangencia> _consultasAbrangenciaMock;
        private readonly Mock<IServicoAbrangencia> _servicoAbrangenciaMock;
        private readonly AbrangenciaController _controller;
        private readonly Faker _faker;

        public AbrangenciaControllerTeste()
        {
            _consultasAbrangenciaMock = new Mock<IConsultasAbrangencia>();
            _servicoAbrangenciaMock = new Mock<IServicoAbrangencia>();
            _faker = new Faker("pt_BR");

            _controller = new AbrangenciaController(
                _consultasAbrangenciaMock.Object,
                _servicoAbrangenciaMock.Object
            );
        }

        [Fact(DisplayName = "Deve lançar exceção quando as dependências do construtor forem nulas")]
        public void DeveLancarExcecao_QuandoInjetarDependenciasNulas()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AbrangenciaController(null, _servicoAbrangenciaMock.Object));
            Assert.Throws<ArgumentNullException>(() => new AbrangenciaController(_consultasAbrangenciaMock.Object, null));
        }

        [Fact(DisplayName = "Deve retornar OK ao obter abrangência por filtro com sucesso")]
        public async Task ObterAbrangenciaAutoComplete_QuandoEncontraResultados_DeveRetornarOk()
        {
            // Arrange
            var filtro = _faker.Lorem.Word();
            ConfigurarRota(true);

            var retornoConsulta = new List<AbrangenciaFiltroRetorno> { new AbrangenciaFiltroRetorno() };
            _consultasAbrangenciaMock.Setup(c => c.ObterAbrangenciaPorfiltro(filtro, true, false)).ReturnsAsync(retornoConsulta);

            // Act
            var resultado = await _controller.ObterAbrangenciaAutoComplete(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsAssignableFrom<IEnumerable<AbrangenciaFiltroRetorno>>(okResult.Value);
            Assert.True(valorRetornado.Any());
        }

        [Fact(DisplayName = "Deve retornar NoContent para filtro de autocomplete com menos de 2 caracteres")]
        public async Task ObterAbrangenciaAutoComplete_QuandoFiltroMenorQueDois_DeveRetornarNoContent()
        {
            // Arrange
            var filtro = "A";

            // Act
            var resultado = await _controller.ObterAbrangenciaAutoComplete(filtro);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(204, statusCodeResult.StatusCode);
            _consultasAbrangenciaMock.Verify(c => c.ObterAbrangenciaPorfiltro(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact(DisplayName = "Deve retornar OK ao obter os anos letivos de turmas")]
        public async Task ObterAnosLetivosTurmas_QuandoEncontraResultados_DeveRetornarOk()
        {
            // Arrange
            var codigoUe = _faker.Random.AlphaNumeric(6);
            var modalidade = Modalidade.Fundamental;
            var anoLetivo = DateTime.Now.Year;
            ConfigurarRota(false);

            var retornoConsulta = new List<OpcaoDropdownDto> { new OpcaoDropdownDto() };
            _consultasAbrangenciaMock.Setup(c => c.ObterAnosTurmasPorUeModalidade(codigoUe, modalidade, false, anoLetivo))
                .ReturnsAsync(retornoConsulta);

            // Act
            var resultado = await _controller.ObterAnosLetivos(codigoUe, (int)modalidade, anoLetivo);

            // Assert
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve retornar OK ao obter DREs")]
        public async Task ObterDres_QuandoEncontraResultados_DeveRetornarOkComListaOrdenada()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAbrangenciaDresUseCase>();
            ConfigurarRota(true);

            var retornoUseCase = new List<AbrangenciaDreRetornoDto>
            {
                new AbrangenciaDreRetornoDto { Nome = "DRE Z" },
                new AbrangenciaDreRetornoDto { Nome = "DRE A" }
            };

            useCaseMock.Setup(u => u.Executar(It.IsAny<Modalidade?>(), It.IsAny<int>(), true, It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(retornoUseCase);

            // Act
            var resultado = await _controller.ObterDres(useCaseMock.Object, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaRetornada = Assert.IsAssignableFrom<IOrderedEnumerable<AbrangenciaDreRetornoDto>>(okResult.Value);
            Assert.Equal("DRE A", listaRetornada.First().Nome);
        }

        [Fact(DisplayName = "Deve retornar OK ao obter turmas")]
        public async Task ObterTurmas_QuandoEncontraResultados_DeveRetornarOk()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var codigoUe = _faker.Random.AlphaNumeric(6);
            ConfigurarRota(true);

            var retornoMediator = new List<AbrangenciaTurmaRetorno> { new AbrangenciaTurmaRetorno() };

            mediatorMock.Setup(m => m.Send(It.Is<ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery>(q => q.CodigoUe == codigoUe && q.ConsideraHistorico), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _controller.ObterTurmas(mediatorMock.Object, codigoUe, Modalidade.Fundamental);

            // Assert
            Assert.IsType<OkObjectResult>(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar OK ao sincronizar abrangência histórica")]
        public async Task SincronizarAbrangenciaTurmasHistoricas_DeveRetornarOk()
        {
            // Arrange
            var professorRf = _faker.Random.AlphaNumeric(7);
            var anoLetivo = DateTime.Now.Year - 1;

            _servicoAbrangenciaMock.Setup(s => s.SincronizarAbrangenciaHistorica(anoLetivo, professorRf, 0))
                .ReturnsAsync(true);

            // Act
            var resultado = await _controller.SincronizarAbrangenciaTurmasHistoricas(professorRf, anoLetivo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        private void ConfigurarRota(bool consideraHistorico)
        {
            var rota = new RouteData();
            rota.Values.Add("consideraHistorico", consideraHistorico.ToString());

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = rota
            };
        }
    }
}