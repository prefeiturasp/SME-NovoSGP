using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.FluenciaLeitora;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Enumerados;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ImportarArquivo
{
    public class ImportacaoArquivoFluenciaLeitoraUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioImportacaoLog> repoImportacaoLogMock;
        private readonly Mock<IRepositorioTurmaConsulta> repoTurmaMock;
        private readonly ImportacaoArquivoFluenciaLeitoraUseCase useCase;

        public ImportacaoArquivoFluenciaLeitoraUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repoImportacaoLogMock = new Mock<IRepositorioImportacaoLog>();
            repoTurmaMock = new Mock<IRepositorioTurmaConsulta>();

            useCase = new ImportacaoArquivoFluenciaLeitoraUseCase(
                mediatorMock.Object,
                repoImportacaoLogMock.Object,
                repoTurmaMock.Object
            );
        }

        private IFormFile CriarArquivoXlsxValido()
        {
            // MemoryStream permanece vivo
            var ms = new MemoryStream();

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var planilha = workbook.Worksheets.Add("Plan1");

                // Cabeçalho
                planilha.Cell(1, 1).Value = "SerieAno";
                planilha.Cell(1, 2).Value = "CodigoEOL";
                planilha.Cell(1, 3).Value = "Nota";

                // Linha válida
                planilha.Cell(2, 1).Value = 1;
                planilha.Cell(2, 2).Value = "123";
                planilha.Cell(2, 3).Value = 5.5;

                workbook.SaveAs(ms); // salva no stream, ainda aberto
            }

            ms.Position = 0; // volta para o início do stream

            // retorna FormFile que usa o stream
            return new FormFile(ms, 0, ms.Length, "arquivo", "teste.xlsx")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoAnoLetivoForZero()
        {
            // Arrange
            var arquivo = CriarArquivoXlsxValido();

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(arquivo, 0, 1));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoArquivoForNulo()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(null, 2025, 1));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoArquivoNaoForXlsx()
        {
            // Arrange
            var ms = new MemoryStream(Encoding.UTF8.GetBytes("arquivo invalido"));
            var arquivo = new FormFile(ms, 0, ms.Length, "arquivo", "teste.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(arquivo, 2025, 1));
        }

        [Fact]
        public async Task Executar_DeveRetornarSucesso_QuandoArquivoValido()
        {
            // Arrange
            var arquivo = CriarArquivoXlsxValido();

            var importacaoLog = new ImportacaoLog
            {
                Id = 99,
                StatusImportacao = SituacaoArquivoImportacao.CarregamentoInicial.GetAttribute<DisplayAttribute>().Name
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarImportacaoLogCommand>(), default))
                .ReturnsAsync(importacaoLog);

            var turma = new Turma { CodigoTurma = "123", Nome = "Turma Teste" };
            repoTurmaMock
                .Setup(r => r.ObterPorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            repoImportacaoLogMock
                .Setup(r => r.SalvarAsync(It.IsAny<ImportacaoLog>()))
                .ReturnsAsync(99L);

            // Act
            var resultado = await useCase.Executar(arquivo, 2025, 1);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Sucesso);
            Assert.Equal(importacaoLog.Id, resultado.Id);

            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarImportacaoLogCommand>(), default), Times.AtLeastOnce);
            repoImportacaoLogMock.Verify(r => r.SalvarAsync(It.IsAny<ImportacaoLog>()), Times.AtLeastOnce);
        }
    }

}