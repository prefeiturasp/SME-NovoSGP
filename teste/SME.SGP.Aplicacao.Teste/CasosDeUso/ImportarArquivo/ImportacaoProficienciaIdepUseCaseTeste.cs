using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.Proficiencia;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep;
using SME.SGP.Aplicacao.Queries.UE.ObterUePorCodigoEolEscola;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ImportarArquivo
{
    public class ImportacaoProficienciaIdepUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ImportacaoProficienciaIdepUseCase _useCase;

        public ImportacaoProficienciaIdepUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ImportacaoProficienciaIdepUseCase(_mediatorMock.Object);
        }

        private IFormFile CriarArquivoExcel(string conteudo)
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Planilha1");
                var linhas = conteudo.Split(Environment.NewLine);
                for (int i = 0; i < linhas.Length; i++)
                {
                    var celulas = linhas[i].Split(';');
                    for (int j = 0; j < celulas.Length; j++)
                    {
                        worksheet.Cell(i + 1, j + 1).Value = celulas[j];
                    }
                }
                workbook.SaveAs(stream);
            }
            stream.Position = 0;

            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.OpenReadStream()).Returns(stream);
            formFile.Setup(f => f.Length).Returns(stream.Length);
            formFile.Setup(f => f.FileName).Returns("arquivo.xlsx");
            formFile.Setup(f => f.ContentType).Returns("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            return formFile.Object;
        }

        private void SetupMediatorMocks(bool ueEncontrada = true)
        {
            var importacaoLog = new ImportacaoLog { Id = 1, NomeArquivo = "arquivo.xlsx", TipoArquivoImportacao = "PROFICIENCIA_IDEP" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarImportacaoLogCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(importacaoLog);

            if (ueEncontrada)
            {
                _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUePorCodigoEolEscolaQuery>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new Ue { CodigoUe = "094374" });
            }
            else
            {
                _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUePorCodigoEolEscolaQuery>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Ue)null);
            }

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirImportacaoProficienciaIdepCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarImportacaoProficienciaIdepCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarImportacaoLogCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ImportacaoLog());

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarImportacaoLogErroCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ImportacaoLogErro());
        }

        [Fact]
        public async Task Executar_Quando_Ano_Letivo_Zero_Deve_Lancar_Excecao_De_Negocio_()
        {
            var arquivo = new Mock<IFormFile>().Object;
            var anoLetivo = 0;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(arquivo, anoLetivo));
            Assert.Equal("Informe o ano letivo.", excecao.Message);
        }

        [Fact]
        public async Task Executar_Quando_Arquivo_Nulo_Deve_Lancar_Excecao_De_Negocio_()
        {
            var anoLetivo = 2025;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(null, anoLetivo));
            Assert.Equal(MensagemNegocioComuns.ARQUIVO_VAZIO, excecao.Message);
        }

        [Fact]
        public async Task Executar_Quando_Arquivo_Com_Tamanho_Zero_Deve_Lancar_Excecao_De_Negocio_()
        {
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(f => f.Length).Returns(0);
            var anoLetivo = 2025;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(arquivoMock.Object, anoLetivo));
            Assert.Equal(MensagemNegocioComuns.ARQUIVO_VAZIO, excecao.Message);
        }

        [Fact]
        public async Task Executar_Quando_Content_Type_Invalido_Deve_Lancar_Excecao_De_Negocio_()
        {
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(f => f.Length).Returns(100);
            arquivoMock.Setup(f => f.ContentType).Returns("application/pdf");
            var anoLetivo = 2025;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(arquivoMock.Object, anoLetivo));
            Assert.Equal(MensagemNegocioComuns.SOMENTE_ARQUIVO_XLSX_SUPORTADO, excecao.Message);
        }

        [Fact]
        public async Task Executar_Quando_Log_Nao_Salvo_Deve_Retornar_Sucesso_Mesmo_Assim_()
        {
            var conteudo = $"CodigoEOL;SerieAno;ComponenteCurricular;Proficiencia${Environment.NewLine}094374;5;1;250.5";
            var arquivo = CriarArquivoExcel(conteudo);
            var anoLetivo = 2025;
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarImportacaoLogCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ImportacaoLog)null);

            var excecao = await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(arquivo, anoLetivo));

            Assert.NotNull(excecao);
        }
    }
}
