using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DashboardFechamentoControllerTeste
{
    private readonly Mock<IObterFechamentoSituacaoUseCase> _situacaoUseCase;
    private readonly Mock<IObterFechamentoSituacaoPorEstudanteUseCase> _situacaoPorEstudanteUseCase;
    private readonly Mock<IObterFechamentoPendenciasUseCase> _pendenciasUseCase;
    private readonly Mock<IObterFechamentoConselhoClasseSituacaoUseCase> _conselhoClasseSituacaoUseCase;
    private readonly Mock<IObterNotasFinaisUseCases> _notasFinaisUseCase;
    private readonly Mock<IObterPendenciaParecerConclusivoUseCases> _parecerConclusivoUseCase;

    private readonly DashboardFechamentoController _controller;

    public DashboardFechamentoControllerTeste()
    {
        _situacaoUseCase = new Mock<IObterFechamentoSituacaoUseCase>();
        _situacaoPorEstudanteUseCase = new Mock<IObterFechamentoSituacaoPorEstudanteUseCase>();
        _pendenciasUseCase = new Mock<IObterFechamentoPendenciasUseCase>();
        _conselhoClasseSituacaoUseCase = new Mock<IObterFechamentoConselhoClasseSituacaoUseCase>();
        _notasFinaisUseCase = new Mock<IObterNotasFinaisUseCases>();
        _parecerConclusivoUseCase = new Mock<IObterPendenciaParecerConclusivoUseCases>();

        _controller = new DashboardFechamentoController();
    }

    [Fact(DisplayName = "ObterSituacoesFechamento deve retornar Ok com lista")]
    public async Task ObterSituacoesFechamento_DeveRetornarOk()
    {
        var filtro = new FiltroDashboardFechamentoDto();
        var retorno = new List<GraficoBaseDto> { new GraficoBaseDto() };

        _situacaoUseCase.Setup(s => s.Executar(filtro))
                        .ReturnsAsync(retorno);

        var result = await _controller.ObterSituacoesFechamento(filtro, _situacaoUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterSituacoesFechamentoPorEstudante deve retornar Ok com lista")]
    public async Task ObterSituacoesFechamentoPorEstudante_DeveRetornarOk()
    {
        // Arrange
        var filtro = new FiltroDashboardFechamentoDto();

        var retorno = new List<GraficoBaseDto>
    {
        new GraficoBaseDto()
    };

        _situacaoPorEstudanteUseCase
            .Setup(s => s.Executar(It.IsAny<FiltroDashboardFechamentoDto>()))
            .ReturnsAsync(retorno);

        // Act
        var result = await _controller.ObterSituacoesFechamentoPorEstudante(
            filtro,
            _situacaoPorEstudanteUseCase.Object);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
    }


    [Fact(DisplayName = "ObterPendenciaFechamento deve retornar Ok com IEnumerable<GraficoBaseDto>")]
    public async Task ObterPendenciaFechamento_DeveRetornarOk()
    {
        var filtro = new FiltroDashboardFechamentoDto();
        var retorno = new List<GraficoBaseDto> { new GraficoBaseDto() };

        _pendenciasUseCase.Setup(s => s.Executar(filtro))
                          .ReturnsAsync(retorno);

        var result = await _controller.ObterPendenciaFechamento(filtro, _pendenciasUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterSituacoesConselhoClasse deve retornar Ok com lista")]
    public async Task ObterSituacoesConselhoClasse_DeveRetornarOk()
    {
        var filtro = new FiltroDashboardFechamentoDto();
        var retorno = new List<GraficoBaseDto> { new GraficoBaseDto() };

        _conselhoClasseSituacaoUseCase.Setup(s => s.Executar(filtro))
                                      .ReturnsAsync(retorno);

        var result = await _controller.ObterSituacoesConselhoClasse(filtro, _conselhoClasseSituacaoUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterPosConselho deve retornar Ok com lista")]
    public async Task ObterPosConselho_DeveRetornarOk()
    {
        var filtro = new FiltroDashboardFechamentoDto();
        var retorno = new List<GraficoBaseDto> { new GraficoBaseDto() };

        _notasFinaisUseCase.Setup(s => s.Executar(filtro))
                           .ReturnsAsync(retorno);

        var result = await _controller.ObterPosConselho(filtro, _notasFinaisUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterPendenciaParecerConclusivo deve retornar Ok com lista")]
    public async Task ObterPendenciaParecerConclusivo_DeveRetornarOk()
    {
        var filtro = new FiltroDashboardFechamentoDto();
        var retorno = new List<GraficoBaseDto> { new GraficoBaseDto() };

        _parecerConclusivoUseCase.Setup(s => s.Executar(filtro))
                                  .ReturnsAsync(retorno);

        var result = await _controller.ObterPendenciaParecerConclusivo(
            filtro, _parecerConclusivoUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
    }
}
