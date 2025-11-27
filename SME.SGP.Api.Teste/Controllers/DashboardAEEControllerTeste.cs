using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashboardAEEControllerTests
    {
        private readonly Mock<IObterEncaminhamentoAEESituacoesUseCase> _situacoesEncaminhamentosUseCase;
        private readonly Mock<IObterEncaminhamentosAEEDeferidosUseCase> _encaminhamentosDeferidosUseCase;
        private readonly Mock<IObterPlanoAEESituacoesUseCase> _planosSituacoesUseCase;
        private readonly Mock<IObterPlanosAEEVigentesUseCase> _planosVigentesUseCase;
        private readonly Mock<IObterPlanosAEEAcessibilidadesUseCase> _planosAcessibilidadesUseCase;
        private readonly Mock<IObterAlunosMatriculadosSRMPAEEUseCase> _matriculadosSrmPaeeUseCase;

        private readonly DashboardAEEController _controller;

        public DashboardAEEControllerTests()
        {
            _situacoesEncaminhamentosUseCase = new Mock<IObterEncaminhamentoAEESituacoesUseCase>();
            _encaminhamentosDeferidosUseCase = new Mock<IObterEncaminhamentosAEEDeferidosUseCase>();
            _planosSituacoesUseCase = new Mock<IObterPlanoAEESituacoesUseCase>();
            _planosVigentesUseCase = new Mock<IObterPlanosAEEVigentesUseCase>();
            _planosAcessibilidadesUseCase = new Mock<IObterPlanosAEEAcessibilidadesUseCase>();
            _matriculadosSrmPaeeUseCase = new Mock<IObterAlunosMatriculadosSRMPAEEUseCase>();

            _controller = new DashboardAEEController();
        }

        [Fact(DisplayName = "ObterSituacoesEncaminhamentos deve retornar Ok com dto")]
        public async Task ObterSituacoesEncaminhamentos_DeveRetornarOk()
        {
            // Arrange
            var retorno = new DashboardAEEEncaminhamentosDto();
            _situacoesEncaminhamentosUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroDashboardAEEDto>()))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterSituacoesEncaminhamentos(2024, 1, 2, _situacoesEncaminhamentosUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            Assert.IsType<DashboardAEEEncaminhamentosDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterEncaminhamentosDeferidos deve retornar Ok com lista")]
        public async Task ObterEncaminhamentosDeferidos_DeveRetornarOk()
        {
            // Arrange
            var retorno = new List<AEETurmaDto>
            {
                new AEETurmaDto()
            };

            _encaminhamentosDeferidosUseCase.Setup(x => x.Executar(It.IsAny<FiltroDashboardAEEDto>())).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterEncaminhamentosDeferidos(2024, 1, 2, _encaminhamentosDeferidosUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsType<List<AEETurmaDto>>(ok.Value);
            Assert.Single(lista);
        }

        [Fact(DisplayName = "ObterSituacoesPlanos deve retornar Ok com dto")]
        public async Task ObterSituacoesPlanos_DeveRetornarOk()
        {
            // Arrange
            var retorno = new DashboardAEEPlanosSituacaoDto();
            _planosSituacoesUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroDashboardAEEDto>()))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterSituacoesPlanos(2024, 1, 2, _planosSituacoesUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            Assert.IsType<DashboardAEEPlanosSituacaoDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterPlanosVigentes deve retornar Ok com dto")]
        public async Task ObterPlanosVigentes_DeveRetornarOk()
        {
            // Arrange
            var retorno = new DashboardAEEPlanosVigentesDto();
            _planosVigentesUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroDashboardAEEDto>()))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPlanosVigentes(2024, 1, 2, _planosVigentesUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            Assert.IsType<DashboardAEEPlanosVigentesDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterPlanosAcessibilidades deve retornar Ok com lista")]
        public async Task ObterPlanosAcessibilidades_DeveRetornarOk()
        {
            // Arrange
            var retorno = new List<AEEAcessibilidadeRetornoDto>
            {
                new AEEAcessibilidadeRetornoDto()
            };

            _planosAcessibilidadesUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroDashboardAEEDto>()))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPlanosAcessibilidades(2024, 1, 2, _planosAcessibilidadesUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsType<List<AEEAcessibilidadeRetornoDto>>(ok.Value);
            Assert.Single(lista);
        }

        [Fact(DisplayName = "ObterAlunosMatriculadosSRMPAEE deve retornar Ok com lista")]
        public async Task ObterAlunosMatriculadosSRMPAEE_DeveRetornarOk()
        {
            // Arrange
            var retorno = new List<AEEAlunosMatriculadosDto>
            {
                new AEEAlunosMatriculadosDto()
            };

            _matriculadosSrmPaeeUseCase.Setup(x => x.Executar(It.IsAny<FiltroDashboardAEEDto>())).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterAlunosMatriculadosSRMPAEE(2024, "01", "0001", _matriculadosSrmPaeeUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsType<List<AEEAlunosMatriculadosDto>>(ok.Value);
            Assert.Single(lista);
        }
    }
}
