using Dapper;
using Moq;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso

{
    //public class ListarPaginadoCriancasEstudantesAusentesBuscaAtivaUseCaseTeste
    //{
    //    private readonly RegistrarRegistroAcaoUseCase useCase;
    //    private readonly ObterListaDevolutivasPorTurmaComponenteUseCase useCase2;
    //    private readonly Mock<IMediator> mediator;

    //    public ListarPaginadoCriancasEstudantesAusentesBuscaAtivaUseCaseTeste()
    //    {
    //        mediator = new Mock<IMediator>();
    //        useCase = new RegistrarRegistroAcaoUseCase(mediator.Object);
    //        useCase2 = new ObterListaDevolutivasPorTurmaComponenteUseCase(mediator.Object);
    //    }

    //    [Fact]
    //    public async Task Deve_Obter_Devolutiva()
    //    {
    //        // Arrange 
    //        mediator.Setup(a => a.Send(It.IsAny<ObterDevolutivaPorIdQuery>(), It.IsAny<CancellationToken>()))
    //            .ReturnsAsync(new Devolutiva() { Id = 1, Descricao = "teste", CriadoEm = DateTime.Today });

    //        mediator.Setup(a => a.Send(It.IsAny<ObterIdsDiariosBordoPorDevolutivaQuery>(), It.IsAny<CancellationToken>()))
    //            .ReturnsAsync(new List<long> { 1, 2 });

    //        // Act
    //        var devolutiva = await useCase.Executar(1);

    //        // Assert
    //        Assert.NotNull(devolutiva);
    //        Assert.NotNull(devolutiva.Auditoria);
    //        Assert.True(devolutiva.DiariosIds.Count() == 2);
    //    }

    //    [Fact]
    //    public async Task Deve_Obter_Todas_Devolutivas()
    //    {
    //        var devolutivasExistentes = new List<DevolutivaResumoDto>()
    //        {
    //            new DevolutivaResumoDto()
    //            {
    //                Id = 1,
    //                PeriodoInicio = DateTime.Today.AddDays(-20),
    //                PeriodoFim = DateTime.Today.AddDays(20),
    //                CriadoEm = DateTime.Today,
    //                CriadoPor = "Sistema"
    //            },
    //            new DevolutivaResumoDto()
    //            {
    //                Id = 2,
    //                PeriodoInicio = DateTime.Today.AddDays(-20),
    //                PeriodoFim = DateTime.Today.AddDays(20),
    //                CriadoEm = DateTime.Today,
    //                CriadoPor = "Sistema"
    //            },
    //        };
    //        // Arrange 
    //        mediator.Setup(a => a.Send(It.IsAny<ObterListaDevolutivasPorTurmaComponenteQuery>(), It.IsAny<CancellationToken>()))
    //            .ReturnsAsync(new PaginacaoResultadoDto<DevolutivaResumoDto>() { Items = devolutivasExistentes, TotalPaginas = 1, TotalRegistros = 2 });

    //        // Act
    //        var devolutivas = await useCase2.Executar(new FiltroListagemDevolutivaDto("1", 534, null));

    //        // Assert
    //        Assert.NotNull(devolutivas);
    //        Assert.True(devolutivas.Items.Count() == 2);
    //        Assert.True(devolutivas.TotalRegistros == 2);
    //        Assert.True(devolutivas.TotalPaginas == 1);
    //        Assert.Contains(devolutivas.Items, i => i.Id == 2);
    //    }
    //}

    //public class ListarPaginadoCriancasEstudantesAusentesBuscaAtivaUseCaseTeste
    //{
    //    private readonly Mock<ISgpContext> mockDatabase;
    //    private readonly Mock<IServicoAuditoria> mockAuditoria;
    //    private readonly RepositorioRegistroAcaoBuscaAtiva repositorio;

    //    public ListarPaginadoCriancasEstudantesAusentesBuscaAtivaUseCaseTeste()
    //    {
    //        mockDatabase = new Mock<ISgpContext>();
    //        mockAuditoria = new Mock<IServicoAuditoria>();
    //        repositorio = new RepositorioRegistroAcaoBuscaAtiva(mockDatabase.Object, mockAuditoria.Object);
    //    }

    //    [Fact]
    //    public async Task Deve_Retornar_Lista_Paginada_Com_Registros()
    //    {
    //        // Arrange
    //        var codigoAluno = "123456";
    //        var turmaId = 1L;
    //        var paginacao = new Paginacao(10, 1);

    //        var mockGridReader = new Mock<SqlMapper.GridReader>();
    //        var registros = new List<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>
    //    {
    //        new RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto { Id = 1 },
    //        new RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto { Id = 2 }
    //    };

    //        mockGridReader.Setup(r => r.Read<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>())
    //            .Returns(registros);
    //        mockGridReader.Setup(r => r.ReadFirst<int>())
    //            .Returns(2);

    //        mockDatabase.Setup(db => db.Conexao.QueryMultipleAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
    //            .ReturnsAsync(mockGridReader.Object);

    //        // Act
    //        var resultado = await repositorio.ListarPaginadoCriancasEstudantesAusentes(codigoAluno, turmaId, paginacao);

    //        // Assert
    //        Assert.NotNull(resultado);
    //        Assert.Equal(2, resultado.Items.Count());
    //        Assert.Equal(2, resultado.TotalRegistros);
    //        Assert.Equal(1, resultado.TotalPaginas);
    //    }

    //    [Fact]
    //    public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Registros()
    //    {
    //        // Arrange
    //        var codigoAluno = "123456";
    //        var turmaId = 1L;
    //        var paginacao = new Paginacao(10, 1);

    //        var mockGridReader = new Mock<SqlMapper.GridReader>();
    //        var registrosVazios = new List<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>();

    //        mockGridReader.Setup(r => r.Read<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>())
    //            .Returns(registrosVazios);
    //        mockGridReader.Setup(r => r.ReadFirst<int>())
    //            .Returns(0);

    //        mockDatabase.Setup(db => db.Conexao.QueryMultipleAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
    //            .ReturnsAsync(mockGridReader.Object);

    //        // Act
    //        var resultado = await repositorio.ListarPaginadoCriancasEstudantesAusentes(codigoAluno, turmaId, paginacao);

    //        // Assert
    //        Assert.NotNull(resultado);
    //        Assert.Empty(resultado.Items);
    //        Assert.Equal(0, resultado.TotalRegistros);
    //        Assert.Equal(0, resultado.TotalPaginas);
    //    }

    //}
}
