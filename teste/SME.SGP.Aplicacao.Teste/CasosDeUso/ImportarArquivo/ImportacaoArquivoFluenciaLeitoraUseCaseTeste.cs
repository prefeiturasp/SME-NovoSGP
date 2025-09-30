using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo.FluenciaLeitora;
using SME.SGP.Aplicacao.Commands.ImportarArquivo;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.FluenciaLeitora;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Enumerados;
using System.Collections.Generic;
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
        private readonly Mock<IRepositorioTurmaConsulta> repoTurmaMock;
        private readonly ImportacaoArquivoFluenciaLeitoraUseCase useCase;

        public ImportacaoArquivoFluenciaLeitoraUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repoTurmaMock = new Mock<IRepositorioTurmaConsulta>();

            useCase = new ImportacaoArquivoFluenciaLeitoraUseCase(
                mediatorMock.Object,
                repoTurmaMock.Object
            );
        }
        private IFormFile CriarArquivoXlsxValido()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Planilha1");

                worksheet.Cell(1, 1).Value = "CodigoTurma";
                worksheet.Cell(1, 2).Value = "CodigoAluno";
                worksheet.Cell(1, 3).Value = "Fluencia";

                worksheet.Cell(2, 1).Value = "123";   
                worksheet.Cell(2, 2).Value = "456";   
                worksheet.Cell(2, 3).Value = "1";    

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                var formFile = new FormFile(stream, 0, stream.Length, "arquivo", "fluencia.xlsx")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };

                return formFile;
            }
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Ano_Letivo_For_Zero()
        {
            var arquivo = CriarArquivoXlsxValido();

            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(arquivo, 0, 1));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Arquivo_For_Nulo()
        {
            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(null, 2025, 1));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Arquivo_Nao_For_Xlsx()
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes("arquivo invalido"));
            var arquivo = new FormFile(ms, 0, ms.Length, "arquivo", "teste.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(arquivo, 2025, 1));
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Sucesso_Quando_Arquivo_Valido()
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

            var turma = new Turma { CodigoTurma = "123", Nome = "Turma Teste" };
            repoTurmaMock
                .Setup(r => r.ObterPorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);
            
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), default))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new AlunoPorTurmaResposta { CodigoAluno = "456" } });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarImportacaoArquivoFluenciaLeitoraCommand>(), default))
                .ReturnsAsync(new Dominio.FluenciaLeitora()); 

            var resultado = await useCase.Executar(arquivo, 2025, 1);

            Assert.NotNull(resultado);
            Assert.True(resultado.Sucesso, "A importação deveria ter sido bem-sucedida, mas retornou uma falha.");

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), default), Times.Once());
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarImportacaoArquivoFluenciaLeitoraCommand>(), default), Times.Once());
        }
    }
}