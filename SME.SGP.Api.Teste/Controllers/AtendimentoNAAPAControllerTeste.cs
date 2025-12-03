using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class AtendimentoNAAPAControllerTeste
    {
        private readonly AtendimentoNAAPAController _controller;
        private readonly Faker _faker;

        public AtendimentoNAAPAControllerTeste()
        {
            _controller = new AtendimentoNAAPAController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para registrar um encaminhamento")]
        public async Task DeveChamarUseCase_ParaRegistrarEncaminhamento()
        {
            // Arrange
            var useCaseMock = new Mock<IRegistrarAtendimentoNAAPAUseCase>();
            var encaminhamentoDto = new AtendimentoNAAPADto { AlunoCodigo = "123" };
            var retornoDto = new ResultadoAtendimentoNAAPADto { Id = 1 };

            useCaseMock.Setup(u => u.Executar(encaminhamentoDto)).ReturnsAsync(retornoDto);

            // Act
            var resultado = await _controller.RegistrarAtendimento(encaminhamentoDto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoDto), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retornoDto, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter encaminhamentos paginados")]
        public async Task DeveChamarUseCase_ParaObterEncaminhamentosPaginados()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAtendimentoNAAPAUseCase>();
            var filtro = new FiltroAtendimentoNAAPADto { AnoLetivo = DateTime.Now.Year };
            var retornoPaginado = new PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(retornoPaginado);

            // Act
            var resultado = await _controller.ObterAtendimentosNAAPA(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retornoPaginado, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter o questionário")]
        public async Task DeveChamarUseCase_ParaObterQuestionario()
        {
            // Arrange
            var useCaseMock = new Mock<IObterQuestionarioAtendimentoNAAPAUseCase>();
            var questionarioId = _faker.Random.Long(1);
            var encaminhamentoId = _faker.Random.Long(1);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var retorno = new List<QuestaoDto>();

            useCaseMock.Setup(u => u.Executar(questionarioId, encaminhamentoId, alunoCodigo, turmaCodigo)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterQuestionario(questionarioId, encaminhamentoId, alunoCodigo, turmaCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(questionarioId, encaminhamentoId, alunoCodigo, turmaCodigo), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve retornar Ok com a lista de situações NAAPA")]
        public void DeveRetornarOk_ComListaDeSituacoes()
        {
            // Arrange
            var totalSituacoes = Enum.GetNames(typeof(SituacaoNAAPA)).Length;

            // Act
            var resultado = _controller.ObterSituacoes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsAssignableFrom<IOrderedEnumerable<EnumeradoRetornoDto>>(okResult.Value);
            Assert.Equal(totalSituacoes, lista.Count());
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para fazer upload de arquivo")]
        public async Task DeveChamarUseCase_ParaUploadDeArquivo()
        {
            // Arrange
            var useCaseMock = new Mock<IUploadDeArquivoUseCase>();
            var retornoGuid = Guid.NewGuid();

            var conteudo = "Arquivo de teste";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(conteudo));
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(a => a.Length).Returns(stream.Length);

            useCaseMock.Setup(u => u.Executar(arquivoMock.Object, Dominio.TipoArquivo.EncaminhamentoNAAPA)).ReturnsAsync(retornoGuid);

            // Act
            var resultado = await _controller.Upload(arquivoMock.Object, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(arquivoMock.Object, Dominio.TipoArquivo.EncaminhamentoNAAPA), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoGuid, okResult.Value);
        }

        [Fact(DisplayName = "Deve retornar BadRequest quando arquivo de upload for vazio")]
        public async Task DeveRetornarBadRequest_QuandoArquivoUploadVazio()
        {
            // Arrange
            var useCaseMock = new Mock<IUploadDeArquivoUseCase>();
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(a => a.Length).Returns(0);

            // Act
            var resultado = await _controller.Upload(arquivoMock.Object, useCaseMock.Object);

            // Assert
            Assert.IsType<BadRequestResult>(resultado);
            useCaseMock.Verify(u => u.Executar(It.IsAny<IFormFile>(), It.IsAny<Dominio.TipoArquivo>()), Times.Never);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para excluir um encaminhamento")]
        public async Task DeveChamarUseCase_ParaExcluirEncaminhamento()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirAtendimentoNAAPAUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);

            useCaseMock.Setup(u => u.Executar(encaminhamentoId)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ExcluirAtendimento(encaminhamentoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter um encaminhamento por ID")]
        public async Task DeveChamarUseCase_ParaObterEncaminhamentoPorId()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAtendimentoNAAPAPorIdUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);
            var retornoDto = new AtendimentoNAAPARespostaDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(encaminhamentoId)).ReturnsAsync(retornoDto);

            // Act
            var resultado = await _controller.ObterAtendimento(encaminhamentoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retornoDto, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter seções do encaminhamento")]
        public async Task DeveChamarUseCase_ParaObterSecoesDeEncaminhamento()
        {
            // Arrange
            var useCaseMock = new Mock<IObterSecoesAtendimentoSecaoNAAPAUseCase>();
            var filtro = new FiltroSecoesAtendimentoNAAPA { Modalidade = (int)Dominio.Modalidade.Fundamental };
            var retorno = new List<SecaoQuestionarioDto>();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterSecoesDeAtendimentoNAAPA(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter seções de itinerância")]
        public async Task DeveChamarUseCase_ParaObterSecoesItinerancia()
        {
            // Arrange
            var useCaseMock = new Mock<IObterSecoesItineranciaDeAtendimentoNAAPAUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);
            var retorno = new PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>();

            useCaseMock.Setup(u => u.Executar(encaminhamentoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterSecoesItineranciaDeAtendimentoNAAPA(encaminhamentoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter prioridades")]
        public async Task DeveChamarUseCase_ParaObterPrioridades()
        {
            // Arrange
            var useCaseMock = new Mock<IObterPrioridadeAtendimentoNAAPAUseCase>();
            var retorno = new List<PrioridadeAtendimentoNAAPADto>();

            useCaseMock.Setup(u => u.Executar()).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPrioridades(useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para excluir arquivo")]
        public async Task DeveChamarUseCase_ParaExcluirArquivo()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirArquivoNAAPAUseCase>();
            var arquivoCodigo = Guid.NewGuid();

            useCaseMock.Setup(u => u.Executar(arquivoCodigo)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ExcluirArquivo(arquivoCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(arquivoCodigo), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter questionário da itinerância")]
        public async Task DeveChamarUseCase_ParaObterQuestionarioItinerario()
        {
            // Arrange
            var useCaseMock = new Mock<IObterQuestionarioItinerarioAtendimentoNAAPAUseCase>();
            var questionarioId = _faker.Random.Long(1);
            var secaoId = _faker.Random.Long(1);
            var retorno = new AtendimentoNAAPASecaoItineranciaQuestoesDto();

            useCaseMock.Setup(u => u.Executar(questionarioId, secaoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterQuestionarioItinerario(questionarioId, secaoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(questionarioId, secaoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para excluir seção de itinerância")]
        public async Task DeveChamarUseCase_ParaExcluirSecaoItinerancia()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirSecaoItineranciaAtendimentoNAAPAUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);
            var secaoId = _faker.Random.Long(1);

            useCaseMock.Setup(u => u.Executar(encaminhamentoId, secaoId)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ExcluirSecaoItinerancia(encaminhamentoId, secaoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId, secaoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para registrar itinerário")]
        public async Task DeveChamarUseCase_ParaRegistrarItinerario()
        {
            // Arrange
            var useCaseMock = new Mock<IRegistrarAtendimentoItinerarioNAAPAUseCase>();
            var itinerarioDto = new AtendimentoNAAPAItineranciaDto();

            useCaseMock.Setup(u => u.Executar(itinerarioDto)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.RegistrarAtendimentoItinerario(itinerarioDto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(itinerarioDto), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter a situação do encaminhamento")]
        public async Task DeveChamarUseCase_ParaObterSituacao()
        {
            // Arrange
            var useCaseMock = new Mock<IObterSituacaoAtendimentoNAAPAUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);
            var retorno = new SituacaoDto();

            useCaseMock.Setup(u => u.Executar(encaminhamentoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterSituacao(encaminhamentoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para encerrar o encaminhamento")]
        public async Task DeveChamarUseCase_ParaEncerrarEncaminhamento()
        {
            // Arrange
            var useCaseMock = new Mock<IEncerrarAtendimentoNAAPAUseCase>();
            var parametros = new EncerramentoAtendimentoDto
            {
                EncaminhamentoId = _faker.Random.Long(1),
                MotivoEncerramento = _faker.Lorem.Sentence()
            };

            useCaseMock.Setup(u => u.Executar(parametros.EncaminhamentoId, parametros.MotivoEncerramento)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.EncerrarAtendimento(parametros, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(parametros.EncaminhamentoId, parametros.MotivoEncerramento), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para reabrir o encaminhamento")]
        public async Task DeveChamarUseCase_ParaReabrirEncaminhamento()
        {
            // Arrange
            var useCaseMock = new Mock<IReabrirAtendimentoNAAPAUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);
            var retorno = new SituacaoDto();

            useCaseMock.Setup(u => u.Executar(encaminhamentoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ReabrirAtendimento(encaminhamentoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter fluxos de alerta")]
        public async Task DeveChamarUseCase_ParaObterFluxosAlerta()
        {
            // Arrange
            var useCaseMock = new Mock<IObterOpcoesRespostaFluxoAlertaAtendimentosNAAPAUseCase>();
            var retorno = new List<OpcaoRespostaSimplesDto>();

            useCaseMock.Setup(u => u.Executar()).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterFluxosAlerta(useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter portas de entrada")]
        public async Task DeveChamarUseCase_ParaObterPortasEntrada()
        {
            // Arrange
            var useCaseMock = new Mock<IObterOpcoesRespostaPortaEntradaAtendimentosNAAPAUseCase>();
            var retorno = new List<OpcaoRespostaSimplesDto>();

            useCaseMock.Setup(u => u.Executar()).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPortasEntrada(useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para imprimir relatório detalhado")]
        public async Task DeveChamarUseCase_ParaImprimirDetalhado()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAtendimentoNaapaDetalhadoUseCase>();
            var filtro = new FiltroRelatorioAtendimentoNaapaDetalhadoDto();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ImprimirDetalhado(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter observações")]
        public async Task DeveChamarUseCase_ParaObterObservacoes()
        {
            // Arrange
            var useCaseMock = new Mock<IObterObservacoesDeAtendimentoNAAPAUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);
            var retorno = new PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>();

            useCaseMock.Setup(u => u.Executar(encaminhamentoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterObservacoes(encaminhamentoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para salvar observação")]
        public async Task DeveChamarUseCase_ParaSalvarObservacao()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarObservacoesDeAtendimentoNAAPAUseCase>();
            var filtro = new AtendimentoNAAPAObservacaoSalvarDto();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.SalvarObservacao(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para excluir observação")]
        public async Task DeveChamarUseCase_ParaExcluirObservacao()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirObservacoesDeAtendimentoNAAPAUseCase>();
            var observacaoId = _faker.Random.Long(1);

            useCaseMock.Setup(u => u.Executar(observacaoId)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ExcluirObservacao(observacaoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(observacaoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter histórico de alterações")]
        public async Task DeveChamarUseCase_ParaObterHistoricoDeAlteracoes()
        {
            // Arrange
            var useCaseMock = new Mock<IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);
            var retorno = new PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>();

            useCaseMock.Setup(u => u.Executar(encaminhamentoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterHistoricoDeAlteracoes(encaminhamentoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para verificar encaminhamento ativo do aluno")]
        public async Task DeveChamarUseCase_ParaVerificarEncaminhamentoAtivo()
        {
            // Arrange
            var useCaseMock = new Mock<IExisteAtendimentoNAAPAAtivoParaAlunoUseCase>();
            var alunoCodigo = _faker.Random.AlphaNumeric(8);

            useCaseMock.Setup(u => u.Executar(alunoCodigo)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ExisteAtendimentoAtivoParaAluno(alunoCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(alunoCodigo), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para excluir arquivo de itinerância")]
        public async Task DeveChamarUseCase_ParaExcluirArquivoItinerancia()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirArquivoItineranciaNAAPAUseCase>();
            var arquivoCodigo = Guid.NewGuid();

            useCaseMock.Setup(u => u.Executar(arquivoCodigo)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ExcluirArquivoItinerancia(arquivoCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(arquivoCodigo), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para fazer upload de arquivo de itinerância")]
        public async Task DeveChamarUseCase_ParaUploadDeArquivoItinerancia()
        {
            // Arrange
            var useCaseMock = new Mock<IUploadDeArquivoUseCase>();
            var retornoGuid = Guid.NewGuid();
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(a => a.Length).Returns(100);

            useCaseMock.Setup(u => u.Executar(arquivoMock.Object, Dominio.TipoArquivo.ItineranciaEncaminhamentoNAAPA)).ReturnsAsync(retornoGuid);

            // Act
            var resultado = await _controller.UploadItinerancia(arquivoMock.Object, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(arquivoMock.Object, Dominio.TipoArquivo.ItineranciaEncaminhamentoNAAPA), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoGuid, okResult.Value);
        }

        [Fact(DisplayName = "Deve retornar BadRequest quando arquivo de upload de itinerância for vazio")]
        public async Task DeveRetornarBadRequest_QuandoArquivoDeUploadItineranciaForVazio()
        {
            // Arrange
            var useCaseMock = new Mock<IUploadDeArquivoUseCase>();
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(a => a.Length).Returns(0);

            // Act
            var resultado = await _controller.UploadItinerancia(arquivoMock.Object, useCaseMock.Object);

            // Assert
            Assert.IsType<BadRequestResult>(resultado);
            useCaseMock.Verify(u => u.Executar(It.IsAny<IFormFile>(), It.IsAny<Dominio.TipoArquivo>()), Times.Never);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter registros de ação para o aluno")]
        public async Task DeveChamarUseCase_ParaObterRegistrosDeAcao()
        {
            // Arrange
            var useCaseMock = new Mock<IObterRegistrosDeAcaoParaNAAPAUseCase>();
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var retorno = new PaginacaoResultadoDto<RegistroAcaoBuscaAtivaNAAPADto>();

            useCaseMock.Setup(u => u.Executar(alunoCodigo)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterRegistrosDeAcaoParaAluno(alunoCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(alunoCodigo), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter tipos de anexos para impressão")]
        public async Task DeveChamarUseCase_ParaObterTiposDeImprimirAnexos()
        {
            // Arrange
            var useCaseMock = new Mock<IObterTiposDeImprimirAnexosNAAPAUseCase>();
            var encaminhamentoId = _faker.Random.Long(1);
            var retorno = new List<ImprimirAnexoDto>();

            useCaseMock.Setup(u => u.Executar(encaminhamentoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterTiposDeImprimirAnexos(encaminhamentoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(encaminhamentoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter profissionais envolvidos")]
        public async Task DeveChamarUseCase_ParaObterProfissionaisEnvolvidos()
        {
            // Arrange
            var useCaseMock = new Mock<IObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase>();
            var filtro = new FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA();
            var retorno = new List<FuncionarioUnidadeDto>();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterProfissionaisEnvolvidosAtendimento(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }
    }
}