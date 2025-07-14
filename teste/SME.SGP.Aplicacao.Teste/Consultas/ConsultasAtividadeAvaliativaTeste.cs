using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ConsultasAtividadeAvaliativaTeste
    {
        private readonly ConsultaAtividadeAvaliativa _consulta;
        private readonly Mock<IConsultasProfessor> _consultasProfessorMock = new();
        private readonly Mock<IRepositorioAtividadeAvaliativa> _repositorioAtividadeAvaliativaMock = new();
        private readonly Mock<IRepositorioAtividadeAvaliativaRegencia> _repositorioRegenciaMock = new();
        private readonly Mock<IRepositorioAtividadeAvaliativaDisciplina> _repositorioDisciplinaMock = new();
        private readonly Mock<IRepositorioPeriodoEscolarConsulta> _repositorioPeriodoEscolarMock = new();
        private readonly Mock<IRepositorioAulaConsulta> _repositorioAulaMock = new();
        private readonly Mock<IRepositorioAtribuicaoCJ> _repositorioAtribuicaoCJMock = new();
        private readonly Mock<IServicoUsuario> _servicoUsuarioMock = new();
        private readonly Mock<IConsultasTurma> _consultasTurmaMock = new();
        private readonly Mock<IConsultasPeriodoEscolar> _consultasPeriodoEscolarMock = new();
        private readonly Mock<IConsultasPeriodoFechamento> _consultasPeriodoFechamentoMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<IContextoAplicacao> _contextoMock = new();

        public ConsultasAtividadeAvaliativaTeste()
        {
            _consulta = new ConsultaAtividadeAvaliativa(
                _consultasProfessorMock.Object,
                _repositorioAtividadeAvaliativaMock.Object,
                _repositorioRegenciaMock.Object,
                _repositorioDisciplinaMock.Object,
                _repositorioPeriodoEscolarMock.Object,
                _repositorioAulaMock.Object,
                _repositorioAtribuicaoCJMock.Object,
                _servicoUsuarioMock.Object,
                _contextoMock.Object,
                _consultasTurmaMock.Object,
                _consultasPeriodoEscolarMock.Object,
                _consultasPeriodoFechamentoMock.Object,
                _mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve listar atividades avaliativas paginadas")]
        public async Task ObterListaAtividadeAvaliativaPaginada()
        {
            var filtro = new FiltroAtividadeAvaliativaDto();
            _repositorioAtividadeAvaliativaMock
                .Setup(x => x.Listar(
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<string>(),
                    It.IsAny<Paginacao>()
                ))
                .ReturnsAsync(new PaginacaoResultadoDto<AtividadeAvaliativa>
                {
                    Items = new List<AtividadeAvaliativa>(),
                    TotalPaginas = 1,
                    TotalRegistros = 0
                });

            var resultado = await _consulta.ListarPaginado(filtro);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.Items);
        }

        [Fact(DisplayName = "Deve retornar atividade avaliativa por ID")]
        public async Task ObterAtividadeAvaliativaPorID()
        {
            var id = 1L;
            _servicoUsuarioMock.Setup(x => x.ObterUsuarioLogado()).ReturnsAsync(new Usuario { PerfilAtual = SME.SGP.Dominio.Perfis.PERFIL_PROFESSOR });
            _repositorioAtividadeAvaliativaMock.Setup(x => x.ObterPorIdAsync(id))
                .ReturnsAsync(new AtividadeAvaliativa { Id = id, EhCj = false, EhRegencia = false });
            _repositorioDisciplinaMock.Setup(x => x.ListarPorIdAtividade(id)).ReturnsAsync(new List<AtividadeAvaliativaDisciplina>());
            _consultasPeriodoFechamentoMock.Setup(x => x.TurmaEmPeriodoDeFechamento(It.IsAny<Turma>(), It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(true);
            _consultasPeriodoEscolarMock.Setup(x => x.ObterBimestre(It.IsAny<DateTime>(), It.IsAny<Modalidade>(), It.IsAny<int>())).ReturnsAsync(1);
            _consultasTurmaMock.Setup(x => x.ObterComUeDrePorCodigo(It.IsAny<string>())).ReturnsAsync(new Turma());

            var result = await _consulta.ObterPorIdAsync(id);
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Deve retornar se atividade está dentro do período")]
        public async Task ObterAtividadeAvaliativaDentroPeriodo_DeveRetornarTrue()
        {
            var turma = new Turma { ModalidadeCodigo = Modalidade.Fundamental, Ano = "2", AnoLetivo = DateTime.Today.Year };
            _consultasTurmaMock.Setup(x => x.ObterComUeDrePorCodigo("T1")).ReturnsAsync(turma);
            _consultasPeriodoEscolarMock.Setup(x => x.ObterBimestre(It.IsAny<DateTime>(), It.IsAny<Modalidade>(), It.IsAny<int>())).ReturnsAsync(1);

            var resultado = await _consulta.AtividadeAvaliativaDentroPeriodo("T1", DateTime.Today);
            Assert.True(resultado);
        }

        [Fact(DisplayName = "Deve lançar exceção se turma não for encontrada")]
        public async Task ObterAtividadeAvaliativaDentroPeriodo_TurmaNaoEncontrada_DeveLancarExcecao()
        {
            _consultasTurmaMock.Setup(x => x.ObterComUeDrePorCodigo("T1")).ReturnsAsync((Turma)null);
            await Assert.ThrowsAsync<NegocioException>(() => _consulta.AtividadeAvaliativaDentroPeriodo("T1", DateTime.Today));
        }

        [Fact(DisplayName = "Deve obter avaliações no bimestre")]
        public async Task ObterAvaliacoesAvaliativasNoBimestre()
        {
            // Arrange
            var turmaCodigo = "123";
            var disciplinaId = "456";
            var periodoInicio = new DateTime(2024, 3, 1);
            var periodoFim = new DateTime(2024, 6, 30);

            var avaliacoesEsperadas = new List<AtividadeAvaliativa>
    {
        new AtividadeAvaliativa { Id = 1, TurmaId = turmaCodigo, DataAvaliacao = new DateTime(2024, 4, 10) },
        new AtividadeAvaliativa { Id = 2, TurmaId = turmaCodigo, DataAvaliacao = new DateTime(2024, 5, 5) }
    };

            _repositorioAtividadeAvaliativaMock
                .Setup(x => x.ListarPorTurmaDisciplinaPeriodo(turmaCodigo, disciplinaId, periodoInicio, periodoFim))
                .ReturnsAsync(avaliacoesEsperadas);

            // Act
            var resultado = await _consulta.ObterAvaliacoesNoBimestre(turmaCodigo, disciplinaId, periodoInicio, periodoFim);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            _repositorioAtividadeAvaliativaMock.Verify(x => x.ListarPorTurmaDisciplinaPeriodo(turmaCodigo, disciplinaId, periodoInicio, periodoFim), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar true se atividade avaliativa estiver dentro do período")]
        public async Task ObterAtividadeAvaliativaDentroPeriodo_QuandoValido()
        {
            // Arrange
            var atividade = new AtividadeAvaliativa
            {
                TurmaId = "turma-01",
                DataAvaliacao = new DateTime(2024, 3, 1)
            };

            var turma = new Turma
            {
                CodigoTurma = "1",
                Nome = "Turma Teste",
            };

            _consultasTurmaMock
                .Setup(x => x.ObterComUeDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            _consultasPeriodoEscolarMock
                .Setup(x => x.ObterBimestre(It.IsAny<DateTime>(), It.IsAny<Modalidade>(), It.IsAny<int>()))
                .ReturnsAsync(2);

            _consultasPeriodoFechamentoMock
                .Setup(x => x.TurmaEmPeriodoDeFechamento(It.IsAny<Turma>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await _consulta.AtividadeAvaliativaDentroPeriodo(atividade);

            // Assert
            Assert.True(resultado);
        }
    }
}