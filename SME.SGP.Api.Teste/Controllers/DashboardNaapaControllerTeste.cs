using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashboardNaapaControllerTeste
    {
        private readonly Mock<IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase> _abaixo50UseCase;
        private readonly Mock<IObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase> _semPresencaUseCase;
        private readonly Mock<IObterQuantidadeEncaminhamentoPorSituacaoUseCase> _encaminhamentoSituacaoUseCase;
        private readonly Mock<IObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase> _encaminhamentoAbertoUseCase;
        private readonly Mock<IObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase> _profissionalMesUseCase;
        private readonly Mock<IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase> _alunosAbaixo50UseCase;
        private readonly Mock<IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase> _alunosSemPresencaUseCase;

        private readonly DashboardNaapaController _controller;

        public DashboardNaapaControllerTeste()
        {
            _abaixo50UseCase = new Mock<IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();
            _semPresencaUseCase = new Mock<IObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();
            _encaminhamentoSituacaoUseCase = new Mock<IObterQuantidadeEncaminhamentoPorSituacaoUseCase>();
            _encaminhamentoAbertoUseCase = new Mock<IObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase>();
            _profissionalMesUseCase = new Mock<IObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase>();
            _alunosAbaixo50UseCase = new Mock<IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();
            _alunosSemPresencaUseCase = new Mock<IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();

            _controller = new DashboardNaapaController();
        }

        [Fact(DisplayName = "ObterFrequenciaTurmaEvasaoAbaixo50Porcento deve retornar Ok com DTO")]
        public async Task ObterFrequenciaTurmaEvasaoAbaixo50Porcento_DeveRetornarOk()
        {
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto();
            var retorno = new FrequenciaTurmaEvasaoDto();

            _abaixo50UseCase.Setup(s => s.Executar(filtro))
                            .ReturnsAsync(retorno);

            var result = await _controller.ObterFrequenciaTurmaEvasaoAbaixo50Porcento(filtro, _abaixo50UseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<FrequenciaTurmaEvasaoDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterFrequenciaTurmaEvasaoSemPresenca deve retornar Ok com DTO")]
        public async Task ObterFrequenciaTurmaEvasaoSemPresenca_DeveRetornarOk()
        {
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto();
            var retorno = new FrequenciaTurmaEvasaoDto();

            _semPresencaUseCase.Setup(s => s.Executar(filtro))
                               .ReturnsAsync(retorno);

            var result = await _controller.ObterFrequenciaTurmaEvasaoSemPresenca(filtro, _semPresencaUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<FrequenciaTurmaEvasaoDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuantidadeEncaminhamentoPorSituacao deve retornar Ok com DTO")]
        public async Task ObterQuantidadeEncaminhamentoPorSituacao_DeveRetornarOk()
        {
            var filtro = new FiltroGraficoEncaminhamentoPorSituacaoDto();
            var retorno = new GraficoEncaminhamentoNAAPADto();

            _encaminhamentoSituacaoUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroGraficoEncaminhamentoPorSituacaoDto>()))
                .ReturnsAsync(retorno);

            var result = await _controller
                .ObterQuantidadeEncaminhamentoPorSituacao(filtro, _encaminhamentoSituacaoUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoEncaminhamentoNAAPADto>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuantidadeEncaminhamentoNAAPAEmAberto deve retornar Ok com DTO")]
        public async Task ObterQuantidadeEncaminhamentoNAAPAEmAberto_DeveRetornarOk()
        {
            var filtro = new FiltroQuantidadeEncaminhamentoNAAPAEmAbertoDto();
            var retorno = new GraficoEncaminhamentoNAAPADto();

            _encaminhamentoAbertoUseCase.Setup(s => s.Executar(filtro))
                                        .ReturnsAsync(retorno);

            var result = await _controller.ObterQuantidadeEncaminhamentoNAAPAEmAberto(filtro, _encaminhamentoAbertoUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoEncaminhamentoNAAPADto>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuantidadeAtendimentoPorProfissionalMes deve retornar Ok com lista")]
        public async Task ObterQuantidadeAtendimentoPorProfissionalMes_DeveRetornarOk()
        {
            var filtro = new FiltroQuantidadeAtendimentoNAAPAPorProfissionalMesDto();
            var retorno = new GraficoEncaminhamentoNAAPADto();

            _profissionalMesUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroQuantidadeAtendimentoNAAPAPorProfissionalMesDto>()))
                .ReturnsAsync(retorno);

            var result = await _controller
                .ObterQuantidadeAtendimentoNAAPAPorProfissionalMes(filtro, _profissionalMesUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoEncaminhamentoNAAPADto>(ok.Value);
        }

        [Fact(DisplayName = "ObterAlunosAbaixo50Porcento deve retornar Ok com lista")]
        public async Task ObterAlunosFrequenciaTurmaEvasaoAbaixo50Porcento_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto();

            var retorno = new PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>
            {
                Items = new List<AlunoFrequenciaTurmaEvasaoDto>
        {
            new AlunoFrequenciaTurmaEvasaoDto()
        }
            };

            _alunosAbaixo50UseCase
                .Setup(s => s.Executar(It.IsAny<FiltroGraficoFrequenciaTurmaEvasaoAlunoDto>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterAlunosFrequenciaTurmaEvasaoAbaixo50Porcento(
                filtro,
                _alunosAbaixo50UseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);

            var dto = Assert.IsType<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>(ok.Value);
            Assert.Single(dto.Items);
        }


        [Fact(DisplayName = "ObterAlunosSemPresenca deve retornar Ok com lista")]
        public async Task ObterAlunosFrequenciaTurmaEvasaoSemPresenca_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto();

            var retorno = new PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>
            {
                Items = new List<AlunoFrequenciaTurmaEvasaoDto>
        {
            new AlunoFrequenciaTurmaEvasaoDto()
        }
            };

            _alunosSemPresencaUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroGraficoFrequenciaTurmaEvasaoAlunoDto>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterAlunosFrequenciaTurmaEvasaoSemPresenca(
                filtro,
                _alunosSemPresencaUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);

            var dto = Assert.IsType<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>(ok.Value);
            Assert.Single(dto.Items);
        }

    }
}
