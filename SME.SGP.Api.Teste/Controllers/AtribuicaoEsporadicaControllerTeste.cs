using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AtribuicaoEsporadicaControllerTeste
    {
        private readonly AtribuicaoEsporadicaController _controller;
        private readonly Mock<IExcluirAtribuicaoEsporadicaUseCase> _excluirAtribuicaoEsporadicaUseCase = new();
        private readonly Mock<IListarAtribuicaoEsporadicaUseCase> _listarAtribuicaoEsporadicaUseCase = new();
        private readonly Mock<IConsultasAtribuicaoEsporadica> _consultasAtribuicaoEsporadica = new();
        private readonly Mock<IComandosAtribuicaoEsporadica> _comandosAtribuicaoEsporadica = new();
        private readonly Mock<IObterPeriodoAtribuicaoPorUeUseCase> _obterPeriodoAtribuicaoPorUeUseCase = new();
        private readonly Faker _faker;

        public AtribuicaoEsporadicaControllerTeste()
        {
            _controller = new AtribuicaoEsporadicaController();
            _faker = new Faker("pt_BR");
        }

        #region BuscaAtribuicaoEsporadicaPorID

        [Fact(DisplayName = "Deve retornar 200 OK com atribuição esporádica quando existir")]
        public async Task DeveRetornarAtribuicaoEsporadica_QuandoEncontrada()
        {
            // Arrange
            long id = 1;
            var atribuicaoMock = new AtribuicaoEsporadicaCompletaDto
            {
                Id = id,
                AnoLetivo = 2025,
                DreId = "dre-1",
                UeId = "ue-1",
                ProfessorRf = "123456",
                ProfessorNome = "João da Silva",
                CriadoEm = DateTime.Now,
                CriadoPor = "Admin",
                CriadoRF = "000001"
            };

            _consultasAtribuicaoEsporadica.Setup(x => x.ObterPorId(id)).ReturnsAsync(atribuicaoMock);

            // Act
            var result = await _controller.Obter(id, _consultasAtribuicaoEsporadica.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<AtribuicaoEsporadicaCompletaDto>(okResult.Value);
            Assert.Equal(id, retorno.Id);
            Assert.Equal("123456", retorno.ProfessorRf);
        }
        #endregion

        #region BuscaAtribuicaEsporadicaNaoExiste

        [Fact(DisplayName = "Deve retornar 204 NoContent quando atribuição esporádica não for encontrada")]
        public async Task DeveRetornarNoContent_QuandoAtribuicaoEsporadicaNaoExiste()
        {
            // Arrange
            long id = 999;
            _consultasAtribuicaoEsporadica.Setup(x => x.ObterPorId(id)).ReturnsAsync((AtribuicaoEsporadicaCompletaDto)null!);

            // Act
            var result = await _controller.Obter(id, _consultasAtribuicaoEsporadica.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region BuscaAtribuicaoEsporadicaPorUE
        [Fact(DisplayName = "Deve retornar 200 OK com atribuição esporádica por UE quando existir")]
        public async Task DeveRetornarOk_QuandoExistirPeriodoAtribuicaoPorUe()
        {
            // Arrange
            long ueId = _faker.Random.Long();
            int anoLetivo = DateTime.Now.Year;
            var dataInicio = DateTime.Today.AddDays(_faker.Random.Int(1, 5));
            var dataFim = dataInicio.AddDays(_faker.Random.Int(10, 20));

            var periodoEsperado = new PeriodoAtribuicaoEsporadicaDto
            {
                DataInicio = dataInicio,
                DataFim = dataFim,
            };

            _obterPeriodoAtribuicaoPorUeUseCase
                .Setup(x => x.Executar(ueId, anoLetivo))
                .ReturnsAsync(periodoEsperado);

            var controller = new AtribuicaoEsporadicaController();

            // Act
            var resultado = await controller.ObterPeriodoAtribuicaoPorUe(ueId, anoLetivo, _obterPeriodoAtribuicaoPorUeUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var retorno = Assert.IsType<PeriodoAtribuicaoEsporadicaDto>(okResult.Value);
            Assert.Equal(dataInicio, retorno.DataInicio);
            Assert.Equal(dataFim, retorno.DataFim);

            _obterPeriodoAtribuicaoPorUeUseCase.Verify(x => x.Executar(ueId, anoLetivo), Times.Once);
        }
        #endregion

        #region ListaPaginadaAtribuicoesEsporadicas

        [Fact(DisplayName = "Deve chamar o caso de uso para buscar atribuições esporadicas de forma paginada")]
        public async Task DeveRetornarListaPaginada_AtribuicoesEsporadicas()
        {
            // Arrange
            var filtro = new FiltroAtribuicaoEsporadicaDto
            {
                AnoLetivo = DateTime.Now.Year,
                DreId = _faker.Random.AlphaNumeric(8),
                UeId = _faker.Random.AlphaNumeric(6),
                ProfessorRF = _faker.Random.AlphaNumeric(6)
            };

            var atribuicoes = new List<AtribuicaoEsporadicaDto>
            {
                new AtribuicaoEsporadicaDto
                {
                    AnoLetivo = 2025,
                    DreId = "dre-1",
                    UeId = "ue-1",
                    ProfessorRf = "123456",
                    ProfessorNome = "Maria Silva",
                    DataInicio = DateTime.Today,
                    DataFim = DateTime.Today.AddDays(10),
                    Id = 1
                }
            };

            var paginacaoEsperada = new PaginacaoResultadoDto<AtribuicaoEsporadicaDto>
            {
                Items = atribuicoes,
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            _listarAtribuicaoEsporadicaUseCase.Setup(x => x.Executar(It.IsAny<FiltroAtribuicaoEsporadicaDto>()))
           .ReturnsAsync(paginacaoEsperada);

            var result = await _controller.Listar(filtro, _listarAtribuicaoEsporadicaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PaginacaoResultadoDto<AtribuicaoEsporadicaDto>>(okResult.Value);
            Assert.Single(retorno.Items);
            Assert.Equal(1, retorno.TotalPaginas);
            Assert.Equal(1, retorno.TotalRegistros);
        }
        #endregion

        #region IncluirAtribuicaoEsporadica

        [Fact(DisplayName = "Deve chamar o caso de uso para incluir atribuição esporadica")]
        public async Task DeveChamarCasoDeUso_ParaIncluirAtribuicaoEsporadica()
        {
            var dto = CriarDto();

            _comandosAtribuicaoEsporadica
                .Setup(x => x.Salvar(It.IsAny<AtribuicaoEsporadicaDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.Post(dto, _comandosAtribuicaoEsporadica.Object);

            // Assert
            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            _comandosAtribuicaoEsporadica.Verify(x => x.Salvar(It.Is<AtribuicaoEsporadicaDto>(a =>
                a.AnoLetivo == dto.AnoLetivo &&
                a.DreId == dto.DreId &&
                a.ProfessorRf == dto.ProfessorRf &&
                a.UeId == dto.UeId)), Times.Once);
        }
        #endregion

        #region ExcluirAtribuicaoEsporadica

        [Fact(DisplayName = "Deve chamar o caso de uso para excluir atribuição esporadica")]
        public async Task DeveRetornarOk_QuandoExclusaoAtribuicaoEsporadicaForBemSucedida()
        {
            var id = _faker.Random.Long(1);
            var retornoEsperado = new RetornoBaseDto("Atribuição esporadica excluída com sucesso");

            _excluirAtribuicaoEsporadicaUseCase.Setup(x => x.Executar(It.IsAny<long>())).ReturnsAsync(true);

            var resultado = await _controller.Excluir(_excluirAtribuicaoEsporadicaUseCase.Object, id);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.True(retorno);
        }
        #endregion

        private AtribuicaoEsporadicaDto CriarDto()
        {
            return new AtribuicaoEsporadicaDto
            {
                AnoLetivo = DateTime.Now.Year,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(30),
                DreId = "dre-01",
                EhInfantil = false,
                Excluido = false,
                Id = 1,
                Migrado = false,
                ProfessorNome = "João Silva",
                ProfessorRf = _faker.Random.AlphaNumeric(6),
                UeId = "ue-01"
            };
        }
    }
}
