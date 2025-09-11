using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{   
    public class ObterListagemDiariosDeBordoPorPeriodoQueryHandlerTeste
        {
            private readonly Mock<IRepositorioDiarioBordo> repositorioDiarioBordoMock;
            private readonly Mock<IContextoAplicacao> contextoAplicacaoMock;
            private readonly ObterListagemDiariosDeBordoPorPeriodoQueryHandler handler;

            public ObterListagemDiariosDeBordoPorPeriodoQueryHandlerTeste()
            {
                repositorioDiarioBordoMock = new Mock<IRepositorioDiarioBordo>();
                contextoAplicacaoMock = new Mock<IContextoAplicacao>();

                handler = new ObterListagemDiariosDeBordoPorPeriodoQueryHandler(
                    contextoAplicacaoMock.Object,
                    repositorioDiarioBordoMock.Object
                );
            }

            [Fact]
            public async Task Handle_DeveRetornarLista_QuandoHouverDiarios()
            {
                // Arrange
                var query = new ObterListagemDiariosDeBordoPorPeriodoQuery(
                    123, "infantil", 2,
                    DateTime.Today.AddDays(-10),
                    DateTime.Today
                );

                var listaDiarios = new List<DiarioBordoResumoDto>
            {
                new DiarioBordoResumoDto
                {
                    Id = 1,
                    DataAula = new DateTime(2025, 7, 1),
                    Nome = "Professor João",
                    CodigoRf = "123456",
                    Pendente = false,
                    Tipo = (int)TipoAula.Normal,
                    AulaId = 1001,
                    InseridoCJ = false
                },
                new DiarioBordoResumoDto
                {
                    Id = 2,
                    DataAula = new DateTime(2025, 7, 2),
                    Nome = "Prof. Maria",
                    CodigoRf = "654321",
                    Pendente = true,
                    Tipo = (int)TipoAula.Reposicao,
                    AulaId = 1002,
                    InseridoCJ = true
                }
            };

                var resultadoPaginado = new PaginacaoResultadoDto<DiarioBordoResumoDto>
                {
                    TotalPaginas = 1,
                    TotalRegistros = 2,
                    Items = listaDiarios
                };

                repositorioDiarioBordoMock
                    .Setup(r => r.ObterListagemDiarioBordoPorPeriodoPaginado(
                        query.TurmaId,
                        query.ComponenteCurricularPaiId,
                        query.ComponenteCurricularFilhoId,
                        query.DataInicio,
                        query.DataFim,
                        It.IsAny<Paginacao>()))
                    .ReturnsAsync(resultadoPaginado);

                // Act
                var resultado = await handler.Handle(query, CancellationToken.None);

                // Assert
                Assert.NotNull(resultado);
                Assert.Equal(2, resultado.Items.Count());
                Assert.Equal(1, resultado.TotalPaginas);
                Assert.Equal(2, resultado.TotalRegistros);

                var item1 = resultado.Items.ElementAt(0);
                Assert.Equal("01/07/2025 - Professor João (123456)", item1.Titulo);
                Assert.False(item1.Pendente);
                Assert.Equal(1001, item1.AulaId);

                var item2 = resultado.Items.ElementAt(1);
                Assert.Equal("02/07/2025 - Prof. Maria (654321) - CJ - Reposição", item2.Titulo);
                Assert.True(item2.Pendente);
                Assert.Equal(1002, item2.AulaId);
            }

            [Fact]
            public async Task Handle_DeveRetornarDtoComListaVazia_QuandoNaoHouverItens()
            {
                // Arrange
                var query = new ObterListagemDiariosDeBordoPorPeriodoQuery(
                    234, "Infantil", 2,
                    DateTime.Today.AddDays(-5),
                    DateTime.Today
                );

                var resultadoVazio = new PaginacaoResultadoDto<DiarioBordoResumoDto>
                {
                    TotalPaginas = 0,
                    TotalRegistros = 0,
                    Items = new List<DiarioBordoResumoDto>()
                };

                repositorioDiarioBordoMock
                    .Setup(r => r.ObterListagemDiarioBordoPorPeriodoPaginado(
                        It.IsAny<long>(),
                        It.IsAny<string>(),
                        It.IsAny<long>(),
                        It.IsAny<DateTime?>(),
                        It.IsAny<DateTime?>(),
                        It.IsAny<Paginacao>()))
                    .ReturnsAsync(resultadoVazio);

                // Act
                var resultado = await handler.Handle(query, CancellationToken.None);

                // Assert
                Assert.NotNull(resultado);
                Assert.Empty(resultado.Items);
                Assert.Equal(0, resultado.TotalPaginas);
                Assert.Equal(0, resultado.TotalRegistros);
            }
        }
    
}
