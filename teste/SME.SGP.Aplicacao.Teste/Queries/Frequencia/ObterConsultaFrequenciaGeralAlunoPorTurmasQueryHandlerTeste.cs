using Bogus;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterConsultaFrequenciaGeralAlunoPorTurmasQueryHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaMock;
        private readonly ObterConsultaFrequenciaGeralAlunoPorTurmasQueryHandler _handler;
        private readonly Faker _faker;

        public ObterConsultaFrequenciaGeralAlunoPorTurmasQueryHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ObterConsultaFrequenciaGeralAlunoPorTurmasQueryHandler(_mediatorMock.Object, _repositorioFrequenciaMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando as dependências forem nulas")]
        public void DeveLancarExcecao_QuandoDependenciasForemNulas()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterConsultaFrequenciaGeralAlunoPorTurmasQueryHandler(null, _repositorioFrequenciaMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ObterConsultaFrequenciaGeralAlunoPorTurmasQueryHandler(_mediatorMock.Object, null));
        }

        [Fact(DisplayName = "Deve executar fluxo padrão para anos diferentes de 2020")]
        public async Task Handle_QuandoAnoDiferente2020_DeveExecutarFluxoPadrao()
        {
            // Arrange
            var turmaCodigo = _faker.Random.Number(10000, 99999).ToString();
            var alunoCodigo = _faker.Random.Number(100000, 999999).ToString();

            var turma = new Turma { AnoLetivo = 2021, CodigoTurma = turmaCodigo };
            var query = new ObterConsultaFrequenciaGeralAlunoPorTurmasQuery(alunoCodigo, new[] { turmaCodigo }, "D1", turma);
            var retornoFrequenciaFinal = "95,50%";

            var tipoCalendario = new TipoCalendario { Id = 1 };
            var aluno = new List<AlunoPorTurmaResposta> { new AlunoPorTurmaResposta() };
            var disciplinas = new List<DisciplinaResposta> { new DisciplinaResposta { CodigoComponenteCurricular = 123 } };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(tipoCalendario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(q =>
                                           q.CodigoTurma == int.Parse(turmaCodigo) &&
                                           q.CodigoAluno == int.Parse(alunoCodigo)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aluno);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDisciplinasPorCodigoTurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(disciplinas);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralAlunoPorTurmasQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(retornoFrequenciaFinal);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(retornoFrequenciaFinal, resultado);
            _repositorioFrequenciaMock.Verify(r => r.ObterPorAlunoBimestreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<TipoFrequenciaAluno>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciaGeralAlunoPorTurmasQuery>(q => q.CodigosDisciplinasTurma.Any()), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve executar fluxo de cálculo específico para o ano de 2020")]
        public async Task Handle_QuandoAno2020_DeveExecutarCalculoEspecifico()
        {
            // Arrange
            var turma = new Turma { AnoLetivo = 2020 };
            var query = new ObterConsultaFrequenciaGeralAlunoPorTurmasQuery("A1", new[] { "T1" }, "D1", turma);

            var tipoCalendario = new TipoCalendario { Id = 1 };
            var periodos = new List<PeriodoEscolar>
            {
                new PeriodoEscolar { Bimestre = 1 }, new PeriodoEscolar { Bimestre = 2 },
                new PeriodoEscolar { Bimestre = 3 }, new PeriodoEscolar { Bimestre = 4 }
            };
            var disciplinasEol = new List<DisciplinaResposta> { new DisciplinaResposta { CodigoComponenteCurricular = 10 } };
            var disciplinas = new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = 10, RegistraFrequencia = true, GrupoMatrizNome = "Grupo 1" } };

            // Para 90% -> 1 falta em 10 aulas. (100 - (1/10 * 100)) = 90
            var frequenciaBim1 = new FrequenciaAluno { TotalAulas = 10, TotalAusencias = 1, TotalCompensacoes = 0 };
            // Para 70% -> 3 faltas em 10 aulas. (100 - (3/10 * 100)) = 70
            var frequenciaBim2 = new FrequenciaAluno { TotalAulas = 10, TotalAusencias = 3, TotalCompensacoes = 0 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(tipoCalendario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeridosEscolaresPorTipoCalendarioIdQuery>(q => q.TipoCalendarioId == tipoCalendario.Id), It.IsAny<CancellationToken>())).ReturnsAsync(periodos);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDisciplinasPorCodigoTurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(disciplinasEol);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(disciplinas);

            _repositorioFrequenciaMock.Setup(r => r.ObterPorAlunoBimestreAsync("A1", 1, It.IsAny<TipoFrequenciaAluno>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>())).ReturnsAsync(frequenciaBim1);
            _repositorioFrequenciaMock.Setup(r => r.ObterPorAlunoBimestreAsync("A1", 2, It.IsAny<TipoFrequenciaAluno>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>())).ReturnsAsync(frequenciaBim2);
            // Retorna null para os bimestres 3 e 4, simulando ausência de registro
            _repositorioFrequenciaMock.Setup(r => r.ObterPorAlunoBimestreAsync("A1", It.IsIn(3, 4), It.IsAny<TipoFrequenciaAluno>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>())).ReturnsAsync((FrequenciaAluno)null);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            // Média esperada: ((90 + 70 + 0 + 0) / 4) = 40
            Assert.Equal("40,00", resultado);
            _repositorioFrequenciaMock.Verify(r => r.ObterPorAlunoBimestreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<TipoFrequenciaAluno>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>()), Times.Exactly(4));
        }

        [Fact(DisplayName = "Deve retornar string vazia para frequência 2020 quando o resultado for zero")]
        public async Task Handle_QuandoCalculo2020ResultaZero_DeveRetornarStringVazia()
        {
            // Arrange
            var turma = new Turma { AnoLetivo = 2020, CodigoTurma = "T1" };
            var query = new ObterConsultaFrequenciaGeralAlunoPorTurmasQuery("A1", new[] { "T1" }, "D1", turma);

            var tipoCalendario = new TipoCalendario { Id = 1 };
            var periodos = new List<PeriodoEscolar> { new PeriodoEscolar { Bimestre = 1 } };
            var disciplinasEol = new List<DisciplinaResposta> { new DisciplinaResposta { CodigoComponenteCurricular = 10 } };
            var disciplinas = new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = 10, RegistraFrequencia = true, GrupoMatrizNome = "Grupo 1" } };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(tipoCalendario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeridosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(periodos);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDisciplinasPorCodigoTurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(disciplinasEol);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(disciplinas);

            _repositorioFrequenciaMock.Setup(r => r.ObterPorAlunoBimestreAsync("A1", 1, It.IsAny<TipoFrequenciaAluno>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>()))
                                      .ReturnsAsync(new FrequenciaAluno { TotalAulas = 10, TotalAusencias = 10 }); // Percentual 0

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(string.Empty, resultado);
        }
    }
}
