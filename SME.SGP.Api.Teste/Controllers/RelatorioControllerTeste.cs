using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Api.Testes.Controllers
{
    public class RelatorioControllerTeste
    {
        private readonly RelatorioController _controller;
        private readonly Faker _faker;

        public RelatorioControllerTeste()
        {
            _controller = new RelatorioController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve realizar o download do relatório com sucesso")]
        public async Task DeveRealizarDownloadRelatorio_ComSucesso()
        {
            // Arrange
            var downloadUseCaseMock = new Mock<IReceberDadosDownloadRelatorioUseCase>();
            var servicoJasperMock = new Mock<ISevicoJasper>();
            var codigoCorrelacao = Guid.NewGuid();

            var relatorioBytes = Encoding.UTF8.GetBytes("Este é um relatório de teste.");
            var contentType = "application/pdf";
            var nomeArquivo = "Relatorio.pdf";

            downloadUseCaseMock.Setup(u => u.Executar(codigoCorrelacao))
                               .ReturnsAsync((relatorioBytes, contentType, nomeArquivo));

            // Act
            var resultado = await _controller.Download(codigoCorrelacao, downloadUseCaseMock.Object, servicoJasperMock.Object);

            // Assert
            downloadUseCaseMock.Verify(u => u.Executar(codigoCorrelacao), Times.Once);

            var fileResult = Assert.IsType<FileContentResult>(resultado);
            Assert.Equal(contentType, fileResult.ContentType);
            Assert.Equal(nomeArquivo, fileResult.FileDownloadName);
            Assert.Equal(relatorioBytes, fileResult.FileContents);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Ata Final")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAtaFinal()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioConselhoClasseAtaFinalUseCase>();
            var filtro = new FiltroRelatorioConselhoClasseAtaFinalDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ConselhoClasseAtaFinal(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Frequência")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioFrequencia()
        {
            // Arrange
            var useCaseMock = new Mock<IGerarRelatorioFrequenciaUseCase>();
            var filtro = new FiltroRelatorioFrequenciaDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Frequencia(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Pendências")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioPendencias()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioPendenciasUseCase>();
            var filtro = new FiltroRelatorioPendenciasDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Gerar(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve lançar exceção ao gerar relatório de alteração de notas para Educação Infantil")]
        public async Task DeveLancarExcecao_AoGerarRelatorioAlteracaoNotas_ParaInfantil()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAlteracaoNotasUseCase>();
            var filtro = new FiltroRelatorioAlteracaoNotas { ModalidadeTurma = Modalidade.EducacaoInfantil };

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _controller.AlteracaoNotas(filtro, useCaseMock.Object));
            useCaseMock.Verify(u => u.Executar(It.IsAny<FiltroRelatorioAlteracaoNotas>()), Times.Never);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de alteração de notas para outras modalidades")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAlteracaoNotas()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAlteracaoNotasUseCase>();
            var filtro = new FiltroRelatorioAlteracaoNotas { ModalidadeTurma = Modalidade.Fundamental };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.AlteracaoNotas(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Plano de Aula")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioPlanoAula()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioPlanoAulaUseCase>();
            var filtro = new FiltroRelatorioPlanoAulaDto { PlanoAulaId = _faker.Random.Long(1) };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.PlanoAula(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Controle de Grade")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioControleGrade()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioControleGradeUseCase>();
            var filtro = new FiltroRelatorioControleGrade { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ControleGrade(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Notificações")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioNotificacoes()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioNotificacaoUseCase>();
            var filtro = new FiltroRelatorioNotificacao { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Notificacoes(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Usuários")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioUsuarios()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioUsuariosUseCase>();
            var filtro = new FiltroRelatorioUsuarios { DiasSemAcesso = 10 };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Usuarios(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Atribuição de CJ")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAtribuicaoCJ()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAtribuicaoCJUseCase>();
            var filtro = new FiltroRelatorioAtribuicaoCJDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Gerar(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Adesão ao App")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAdesaoApp()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAEAdesaoUseCase>();
            var filtro = new FiltroRelatorioAEAdesaoDto { DreCodigo = _faker.Random.AlphaNumeric(6) };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.AdesaoApp(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Leitura de Comunicados")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioLeituraComunicados()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioLeituraComunicadosUseCase>();
            var filtro = new FiltroRelatorioLeituraComunicados { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.LeituraComunicados(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Planejamento Diário")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioPlanejamentoDiario()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioPlanejamentoDiarioUseCase>();
            var filtro = new FiltroRelatorioPlanejamentoDiario { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.PlanejamentoDiario(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Devolutivas")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioDevolutivas()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioDevolutivasUseCase>();
            var filtro = new FiltroRelatorioDevolutivas { Ano = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Devolutivas(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Itinerâncias")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioItinerancias()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioItineranciasUseCase>();
            var itinerancias = new long[] { _faker.Random.Long(1), _faker.Random.Long(2) };

            useCaseMock.Setup(u => u.Executar(itinerancias)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Itinerancias(itinerancias, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(itinerancias), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Registro Individual")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioRegistroIndividual()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioRegistroIndividualUseCase>();
            var filtro = new FiltroRelatorioRegistroIndividualDto { TurmaId = _faker.Random.Long(1) };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.RegistroIndividual(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Acompanhamento da Aprendizagem")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAcompanhamentoAprendizagem()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAcompanhamentoAprendizagemUseCase>();
            var filtro = new FiltroRelatorioAcompanhamentoAprendizagemDto { TurmaId = _faker.Random.Long(1) };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.AcompanhamentoAprendizagem(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Acompanhamento de Fechamento")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAcompanhamentoFechamento()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAcompanhamentoFechamentoUseCase>();
            var filtro = new FiltroRelatorioAcompanhamentoFechamentoDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.AcompanhamentoFechamento(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para listar os tipos de pendências")]
        public void DeveChamarUseCase_ParaListarTiposPendencia()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioPendenciasUseCase>();
            var retorno = new List<FiltroTipoPendenciaDto>();

            useCaseMock.Setup(u => u.ListarTodosTipos(true)).Returns(retorno);

            // Act
            var resultado = _controller.ObterTipoPendencias(true, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.ListarTodosTipos(true), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Ata Bimestral")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAtaBimestral()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAtaBimestralUseCase>();
            var filtro = new FiltroRelatorioAtaBimestralDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Gerar(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Acompanhamento de Registros Pedagógicos")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAcompanhamentoRegistrosPedagogicos()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAcompanhamentoRegistrosPedagogicosUseCase>();
            var filtro = new FiltroRelatorioAcompanhamentoRegistrosPedagogicosDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.AcompanhamentoRegistrosPedagogicos(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Acompanhamento de Frequência")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioAcompanhamentoFrequencia()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAcompanhamentoDeFrequênciaUseCase>();
            var filtro = new FiltroAcompanhamentoFrequenciaJustificativaDto { Bimestre = 1 };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ImprimirAcompanhamentoFrequencia(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Ocorrências")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioOcorrencias()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioOcorrenciasUseCase>();
            var filtro = new FiltroImpressaoOcorrenciaDto { TurmaId = _faker.Random.Long(1) };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ImprimirRelatorioOcorrencias(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Planos AEE")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioPlanosAee()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioPlanosAEEUseCase>();
            var filtro = new FiltroRelatorioPlanosAEEDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.PlanosAee(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Encaminhamento AEE")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioEncaminhamentoAee()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioEncaminhamentoAEEUseCase>();
            var filtro = new FiltroRelatorioEncaminhamentoAEEDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.EncaminhamentoAee(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de Encaminhamento NAAPA")]
        public async Task DeveChamarUseCase_ParaGerarRelatorioEncaminhamentoNAAPA()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioEncaminhamentoNAAPAUseCase>();
            var filtro = new FiltroRelatorioEncaminhamentoNAAPADto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.EncaminhamentoNAAPA(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório analítico de sondagem")]
        public async Task DeveChamarCasoDeUso_ParaGerarRelatorioAnaliticoSondagem()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioAnaliticoSondagemUseCase>();
            var filtro = new FiltroRelatorioAnaliticoSondagemDto { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Gerar(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve retornar os tipos de sondagem")]
        public void DeveRetornar_TiposDeSondagem()
        {
            // Arrange
            var totalTiposSondagem = Enum.GetNames(typeof(TipoSondagem)).Length;

            // Act
            var resultado = _controller.ObterTipoSondagem();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsAssignableFrom<IEnumerable<EnumeradoRetornoDto>>(okResult.Value);
            Assert.Equal(totalTiposSondagem, lista.Count());
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de listagem de itinerâncias")]
        public async Task DeveChamarCasoDeUso_ParaGerarRelatorioListagemItinerancias()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioListagemItineranciasUseCase>();
            var filtro = new FiltroRelatorioListagemItineranciasDto() { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ListarItinerancias(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de controle de frequência mensal")]
        public async Task DeveChamarCasoDeUso_ParaGerarRelatorioControleFrequenciaMensal()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioControleFrequenciaMensalUseCase>();
            var filtro = new FiltroRelatorioControleFrenquenciaMensalDto() { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Gerar(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de listagem de ocorrências")]
        public async Task DeveChamarCasoDeUso_ParaGerarRelatorioListagemOcorrencias()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioListagemOcorrenciasUseCase>();
            var filtro = new FiltroRelatorioListagemOcorrenciasDto() { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ListagemOcorrencias(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de plano anual")]
        public async Task DeveChamarCasoDeUso_ParaGerarRelatorioPlanoAnual()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioPlanoAnualUseCase>();
            var filtro = new FiltroRelatorioPlanoAnualDto() { Id = _faker.Random.Long(1) };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.PlanoAnual(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de mapeamento de estudantes")]
        public async Task DeveChamarCasoDeUso_ParaGerarRelatorioMapeamentoEstudantes()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioMapeamentoEstudantesUseCase>();
            var filtro = new FiltroRelatorioMapeamentoEstudantesDto() { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.MapeamentoEstudante(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de busca ativa")]
        public async Task DeveChamarCasoDeUso_ParaGerarRelatorioBuscaAtiva()
        {
            // Arrange
            var useCaseMock = new Mock<IRelatorioBuscasAtivasUseCase>();
            var filtro = new FiltroRelatorioBuscasAtivasDto() { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.BuscaAtiva(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para gerar o relatório de produtividade de frequência")]
        public async Task DeveChamarCasoDeUso_ParaGerarRelatorioProdutividadeFrequencia()
        {
            // Arrange
            var useCaseMock = new Mock<IGerarRelatorioProdutividadeFrequenciaUseCase>();
            var filtro = new FiltroRelatorioProdutividadeFrequenciaDto() { AnoLetivo = DateTime.Now.Year };

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ProdutividadeFrequencia(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }
    }
}