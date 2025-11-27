using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class ComunicadoControllerTeste
    {
        private readonly ComunicadoController _controller;
        private readonly Mock<ISolicitarInclusaoComunicadoEscolaAquiUseCase> _solicitarInclusaoComunicadoEscolaAquiUseCase = new();
        private readonly Mock<ISolicitarAlteracaoComunicadoEscolaAquiUseCase> _solicitarAlteracaoComunicadoEscolaAquiUseCase = new();
        private readonly Mock<ISolicitarExclusaoComunicadosEscolaAquiUseCase> _solicitarExclusaoComunicadosEscolaAquiUseCase = new();
        private readonly Mock<IObterComunicadosPaginadosEscolaAquiUseCase> _obterComunicadosPaginadosEscolaAquiUseCase = new();
        private readonly Mock<IObterComunicadoEscolaAquiUseCase> _obterComunicadoEscolaAquiUseCase = new();
        private readonly Mock<IObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase> _obterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase = new();
        private readonly Mock<IObterAnosPorCodigoUeModalidadeEscolaAquiUseCase> _obterAnosPorCodigoUeModalidadeEscolaAquiUseCase = new();
        private readonly Mock<IObterComunicadosPaginadosAlunoUseCase> _obterComunicadosPaginadosAlunoUseCase = new();
        private readonly Mock<IObterAnosLetivosComunicadoUseCase> _obterAnosLetivosComunicadoUseCase = new();
        private readonly Mock<IObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase> _obterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase = new();
        private readonly Mock<IObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase> _obterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase = new();
        private readonly Mock<IObterQuantidadeCriancaUseCase> _obterQuantidadeCriancaUseCase = new();
        private readonly Faker _faker;

        public ComunicadoControllerTeste()
        {
            _controller = new ComunicadoController();
            _faker = new Faker("pt_BR");
        }

        #region InserirComunicado
        [Fact(DisplayName = "Deve chamar o caso de uso que insere comunicado")]
        public async Task DeveRetornarOk_QuandoInserirComunicadoValido()
        {
            // Arrange
            var dto = new ComunicadoInserirDto();

            _solicitarInclusaoComunicadoEscolaAquiUseCase.Setup(x => x.Executar(dto)).ReturnsAsync("OK");

            // Act
            var resultado = await _controller.PostAsync(dto, _solicitarInclusaoComunicadoEscolaAquiUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region AlterarComunicado
        [Fact(DisplayName = "Deve chamar o caso de uso que altera comunicado")]
        public async Task DeveRetornarOk_QuandoAlterarAulaComDadosValidos()
        {
            // Arrange
            var id = _faker.Random.Long(1);
            var dto = new ComunicadoAlterarDto();

            _solicitarAlteracaoComunicadoEscolaAquiUseCase
                .Setup(x => x.Executar(id, It.IsAny<ComunicadoAlterarDto>()))
                .ReturnsAsync("Comunicado alterado com sucesso");

            // Act
            var resultado = await _controller.Alterar(id, dto, _solicitarAlteracaoComunicadoEscolaAquiUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<string>(okResult.Value);
        }
        #endregion

        #region ExcluirComunciado
        [Fact(DisplayName = "Deve chamar o caso de uso que exclui comunicado")]
        public async Task Excluir_DeveRetornarOk_ComResultadoEsperado()
        {
            // Arrange
            long[] ids = { 1, 2, 3 };

            _solicitarExclusaoComunicadosEscolaAquiUseCase.Setup(u => u.Executar(ids))
                   .ReturnsAsync(true);

            var controller = new ComunicadoController();

            // Act
            var result = await controller.Excluir(ids, _solicitarExclusaoComunicadosEscolaAquiUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)ok.Value);
        }
        #endregion

        #region ListarComunicados
        [Fact(DisplayName = "Deve retornar Ok ao listar comunicados")]
        public async Task ListarComunicados_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroComunicadoDto();
            var retorno = new PaginacaoResultadoDto<ComunicadoListaPaginadaDto>
            {
                Items = new List<ComunicadoListaPaginadaDto>
                    {
                        new ComunicadoListaPaginadaDto()
                    }
            };

            _obterComunicadosPaginadosEscolaAquiUseCase
                .Setup(x => x.Executar(filtro))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ListarComunicados(filtro, _obterComunicadosPaginadosEscolaAquiUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>>(ok.Value);
            Assert.Single(valorRetornado.Items);
        }
        #endregion

        #region BuscarComunicadoPorId
        [Fact(DisplayName = "Deve retornar Ok ao buscar comunicado por id")]
        public async Task BuscarPorId_DeveRetornarOk_ComObjetoEsperado()
        {
            // Arrange
            var id = 10;
            var comunicado = new ComunicadoCompletoDto();

            _obterComunicadoEscolaAquiUseCase
                .Setup(u => u.Executar(id))
                .ReturnsAsync(comunicado);

            // Act
            var resultado = await _controller.BuscarPorId(id, _obterComunicadoEscolaAquiUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<ComunicadoCompletoDto>(okResult.Value);
            Assert.Equal(comunicado, valorRetornado);
        }
        #endregion

        #region BuscarAlunosRetornarNoContentQuandoVazio
        [Fact(DisplayName = "Deve retornar NoContent quando não houver alunos")]
        public async Task BuscarAlunos_DeveRetornarNoContent_QuandoVazio()
        {
            // Arrange
            _obterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase
                .Setup(u => u.Executar("T1", 2024))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            // Act
            var resultado = await _controller.BuscarAlunos("T1", 2024, _obterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }
        #endregion

        #region BuscarAlunosRetornarOkQuandoHouverAlunos
        [Fact(DisplayName = "Deve retornar Ok quando houver alunos")]
        public async Task BuscarAlunos_DeveRetornarOk_QuandoHouverAlunos()
        {
            // Arrange
            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta()
            };

            _obterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase
                .Setup(u => u.Executar("T1", 2024))
                .ReturnsAsync(alunos);

            // Act
            var resultado = await _controller.BuscarAlunos("T1", 2024, _obterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<List<AlunoPorTurmaResposta>>(ok.Value);
            Assert.Single(valorRetornado);
        }
        #endregion

        #region BuscarTodosPaginadaQuandoHouverComunicadosComListaPaginada
        [Fact(DisplayName = "BuscarTodos deve retornar Ok quando houver comunicados com lista paginada")]
        public async Task BuscarTodosAsync_DeveRetornarOk_QuandoHouverItens()
        {
            // Arrange
            var filtro = new FiltroComunicadoDto();

            var retorno = new PaginacaoResultadoDto<ComunicadoListaPaginadaDto>
            {
                Items = new List<ComunicadoListaPaginadaDto>
                {
                    new ComunicadoListaPaginadaDto()
                }
            };

            _obterComunicadosPaginadosEscolaAquiUseCase.Setup(s => s.Executar(filtro)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.BuscarTodosAsync(
                filtro,
                _obterComunicadosPaginadosEscolaAquiUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsType<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>>(ok.Value);
            Assert.Single(valor.Items);
        }
        #endregion

        #region BuscarTodosQuandoNaoHouverComunicadosComListaPaginada
        [Fact(DisplayName = "BuscarTodos deve retornar NoContent quando não houver comunicados com lista paginada")]
        public async Task BuscarTodosAsync_DeveRetornarNoContent_QuandoVazio()
        {
            // Arrange
            var filtro = new FiltroComunicadoDto();

            var retorno = new PaginacaoResultadoDto<ComunicadoListaPaginadaDto>
            {
                Items = new List<ComunicadoListaPaginadaDto>()
            };

            _obterComunicadosPaginadosEscolaAquiUseCase.Setup(s => s.Executar(filtro)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.BuscarTodosAsync(
                filtro,
                _obterComunicadosPaginadosEscolaAquiUseCase.Object
            );

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }
        #endregion

        #region ObterAnosPorCodigoUeModalidade
        [Fact(DisplayName = "ObterAnosPorCodigoUeModalidade deve retornar Ok com lista de anos")]
        public async Task ObterAnosPorCodigoUeModalidade_DeveRetornarOk()
        {
            // Arrange
            var codigoUe = "UE123";
            var modalidades = new[] { 1, 2 };

            var retorno = new List<AnosPorCodigoUeModalidadeEscolaAquiResult>
    {
        new AnosPorCodigoUeModalidadeEscolaAquiResult()
    };

            _obterAnosPorCodigoUeModalidadeEscolaAquiUseCase
                .Setup(x => x.Executar(codigoUe, modalidades))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterAnosPorCodigoUeModalidade(
                codigoUe,
                modalidades,
                _obterAnosPorCodigoUeModalidadeEscolaAquiUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var valor = Assert.IsType<List<AnosPorCodigoUeModalidadeEscolaAquiResult>>(ok.Value);
            Assert.Single(valor);
        }
        #endregion

        #region ObterComunicadosDoAlunoComListaPaginada
        [Fact(DisplayName = "ObterComunicadosDoAluno deve retornar Ok com lista paginada")]
        public async Task ObterComunicadosDoAluno_DeveRetornarOk()
        {
            // Arrange
            long turmaId = 10;
            int semestre = 1;
            long alunoCodigo = 555;

            var retorno = new PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>
            {
                Items = new List<ComunicadoAlunoReduzidoDto>
        {
            new ComunicadoAlunoReduzidoDto()
        }
            };

            _obterComunicadosPaginadosAlunoUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroTurmaAlunoSemestreDto>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterComunicadosDoAluno(
                turmaId,
                semestre,
                alunoCodigo,
                _obterComunicadosPaginadosAlunoUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var valor = Assert.IsType<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>>(ok.Value);
            Assert.Single(valor.Items);
        }
        #endregion

        #region ObterAnosLetivos
        [Fact(DisplayName = "ObterAnosLetivos deve retornar Ok com anos letivos")]
        public async Task ObterAnosLetivos_DeveRetornarOk()
        {
            // Arrange
            int anoMinimo = 2019;

            var retorno = new AnoLetivoComunicadoDto
            {
                AnosLetivosHistorico = new List<int> { 2018, 2019, 2020 },
                TemHistorico = true
            };

            _obterAnosLetivosComunicadoUseCase
                .Setup(x => x.Executar(anoMinimo))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterAnosLetivos(
                anoMinimo,
                _obterAnosLetivosComunicadoUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsType<AnoLetivoComunicadoDto>(ok.Value);

            Assert.Equal(3, valor.AnosLetivosHistorico is ICollection<int> col ? col.Count : 3);
            Assert.True(valor.TemHistorico);
        }
        #endregion

        #region ObterTurmasPorAnoLetivo
        [Fact(DisplayName = "ObterTurmasPorAnoLetivo deve retornar Ok com lista de turmas")]
        public async Task ObterTurmasPorAnoLetivo_DeveRetornarOk()
        {
            // Arrange
            int anoLetivo = 2024;
            string codigoUe = "UE123";
            int semestre = 1;
            int[] modalidades = { 1, 2 };
            string[] anos = { "1", "2" };
            bool consideraHistorico = true;

            var retorno = new List<DropdownTurmaRetornoDto>
    {
        new DropdownTurmaRetornoDto()
    };

            _obterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase
                .Setup(x => x.Executar(anoLetivo, codigoUe, modalidades, semestre, anos, consideraHistorico))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolares(
                anoLetivo,
                codigoUe,
                semestre,
                modalidades,
                anos,
                consideraHistorico,
                _obterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var valor = Assert.IsType<List<DropdownTurmaRetornoDto>>(ok.Value);
            Assert.Single(valor);
        }

        #endregion

        #region ObterSemestres
        [Fact(DisplayName = "ObterSemestres deve retornar Ok com lista de semestres")]
        public async Task ObterSemestres_DeveRetornarOk()
        {
            // Arrange
            bool consideraHistorico = true;
            int modalidade = 1;
            int anoLetivo = 2024;
            string ueCodigo = "UE123";

            var retorno = new List<int> { 1, 2 };

            _obterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase
                .Setup(x => x.Executar(consideraHistorico, modalidade, anoLetivo, ueCodigo))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterSemestres(
                consideraHistorico,
                modalidade,
                anoLetivo,
                ueCodigo,
                _obterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var valor = Assert.IsType<List<int>>(ok.Value);
            Assert.Equal(2, valor.Count);
        }
        #endregion

        #region ObterQuantidadeCrianca
        [Fact(DisplayName = "ObterQuantidadeCrianca deve retornar Ok com quantidade")]
        public async Task ObterQuantidadeCrianca_DeveRetornarOk()
        {
            // Arrange
            int anoLetivo = 2024;
            string dreCodigo = "DRE1";
            string ueCodigo = "UE1";
            string[] turmas = { "T1" };
            int[] modalidades = { 1 };
            string[] anos = { "1" };

            var retorno = new QuantidadeCriancaDto
            {
                MensagemQuantidade = "12"
            };

            _obterQuantidadeCriancaUseCase
                .Setup(x => x.Executar(anoLetivo, turmas, dreCodigo, ueCodigo, modalidades, anos))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterQuantidadeCrianca(
                anoLetivo,
                dreCodigo,
                ueCodigo,
                turmas,
                modalidades,
                anos,
                _obterQuantidadeCriancaUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var valor = Assert.IsType<QuantidadeCriancaDto>(ok.Value);
            Assert.Equal("12", valor.MensagemQuantidade);
        }
        #endregion
    }
}
