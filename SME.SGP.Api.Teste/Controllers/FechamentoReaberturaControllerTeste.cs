using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class FechamentoReaberturaControllerTeste
    {
        private readonly FechamentoReaberturaController _controller;
        private readonly Mock<IComandosFechamentoReabertura> _comandosFechamentoReabertura = new();
        private readonly Mock<IConsultasFechamentoReabertura> _consultasFechamentoReabertura = new();

        public FechamentoReaberturaControllerTeste()
        {
            _controller = new FechamentoReaberturaController();
        }

        [Fact]
        public async Task Executar_Quando_Alterar_Deve_Retornar_Ok_Com_Sucesso()
        {
            var fechamentoReaberturaPersistenciaDto = new FechamentoReaberturaPersistenciaDto
            {
                Descricao = "Teste alteração",
                DreCodigo = "DRE001",
                TipoCalendarioId = 1,
                UeCodigo = "UE001"
            };
            var id = 1L;
            var retornoEsperado = "Alterado com sucesso";

            _comandosFechamentoReabertura
                .Setup(x => x.Alterar(fechamentoReaberturaPersistenciaDto, id))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Alterar(_comandosFechamentoReabertura.Object, fechamentoReaberturaPersistenciaDto, id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
            _comandosFechamentoReabertura.Verify(x => x.Alterar(fechamentoReaberturaPersistenciaDto, id), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Alterar_Com_Dto_Nulo_Deve_Retornar_Ok()
        {
            FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto = null;
            var id = 1L;
            var retornoEsperado = "Processado";

            _comandosFechamentoReabertura
                .Setup(x => x.Alterar(fechamentoReaberturaPersistenciaDto, id))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Alterar(_comandosFechamentoReabertura.Object, fechamentoReaberturaPersistenciaDto, id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
        }

        [Fact]
        public async Task Executar_Quando_Excluir_Com_Ids_Validos_Deve_Retornar_Ok()
        {
            var ids = new long[] { 1, 2, 3 };
            var retornoEsperado = "Excluído com sucesso";

            _comandosFechamentoReabertura
                .Setup(x => x.Excluir(ids))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Excluir(_comandosFechamentoReabertura.Object, ids);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
            _comandosFechamentoReabertura.Verify(x => x.Excluir(ids), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Excluir_Com_Array_Vazio_Deve_Retornar_Ok()
        {
            var ids = new long[] { };
            var retornoEsperado = "Nenhum item para excluir";

            _comandosFechamentoReabertura
                .Setup(x => x.Excluir(ids))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Excluir(_comandosFechamentoReabertura.Object, ids);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
        }

        [Fact]
        public async Task Executar_Quando_Excluir_Com_Array_Nulo_Deve_Retornar_Ok()
        {
            long[] ids = null;
            var retornoEsperado = "Array nulo processado";

            _comandosFechamentoReabertura
                .Setup(x => x.Excluir(ids))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Excluir(_comandosFechamentoReabertura.Object, ids);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
        }

        [Fact]
        public async Task Executar_Quando_Listar_Com_Filtros_Validos_Deve_Retornar_Ok()
        {
            var filtroDto = new FechamentoReaberturaFiltroDto
            {
                TipoCalendarioId = 1,
                DreCodigo = "DRE001",
                UeCodigo = "UE001"
            };

            var retornoEsperado = new PaginacaoResultadoDto<FechamentoReaberturaListagemDto>
            {
                Items = new List<FechamentoReaberturaListagemDto>
                {
                    new FechamentoReaberturaListagemDto
                    {
                        Id = 1,
                        Descricao = "Teste"
                    }
                },
                TotalRegistros = 1,
                TotalPaginas = 1
            };

            _consultasFechamentoReabertura
                .Setup(x => x.Listar(filtroDto.TipoCalendarioId, filtroDto.DreCodigo, filtroDto.UeCodigo))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Listar(_consultasFechamentoReabertura.Object, filtroDto);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<PaginacaoResultadoDto<FechamentoReaberturaListagemDto>>(okResult.Value);
            Assert.Equal(retornoEsperado.TotalRegistros, retorno.TotalRegistros);
            _consultasFechamentoReabertura.Verify(x => x.Listar(filtroDto.TipoCalendarioId, filtroDto.DreCodigo, filtroDto.UeCodigo), Times.Once);
        }
        
        [Fact]
        public async Task Executar_Quando_Listar_Com_Lista_Vazia_Deve_Retornar_Ok()
        {
            var filtroDto = new FechamentoReaberturaFiltroDto
            {
                TipoCalendarioId = 999,
                DreCodigo = "INEXISTENTE",
                UeCodigo = "INEXISTENTE"
            };

            var retornoEsperado = new PaginacaoResultadoDto<FechamentoReaberturaListagemDto>
            {
                Items = new List<FechamentoReaberturaListagemDto>(),
                TotalRegistros = 0,
                TotalPaginas = 0
            };

            _consultasFechamentoReabertura
                .Setup(x => x.Listar(filtroDto.TipoCalendarioId, filtroDto.DreCodigo, filtroDto.UeCodigo))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Listar(_consultasFechamentoReabertura.Object, filtroDto);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<PaginacaoResultadoDto<FechamentoReaberturaListagemDto>>(okResult.Value);
            Assert.Empty(retorno.Items);
        }

        [Fact]
        public void Executar_Quando_ObterPorId_Com_Id_Valido_Deve_Retornar_Ok()
        {
            var id = 1L;
            var retornoEsperado = new FechamentoReaberturaRetornoDto
            {
                Descricao = "Teste retorno",
                DreCodigo = "DRE001",
                TipoCalendarioId = 1
            };

            _consultasFechamentoReabertura
                .Setup(x => x.ObterPorId(id))
                .Returns(retornoEsperado);

            var resultado = _controller.ObterPorId(_consultasFechamentoReabertura.Object, id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<FechamentoReaberturaRetornoDto>(okResult.Value);
            Assert.Equal(retornoEsperado.Descricao, retorno.Descricao);
            _consultasFechamentoReabertura.Verify(x => x.ObterPorId(id), Times.Once);
        }

        [Fact]
        public void Executar_Quando_ObterPorId_Com_Id_Inexistente_Deve_Retornar_Ok()
        {
            var id = 999L;
            FechamentoReaberturaRetornoDto retornoEsperado = null;

            _consultasFechamentoReabertura
                .Setup(x => x.ObterPorId(id))
                .Returns(retornoEsperado);

            var resultado = _controller.ObterPorId(_consultasFechamentoReabertura.Object, id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public void Executar_Quando_ObterPorId_Com_Id_Zero_Deve_Retornar_Ok()
        {
            var id = 0L;
            FechamentoReaberturaRetornoDto retornoEsperado = null;

            _consultasFechamentoReabertura
                .Setup(x => x.ObterPorId(id))
                .Returns(retornoEsperado);

            var resultado = _controller.ObterPorId(_consultasFechamentoReabertura.Object, id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async Task Executar_Quando_Salvar_Com_Dto_Valido_Deve_Retornar_Ok()
        {
            var fechamentoReaberturaPersistenciaDto = new FechamentoReaberturaPersistenciaDto
            {
                Descricao = "Novo fechamento",
                DreCodigo = "DRE001",
                TipoCalendarioId = 1,
                UeCodigo = "UE001"
            };
            var retornoEsperado = "Salvo com sucesso";

            _comandosFechamentoReabertura
                .Setup(x => x.Salvar(fechamentoReaberturaPersistenciaDto))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Salvar(_comandosFechamentoReabertura.Object, fechamentoReaberturaPersistenciaDto);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
            _comandosFechamentoReabertura.Verify(x => x.Salvar(fechamentoReaberturaPersistenciaDto), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Salvar_Com_Dto_Nulo_Deve_Retornar_Ok()
        {
            FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto = null;
            var retornoEsperado = "Dto nulo processado";

            _comandosFechamentoReabertura
                .Setup(x => x.Salvar(fechamentoReaberturaPersistenciaDto))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Salvar(_comandosFechamentoReabertura.Object, fechamentoReaberturaPersistenciaDto);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
        }

        [Fact]
        public async Task Executar_Quando_Salvar_Com_Dto_Vazio_Deve_Retornar_Ok()
        {
            var fechamentoReaberturaPersistenciaDto = new FechamentoReaberturaPersistenciaDto();
            var retornoEsperado = "Dto vazio processado";

            _comandosFechamentoReabertura
                .Setup(x => x.Salvar(fechamentoReaberturaPersistenciaDto))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Salvar(_comandosFechamentoReabertura.Object, fechamentoReaberturaPersistenciaDto);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
        }

        [Fact]
        public async Task Executar_Quando_Listar_Com_Filtro_Nulo_Deve_Lancar_NullReferenceException()
        {
            FechamentoReaberturaFiltroDto filtroDto = null;

            var exception = await Assert.ThrowsAsync<System.NullReferenceException>(
                () => _controller.Listar(_consultasFechamentoReabertura.Object, filtroDto));

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task Executar_Quando_Listar_Com_Filtro_Com_Valores_Default_Deve_Retornar_Ok()
        {
            var filtroDto = new FechamentoReaberturaFiltroDto
            {
                TipoCalendarioId = 0,
                DreCodigo = null,
                UeCodigo = null
            };
            
            var retornoEsperado = new PaginacaoResultadoDto<FechamentoReaberturaListagemDto>
            {
                Items = new List<FechamentoReaberturaListagemDto>(),
                TotalRegistros = 0,
                TotalPaginas = 0
            };

            _consultasFechamentoReabertura
                .Setup(x => x.Listar(filtroDto.TipoCalendarioId, filtroDto.DreCodigo, filtroDto.UeCodigo))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _controller.Listar(_consultasFechamentoReabertura.Object, filtroDto);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.IsType<PaginacaoResultadoDto<FechamentoReaberturaListagemDto>>(okResult.Value);
        }
    }
}