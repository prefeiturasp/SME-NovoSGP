using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.Ideb;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Enumerados;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ImportarArquivo
{
    public class ImportacaoArquivoIdebUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioUeConsulta> repoUeMock;
        private readonly ImportacaoArquivoIdebUseCase useCase;

        public ImportacaoArquivoIdebUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repoUeMock = new Mock<IRepositorioUeConsulta>();

            useCase = new ImportacaoArquivoIdebUseCase(
                mediatorMock.Object,
                repoUeMock.Object
            );
        }

        private IFormFile CriarArquivoXlsxValido()
        {
            var ms = new MemoryStream();

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var planilha = workbook.Worksheets.Add("Plan1");

                planilha.Cell(1, 1).Value = "SerieAno";
                planilha.Cell(1, 2).Value = "CodigoEOL";
                planilha.Cell(1, 3).Value = "Nota";

                planilha.Cell(2, 1).Value = 1;
                planilha.Cell(2, 2).Value = "123";
                planilha.Cell(2, 3).Value = 5.5;

                workbook.SaveAs(ms);
            }

            ms.Position = 0; 

            return new FormFile(ms, 0, ms.Length, "arquivo", "teste.xlsx")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoAnoLetivoForZero()
        {
            var arquivo = CriarArquivoXlsxValido();

            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(arquivo, 0));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoArquivoForNulo()
        {
            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(null, 2025));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoArquivoNaoForXlsx()
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes("arquivo invalido"));
            var arquivo = new FormFile(ms, 0, ms.Length, "arquivo", "teste.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(arquivo, 2025));
        }

        [Fact]
        public async Task Executar_DeveRetornarSucesso_QuandoArquivoValido()
        {
            var arquivo = CriarArquivoXlsxValido();

            var importacaoLog = new ImportacaoLog
            {
                Id = 99,
                StatusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarImportacaoLogCommand>(), default))
                .ReturnsAsync(importacaoLog);

            var ue = new Ue { CodigoUe = "123", Nome = "UE Teste" };
            repoUeMock
                .Setup(r => r.ObterPorCodigo(It.IsAny<string>()))
                .Returns(ue);

            var resultado = await useCase.Executar(arquivo, 2025);

            Assert.NotNull(resultado);
            Assert.True(resultado.Sucesso);
            Assert.Equal(importacaoLog.Id, resultado.Id);

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarImportacaoLogCommand>(), default), Times.AtLeast(2));
        }
    }
}