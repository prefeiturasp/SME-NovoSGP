using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

public class PendenciasControllerTeste
{
    [Fact]
    public async Task Listar_DeveRetornarOkComPendenciaUnica()
    {
        var mockUseCase = new Mock<IObterPendenciasUseCase>();
        var pendencias = new PaginacaoResultadoDto<PendenciaDto>
        {
            Items = new List<PendenciaDto>
            {
                new PendenciaDto { Titulo = "Pendência A" }
            },
            TotalRegistros = 1
        };

        mockUseCase.Setup(x => x.Executar("123", 1, "teste"))
                   .ReturnsAsync(pendencias);

        var controller = new PendenciasController();

        var result = await controller.Listar(mockUseCase.Object, "123", 1, "teste");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<PaginacaoResultadoDto<PendenciaDto>>(okResult.Value);
        Assert.Single(value.Items);
        Assert.Equal("Pendência A", value.Items.First().Titulo);

        mockUseCase.Verify(x => x.Executar("123", 1, "teste"), Times.Once);
    }

    [Fact]
    public async Task Listar_DeveRetornarOkComListaVazia()
    {
        var mockUseCase = new Mock<IObterPendenciasUseCase>();
        var pendencias = new PaginacaoResultadoDto<PendenciaDto>
        {
            Items = new List<PendenciaDto>(),
            TotalRegistros = 0
        };

        mockUseCase.Setup(x => x.Executar("123", 1, "teste"))
                   .ReturnsAsync(pendencias);

        var controller = new PendenciasController();

        var result = await controller.Listar(mockUseCase.Object, "123", 1, "teste");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<PaginacaoResultadoDto<PendenciaDto>>(okResult.Value);
        Assert.Empty(value.Items);

        mockUseCase.Verify(x => x.Executar("123", 1, "teste"), Times.Once);
    }

    [Fact]
    public async Task Listar_DeveRetornarOkMesmoQuandoNulo()
    {
        var mockUseCase = new Mock<IObterPendenciasUseCase>();
        mockUseCase.Setup(x => x.Executar("123", 1, "teste"))
                   .ReturnsAsync((PaginacaoResultadoDto<PendenciaDto>)null);

        var controller = new PendenciasController();

        var result = await controller.Listar(mockUseCase.Object, "123", 1, "teste");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okResult.Value);

        mockUseCase.Verify(x => x.Executar("123", 1, "teste"), Times.Once);
    }

    [Fact]
    public async Task Listar_DeveRetornarOkComMultiplasPendencias()
    {
        var mockUseCase = new Mock<IObterPendenciasUseCase>();
        var pendencias = new PaginacaoResultadoDto<PendenciaDto>
        {
            Items = new List<PendenciaDto>
            {
                new PendenciaDto { Titulo = "Pendência A" },
                new PendenciaDto { Titulo = "Pendência B" },
                new PendenciaDto { Titulo = "Pendência C" }
            },
            TotalRegistros = 3
        };

        mockUseCase.Setup(x => x.Executar("123", 1, "teste"))
                   .ReturnsAsync(pendencias);

        var controller = new PendenciasController();

        var result = await controller.Listar(mockUseCase.Object, "123", 1, "teste");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<PaginacaoResultadoDto<PendenciaDto>>(okResult.Value);
        Assert.Equal(3, value.Items.Count());
        Assert.Collection(value.Items,
            p => Assert.Equal("Pendência A", p.Titulo),
            p => Assert.Equal("Pendência B", p.Titulo),
            p => Assert.Equal("Pendência C", p.Titulo));

        mockUseCase.Verify(x => x.Executar("123", 1, "teste"), Times.Once);
    }
}
