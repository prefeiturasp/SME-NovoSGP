using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Xunit;
using System.Linq;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterListaFrequenciaAulasQueryHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterListaFrequenciaAulasQueryHandler _handler;
        private readonly Faker _faker;

        public ObterListaFrequenciaAulasQueryHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _handler = new ObterListaFrequenciaAulasQueryHandler(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando o mediator for nulo")]
        public void DeveLancarExcecao_QuandoMediatorNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObterListaFrequenciaAulasQueryHandler(null));
        }

        [Fact(DisplayName = "Deve montar o registro de frequência completo quando registra frequência")]
        public async Task DeveMontarRegistroFrequenciaCompleto_QuandoRegistraFrequencia()
        {
            // Arrange
            var query = CriarQueryFalsa(registraFrequencia: true);
            var usuario = new Usuario { CodigoRf = "RF123" };
            usuario.GetType().GetProperty("Perfis").SetValue(usuario, new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PROFESSOR } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMarcadorFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MarcadorFrequenciaDto());

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado.Aulas);
            Assert.Single(resultado.Alunos);

            var alunoResultado = Assert.Single(resultado.Alunos);
            Assert.True(alunoResultado.EhAtendidoAEE);
            Assert.False(alunoResultado.EhMatriculadoTurmaPAP);
            Assert.NotNull(alunoResultado.IndicativoFrequencia);
            Assert.Single(alunoResultado.Aulas);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterMarcadorFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve carregar apenas anotações quando não registra frequência")]
        public async Task DeveCarregarApenasAnotacoes_QuandoNaoRegistraFrequencia()
        {
            // Arrange
            var query = CriarQueryFalsa(registraFrequencia: false);
            var usuario = new Usuario { CodigoRf = "RF123" };
            usuario.GetType().GetProperty("Perfis").SetValue(usuario, new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PROFESSOR } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMarcadorFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MarcadorFrequenciaDto());

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            var alunoResultado = Assert.Single(resultado.Alunos);
            Assert.Single(alunoResultado.Aulas);
            Assert.True(alunoResultado.Aulas.First().PossuiAnotacao);
        }

        [Fact(DisplayName = "Deve retornar nulo quando não houver aulas na requisição")]
        public async Task DeveRetornarNulo_QuandoNaoHouverAulas()
        {
            // Arrange
            var query = CriarQueryFalsa(comAulas: false);
            var usuario = new Usuario { CodigoRf = "RF123" };
            usuario.GetType().GetProperty("Perfis").SetValue(usuario, new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PROFESSOR } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(resultado);
        }

        private ObterListaFrequenciaAulasQuery CriarQueryFalsa(bool registraFrequencia = true, bool comAulas = true)
        {
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var turmaCodigo = _faker.Random.Number(1000, 9999).ToString();
            var aulaId = _faker.Random.Long(1);

            var filtro = new FiltroFrequenciaAulasDto
            {
                Turma = new Turma { CodigoTurma = turmaCodigo, AnoLetivo = DateTime.Now.Year, ModalidadeCodigo = Modalidade.Fundamental },
                AlunosDaTurma = new List<AlunoPorTurmaResposta>
                {
                    new AlunoPorTurmaResposta { CodigoAluno = alunoCodigo, NomeAluno = _faker.Name.FullName() }
                },
                Aulas = comAulas ? new List<Aula>
                {
                    new Aula { Id = aulaId, TurmaId = turmaCodigo, DataAula = DateTime.Now }
                } : new List<Aula>(),
                FrequenciaAlunos = new List<FrequenciaAluno>
                {
                    new FrequenciaAluno { CodigoAluno = alunoCodigo, TotalAulas = 10, TotalAusencias = 2 }
                },
                RegistrosFrequenciaAlunos = new List<RegistroFrequenciaAlunoPorAulaDto>
                {
                    new RegistroFrequenciaAlunoPorAulaDto { AlunoCodigo = alunoCodigo, AulaId = aulaId, NumeroAula = 1, TipoFrequencia = TipoFrequencia.F }
                },
                AnotacoesTurma = new List<AnotacaoAlunoAulaDto>
                {
                    new AnotacaoAlunoAulaDto { AlunoCodigo = alunoCodigo, AulaId = aulaId }
                },
                CompensacaoAusenciaAlunoAulas = new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>(),
                FrequenciasPreDefinidas = new List<FrequenciaPreDefinidaDto>(),
                RegistraFrequencia = registraFrequencia
            };

            return new ObterListaFrequenciaAulasQuery(filtro);
        }
    }
}