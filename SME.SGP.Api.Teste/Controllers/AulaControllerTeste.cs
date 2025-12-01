using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AulaControllerTeste
    {
        private readonly AulaController _controller;
        private readonly Mock<IAlterarAulaUseCase> _alterarAulaUseCase = new();
        private readonly Mock<IObterAulaPorIdUseCase> _obterAulaPorIdUseCase = new();
        private readonly Mock<IExcluirAulaUseCase> _excluirAulaUseCase = new();
        private readonly Mock<IInserirAulaUseCase> _inserirAulaUseCase = new();
        private readonly Mock<IConsultasAula> _consultasAula = new();
        private readonly Mock<IObterFrequenciaOuPlanoNaRecorrenciaUseCase> _obterFrequenciaOuPlanoNaRecorrenciaUseCase = new();
        private readonly Mock<IPodeCadastrarAulaUseCase> _podeCadastrarAulaUseCase = new();
        private readonly Mock<IMediator> _mediator = new();
        private readonly Faker _faker;

        public AulaControllerTeste()
        {
            _controller = new AulaController();
            _faker = new Faker("pt_BR");
        }

        #region AlterarAula
        [Fact(DisplayName = "Deve chamar o caso de uso que altera aula")]
        public async Task DeveRetornarOk_QuandoAlterarAulaComDadosValidos()
        {
            // Arrange
            var id = _faker.Random.Long(1);
            var dto = CriarDto();
            var retornoEsperado = new RetornoBaseDto("Aula alterada com sucesso");

            _alterarAulaUseCase
                .Setup(x => x.Executar(It.IsAny<PersistirAulaDto>()))
                .ReturnsAsync(retornoEsperado);
            // Act
            var resultado = await _controller.Alterar(dto, id, _alterarAulaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<RetornoBaseDto>(okResult.Value);
            Assert.Equal("Aula alterada com sucesso", valorRetornado.Mensagens[0]);
        }
        #endregion

        #region BuscarAulaPorID
        [Fact(DisplayName = "Deve chamar o caso de uso que busca aula por ID")]
        public async Task DeveRetornarOk_QuandoBuscarAulaPorId()
        {
            // Arrange
            var id = 123L;
            var aulaEsperada = new AulaConsultaDto
            {
                Id = 1,
                ProfessorRf = "123456",
                DataAula = DateTime.Today,
                TurmaId = "TURMA01"
            };

            _obterAulaPorIdUseCase.Setup(x => x.Executar(It.IsAny<long>())).ReturnsAsync(aulaEsperada);

            // Act
            var resultado = await _obterAulaPorIdUseCase.Object.Executar(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
        }
        #endregion

        #region ExcluirRecorrenciaAula
        [Fact(DisplayName = "Deve chamar o caso de uso que exclui a recorrência de aula")]
        public async Task Excluir_DeveRetornarOk_ComResultadoEsperado()
        {
            // Arrange
            long aulaId = 123;
            var recorrencia = RecorrenciaAula.AulaUnica;

            var retornoEsperado = new RetornoBaseDto("Recorrência de aula excluída com sucesso");

            _excluirAulaUseCase
                .Setup(x => x.Executar(It.Is<ExcluirAulaDto>(dto =>
                    dto.AulaId == aulaId &&
                    dto.RecorrenciaAula == recorrencia)))
                .ReturnsAsync(retornoEsperado);

            var controller = new AulaController();
            controller.ControllerContext = new ControllerContext();

            // Act
            var resultado = await controller.Excluir(aulaId, recorrencia, _excluirAulaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(retornoEsperado, okResult.Value);
        }
        #endregion

        #region InserirAula
        [Fact(DisplayName = "Deve chamar o caso de uso que inserir aula")]
        public async Task DeveRetornarOk_QuandoInserirAulaValida()
        {
            // Arrange
            var dto = CriarDto();

            var retornoEsperado = new RetornoBaseDto("Aula inserida com sucesso!");

            _inserirAulaUseCase
                .Setup(x => x.Executar(dto))
                .ReturnsAsync(retornoEsperado);

            _inserirAulaUseCase.Setup(x => x.Executar(dto)).ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.Inserir(dto, _inserirAulaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(retornoEsperado, okResult.Value);
        }
        #endregion

        #region BuscarRecorrenciaDaSerie
        [Fact(DisplayName = "Deve chamar o caso de uso que busca a recorrencia da serie")]
        public async Task DeveRetornar200_QuandoObterRecorrenciaDaSerieComSucesso()
        {
            // Arrange
            var aulaId = 1L;
            var recorrenciaEsperada = (int)RecorrenciaAula.RepetirTodosBimestres;
            var quantidadeAulasEsperada = 5;
            var existeFrequenciaOuPlano = true;

            _consultasAula.Setup(c => c.ObterRecorrenciaDaSerie(aulaId))
                         .Returns(recorrenciaEsperada);

            _consultasAula.Setup(c => c.ObterQuantidadeAulasRecorrentes(aulaId, RecorrenciaAula.RepetirTodosBimestres))
                         .ReturnsAsync(quantidadeAulasEsperada);

            _obterFrequenciaOuPlanoNaRecorrenciaUseCase.Setup(o => o.Executar(aulaId))
                                      .ReturnsAsync(existeFrequenciaOuPlano);

            var controller = new AulaController();

            // Act
            var resultado = await controller.ObterRecorrenciaDaSerie(aulaId, _consultasAula.Object, _obterFrequenciaOuPlanoNaRecorrenciaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var retorno = Assert.IsType<AulaRecorrenciaDto>(okResult.Value);
            Assert.Equal(aulaId, retorno.AulaId);
            Assert.Equal(recorrenciaEsperada, retorno.RecorrenciaAula);
            Assert.Equal(quantidadeAulasEsperada, retorno.QuantidadeAulasRecorrentes);
            Assert.True(retorno.ExisteFrequenciaOuPlanoAula);
        }
        #endregion

        #region ValidaSePoderCadastrarAula

        [Fact(DisplayName = "Deve chamar o caso de uso que valida se pode cadastrar aula")]
        public async Task DeveRetornarOk_QuandoPodeCadastrarAulaUseCaseRetornarSucesso()
        {
            // Arrange
            var aulaId = 123L;
            var turmaCodigo = "TURMA01";
            var componenteCurricular = 456L;
            var dataAula = DateTime.Now.Date;
            var ehRegencia = true;
            var tipoAula = TipoAula.Normal;

            var esperado = new CadastroAulaDto
            {
                PodeCadastrarAula = _faker.Random.Bool(),
                Grade = new GradeComponenteTurmaAulasDto
                {
                    QuantidadeAulasGrade = _faker.Random.Int(1),
                    QuantidadeAulasRestante = _faker.Random.Int(1),
                    PodeEditar = _faker.Random.Bool()
                }
            };

            _podeCadastrarAulaUseCase
                .Setup(x => x.Executar(It.Is<FiltroPodeCadastrarAulaDto>(f =>
                    f.AulaId == aulaId &&
                    f.TurmaCodigo == turmaCodigo &&
                    f.ComponentesCurriculares.Length == 1 &&
                    f.ComponentesCurriculares[0] == componenteCurricular &&
                    f.DataAula == dataAula &&
                    f.EhRegencia == ehRegencia &&
                    f.TipoAula == tipoAula)))
                .ReturnsAsync(esperado);

            var controller = new AulaController();

            // Act
            var resultado = await controller.CadastroAula(_podeCadastrarAulaUseCase.Object, aulaId, turmaCodigo, componenteCurricular, dataAula, ehRegencia, tipoAula);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<CadastroAulaDto>(okResult.Value);
            Assert.Equal(esperado, retorno);
        }
        #endregion

        #region SincronizarAulasRegencia
        [Fact(DisplayName = "Deve chamar o caso de uso que sincroniza regencia de aula")]
        public async Task DeveRetornarOk_QuandoSincronizarAulasRegencia()
        {
            // Arrange
            PublicarFilaSgpCommand comandoCapturado = null;

            _mediator
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default))
                .Callback<IRequest<bool>, CancellationToken>((cmd, _) => comandoCapturado = (PublicarFilaSgpCommand)cmd)
                .ReturnsAsync(true);

            var controller = new AulaController(); // ajuste necessário se usar injeção
            var codigoTurma = 123L;

            // Act
            var resultado = await controller.SincronizarAulasRegencia(codigoTurma, _mediator.Object);

            // Assert
            Assert.IsType<OkResult>(resultado);
        }
        #endregion


        private PersistirAulaDto CriarDto()
        {
            return new PersistirAulaDto
            {
                Id = _faker.Random.Int(1),
                DataAula = DateTime.Now,
                CodigoComponenteCurricular = _faker.Random.Long(1),
                NomeComponenteCurricular = _faker.Name.FullName(),
                DisciplinaCompartilhadaId = _faker.Random.Long(1),
                Quantidade = _faker.Random.Int(1),
                RecorrenciaAula = Dominio.RecorrenciaAula.AulaUnica,
                TipoAula = Dominio.TipoAula.Normal,
                TipoCalendarioId = _faker.Random.Long(1),
                CodigoTurma = _faker.Random.AlphaNumeric(6),
                CodigoUe = _faker.Random.AlphaNumeric(6),
                AulaCJ = _faker.Random.Bool(),
                EhRegencia = _faker.Random.Bool(),
            };
        }
    }
}
