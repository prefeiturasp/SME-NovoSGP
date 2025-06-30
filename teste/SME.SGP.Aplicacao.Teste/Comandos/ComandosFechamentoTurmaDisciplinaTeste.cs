using Bogus;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosFechamentoTurmaDisciplinaTeste
    {
        private readonly Mock<IServicoFechamentoTurmaDisciplina> _servicoFechamentoTurmaDisciplinaMock;
        private readonly Mock<IRepositorioFechamentoTurmaConsulta> _repositorioFechamentoTurmaMock;
        private readonly Mock<IRepositorioFechamentoTurmaDisciplinaConsulta> _repositorioFechamentoTurmaDisciplinaMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ComandosFechamentoTurmaDisciplina _comandosHandler;
        private readonly Faker _faker;

        public ComandosFechamentoTurmaDisciplinaTeste()
        {
            _servicoFechamentoTurmaDisciplinaMock = new Mock<IServicoFechamentoTurmaDisciplina>();
            _repositorioFechamentoTurmaMock = new Mock<IRepositorioFechamentoTurmaConsulta>();
            _repositorioFechamentoTurmaDisciplinaMock = new Mock<IRepositorioFechamentoTurmaDisciplinaConsulta>();
            _mediatorMock = new Mock<IMediator>();
            _faker = new Faker("pt_BR");

            _comandosHandler = new ComandosFechamentoTurmaDisciplina(
                _servicoFechamentoTurmaDisciplinaMock.Object,
                _repositorioFechamentoTurmaMock.Object,
                _repositorioFechamentoTurmaDisciplinaMock.Object,
                _mediatorMock.Object
            );
        }

        [Fact(DisplayName = "Deve Disparar Exceção ao Instanciar Sem Dependências")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ComandosFechamentoTurmaDisciplina(null, _repositorioFechamentoTurmaMock.Object, _repositorioFechamentoTurmaDisciplinaMock.Object, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosFechamentoTurmaDisciplina(_servicoFechamentoTurmaDisciplinaMock.Object, null, _repositorioFechamentoTurmaDisciplinaMock.Object, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosFechamentoTurmaDisciplina(_servicoFechamentoTurmaDisciplinaMock.Object, _repositorioFechamentoTurmaMock.Object, null, _mediatorMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosFechamentoTurmaDisciplina(_servicoFechamentoTurmaDisciplinaMock.Object, _repositorioFechamentoTurmaMock.Object, _repositorioFechamentoTurmaDisciplinaMock.Object, null));
        }

        [Fact(DisplayName = "Deve Reprocessar Fechamento por Id")]
        public async Task DeveReprocessar_QuandoRecebeId_DeveChamarServico()
        {
            // Arrange
            var fechamentoId = _faker.Random.Long(1);
            var usuario = new Usuario { CodigoRf = _faker.Random.AlphaNumeric(7) };

            _servicoFechamentoTurmaDisciplinaMock.Setup(s => s.Reprocessar(fechamentoId, usuario))
                .Returns(Task.CompletedTask);

            // Act
            await _comandosHandler.Reprocessar(fechamentoId, usuario);

            // Assert
            _servicoFechamentoTurmaDisciplinaMock.Verify(s => s.Reprocessar(fechamentoId, usuario), Times.Once);
        }

        [Fact(DisplayName = "Deve Reprocessar Múltiplos Fechamentos por Lista de Ids")]
        public async Task DeveReprocessar_QuandoRecebeListaDeIds_DeveChamarServicoParaCadaId()
        {
            // Arrange
            var fechamentoIds = new List<long> { _faker.Random.Long(1), _faker.Random.Long(2), _faker.Random.Long(3) };
            var usuario = new Usuario { CodigoRf = _faker.Random.AlphaNumeric(7) };

            _servicoFechamentoTurmaDisciplinaMock.Setup(s => s.Reprocessar(It.IsAny<long>(), usuario))
                .Returns(Task.CompletedTask);

            // Act
            await _comandosHandler.Reprocessar(fechamentoIds, usuario);

            // Assert
            _servicoFechamentoTurmaDisciplinaMock.Verify(s => s.Reprocessar(It.IsAny<long>(), usuario), Times.Exactly(fechamentoIds.Count));
            foreach (var id in fechamentoIds)
            {
                _servicoFechamentoTurmaDisciplinaMock.Verify(s => s.Reprocessar(id, usuario), Times.Once);
            }
        }

        [Fact(DisplayName = "Deve Salvar Fechamento com Sucesso")]
        public async Task DeveSalvar_QuandoDadosValidos_DeveRetornarSucesso()
        {
            // Arrange
            var fechamentosDto = new List<FechamentoTurmaDisciplinaDto> { CriarFechamentoDtoFalso() };
            var turma = new Turma { Id = long.Parse(fechamentosDto.First().TurmaId), CodigoTurma = fechamentosDto.First().TurmaId, AnoLetivo = DateTime.Now.Year };

            ConfigurarMocksSalvar(fechamentosDto.First(), turma, 100); // Justificativa com 100 caracteres

            _servicoFechamentoTurmaDisciplinaMock.Setup(s => s.Salvar(It.IsAny<long>(), It.IsAny<FechamentoTurmaDisciplinaDto>(), false, false))
                .ReturnsAsync(new AuditoriaPersistenciaDto { Sucesso = true });

            // Act
            var resultado = await _comandosHandler.Salvar(fechamentosDto);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.All(r => r.Sucesso));
            _servicoFechamentoTurmaDisciplinaMock.Verify(s => s.Salvar(It.IsAny<long>(), It.IsAny<FechamentoTurmaDisciplinaDto>(), false, false), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverChaveCacheCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact(DisplayName = "Não Deve Salvar com Justificativa Longa e Deve Lançar Exceção")]
        public async Task NaoDeveSalvar_QuandoJustificativaExcedeLimite_DeveLancarNegocioException()
        {
            // Arrange
            var fechamentosDto = new List<FechamentoTurmaDisciplinaDto> { CriarFechamentoDtoFalso() };
            var turma = new Turma { Id = long.Parse(fechamentosDto.First().TurmaId), CodigoTurma = fechamentosDto.First().TurmaId, AnoLetivo = DateTime.Now.Year };
            var limite = int.Parse(FechamentoTurmaDisciplinaEnum.TamanhoCampoJustificativa.Description());

            ConfigurarMocksSalvar(fechamentosDto.First(), turma, limite + 1); // Justificativa com tamanho acima do limite

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NegocioException>(() => _comandosHandler.Salvar(fechamentosDto));
            Assert.Contains($"Justificativa não pode ter mais que {limite} caracteres", exception.Message);

            _servicoFechamentoTurmaDisciplinaMock.Verify(s => s.Salvar(It.IsAny<long>(), It.IsAny<FechamentoTurmaDisciplinaDto>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact(DisplayName = "Deve Lançar Exceção Quando Nenhum Fechamento for Salvo com Sucesso")]
        public async Task DeveLancarExcecao_QuandoNenhumItemSalvoComSucesso_DeveLancarNegocioException()
        {
            // Arrange
            var fechamentosDto = new List<FechamentoTurmaDisciplinaDto> { CriarFechamentoDtoFalso() };
            var turma = new Turma { Id = long.Parse(fechamentosDto.First().TurmaId), CodigoTurma = fechamentosDto.First().TurmaId, AnoLetivo = DateTime.Now.Year };

            ConfigurarMocksSalvar(fechamentosDto.First(), turma, 100);

            _servicoFechamentoTurmaDisciplinaMock.Setup(s => s.Salvar(It.IsAny<long>(), It.IsAny<FechamentoTurmaDisciplinaDto>(), false, false))
                .ThrowsAsync(new Exception("Erro simulado no serviço."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NegocioException>(() => _comandosHandler.Salvar(fechamentosDto));
            Assert.Contains("Erro simulado no serviço", exception.Message);
        }

        [Fact(DisplayName = "Deve Processar Fechamentos Pendentes")]
        public async Task DeveProcessarPendentes_QuandoExistemPendentes_DeveExecutarFluxoCompleto()
        {
            // Arrange
            var anoLetivo = DateTime.Now.Year;
            var fechamentosPendentes = new List<FechamentoTurmaDisciplina>
            {
                new FechamentoTurmaDisciplina { Id = 1, FechamentoTurmaId = 10, CriadoRF = "1234567" }
            };

            var fechamentoTurma = new FechamentoTurma { Id = 10, TurmaId = 100, PeriodoEscolarId = 20 };
            var turma = new Turma { Id = 100 };
            var periodoEscolar = new PeriodoEscolar { Id = 20 };
            var usuario = new Usuario { CodigoRf = "1234567" };

            _repositorioFechamentoTurmaDisciplinaMock.Setup(r => r.ObterFechamentosComSituacaoEmProcessamentoPorAnoLetivo(anoLetivo))
                .ReturnsAsync(fechamentosPendentes);
            _repositorioFechamentoTurmaMock.Setup(r => r.ObterPorIdAsync(fechamentoTurma.Id))
                .ReturnsAsync(fechamentoTurma);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == turma.Id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarePorIdQuery>(q => q.Id == periodoEscolar.Id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterUsuarioPorRfQuery>(q => q.CodigoRf == usuario.CodigoRf), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            // Act
            await _comandosHandler.ProcessarPendentes(anoLetivo);

            // Assert
            _repositorioFechamentoTurmaMock.Verify(r => r.ObterPorIdAsync(fechamentoTurma.Id), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPeriodoEscolarePorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPorRfQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Não Deve Processar Pendente Sem Período Escolar")]
        public async Task NaoDeveProcessarPendentes_QuandoNaoTemPeriodoEscolar_DeveIgnorar()
        {
            // Arrange
            var anoLetivo = DateTime.Now.Year;
            var fechamentosPendentes = new List<FechamentoTurmaDisciplina>
            {
                new FechamentoTurmaDisciplina { Id = 1, FechamentoTurmaId = 10 }
            };
            // PeriodoEscolarId é nulo
            var fechamentoTurma = new FechamentoTurma { Id = 10, TurmaId = 100, PeriodoEscolarId = null };

            _repositorioFechamentoTurmaDisciplinaMock.Setup(r => r.ObterFechamentosComSituacaoEmProcessamentoPorAnoLetivo(anoLetivo))
                .ReturnsAsync(fechamentosPendentes);
            _repositorioFechamentoTurmaMock.Setup(r => r.ObterPorIdAsync(fechamentoTurma.Id))
                .ReturnsAsync(fechamentoTurma);

            // Act
            await _comandosHandler.ProcessarPendentes(anoLetivo);

            // Assert
            _repositorioFechamentoTurmaMock.Verify(r => r.ObterPorIdAsync(fechamentoTurma.Id), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPeriodoEscolarePorIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPorRfQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #region Métodos Privados

        private FechamentoTurmaDisciplinaDto CriarFechamentoDtoFalso()
        {
            return new FechamentoTurmaDisciplinaDto
            {
                Id = _faker.Random.Long(1, 1000),
                TurmaId = _faker.Random.Long(1, 1000).ToString(),
                Bimestre = _faker.Random.Int(1, 4),
                DisciplinaId = _faker.Random.Long(1, 1000),
                Justificativa = _faker.Lorem.Sentence(10),
                NotaConceitoAlunos = new List<FechamentoNotaDto>
                {
                    new FechamentoNotaDto { CodigoAluno = _faker.Random.Long(1, 1000).ToString() }
                }
            };
        }

        private void ConfigurarMocksSalvar(FechamentoTurmaDisciplinaDto dto, Turma turma, int tamanhoJustificativa)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTamanhoCaracteresJustificativaNotaQuery>(q => q.Justificativa == dto.Justificativa), It.IsAny<CancellationToken>()))
               .ReturnsAsync(tamanhoJustificativa);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == dto.TurmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQuery>(q => q.TurmaCodigo == dto.TurmaId && q.Bimestre == dto.Bimestre), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_faker.Random.Long(1));

            _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverChaveCacheCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
        }

        #endregion
    }
}