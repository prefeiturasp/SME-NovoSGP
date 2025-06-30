using Bogus;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoNotificacaoFrequenciaTeste
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IRepositorioFrequenciaConsulta> _repositorioFrequenciaMock;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaAlunoMock;
        private readonly Mock<IRepositorioNotificacaoFrequencia> _repositorioNotificacaoFrequenciaMock;
        private readonly Mock<IRepositorioPeriodoEscolarConsulta> _repositorioPeriodoEscolarMock;
        private readonly Mock<IRepositorioTurmaConsulta> _repositorioTurmaMock;
        private readonly Mock<IRepositorioParametrosSistemaConsulta> _repositorioParametrosSistemaMock;
        private readonly Mock<IRepositorioTipoCalendarioConsulta> _repositorioTipoCalendarioMock;
        private readonly Mock<IServicoNotificacao> _servicoNotificacaoMock;
        private readonly Mock<IServicoUsuario> _servicoUsuarioMock;
        private readonly Mock<IConsultasFeriadoCalendario> _consultasFeriadoCalendarioMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ServicoNotificacaoFrequencia _servicoNotificacaoFrequencia;
        private readonly Faker _faker;

        public ServicoNotificacaoFrequenciaTeste()
        {
            _configurationMock = new Mock<IConfiguration>();
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaConsulta>();
            _repositorioFrequenciaAlunoMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _repositorioNotificacaoFrequenciaMock = new Mock<IRepositorioNotificacaoFrequencia>();
            _repositorioPeriodoEscolarMock = new Mock<IRepositorioPeriodoEscolarConsulta>();
            _repositorioTurmaMock = new Mock<IRepositorioTurmaConsulta>();
            _repositorioParametrosSistemaMock = new Mock<IRepositorioParametrosSistemaConsulta>();
            _repositorioTipoCalendarioMock = new Mock<IRepositorioTipoCalendarioConsulta>();
            _servicoNotificacaoMock = new Mock<IServicoNotificacao>();
            _servicoUsuarioMock = new Mock<IServicoUsuario>();
            _consultasFeriadoCalendarioMock = new Mock<IConsultasFeriadoCalendario>();
            _mediatorMock = new Mock<IMediator>();
            _faker = new Faker("pt_BR");

            _servicoNotificacaoFrequencia = new ServicoNotificacaoFrequencia(
                _repositorioNotificacaoFrequenciaMock.Object,
                _repositorioParametrosSistemaMock.Object,
                _repositorioFrequenciaMock.Object,
                _repositorioTurmaMock.Object,
                _repositorioPeriodoEscolarMock.Object,
                _repositorioFrequenciaAlunoMock.Object,
                _repositorioTipoCalendarioMock.Object,
                _servicoNotificacaoMock.Object,
                _servicoUsuarioMock.Object,
                _configurationMock.Object,
                _mediatorMock.Object,
                _consultasFeriadoCalendarioMock.Object
            );
        }

        [Fact(DisplayName = "Deve notificar sobre alteração de frequência quando exceder o limite de dias")]
        public async Task VerificaRegraAlteracaoFrequencia_QuandoExcedeLimite_DeveNotificar()
        {
            // Arrange
            var registroFrequenciaId = _faker.Random.Long(1);
            var criadoEm = DateTime.Now.AddDays(-5);
            var alteradoEm = DateTime.Now;

            var registroDto = new RegistroFrequenciaAulaDto
            {
                ProfessorRf = _faker.Random.Int().ToString(),
                CodigoTurma = _faker.Random.Int().ToString(),
                CodigoUe = _faker.Random.Int().ToString(),
                NomeUe = _faker.Company.CompanyName(),
                CodigoDre = _faker.Random.Int().ToString(),
                NomeDre = _faker.Address.City(),
                NomeTurma = "1A",
                NomeTipoEscola = "EMEF",
                CodigoDisciplina = "123"
            };
            var professorDto = new MeusDadosDto { Nome = _faker.Name.FullName() };
            var gestorUsuario = new Usuario { Id = _faker.Random.Long() };
            var gestores = new List<(Cargo?, Usuario)> { (Cargo.Diretor, gestorUsuario) };
            var gestoresEol = new List<(Cargo?, string)> { (Cargo.Diretor, "RF_GESTOR") };

            _repositorioParametrosSistemaMock.Setup(r => r.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeDiasNotificarAlteracaoChamadaEfetivada, It.IsAny<int?>())).ReturnsAsync("2");
            _repositorioFrequenciaMock.Setup(r => r.ObterAulaDaFrequencia(registroFrequenciaId)).Returns(registroDto);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioCoreSSOQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(professorDto);
            _servicoNotificacaoMock.Setup(s => s.ObterFuncionariosPorNivel(It.IsAny<string>(), It.IsAny<Cargo?>(), It.IsAny<bool>(), It.IsAny<bool?>()))
                                 .Returns(gestoresEol);                                             
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(gestorUsuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new DisciplinaDto { Nome = "Componente Teste" });

            // Act
            await _servicoNotificacaoFrequencia.VerificaRegraAlteracaoFrequencia(registroFrequenciaId, criadoEm, alteradoEm);

            // Assert
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact(DisplayName = "Não deve notificar sobre alteração de frequência quando estiver dentro do limite de dias")]
        public async Task VerificaRegraAlteracaoFrequencia_QuandoDentroDoLimite_NaoDeveNotificar()
        {
            // Arrange
            var registroFrequenciaId = _faker.Random.Long(1);
            var criadoEm = DateTime.Now.AddDays(-1);
            var alteradoEm = DateTime.Now;
            _repositorioParametrosSistemaMock.Setup(r => r.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeDiasNotificarAlteracaoChamadaEfetivada, It.IsAny<int?>())).ReturnsAsync("5");

            // Act
            await _servicoNotificacaoFrequencia.VerificaRegraAlteracaoFrequencia(registroFrequenciaId, criadoEm, alteradoEm);

            // Assert
            _repositorioFrequenciaMock.Verify(r => r.ObterAulaDaFrequencia(It.IsAny<long>()), Times.Never);
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.Never);
        }

        [Fact(DisplayName = "Deve notificar alunos faltosos ao final do bimestre")]
        public async Task NotificarAlunosFaltososBimestre_QuandoFinalBimestre_DeveNotificar()
        {
            // Arrange
            var dataReferencia = DateTime.Today.AddDays(-1);
            var periodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1, PeriodoFim = dataReferencia };
            var alunosFaltosos = new List<AlunoFaltosoBimestreDto>
            {
                new AlunoFaltosoBimestreDto
                {
                    UeCodigo = "UE1", DreCodigo = "DRE1", TurmaCodigo = "T1", AlunoCodigo = "A1",
                    TipoEscola = 1, TurmaModalidade = Modalidade.Fundamental
                }
            };
            var gestoresEol = new List<(Cargo?, string)> { (Cargo.Diretor, "RF_GESTOR") };

            _repositorioParametrosSistemaMock.Setup(r => r.ObterValorPorTipoEAno(TipoParametroSistema.PercentualFrequenciaCritico, dataReferencia.Year)).ReturnsAsync("75");
            _repositorioParametrosSistemaMock.Setup(r => r.ObterValorPorTipoEAno(TipoParametroSistema.PercentualFrequenciaMinimaInfantil, dataReferencia.Year)).ReturnsAsync("60");

            _repositorioTipoCalendarioMock.Setup(r => r.BuscarPorAnoLetivoEModalidade(dataReferencia.Year, It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).ReturnsAsync(new TipoCalendario());
            _repositorioPeriodoEscolarMock.Setup(r => r.ObterPorTipoCalendarioData(It.IsAny<long>(), dataReferencia)).ReturnsAsync(periodoEscolar);
            _repositorioFrequenciaAlunoMock
                .Setup(r => r.ObterAlunosFaltososBimestre(It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int?>()))
                .Returns(alunosFaltosos);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosEolPorTurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<AlunoPorTurmaResposta> { new AlunoPorTurmaResposta { CodigoAluno = "A1", NomeAluno = "Aluno Teste" } });
            _servicoNotificacaoMock.Setup(s => s.ObterFuncionariosPorNivel(It.IsAny<string>(), It.IsAny<Cargo?>(), It.IsAny<bool>(), It.IsAny<bool?>()))
                                 .Returns(gestoresEol);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new Usuario { Id = _faker.Random.Long() });

            // Act
            await _servicoNotificacaoFrequencia.NotificarAlunosFaltososBimestre();

            // Assert
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Não deve notificar alunos faltosos se não for o dia após o fim do bimestre")]
        public async Task NotificarAlunosFaltososBimestre_QuandoNaoForFinalBimestre_NaoDeveNotificar()
        {
            // Arrange
            var dataReferencia = DateTime.Today.AddDays(-1);
            var periodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1, PeriodoFim = dataReferencia.AddDays(-1) }; // Data diferente

            _repositorioParametrosSistemaMock.Setup(r => r.ObterValorPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int?>())).ReturnsAsync("75");
            _repositorioTipoCalendarioMock.Setup(r => r.BuscarPorAnoLetivoEModalidade(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).ReturnsAsync(new TipoCalendario());
            _repositorioPeriodoEscolarMock.Setup(r => r.ObterPorTipoCalendarioData(It.IsAny<long>(), dataReferencia)).ReturnsAsync(periodoEscolar);

            // Act
            await _servicoNotificacaoFrequencia.NotificarAlunosFaltososBimestre();

            // Assert
            _repositorioFrequenciaAlunoMock.Verify(r => r.ObterAlunosFaltososBimestre(It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Never);
        }

        [Fact(DisplayName = "Deve executar a notificação de ausência de registro de frequência para professores")]
        public async Task ExecutaNotificacaoRegistroFrequencia_DeveNotificarProfessores()
        {
            // Arrange
            var tipoNotificacao = TipoNotificacaoFrequencia.Professor;
            var turmasSemRegistro = new List<RegistroFrequenciaFaltanteDto>
            {
                new RegistroFrequenciaFaltanteDto { CodigoTurma = "T1", DisciplinaId = "123", ModalidadeTurma = Modalidade.Fundamental }
            };
            var aulasSemRegistro = new List<AulasPorTurmaDisciplinaDto>
            {
                new AulasPorTurmaDisciplinaDto { ProfessorId = "RF_OUTRO", DataAula = DateTime.Now.AddDays(-2) },
                new AulasPorTurmaDisciplinaDto { ProfessorId = "RF_PROF", DataAula = DateTime.Now.AddDays(-1) },
                new AulasPorTurmaDisciplinaDto { ProfessorId = "RF_PROF", DataAula = DateTime.Now.AddDays(-3) }
            };
            var disciplinaDto = new DisciplinaDto { Nome = "Componente", RegistraFrequencia = true };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQuery>(q => q.Tipo == tipoNotificacao), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasSemRegistro);
            _repositorioParametrosSistemaMock.Setup(r => r.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeAulasNotificarProfessor, It.IsAny<int?>()))
                .ReturnsAsync("3");
            _repositorioFrequenciaMock.Setup(r => r.ObterAulasSemRegistroFrequencia("T1", "123", tipoNotificacao)).Returns(aulasSemRegistro);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.Is<string>(rf => rf == "RF_PROF"), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new Usuario { Id = _faker.Random.Long(1, 10000) });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(disciplinaDto);
            _configurationMock.Setup(c => c["UrlFrontEnd"]).Returns("http://localhost");

            // Act
            await _servicoNotificacaoFrequencia.ExecutaNotificacaoRegistroFrequencia();

            // Assert
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.AtLeastOnce);
            _repositorioNotificacaoFrequenciaMock.Verify(r => r.Salvar(It.IsAny<NotificacaoFrequencia>()), Times.AtLeast(aulasSemRegistro.Count));
        }
    }
}
