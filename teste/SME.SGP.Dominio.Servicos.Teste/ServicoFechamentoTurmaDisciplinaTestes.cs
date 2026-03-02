using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
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
    public class ServicoFechamentoTurmaDisciplinaTestes
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IServicoUsuario> _servicoUsuarioMock;
        private readonly Mock<IServicoPeriodoFechamento> _servicoPeriodoFechamentoMock;
        private readonly Mock<IServicoNotificacao> _servicoNotificacaoMock;
        private readonly Mock<IRepositorioTurmaConsulta> _repositorioTurmaConsultaMock;
        private readonly Mock<IRepositorioPeriodoEscolarConsulta> _repositorioPeriodoEscolarConsultaMock;
        private readonly Mock<IRepositorioFechamentoTurmaDisciplina> _repositorioFechamentoTurmaDisciplinaMock;
        private readonly Mock<IRepositorioFechamentoTurma> _repositorioFechamentoTurmaMock;
        private readonly Mock<IRepositorioFechamentoAluno> _repositorioFechamentoAlunoMock;
        private readonly Mock<IRepositorioFechamentoNota> _repositorioFechamentoNotaMock;
        private readonly Mock<IRepositorioFechamentoAlunoConsulta> _repositorioFechamentoAlunoConsultaMock;
        private readonly Mock<IConsultasDisciplina> _consultasDisciplinaMock;
        private readonly Mock<IRepositorioTipoCalendarioConsulta> _repositorioTipoCalendarioMock;
        private readonly Mock<IRepositorioParametrosSistemaConsulta> _repositorioParametrosSistemaMock;
        private readonly Mock<IRepositorioCache> _repositorioCacheMock;
        private readonly Mock<IConsultasSupervisor> _consultasSupervisorMock;
        private readonly Mock<IRepositorioEventoTipo> _repositorioEventoTipoMock;
        private readonly Mock<IRepositorioEvento> _repositorioEventoMock;
        private readonly Mock<IRepositorioFechamentoTurmaConsulta> _repositorioFechamentoTurmaConsultaMock;
        private readonly Mock<IRepositorioFechamentoReabertura> _repositorioFechamentoReaberturaMock;
        private readonly ServicoFechamentoTurmaDisciplina _servico;
        private readonly Faker _faker;

        public ServicoFechamentoTurmaDisciplinaTestes()
        {
            _mediatorMock = new Mock<IMediator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _servicoUsuarioMock = new Mock<IServicoUsuario>();
            _servicoPeriodoFechamentoMock = new Mock<IServicoPeriodoFechamento>();
            _servicoNotificacaoMock = new Mock<IServicoNotificacao>();
            _repositorioTurmaConsultaMock = new Mock<IRepositorioTurmaConsulta>();
            _repositorioPeriodoEscolarConsultaMock = new Mock<IRepositorioPeriodoEscolarConsulta>();
            _repositorioFechamentoTurmaDisciplinaMock = new Mock<IRepositorioFechamentoTurmaDisciplina>();
            _repositorioFechamentoTurmaMock = new Mock<IRepositorioFechamentoTurma>();
            _repositorioFechamentoAlunoMock = new Mock<IRepositorioFechamentoAluno>();
            _repositorioFechamentoNotaMock = new Mock<IRepositorioFechamentoNota>();
            _repositorioFechamentoAlunoConsultaMock = new Mock<IRepositorioFechamentoAlunoConsulta>();
            _consultasDisciplinaMock = new Mock<IConsultasDisciplina>();
            _repositorioTipoCalendarioMock = new Mock<IRepositorioTipoCalendarioConsulta>();
            _repositorioParametrosSistemaMock = new Mock<IRepositorioParametrosSistemaConsulta>();
            _repositorioCacheMock = new Mock<IRepositorioCache>();
            _consultasSupervisorMock = new Mock<IConsultasSupervisor>();
            _repositorioEventoTipoMock = new Mock<IRepositorioEventoTipo>();
            _repositorioFechamentoTurmaConsultaMock = new Mock<IRepositorioFechamentoTurmaConsulta>();
            _repositorioEventoMock = new Mock<IRepositorioEvento>();
            _repositorioFechamentoReaberturaMock = new Mock<IRepositorioFechamentoReabertura>();
            _faker = new Faker("pt_BR");

            _servico = new ServicoFechamentoTurmaDisciplina(
                _repositorioFechamentoTurmaDisciplinaMock.Object,
                _repositorioFechamentoTurmaMock.Object,
                _repositorioFechamentoTurmaConsultaMock.Object,
                _repositorioFechamentoAlunoConsultaMock.Object,
                _repositorioFechamentoAlunoMock.Object,
                _repositorioFechamentoNotaMock.Object,
                _repositorioTurmaConsultaMock.Object,
                _servicoPeriodoFechamentoMock.Object,
                _repositorioPeriodoEscolarConsultaMock.Object,
                _repositorioTipoCalendarioMock.Object,
                _repositorioParametrosSistemaMock.Object,
                _consultasDisciplinaMock.Object,
                _servicoNotificacaoMock.Object,
                _servicoUsuarioMock.Object,
                _unitOfWorkMock.Object,
                _consultasSupervisorMock.Object,
                _repositorioEventoMock.Object,
                _repositorioEventoTipoMock.Object,
                _repositorioFechamentoReaberturaMock.Object,
                _repositorioCacheMock.Object,
                _mediatorMock.Object
            );
        }

        [Fact]
        public async Task DadoExistenciaDeTodosOsCargos_QuandoGerarNotificacao_DeveNotificarTodosOsUsuarios()
        {
            // Arrange
            var (turma, usuarioLogado, ue, bimestre, alunosHtml) = PrepararCenarioBase();

            var listaCPs = GerarUsuariosEolRetornoDto(2, "CP");
            var listaDiretores = GerarUsuariosEolRetornoDto(1, "DIR");
            var listaSupervisores = new List<ResponsavelEscolasDto> { new ResponsavelEscolasDto { ResponsavelId = "RF-SUP0", Responsavel = "Supervisor" } };
            var totalDestinatarios = listaCPs.Count() + listaDiretores.Count() + listaSupervisores.Count();

            ConfigurarMocks(listaCPs, listaDiretores, listaSupervisores);

            // Act
            await _servico.GerarNotificacaoAlteracaoLimiteDias(turma, usuarioLogado, ue, bimestre, alunosHtml);

            // Assert
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.Exactly(totalDestinatarios));
            _servicoUsuarioMock.Verify(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), "", "", "", false), Times.Exactly(totalDestinatarios));
        }

        [Fact]
        public async Task DadoInexistenciaDeCoordenadores_QuandoGerarNotificacao_DeveNotificarApenasDiretoresESupervisores()
        {
            // Arrange
            var (turma, usuarioLogado, ue, bimestre, alunosHtml) = PrepararCenarioBase();

            var listaDiretores = GerarUsuariosEolRetornoDto(1, "DIR");
            var listaSupervisores = new List<ResponsavelEscolasDto> { new ResponsavelEscolasDto { ResponsavelId = "RF-SUP0", Responsavel = "Supervisor" } };
            var totalDestinatarios = listaDiretores.Count() + listaSupervisores.Count();

            // Coordenadores retornam nulo
            ConfigurarMocks(null, listaDiretores, listaSupervisores);

            // Act
            await _servico.GerarNotificacaoAlteracaoLimiteDias(turma, usuarioLogado, ue, bimestre, alunosHtml);

            // Assert
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.Exactly(totalDestinatarios));
        }

        [Fact]
        public async Task DadoInexistenciaDeDiretores_QuandoGerarNotificacao_DeveNotificarApenasCoordenadoresESupervisores()
        {
            // Arrange
            var (turma, usuarioLogado, ue, bimestre, alunosHtml) = PrepararCenarioBase();

            var listaCPs = GerarUsuariosEolRetornoDto(2, "CP");
            var listaSupervisores = new List<ResponsavelEscolasDto> { new ResponsavelEscolasDto { ResponsavelId = "RF-SUP0", Responsavel = "Supervisor" } };
            var totalDestinatarios = listaCPs.Count() + listaSupervisores.Count();

            // Diretores retornam lista vazia
            ConfigurarMocks(listaCPs, new List<UsuarioEolRetornoDto>(), listaSupervisores);

            // Act
            await _servico.GerarNotificacaoAlteracaoLimiteDias(turma, usuarioLogado, ue, bimestre, alunosHtml);

            // Assert
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.Exactly(totalDestinatarios));
        }

        [Fact]
        public async Task DadoInexistenciaDeSupervisores_QuandoGerarNotificacao_DeveNotificarApenasCoordenadoresEDiretores()
        {
            // Arrange
            var (turma, usuarioLogado, ue, bimestre, alunosHtml) = PrepararCenarioBase();

            var listaCPs = GerarUsuariosEolRetornoDto(2, "CP");
            var listaDiretores = GerarUsuariosEolRetornoDto(1, "DIR");
            var totalDestinatarios = listaCPs.Count() + listaDiretores.Count();

            // Supervisores retornam nulo
            ConfigurarMocks(listaCPs, listaDiretores, null);

            // Act
            await _servico.GerarNotificacaoAlteracaoLimiteDias(turma, usuarioLogado, ue, bimestre, alunosHtml);

            // Assert
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.Exactly(totalDestinatarios));
        }

        [Fact]
        public async Task DadoInexistenciaDeTodosOsCargos_QuandoGerarNotificacao_NaoDeveNotificarNinguem()
        {
            // Arrange
            var (turma, usuarioLogado, ue, bimestre, alunosHtml) = PrepararCenarioBase();

            // Todos os cargos retornam nulo ou vazio
            ConfigurarMocks(null, new List<UsuarioEolRetornoDto>(), null);

            // Act
            await _servico.GerarNotificacaoAlteracaoLimiteDias(turma, usuarioLogado, ue, bimestre, alunosHtml);

            // Assert
            _servicoNotificacaoMock.Verify(s => s.Salvar(It.IsAny<Notificacao>()), Times.Never);
            _servicoUsuarioMock.Verify(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), "", "", "", false), Times.Never);
        }

        [Fact]
        public async Task DadoInexistenciaDeFechamentoTurmaDisciplina_QuandoReprocessar_DeveLancarExcecao()
        {
            // Arrange
            var fechamentoTurmaId = _faker.Random.Long(1);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Reprocessar(fechamentoTurmaId));
        }

        [Fact]
        public async Task DadoTurmaInexistente_QuandoReprocessar_DeveLancarExcecao()
        {
            // Arrange
            var fechamentoTurmaId = _faker.Random.Long(1);
            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina { Id = fechamentoTurmaId, FechamentoTurma = new FechamentoTurma { TurmaId = _faker.Random.Long(1) } };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaDisciplinaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(fechamentoTurmaDisciplina);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Reprocessar(fechamentoTurmaId));
        }

        [Fact]
        public async Task DadoDisciplinaInexistente_QuandoReprocessar_DeveLancarExcecao()
        {
            // Arrange
            var fechamentoTurmaId = _faker.Random.Long(1);
            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina
            {
                Id = fechamentoTurmaId,
                FechamentoTurma = new FechamentoTurma { TurmaId = _faker.Random.Long(1) },
                DisciplinaId = _faker.Random.Long(1)
            };
            var turma = new Turma { Id = _faker.Random.Long(1) };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaDisciplinaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(fechamentoTurmaDisciplina);
            _repositorioTurmaConsultaMock.Setup(r => r.ObterTurmaComUeEDrePorId(It.IsAny<long>()))
                                         .ReturnsAsync(turma);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Reprocessar(fechamentoTurmaId));
        }

        [Fact]
        public async Task DadoPeriodoEscolarInexistente_QuandoReprocessar_DeveLancarExcecao()
        {
            // Arrange
            var fechamentoTurmaId = _faker.Random.Long(1);
            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina
            {
                Id = fechamentoTurmaId,
                FechamentoTurma = new FechamentoTurma { TurmaId = _faker.Random.Long(1), PeriodoEscolarId = _faker.Random.Long(1) },
                DisciplinaId = _faker.Random.Long(1)
            };
            var turma = new Turma { Id = _faker.Random.Long(1) };
            var disciplina = new DisciplinaDto
            {
                LancaNota = true,
                RegistraFrequencia = true
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaDisciplinaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(fechamentoTurmaDisciplina);
            _repositorioTurmaConsultaMock.Setup(r => r.ObterTurmaComUeEDrePorId(It.IsAny<long>()))
                                         .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(disciplina);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Reprocessar(fechamentoTurmaId));
        }

        [Fact]
        public async Task DadoTipoTurmaDiferenteDePrograma_QuandoReprocessar_DeveChamarCommandParaIncluirNaFilaDePendencias()
        {
            // Arrange
            var fechamentoTurmaId = _faker.Random.Long(1);
            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina
            {
                Id = fechamentoTurmaId,
                FechamentoTurma = new FechamentoTurma { TurmaId = _faker.Random.Long(1), PeriodoEscolarId = _faker.Random.Long(1) },
                DisciplinaId = _faker.Random.Long(1)
            };
            var turma = new Turma { Id = _faker.Random.Long(1), TipoTurma = Enumerados.TipoTurma.Regular };
            var disciplina = new DisciplinaDto
            {
                LancaNota = true,
                RegistraFrequencia = true
            };

            var periodoEscolar = new PeriodoEscolar
            {
                Id = _faker.Random.Long(1),
                PeriodoInicio = _faker.Date.Past(),
                PeriodoFim = _faker.Date.Recent(),
                Bimestre = _faker.Random.Int(1)
            };

            var usuario = new Usuario { Id = _faker.Random.Long(1) };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaDisciplinaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(fechamentoTurmaDisciplina);
            _repositorioTurmaConsultaMock.Setup(r => r.ObterTurmaComUeEDrePorId(It.IsAny<long>()))
                                         .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(disciplina);
            _repositorioPeriodoEscolarConsultaMock.Setup(r => r.ObterPorId(It.IsAny<long>()))
                                                  .Returns(periodoEscolar);
            _servicoUsuarioMock.Setup(r => r.ObterUsuarioLogado())
                               .ReturnsAsync(usuario);

            // Act
            await _servico.Reprocessar(fechamentoTurmaId);

            // Assert
            _repositorioFechamentoTurmaDisciplinaMock.Verify(r => r.Salvar(
                It.Is<FechamentoTurmaDisciplina>(f => f.DisciplinaId == fechamentoTurmaDisciplina.DisciplinaId &&
                                                      f.FechamentoTurma.TurmaId == fechamentoTurmaDisciplina.FechamentoTurma.TurmaId &&
                                                      f.FechamentoTurma.PeriodoEscolar.Id == periodoEscolar.Id &&
                                                      f.FechamentoTurma.PeriodoEscolar.PeriodoInicio == periodoEscolar.PeriodoInicio &&
                                                      f.FechamentoTurma.PeriodoEscolar.PeriodoFim == periodoEscolar.PeriodoFim &&
                                                      f.FechamentoTurma.PeriodoEscolar.Bimestre == periodoEscolar.Bimestre)), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaGeracaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DadoTipoTurmaDePrograma_QuandoReprocessar_NaoDeveChamarCommandParaIncluirNaFilaDePendencias()
        {
            // Arrange
            var fechamentoTurmaId = _faker.Random.Long(1);
            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina
            {
                Id = fechamentoTurmaId,
                FechamentoTurma = new FechamentoTurma { TurmaId = _faker.Random.Long(1), PeriodoEscolarId = _faker.Random.Long(1) },
                DisciplinaId = _faker.Random.Long(1)
            };
            var turma = new Turma { Id = _faker.Random.Long(1), TipoTurma = Enumerados.TipoTurma.Programa };
            var disciplina = new DisciplinaDto
            {
                LancaNota = true,
                RegistraFrequencia = true
            };

            var periodoEscolar = new PeriodoEscolar
            {
                Id = _faker.Random.Long(1),
                PeriodoInicio = _faker.Date.Past(),
                PeriodoFim = _faker.Date.Recent(),
                Bimestre = _faker.Random.Int(1)
            };

            var usuario = new Usuario { Id = _faker.Random.Long(1) };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaDisciplinaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(fechamentoTurmaDisciplina);
            _repositorioTurmaConsultaMock.Setup(r => r.ObterTurmaComUeEDrePorId(It.IsAny<long>()))
                                         .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(disciplina);
            _repositorioPeriodoEscolarConsultaMock.Setup(r => r.ObterPorId(It.IsAny<long>()))
                                                  .Returns(periodoEscolar);

            // Act
            await _servico.Reprocessar(fechamentoTurmaId, usuario);

            // Assert
            _repositorioFechamentoTurmaDisciplinaMock.Verify(r => r.Salvar(
                It.Is<FechamentoTurmaDisciplina>(f => f.DisciplinaId == fechamentoTurmaDisciplina.DisciplinaId &&
                                                      f.FechamentoTurma.TurmaId == fechamentoTurmaDisciplina.FechamentoTurma.TurmaId &&
                                                      f.FechamentoTurma.PeriodoEscolar.Id == periodoEscolar.Id &&
                                                      f.FechamentoTurma.PeriodoEscolar.PeriodoInicio == periodoEscolar.PeriodoInicio &&
                                                      f.FechamentoTurma.PeriodoEscolar.PeriodoFim == periodoEscolar.PeriodoFim &&
                                                      f.FechamentoTurma.PeriodoEscolar.Bimestre == periodoEscolar.Bimestre)), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaGeracaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DadoPeriodoDeFechamentoNaoLocalizado_QuandoSalvar_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            _servicoPeriodoFechamentoMock.Setup(s => s.ObterPorTipoCalendarioSme(It.IsAny<long>(), It.IsAny<Aplicacao>()))
                                      .ReturnsAsync((FechamentoDto)null);
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Salvar(0, fechamentoDto));
        }

        [Fact]
        public async Task DadoPeriodoNaoEstaEmAberto_QuandoSalvar_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            _mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Salvar(0, fechamentoDto));
        }

        [Fact]
        public async Task DadoAlunosInativosNoFechamento_QuandoSalvar_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            var alunosAtivos = new List<AlunoPorTurmaResposta> { new AlunoPorTurmaResposta { CodigoAluno = "ALUNO_ATIVO_1" } };
            fechamentoDto.NotaConceitoAlunos = new List<FechamentoNotaDto> { new FechamentoNotaDto { CodigoAluno = "ALUNO_INATIVO_1" } };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(alunosAtivos);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Salvar(0, fechamentoDto));
        }

        [Fact]
        public async Task DadoNotaMaiorQueDez_QuandoSalvar_DeveLancarExcecaoERealizarRollback()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            fechamentoDto.NotaConceitoAlunos.First().Nota = 11;

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Salvar(0, fechamentoDto));

            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
        }

        [Fact]
        public async Task DadoFluxoCorretoParaNovoFechamento_QuandoSalvar_DevePersistirDadosEConsolidarNotas()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            _repositorioFechamentoTurmaMock.Setup(r => r.SalvarAsync(It.IsAny<FechamentoTurma>())).ReturnsAsync(99); // Retorna o ID do FechamentoTurma salvo

            // Act
            var resultado = await _servico.Salvar(0, fechamentoDto, false, false);

            // Assert
            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _repositorioFechamentoTurmaMock.Verify(r => r.SalvarAsync(It.IsAny<FechamentoTurma>()), Times.Once);
            _repositorioFechamentoTurmaDisciplinaMock.Verify(r => r.SalvarAsync(It.IsAny<FechamentoTurmaDisciplina>()), Times.Once);
            _repositorioFechamentoAlunoMock.Verify(r => r.SalvarAsync(It.IsAny<FechamentoAluno>()), Times.Once);
            _repositorioFechamentoNotaMock.Verify(r => r.SalvarAsync(It.IsAny<FechamentoNota>()), Times.Once);

            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);

            _repositorioCacheMock.Verify(c => c.ObterObjetoAsync<List<NotaConceitoBimestreComponenteDto>>(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCachePorValorObjetoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ConsolidacaoNotaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaGeracaoPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicaFilaExcluirPendenciaAusenciaFechamentoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.False(resultado.EmAprovacao);
            Assert.Equal("Suas informações foram salvas com sucesso.", resultado.MensagemConsistencia);
        }

        [Fact]
        public async Task DadoQueProfessorNaoPodeAlterarTurma_QuandoSalvar_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            var usuario = new Usuario
            {
                Id = 1,
                Nome = _faker.Person.FullName,
                CodigoRf = _faker.Person.Cpf(),
                PerfilAtual = Guid.NewGuid()
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>());
            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuario);
            _servicoUsuarioMock
                .Setup(s => s.PodePersistirTurmaDisciplina(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Usuario>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Salvar(0, fechamentoDto));
        }

        [Fact]
        public async Task DadoDisciplinaNaoEncontradaNoEOL_QuandoSalvar_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            _consultasDisciplinaMock.Setup(c => c.ObterDisciplina(fechamentoDto.DisciplinaId)).ReturnsAsync((DisciplinaDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Salvar(0, fechamentoDto));
        }

        [Fact]
        public async Task DadoQueAnoAnteriorComAlteracaoNotaFechamentoAtivo_QuandoSalvar_DeveRetornarAuditoriaComMensagemConsistenciaComAviso()
        {
            // Arrange
            var alunoCodigo = _faker.Random.Int(1).ToString();
            var disciplinaId = _faker.Random.Long(1);
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            fechamentoDto.NotaConceitoAlunos.First().Id = 0;
            _repositorioFechamentoTurmaDisciplinaMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .Returns(new FechamentoTurmaDisciplina { FechamentoTurma = new FechamentoTurma(), Id = _faker.Random.Long(1), FechamentoTurmaId = _faker.Random.Long(1) });
            _repositorioFechamentoTurmaConsultaMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .Returns(new FechamentoTurma());
            _repositorioFechamentoAlunoConsultaMock
                .Setup(r => r.ObterPorFechamentoTurmaDisciplina(It.IsAny<long>()))
                .ReturnsAsync(new List<FechamentoAluno>
                {
                    new FechamentoAluno
                    {
                        AlunoCodigo = alunoCodigo,
                        FechamentoNotas = new List<FechamentoNota>{new FechamentoNota { DisciplinaId = disciplinaId } }
                    }
                });
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPorAlunosDisciplinasDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno> { new FrequenciaAluno { CodigoAluno = alunoCodigo, DisciplinaId = disciplinaId.ToString() } });
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterSinteseAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SinteseDto { Id = SinteseEnum.Frequente });
            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(o => o.TipoParametroSistema == TipoParametroSistema.AprovacaoAlteracaoNotaFechamento), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });

            turma.TipoTurma = TipoTurma.Programa;
            turma.AnoLetivo = DateTime.Now.Year - 1;
            _repositorioTurmaConsultaMock.Setup(r => r.ObterTurmaComUeEDrePorId(It.IsAny<long>()))
                                         .ReturnsAsync(turma);
            // Act
            var auditoria = await _servico.Salvar(1, fechamentoDto, true, true);

            // Assert
            auditoria.Should().NotBeNull();
            auditoria.MensagemConsistencia.Should().Be(MensagensNegocioLancamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO);
            _mediatorMock
                .Verify(m =>
                m.Send(It.Is<PublicarFilaSgpCommand>(p => p.Rota == RotasRabbitSgpFechamento.GerarNotificacaoAlteracaoLimiteDias), It.IsAny<CancellationToken>())
                , Times.Never);

        }

        [Fact]
        public async Task DadoAlunoComNotaAlterada_QuandoSalvar_DeveChamarMediatorParaPublicarNaFilaDeNotificacao()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();

            _servicoPeriodoFechamentoMock.Setup(s => s.ObterPorTipoCalendarioSme(It.IsAny<long>(), It.IsAny<Aplicacao>())).ReturnsAsync((FechamentoDto)null);
            _repositorioFechamentoAlunoConsultaMock
                .Setup(r => r.ObterPorFechamentoTurmaDisciplina(It.IsAny<long>()))
                .ReturnsAsync(new List<FechamentoAluno>
                {
                    new FechamentoAluno
                    {
                        AlunoCodigo = fechamentoDto.NotaConceitoAlunos.FirstOrDefault().CodigoAluno,
                        FechamentoNotas = new List<FechamentoNota>
                        {
                            new FechamentoNota
                            {
                                DisciplinaId = fechamentoDto.NotaConceitoAlunos.FirstOrDefault().DisciplinaId,
                                Nota = _faker.Random.Double(0, 100)
                            }
                        }
                    }
                });
            _repositorioEventoMock
                .Setup(r => r.TemEventoNosDiasETipo(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<TipoEvento>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);
            _repositorioFechamentoReaberturaMock
                .Setup(r => r.ObterReaberturaFechamentoBimestrePorDataReferencia(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new FechamentoReabertura { Inicio = _faker.Date.Past(), Fim = _faker.Date.Recent() });
            _repositorioPeriodoEscolarConsultaMock.Setup(r => r.ObterPorTipoCalendario(It.IsAny<long>()))
                                                  .ReturnsAsync(new List<PeriodoEscolar> { new PeriodoEscolar { Bimestre = fechamentoDto.Bimestre } });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaTipoValorPorTurmaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new NotaTipoValor { TipoNota = TipoNota.Todas });
            _repositorioParametrosSistemaMock
                .Setup(r => r.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal, It.IsAny<int>()))
                .ReturnsAsync("-1");
            _repositorioCacheMock
                .Setup(c => c.ObterObjetoAsync<List<NotaConceitoBimestreComponenteDto>>(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<NotaConceitoBimestreComponenteDto>()
                {
                    new NotaConceitoBimestreComponenteDto
                    {
                        AlunoCodigo = fechamentoDto.NotaConceitoAlunos.FirstOrDefault().CodigoAluno,
                        Bimestre = fechamentoDto.Bimestre,
                        ComponenteCurricularCodigo = fechamentoDto.NotaConceitoAlunos.FirstOrDefault().DisciplinaId
                    }
                });

            // Act
            await _servico.Salvar(1, fechamentoDto, false, true);

            // Assert
            _mediatorMock
                .Verify(m =>
                m.Send(It.Is<PublicarFilaSgpCommand>(p => p.Rota == RotasRabbitSgpFechamento.GerarNotificacaoAlteracaoLimiteDias), It.IsAny<CancellationToken>())
                , Times.Once);
        }

        [Fact]
        public async Task DadoAprovacaoAlteracaoNotaFechamentoInexistente_QuandoSalvar_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            var usuario = new Usuario
            {
                Id = 1,
                Nome = _faker.Person.FullName,
                CodigoRf = _faker.Person.Cpf(),
                PerfilAtual = Guid.NewGuid()
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>());
            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuario);
            turma.AnoLetivo = DateTime.Now.Year - 1;
            _repositorioTurmaConsultaMock
                .Setup(r => r.ObterTurmaComUeEDrePorCodigo(turma.CodigoTurma))
                .ReturnsAsync(turma);
            _servicoPeriodoFechamentoMock.Setup(s => s.ObterPorTipoCalendarioSme(It.IsAny<long>(), It.IsAny<Aplicacao>())).ReturnsAsync((FechamentoDto)null);
            _repositorioFechamentoAlunoConsultaMock
                .Setup(r => r.ObterPorFechamentoTurmaDisciplina(It.IsAny<long>()))
                .ReturnsAsync(new List<FechamentoAluno>
                {
                    new FechamentoAluno
                    {
                        AlunoCodigo = fechamentoDto.NotaConceitoAlunos.FirstOrDefault().CodigoAluno,
                        FechamentoNotas = new List<FechamentoNota>
                        {
                            new FechamentoNota
                            {
                                DisciplinaId = fechamentoDto.NotaConceitoAlunos.FirstOrDefault().DisciplinaId,
                                Nota = _faker.Random.Double(0, 100)
                            }
                        }
                    }
                });
            _repositorioEventoMock
                .Setup(r => r.TemEventoNosDiasETipo(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<TipoEvento>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);
            _repositorioFechamentoReaberturaMock
                .Setup(r => r.ObterReaberturaFechamentoBimestrePorDataReferencia(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new FechamentoReabertura { Inicio = _faker.Date.Past(), Fim = _faker.Date.Recent() });
            _repositorioPeriodoEscolarConsultaMock.Setup(r => r.ObterPorTipoCalendario(It.IsAny<long>()))
                                                  .ReturnsAsync(new List<PeriodoEscolar> { new PeriodoEscolar { Bimestre = fechamentoDto.Bimestre } });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaTipoValorPorTurmaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new NotaTipoValor { TipoNota = TipoNota.Todas });
            _mediatorMock
                .SetupSequence(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(o => o.TipoParametroSistema == TipoParametroSistema.AprovacaoAlteracaoNotaFechamento), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true })
                .ReturnsAsync((ParametrosSistema)null);

            // Act
            await Assert.ThrowsAsync<NegocioException>(async () => await _servico.Salvar(1, fechamentoDto, false, true));
        }

        [Fact]
        public async Task DadoPeriodoComEventoSemReaberturaDeFechamento_QuandoSalvar_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();

            _servicoPeriodoFechamentoMock.Setup(s => s.ObterPorTipoCalendarioSme(It.IsAny<long>(), It.IsAny<Aplicacao>())).ReturnsAsync((FechamentoDto)null);
            _repositorioFechamentoAlunoConsultaMock
                .Setup(r => r.ObterPorFechamentoTurmaDisciplina(It.IsAny<long>()))
                .ReturnsAsync(new List<FechamentoAluno>
                {
                    new FechamentoAluno
                    {
                        AlunoCodigo = fechamentoDto.NotaConceitoAlunos.FirstOrDefault().CodigoAluno
                    }
                });
            _repositorioEventoMock
                .Setup(r => r.TemEventoNosDiasETipo(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<TipoEvento>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Salvar(1, fechamentoDto));
        }

        [Fact]
        public async Task DadoTurmaFechamentoInexistente_QuandoSalvar_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var (fechamentoDto, turma) = PrepararCenarioBaseSalvar();
            _repositorioTurmaConsultaMock.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                                         .ReturnsAsync((Turma)null);
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Salvar(1, fechamentoDto));
        }

        #region Métodos de Apoio
        private (Turma turma, Usuario usuarioLogado, Ue ue, int bimestre, string alunosHtml) PrepararCenarioBase()
        {
            var dre = new Dre { Id = _faker.Random.Long(1), Nome = "DRE TESTE" };
            var ue = new Ue { Nome = "UE TESTE", Dre = dre };
            var turma = new Turma
            {
                Id = _faker.Random.Long(1),
                Nome = "1A",
                AnoLetivo = 2025,
                Ue = ue,
                UeId = ue.Id
            };
            var usuarioLogado = new Usuario { Nome = "Professor Teste", CodigoRf = "RF12345" };
            var bimestre = 3;
            var alunosHtml = "<li>12345 - Aluno 1</li>";

            return (turma, usuarioLogado, ue, bimestre, alunosHtml);
        }

        private IEnumerable<UsuarioEolRetornoDto> GerarUsuariosEolRetornoDto(int quantidade, string prefixo)
        {
            return new Faker<UsuarioEolRetornoDto>("pt_BR")
                .RuleFor(u => u.CodigoRf, f => $"RF-{prefixo}{f.IndexFaker}")
                .RuleFor(u => u.NomeServidor, f => f.Name.FullName())
                .Generate(quantidade);
        }

        private void ConfigurarMocks(IEnumerable<UsuarioEolRetornoDto> listaCPs, IEnumerable<UsuarioEolRetornoDto> listaDiretores, IEnumerable<ResponsavelEscolasDto> listaSupervisores)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorCargoUeQuery>(q => q.CargoId == (long)Cargo.CP), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaCPs);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorCargoUeQuery>(q => q.CargoId == (long)Cargo.Diretor), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaDiretores);

            _consultasSupervisorMock.Setup(c => c.ObterAtribuicaoResponsavel(It.IsAny<FiltroObterSupervisorEscolasDto>()))
                                    .ReturnsAsync(listaSupervisores);

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), "", "", "", false))
                               .ReturnsAsync((string rf, string l, string n, string e, bool b) => new Usuario { Id = _faker.Random.Long(1), CodigoRf = rf });
        }
        private (FechamentoTurmaDisciplinaDto fechamentoDto, Turma turma) PrepararCenarioBaseSalvar()
        {
            var perfilAtual = Guid.NewGuid();
            var usuario = new Usuario
            {
                Id = 1,
                Nome = _faker.Person.FullName,
                CodigoRf = _faker.Person.Cpf(),
                PerfilAtual = perfilAtual
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
                {
                    new PrioridadePerfil { Tipo = TipoPerfil.SME, CodigoPerfil = perfilAtual },
                    new PrioridadePerfil { Tipo = TipoPerfil.DRE, CodigoPerfil = perfilAtual },
                    new PrioridadePerfil { Tipo = TipoPerfil.UE, CodigoPerfil = perfilAtual }
                });
            var turma = new Turma
            {
                Id = 1,
                AnoLetivo = DateTime.Now.Year,
                CodigoTurma = "T1",
                Nome = "Turma Teste",
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ue = new Ue { CodigoUe = "UE1", Dre = new Dre { CodigoDre = "DRE1" } },
                TipoTurma = TipoTurma.Regular
            };

            var fechamentoDto = new FechamentoTurmaDisciplinaDto
            {
                TurmaId = turma.CodigoTurma,
                DisciplinaId = 1,
                Bimestre = 1,
                NotaConceitoAlunos = new List<FechamentoNotaDto>
                {
                    new FechamentoNotaDto { CodigoAluno = "A1", DisciplinaId = 1, Nota = 9.5 }
                }
            };

            var periodoEscolar = new PeriodoEscolar { Id = 1, Bimestre = 1, PeriodoInicio = DateTime.Now.AddMonths(-1), PeriodoFim = DateTime.Now };
            var periodoFechamento = new FechamentoDto
            {
                FechamentosBimestres = new List<FechamentoBimestreDto>
                {
                    new FechamentoBimestreDto { Bimestre = 1, InicioDoFechamento = DateTime.Now.AddDays(-5), FinalDoFechamento = DateTime.Now.AddDays(5), PeriodoEscolar = periodoEscolar }
                }
            };

            var disciplinaEol = new DisciplinaDto { Id = 1, RegistraFrequencia = true };

            var alunosAtivos = fechamentoDto.NotaConceitoAlunos.Select(a => new AlunoPorTurmaResposta { CodigoAluno = a.CodigoAluno, NomeAluno = $"Nome {a.CodigoAluno}" }).ToList();

            // Mocks Padrão
            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuario);
            _servicoUsuarioMock.Setup(s => s.PodePersistirTurmaDisciplina(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Usuario>())).ReturnsAsync(true);
            _repositorioTurmaConsultaMock.Setup(r => r.ObterTurmaComUeEDrePorCodigo(turma.CodigoTurma)).ReturnsAsync(turma);
            _repositorioTurmaConsultaMock.Setup(r => r.ObterTurmaComUeEDrePorId(turma.Id)).ReturnsAsync(turma);
            _repositorioTipoCalendarioMock.Setup(r => r.BuscarPorAnoLetivoEModalidade(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).ReturnsAsync(new TipoCalendario { Id = 1 });
            _servicoPeriodoFechamentoMock.Setup(s => s.ObterPorTipoCalendarioSme(It.IsAny<long>(), It.IsAny<Aplicacao>())).ReturnsAsync(periodoFechamento);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ParametrosSistema { Ativo = false });
            _mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _consultasDisciplinaMock.Setup(c => c.ObterDisciplina(fechamentoDto.DisciplinaId)).ReturnsAsync(disciplinaEol);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaTipoValorPorTurmaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new NotaTipoValor { TipoNota = TipoNota.Nota });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(alunosAtivos);
            _repositorioFechamentoTurmaDisciplinaMock.Setup(r => r.ObterPorId(It.IsAny<long>())).Returns(new FechamentoTurmaDisciplina { FechamentoTurma = new FechamentoTurma() });
            _repositorioCacheMock.Setup(c => c.ObterObjetoAsync<List<NotaConceitoBimestreComponenteDto>>(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(new List<NotaConceitoBimestreComponenteDto>());
            _repositorioEventoTipoMock.Setup(r => r.ObterPorCodigo(It.IsAny<long>())).Returns(new EventoTipo { Id = 1, Codigo = 1, Descricao = "Fechamento de Notas" });

            return (fechamentoDto, turma);
        }
        #endregion
    }
}
