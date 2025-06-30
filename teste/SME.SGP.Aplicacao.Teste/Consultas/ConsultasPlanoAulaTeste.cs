using Bogus;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasAulaTeste
    {
        private readonly Mock<IRepositorioAulaConsulta> _repositorioConsultaMock;
        private readonly Mock<IConsultasPeriodoEscolar> _consultasPeriodoEscolarMock;
        private readonly Mock<IConsultasTipoCalendario> _consultasTipoCalendarioMock;
        private readonly Mock<IServicoUsuario> _servicoUsuarioMock;
        private readonly Mock<IConsultasDisciplina> _consultasDisciplinaMock;
        private readonly Mock<IConsultasTurma> _consultasTurmaMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsultasAula _consultasAula;
        private readonly Faker _faker;

        public ConsultasAulaTeste()
        {
            _repositorioConsultaMock = new Mock<IRepositorioAulaConsulta>();
            _consultasPeriodoEscolarMock = new Mock<IConsultasPeriodoEscolar>();
            _consultasTipoCalendarioMock = new Mock<IConsultasTipoCalendario>();
            _servicoUsuarioMock = new Mock<IServicoUsuario>();
            _consultasDisciplinaMock = new Mock<IConsultasDisciplina>();
            _consultasTurmaMock = new Mock<IConsultasTurma>();
            _mediatorMock = new Mock<IMediator>();
            _faker = new Faker("pt_BR");

            _consultasAula = new ConsultasAula(
                _repositorioConsultaMock.Object,
                _consultasPeriodoEscolarMock.Object,
                _consultasTipoCalendarioMock.Object,
                _servicoUsuarioMock.Object,
                _consultasDisciplinaMock.Object,
                _consultasTurmaMock.Object,
                _mediatorMock.Object
            );
        }

        [Fact(DisplayName = "Deve Disparar Exceção ao Instanciar Sem Dependências")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasAula(null, _consultasPeriodoEscolarMock.Object, _consultasTipoCalendarioMock.Object, _servicoUsuarioMock.Object, _consultasDisciplinaMock.Object, _consultasTurmaMock.Object, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ConsultasAula(_repositorioConsultaMock.Object, null, _consultasTipoCalendarioMock.Object, _servicoUsuarioMock.Object, _consultasDisciplinaMock.Object, _consultasTurmaMock.Object, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ConsultasAula(_repositorioConsultaMock.Object, _consultasPeriodoEscolarMock.Object, null, _servicoUsuarioMock.Object, _consultasDisciplinaMock.Object, _consultasTurmaMock.Object, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ConsultasAula(_repositorioConsultaMock.Object, _consultasPeriodoEscolarMock.Object, _consultasTipoCalendarioMock.Object, null, _consultasDisciplinaMock.Object, _consultasTurmaMock.Object, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ConsultasAula(_repositorioConsultaMock.Object, _consultasPeriodoEscolarMock.Object, _consultasTipoCalendarioMock.Object, _servicoUsuarioMock.Object, null, _consultasTurmaMock.Object, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ConsultasAula(_repositorioConsultaMock.Object, _consultasPeriodoEscolarMock.Object, _consultasTipoCalendarioMock.Object, _servicoUsuarioMock.Object, _consultasDisciplinaMock.Object, null, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ConsultasAula(_repositorioConsultaMock.Object, _consultasPeriodoEscolarMock.Object, _consultasTipoCalendarioMock.Object, _servicoUsuarioMock.Object, _consultasDisciplinaMock.Object, _consultasTurmaMock.Object, null));
        }

        [Fact(DisplayName = "Deve retornar 'True' para aula dentro do período de fechamento")]
        public async Task AulaDentroPeriodo_QuandoForaDoBimestreMasEmFechamento_DeveRetornarTrue()
        {
            // Arrange
            var turmaId = "123";
            var dataAula = DateTime.Now.AddMonths(-2);
            var turma = new Turma { CodigoTurma = turmaId, ModalidadeCodigo = Modalidade.Fundamental, Semestre = 1 };

            _consultasTurmaMock.Setup(c => c.ObterComUeDrePorCodigo(turmaId)).ReturnsAsync(turma);
            _consultasPeriodoEscolarMock.Setup(c => c.ObterBimestre(It.IsAny<DateTime>(), turma.ModalidadeCodigo, turma.Semestre))
                .Returns((DateTime data, Modalidade _, int _) => Task.FromResult(data.Month == DateTime.Now.Month ? 2 : 1));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaEmPeriodoDeFechamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _consultasAula.AulaDentroPeriodo(turmaId, dataAula);

            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaEmPeriodoDeFechamentoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar 'True' para aula no bimestre atual ou futuro")]
        public async Task AulaDentroPeriodo_QuandoNoBimestreAtualOuFuturo_DeveRetornarTrue()
        {
            // Arrange
            var turmaId = "123";
            var dataAula = DateTime.Now;
            var turma = new Turma { CodigoTurma = turmaId, ModalidadeCodigo = Modalidade.Fundamental, Semestre = 1 };

            _consultasTurmaMock.Setup(c => c.ObterComUeDrePorCodigo(turmaId)).ReturnsAsync(turma);
            _consultasPeriodoEscolarMock.Setup(c => c.ObterBimestre(It.IsAny<DateTime>(), turma.ModalidadeCodigo, turma.Semestre))
                .ReturnsAsync(2); // Ambos no mesmo bimestre

            // Act
            var resultado = await _consultasAula.AulaDentroPeriodo(turmaId, dataAula);

            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaEmPeriodoDeFechamentoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Deve buscar aula por ID com sucesso")]
        public async Task BuscarPorId_QuandoAulaExiste_DeveRetornarDto()
        {
            // Arrange
            var aulaId = _faker.Random.Long(1);
            var aula = new Aula { Id = aulaId, TurmaId = "123", DisciplinaId = "456", DataAula = DateTime.Now, ProfessorRf = "123" };
            var usuario = new Usuario { CodigoRf = "123", PerfilAtual = Perfis.PERFIL_PROFESSOR };

            _repositorioConsultaMock.Setup(r => r.ObterPorId(aulaId)).Returns(aula);
            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuario);

            // Mockando o fluxo interno de AulaDentroPeriodo
            var turma = new Turma { CodigoTurma = aula.TurmaId, ModalidadeCodigo = Modalidade.Fundamental, Semestre = 1 };
            _consultasTurmaMock.Setup(c => c.ObterComUeDrePorCodigo(aula.TurmaId)).ReturnsAsync(turma);
            _consultasPeriodoEscolarMock.Setup(c => c.ObterBimestre(It.IsAny<DateTime>(), It.IsAny<Modalidade>(), It.IsAny<int>())).ReturnsAsync(1);

            // Mockando o fluxo interno de ObterDisciplinaIdAulaEOL
            var componentes = new List<ComponenteCurricularEol> { new ComponenteCurricularEol { Codigo = long.Parse(aula.DisciplinaId) } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentes);
            _consultasDisciplinaMock.Setup(c => c.MapearComponentes(componentes))
               .Returns(new List<DisciplinaResposta> { new DisciplinaResposta { CodigoComponenteCurricular = long.Parse(aula.DisciplinaId) } });

            // Act
            var dto = await _consultasAula.BuscarPorId(aulaId);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(aulaId, dto.Id);
            Assert.False(dto.SomenteLeitura);
        }

        [Fact(DisplayName = "Deve lançar exceção ao buscar aula por ID inexistente")]
        public async Task BuscarPorId_QuandoAulaNaoExiste_DeveLancarNegocioException()
        {
            // Arrange
            var aulaId = _faker.Random.Long(1);
            _repositorioConsultaMock.Setup(r => r.ObterPorId(aulaId)).Returns((Aula)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NegocioException>(() => _consultasAula.BuscarPorId(aulaId));
            Assert.Equal($"Aula de id {aulaId} não encontrada", exception.Message);
        }

        [Fact(DisplayName = "Deve obter a quantidade de aulas recorrentes")]
        public async Task ObterQuantidadeAulasRecorrentes_DeveRetornarContagemCorreta()
        {
            // Arrange
            var aulaInicialId = 1L;
            var aulaInicio = new Aula { Id = aulaInicialId, TipoCalendarioId = 1, DataAula = DateTime.Now };
            var aulasRecorrentes = new List<Aula> { new Aula(), new Aula() }; // 2 aulas futuras

            _repositorioConsultaMock.Setup(r => r.ObterPorId(aulaInicialId)).Returns(aulaInicio);
            _consultasPeriodoEscolarMock.Setup(c => c.ObterFimPeriodoRecorrencia(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<RecorrenciaAula>()))
                .ReturnsAsync(DateTime.Now.AddMonths(1));
            _repositorioConsultaMock.Setup(r => r.ObterAulasRecorrencia(aulaInicialId, aulaInicialId, It.IsAny<DateTime>()))
                .ReturnsAsync(aulasRecorrentes);

            // Act
            var quantidade = await _consultasAula.ObterQuantidadeAulasRecorrentes(aulaInicialId, RecorrenciaAula.RepetirBimestreAtual);

            // Assert
            // 1 (aula inicial) + 2 (aulas recorrentes) = 3
            Assert.Equal(3, quantidade);
        }

        [Fact(DisplayName = "Deve obter a quantidade de aulas no dia para disciplina normal")]
        public async Task ObterQuantidadeAulasTurmaDiaProfessor_DisciplinaNormal_DeveChamarRepositorioCorreto()
        {
            // Arrange
            var disciplinaId = "123"; // ID não é de experiência pedagógica
            var aulas = new List<AulasPorTurmaDisciplinaDto> { new AulasPorTurmaDisciplinaDto { Quantidade = 2 }, new AulasPorTurmaDisciplinaDto { Quantidade = 1 } };
            _repositorioConsultaMock.Setup(r => r.ObterAulasTurmaDisciplinaDiaProfessor(It.IsAny<string>(), disciplinaId, It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(aulas);

            // Act
            var quantidade = await _consultasAula.ObterQuantidadeAulasTurmaDiaProfessor("T1", disciplinaId, DateTime.Now, "RF123");

            // Assert
            Assert.Equal(3, quantidade);
            _repositorioConsultaMock.Verify(r => r.ObterAulasTurmaDisciplinaDiaProfessor("T1", disciplinaId, It.IsAny<DateTime>(), "RF123"), Times.Once);
            _repositorioConsultaMock.Verify(r => r.ObterAulasTurmaExperienciasPedagogicasDia(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact(DisplayName = "Deve obter a quantidade de aulas no dia para experiência pedagógica")]
        public async Task ObterQuantidadeAulasTurmaDiaProfessor_ExperienciaPedagogica_DeveChamarRepositorioCorreto()
        {
            // Arrange
            var disciplinaId = "1214"; // ID de experiência pedagógica
            var aulas = new List<AulasPorTurmaDisciplinaDto> { new AulasPorTurmaDisciplinaDto { Quantidade = 5 } };
            _repositorioConsultaMock.Setup(r => r.ObterAulasTurmaExperienciasPedagogicasDia(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(aulas);

            // Act
            var quantidade = await _consultasAula.ObterQuantidadeAulasTurmaDiaProfessor("T1", disciplinaId, DateTime.Now, "RF123");

            // Assert
            Assert.Equal(5, quantidade);
            _repositorioConsultaMock.Verify(r => r.ObterAulasTurmaExperienciasPedagogicasDia("T1", It.IsAny<DateTime>()), Times.Once);
            _repositorioConsultaMock.Verify(r => r.ObterAulasTurmaDisciplinaDiaProfessor(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);
        }
    }
}