using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class DashboardEAControllerTeste
{
    private readonly Faker _faker = new();
    private readonly DashboardEAController _controller;

    private readonly Mock<IObterTotaisAdesaoUseCase> _totaisAdesaoUseCase = new();
    private readonly Mock<IObterTotaisAdesaoAgrupadosPorDreUseCase> _totaisAgrupadosUseCase = new();
    private readonly Mock<IObterUltimaAtualizacaoPorProcessoUseCase> _ultimaAtualizacaoUseCase = new();
    private readonly Mock<IObterComunicadosTotaisUseCase> _comunicadosTotaisUseCase = new();
    private readonly Mock<IObterComunicadosTotaisAgrupadosPorDreUseCase> _comunicadosAgrupadosUseCase = new();
    private readonly Mock<IObterDadosDeLeituraDeComunicadosUseCase> _dadosLeituraUseCase = new();
    private readonly Mock<IObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase> _dadosAgrupadosUseCase = new();
    private readonly Mock<IObterComunicadosParaFiltroDaDashboardUseCase> _filtroComunicadosUseCase = new();
    private readonly Mock<IObterDadosDeLeituraDeComunicadosPorModalidadeUseCase> _leituraPorModalidadeUseCase = new();
    private readonly Mock<IObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase> _leituraPorTurmaUseCase = new();
    private readonly Mock<IObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase> _leituraAlunosUseCase = new();

    public DashboardEAControllerTeste()
    {
        _controller = new DashboardEAController();
    }

    [Fact(DisplayName = "ObterTotaisAdesaoAgrupados deve retornar Ok com lista")]
    public async Task ObterTotaisAdesaoAgrupados_DeveRetornarOk()
    {
        var lista = new List<TotaisAdesaoAgrupadoProDreResultado>();

        _totaisAgrupadosUseCase.Setup(s => s.Executar())
                              .ReturnsAsync(lista);

        var result = await _controller.ObterTotaisAdesaoAgrupadosPorDre(_totaisAgrupadosUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<TotaisAdesaoAgrupadoProDreResultado>>(ok.Value);
    }

    [Fact(DisplayName = "ObterUltimaAtualizacaoPorProcesso deve retornar Ok")]
    public async Task ObterUltimaAtualizacaoPorProcesso_DeveRetornarOk()
    {
        var retorno = new UltimaAtualizaoWorkerPorProcessoResultado();

        _ultimaAtualizacaoUseCase.Setup(s => s.Executar(It.IsAny<string>()))
                                 .ReturnsAsync(retorno);

        var result = await _controller.ObterUltimaAtualizacaoPorProcesso("P1", _ultimaAtualizacaoUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<UltimaAtualizaoWorkerPorProcessoResultado>(ok.Value);
    }

    // 3. Obter Comunicados Totais SME
    [Fact(DisplayName = "ObterComunicadosTotaisSme deve retornar Ok")]
    public async Task ObterComunicadosTotaisSme_DeveRetornarOk()
    {
        var retorno = new ComunicadosTotaisResultado();

        _comunicadosTotaisUseCase.Setup(s => s.Executar(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                                 .ReturnsAsync(retorno);

        var result = await _controller.ObterComunicadosTotaisSme(2024, "D1", "U1", _comunicadosTotaisUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<ComunicadosTotaisResultado>(ok.Value);
    }

    [Fact(DisplayName = "ObterComunicadosTotaisAgrupadosPorDre deve retornar Ok com lista de DTO")]
    public async Task ObterComunicadosTotaisAgrupadosPorDre_DeveRetornarOk()
    {
        // Arrange
        var retorno = new List<ComunicadosTotaisPorDreResultado>
    {
        new ComunicadosTotaisPorDreResultado()
    };

        _comunicadosAgrupadosUseCase
            .Setup(s => s.Executar(It.IsAny<int>()))
            .ReturnsAsync(retorno);

        // Act
        var result = await _controller.ObterComunicadosTotaisAgrupadosPorDre(
            2024,
            _comunicadosAgrupadosUseCase.Object
        );

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<ComunicadosTotaisPorDreResultado>>(ok.Value);
    }

    [Fact(DisplayName = "ObterDadosDeLeituraDeComunicados deve retornar Ok com lista")]
    public async Task ObterDadosDeLeituraDeComunicados_DeveRetornarOk()
    {
        var lista = new List<DadosDeLeituraDoComunicadoDto>
        {
            new DadosDeLeituraDoComunicadoDto()
        };

        _dadosLeituraUseCase.Setup(s => s.Executar(It.IsAny<ObterDadosDeLeituraDeComunicadosDto>()))
                            .ReturnsAsync(lista);

        var result = await _controller.ObterDadosDeLeituraDeComunicados(new(), _dadosLeituraUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<DadosDeLeituraDoComunicadoDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterDadosDeLeituraDeComunicadosAgrupados deve retornar Ok com lista")]
    public async Task ObterDadosDeLeituraDeComunicadosAgrupados_DeveRetornarOk()
    {
        var lista = new List<DadosDeLeituraDoComunicadoDto> { new() };

        _dadosAgrupadosUseCase.Setup(s => s.Executar(It.IsAny<ObterDadosDeLeituraDeComunicadosAgrupadosPorDreDto>()))
                              .ReturnsAsync(lista);

        var result = await _controller.ObterDadosDeLeituraDeComunicadosAgrupadosPorDre(new(), _dadosAgrupadosUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<DadosDeLeituraDoComunicadoDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterComunicadosParaFiltroDaDashboard deve retornar Ok com lista")]
    public async Task ObterComunicadosParaFiltro_DeveRetornarOk()
    {
        var lista = new List<ComunicadoParaFiltroDaDashboardDto>();

        _filtroComunicadosUseCase.Setup(s => s.Executar(It.IsAny<ObterComunicadosParaFiltroDaDashboardDto>()))
                                 .ReturnsAsync(lista);

        var result = await _controller.ObterComunicadosParaFiltroDaDashboard(new(), _filtroComunicadosUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<ComunicadoParaFiltroDaDashboardDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterDadosDeLeituraDeComunicadosPorModalidades deve retornar Ok com lista")]
    public async Task ObterDadosPorModalidades_DeveRetornarOk()
    {
        var lista = new List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto> { new() };

        _leituraPorModalidadeUseCase.Setup(s => s.Executar(It.IsAny<ObterDadosDeLeituraDeComunicadosDto>()))
                                    .ReturnsAsync(lista);

        var result = await _controller.ObterDadosDeLeituraDeComunicadosPorModalidades(new(), _leituraPorModalidadeUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterDadosDeLeituraDeComunicadosPorModalidadeETurma deve retornar Ok com lista")]
    public async Task ObterDadosPorModalidadeETurma_DeveRetornarOk()
    {
        var lista = new List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto> { new() };

        _leituraPorTurmaUseCase.Setup(s => s.Executar(It.IsAny<FiltroDadosDeLeituraDeComunicadosPorModalidadeDto>()))
                               .ReturnsAsync(lista);

        var result = await _controller.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma(new(), _leituraPorTurmaUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>>(ok.Value);
    }

    [Fact(DisplayName = "ObterDadosDeLeituraDeComunicadosPorTurma deve retornar Ok com lista")]
    public async Task ObterDadosPorAlunosDaTurma_DeveRetornarOk()
    {
        var lista = new List<DadosLeituraAlunosComunicadoPorTurmaDto> { new() };

        _leituraAlunosUseCase.Setup(s => s.Executar(It.IsAny<FiltroDadosDeLeituraDeComunicadosPorAlunosTurmaDto>()))
                              .ReturnsAsync(lista);

        var result = await _controller.ObterDadosDeLeituraDeComunicadosPorTurma(new(), _leituraAlunosUseCase.Object);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<DadosLeituraAlunosComunicadoPorTurmaDto>>(ok.Value);
    }
}
