using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Questionario;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class PlanoAEEControllerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PlanoAEEController _controller;
        private readonly Faker _faker;

        public PlanoAEEControllerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PlanoAEEController(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando o mediator for nulo")]
        public void DeveLancarExcecao_QuandoMediatorNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PlanoAEEController(null));
        }

        [Fact(DisplayName = "Deve retornar a lista de situações do plano AEE")]
        public void DeveRetornarListaDeSituacoesPlanoAEE()
        {
            // Arrange
            var totalSituacoes = Enum.GetValues(typeof(SituacaoPlanoAEE)).Length;

            // Act
            var resultado = _controller.ObterSituacoes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaRetornada = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(totalSituacoes, listaRetornada.Count());
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter planos AEE paginados")]
        public async Task DeveChamarUseCase_ParaObterPlanosAEE()
        {
            // Arrange
            var useCaseMock = new Mock<IObterPlanosAEEUseCase>();
            var filtro = new FiltroPlanosAEEDto { AnoLetivo = _faker.Random.Int(2020, 2025) };
            var retornoPaginado = new PaginacaoResultadoDto<PlanoAEEResumoDto>();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(retornoPaginado);

            // Act
            var resultado = await _controller.ObterPlanosAEE(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retornoPaginado, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter plano AEE por estudante")]
        public async Task DeveChamarUseCase_ParaObterPlanoAEEPorEstudante()
        {
            // Arrange
            var useCaseMock = new Mock<IObterPlanoAEEPorCodigoEstudanteUseCase>();
            var codigoEstudante = _faker.Random.AlphaNumeric(8);
            var retorno = new PlanoAEEDto();

            useCaseMock.Setup(u => u.Executar(codigoEstudante)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPlanoAeeEstudante(codigoEstudante, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(codigoEstudante), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter plano AEE por ID")]
        public async Task DeveChamarUseCase_ParaObterPlanoAEEPorId()
        {
            // Arrange
            var useCaseMock = new Mock<IObterPlanoAEEPorIdUseCase>();
            var planoId = _faker.Random.Long(1);
            var alunoCodigo = _faker.Random.Long(1);
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var retorno = new PlanoAEEDto();

            useCaseMock.Setup(u => u.Executar(It.Is<FiltroPesquisaQuestoesPorPlanoAEEIdDto>(f => f.PlanoAEEId == planoId && f.CodigoAluno == alunoCodigo)))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPlanoAee(planoId, alunoCodigo, turmaCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<FiltroPesquisaQuestoesPorPlanoAEEIdDto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter plano AEE por versão")]
        public async Task DeveChamarUseCase_ParaObterPlanoAEEPorVersao()
        {
            // Arrange
            var useCaseMock = new Mock<IObterQuestoesPlanoAEEPorVersaoUseCase>();
            var versaoPlanoId = _faker.Random.Long(1);
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var questionarioId = _faker.Random.Long(1);
            var retorno = new List<QuestaoDto>();

            useCaseMock.Setup(u => u.Executar(It.Is<FiltroPesquisaQuestoesPlanoAEEDto>(f => f.VersaoPlanoId == versaoPlanoId && f.QuestionarioId == questionarioId)))
                       .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPlanoAeePorVersao(versaoPlanoId, turmaCodigo, questionarioId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<FiltroPesquisaQuestoesPlanoAEEDto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para salvar o plano AEE")]
        public async Task DeveChamarUseCase_ParaSalvarPlanoAEE()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarPlanoAEEUseCase>();
            var planoDto = new PlanoAEEPersistenciaDto();
            var retorno = new RetornoPlanoAEEDto(0,0);

            useCaseMock.Setup(u => u.Executar(planoDto)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.Salvar(planoDto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(planoDto), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para verificar existência de plano AEE")]
        public async Task DeveChamarUseCase_ParaVerificarExistenciaPlanoAEE()
        {
            // Arrange
            var useCaseMock = new Mock<IVerificarExistenciaPlanoAEEPorEstudanteUseCase>();
            var codigoEstudante = _faker.Random.AlphaNumeric(8);

            useCaseMock.Setup(u => u.Executar(codigoEstudante)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.VerificarExistenciaPlanoAEEPorEstudante(codigoEstudante, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(codigoEstudante), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter reestruturações do plano")]
        public async Task DeveChamarUseCase_ParaObterReestruturacoes()
        {
            // Arrange
            var useCaseMock = new Mock<IObterRestruturacoesPlanoAEEPorIdUseCase>();
            var planoId = _faker.Random.Long(1);
            var retorno = new List<PlanoAEEReestruturacaoDto>();

            useCaseMock.Setup(u => u.Executar(planoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterReestruturacoesPlanoAEE(planoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(planoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter versões do plano")]
        public async Task DeveChamarUseCase_ParaObterVersoes()
        {
            // Arrange
            var useCaseMock = new Mock<IObterVersoesPlanoAEEUseCase>();
            var planoId = _faker.Random.Long(1);
            var reestruturacaoId = _faker.Random.Long(1);
            var retorno = new List<PlanoAEEDescricaoVersaoDto>();

            useCaseMock.Setup(u => u.Executar(It.Is<FiltroVersoesPlanoAEEDto>(f => f.PlanoId == planoId && f.ReestruturacaoId == reestruturacaoId)))
                       .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterVersoes(planoId, reestruturacaoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<FiltroVersoesPlanoAEEDto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter parecer do plano")]
        public async Task DeveChamarUseCase_ParaObterParecer()
        {
            // Arrange
            var useCaseMock = new Mock<IObterParecerPlanoAEEPorIdUseCase>();
            var planoId = _faker.Random.Long(1);
            var retorno = new PlanoAEEParecerDto();

            useCaseMock.Setup(u => u.Executar(planoId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterParecerPlanoAEE(planoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(planoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para encerrar plano AEE")]
        public async Task DeveChamarUseCase_ParaEncerrarPlanoAEE()
        {
            // Arrange
            var useCaseMock = new Mock<IEncerrarPlanoAEEUseCase>();
            var planoAeeId = _faker.Random.Long(1);
            var retorno = new RetornoEncerramentoPlanoAEEDto(planoAeeId, SituacaoPlanoAEE.Encerrado);

            useCaseMock.Setup(u => u.Executar(planoAeeId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.EncerrarPlanoAEE(planoAeeId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(planoAeeId), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para cadastrar parecer do CP")]
        public async Task DeveChamarUseCase_ParaCadastrarParecerCP()
        {
            // Arrange
            var useCaseMock = new Mock<ICadastrarParecerCPPlanoAEEUseCase>();
            var planoId = _faker.Random.Long(1);
            var parecerDto = new PlanoAEECadastroParecerDto { Parecer = "Parecer teste" };

            useCaseMock.Setup(u => u.Executar(planoId, parecerDto)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.CadastrarParecerCPPlanoAEE(planoId, parecerDto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(planoId, parecerDto), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para cadastrar devolutiva do PAAI")]
        public async Task DeveChamarUseCase_ParaCadastrarDevolutivaPAAI()
        {
            // Arrange
            var useCaseMock = new Mock<ICadastrarParecerPAAIPlanoAEEUseCase>();
            var planoId = _faker.Random.Long(1);
            var parecerDto = new PlanoAEECadastroParecerDto { Parecer = "Parecer teste" };

            useCaseMock.Setup(u => u.Executar(planoId, parecerDto)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.CadastrarDevolutivaPAAIPlanoAEE(planoId, parecerDto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(planoId, parecerDto), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para atribuir responsável")]
        public async Task DeveChamarUseCase_ParaAtribuirResponsavel()
        {
            // Arrange
            var useCaseMock = new Mock<IAtribuirResponsavelPlanoAEEUseCase>();
            var parametros = new AtribuirResponsavelPlanoAEEDto
            {
                PlanoAEEId = _faker.Random.Long(1),
                ResponsavelRF = _faker.Random.AlphaNumeric(7)
            };

            useCaseMock.Setup(u => u.Executar(parametros.PlanoAEEId, parametros.ResponsavelRF)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.AtribuirResponsavelPlanoAEE(parametros, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(parametros.PlanoAEEId, parametros.ResponsavelRF), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para devolver o plano AEE")]
        public async Task DeveChamarUseCase_ParaDevolverPlanoAEE()
        {
            // Arrange
            var useCaseMock = new Mock<IDevolverPlanoAEEUseCase>();
            var devolucaoDto = new DevolucaoPlanoAEEDto { PlanoAEEId = 1, Motivo = "Motivo teste" };

            useCaseMock.Setup(u => u.Executar(devolucaoDto)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.DevolverPlanoAEE(devolucaoDto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(devolucaoDto), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<RetornoBaseDto>(okResult.Value);
            Assert.Equal("Plano devolvido com sucesso", retorno.Mensagens.FirstOrDefault());
        }

        [Fact(DisplayName = "Deve enviar comando para encerrar planos em lote")]
        public async Task DeveEnviarComando_ParaEncerrarPlanosEmLote()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpAEE.EncerrarPlanoAEEEstudantesInativos), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _controller.EncerrarPlanos();

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<OkResult>(resultado);
        }

        [Fact(DisplayName = "Deve enviar comando para expirar planos")]
        public async Task DeveEnviarComando_ParaExpirarPlanos()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpAEE.GerarPendenciaValidadePlanoAEE), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _controller.ExpirarPlanos();

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<OkResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para atribuir responsável geral")]
        public async Task DeveChamarUseCase_ParaAtribuirResponsavelGeral()
        {
            // Arrange
            var useCaseMock = new Mock<IAtribuirResponsavelGeralDoPlanoUseCase>();
            var parametros = new AtribuirResponsavelPlanoAEEDto
            {
                PlanoAEEId = _faker.Random.Long(1),
                ResponsavelRF = _faker.Random.AlphaNumeric(7),
                ResponsavelNome = _faker.Name.FullName()
            };

            useCaseMock.Setup(u => u.Executar(parametros.PlanoAEEId, parametros.ResponsavelRF, parametros.ResponsavelNome)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.AtribuirResponsavelGeralDoPlano(parametros, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(parametros.PlanoAEEId, parametros.ResponsavelRF, parametros.ResponsavelNome), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter PAAI por DRE")]
        public async Task DeveChamarUseCase_ParaObterPAAIPorDre()
        {
            // Arrange
            var useCaseMock = new Mock<IObterPAAIPorDreUseCase>();
            var codigoDre = _faker.Random.AlphaNumeric(6);
            var retorno = new List<SupervisorEscolasDreDto>();

            useCaseMock.Setup(u => u.Executar(codigoDre)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPAAI(codigoDre, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(codigoDre), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para excluir o plano AEE")]
        public async Task DeveChamarUseCase_ParaExcluirPlanoAEE()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirPlanoAEEUseCase>();
            var id = _faker.Random.Long(1);

            useCaseMock.Setup(u => u.Executar(id)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Excluir(id, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(id), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para remover responsável do plano AEE")]
        public async Task DeveChamarUseCase_ParaRemoverResponsavel()
        {
            // Arrange
            var useCaseMock = new Mock<IRemoverResponsavelPlanoAEEUseCase>();
            var planoId = _faker.Random.Long(1);

            useCaseMock.Setup(u => u.Executar(planoId)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.RemoverAtribuicaoResponsavelPlanoAee(planoId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(planoId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para imprimir o plano AEE")]
        public async Task DeveChamarUseCase_ParaImprimirPlanoAEE()
        {
            // Arrange
            var useCaseMock = new Mock<IImpressaoPlanoAeeUseCase>();
            var filtro = new FiltroRelatorioPlanoAeeDto();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ImprimirConselhoTurma(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter dados SRM PAEE Colaborativo")]
        public async Task DeveChamarUseCase_ParaObterDadosSrmPaeeColaborativo()
        {
            // Arrange
            var useCaseMock = new Mock<IObterSrmPaeeColaborativoUseCase>();
            var filtro = new FiltroSrmPaeeColaborativoDto();
            var retorno = new SrmPaeeColaborativoSgpDto();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(new List<SrmPaeeColaborativoSgpDto> { retorno });

            // Act
            var resultado = await _controller.ObterDadosSrmPaeeColaborativo(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter responsáveis do plano AEE")]
        public async Task DeveChamarUseCase_ParaObterResponsaveis()
        {
            // Arrange
            var useCaseMock = new Mock<IObterResponsaveisPlanosAEEUseCase>();
            var filtro = new FiltroPlanosAEEDto();
            var retorno = new List<UsuarioEolRetornoDto>();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterResponsaveis(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }
    }
}