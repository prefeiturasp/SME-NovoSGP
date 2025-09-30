using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Queries.ImportarArquivo;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ImportarArquivo
{
    public class ImportacaoLogUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IContextoAplicacao> contextoAplicacaoMock;

        public ImportacaoLogUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            contextoAplicacaoMock = new Mock<IContextoAplicacao>();
        }

        [Fact]
        public void Construtor_Mediator_Nulo_Deve_Lancar_Excecao()
        {
            Assert.Throws<ArgumentNullException>(() => new ImportacaoLogUseCase(contextoAplicacaoMock.Object, null));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_E_Retornar_Resultado()
        {
            var filtro = new FiltroPesquisaImportacaoDto { ImportacaoLogId = 123 };

            var retornoEsperado = new PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>
            {
                Items = new List<ImportacaoLogQueryRetornoDto>
                {
                    new ImportacaoLogQueryRetornoDto
                    {
                        Id = 1,
                        NomeArquivo = "arquivo.csv",
                        StatusImportacao = "Concluido",
                        TotalRegistros = 10,
                        RegistrosProcessados = 10
                    }
                },
                TotalPaginas = 1,
                TotalRegistros = 10
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterImportacaoLogQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var useCase = new ImportacaoLogUseCase(contextoAplicacaoMock.Object, mediatorMock.Object);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal("arquivo.csv", ((List<ImportacaoLogQueryRetornoDto>)resultado.Items)[0].NomeArquivo);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterImportacaoLogQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Obter_Importacao_Log_Query_Deve_Armazenar_Valores()
        {
            var paginacao = new Paginacao(2, 10);
            var filtro = new FiltroPesquisaImportacaoDto { ImportacaoLogId = 456 };

            var query = new ObterImportacaoLogQuery(paginacao, filtro);

            Assert.Equal(paginacao, query.Paginacao);
            Assert.Equal(filtro, query.Filtros);
        }

        [Theory(DisplayName = "Paginacao deve calcular corretamente QuantidadeRegistros e Ignorados")]
        [InlineData(2, 10, 10, 10)]  
        [InlineData(0, 5, 5, 0)]     
        [InlineData(1, 0, 0, 0)]   
        [InlineData(-1, -1, 0, 0)]  
        public void Paginacao_Deve_Calcular_Corretamente(int pagina, int registros, int esperadoRegistros, int esperadoIgnorados)
        {
            var paginacao = new Paginacao(pagina, registros);

            Assert.Equal(esperadoRegistros, paginacao.QuantidadeRegistros);
            Assert.Equal(esperadoIgnorados, paginacao.QuantidadeRegistrosIgnorados);
        }

        [Fact]
        public void Dtos_Deve_Permitir_Get_Set()
        {
            var dto = new ImportacaoLogQueryRetornoDto
            {
                Id = 10,
                NomeArquivo = "teste.csv",
                StatusImportacao = "Processando",
                TotalRegistros = 20,
                RegistrosProcessados = 5
            };

            var filtro = new FiltroPesquisaImportacaoDto { ImportacaoLogId = 99 };

            var paginacaoResultado = new PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>
            {
                Items = new List<ImportacaoLogQueryRetornoDto> { dto },
                TotalPaginas = 2,
                TotalRegistros = 20
            };

            Assert.Equal(10, dto.Id);
            Assert.Equal("teste.csv", dto.NomeArquivo);
            Assert.Equal("Processando", dto.StatusImportacao);
            Assert.Equal(20, dto.TotalRegistros);
            Assert.Equal(5, dto.RegistrosProcessados);

            Assert.Equal(99, filtro.ImportacaoLogId);

            Assert.Single(paginacaoResultado.Items);
            Assert.Equal(2, paginacaoResultado.TotalPaginas);
            Assert.Equal(20, paginacaoResultado.TotalRegistros);
        }
    }
}
