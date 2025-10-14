using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using GrupoMatriz = SME.SGP.Aplicacao.Integracoes.Respostas.GrupoMatriz;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterSinteseConselhoDeClasseUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterSinteseConselhoDeClasseUseCase _useCase;

        public ObterSinteseConselhoDeClasseUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterSinteseConselhoDeClasseUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterSinteseConselhoDeClasseUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Nao_Encontrar_Disciplinas_Deve_Retornar_Nulo()
        {
            var conselhoDto = new ConselhoClasseSinteseDto(1, 1, "123456", "987654", 1);
            var fechamentoTurma = new FechamentoTurma { Turma = new Turma() };

            SetupMocksIniciais(fechamentoTurma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDisciplinasPorCodigoTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Enumerable.Empty<DisciplinaResposta>());

            var resultado = await _useCase.Executar(conselhoDto);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Executar_Quando_Fechamento_Nulo_E_Turma_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            var conselhoDto = new ConselhoClasseSinteseDto(1, 1, "12345", "98765", 0);
            SetupMocksIniciais(null);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Turma)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(conselhoDto));
        }

        [Fact]
        public async Task Executar_Quando_Fechamento_Nulo_E_Turma_Nao_For_Ano_Anterior_Deve_Lancar_NegocioException()
        {
            var conselhoDto = new ConselhoClasseSinteseDto(1, 1, "12345", "98765", 0);
            var turma = new Turma { AnoLetivo = DateTime.Now.Year };
            SetupMocksIniciais(null);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(conselhoDto));
        }

        [Fact]
        public async Task Executar_Quando_Bimestre_Final_Ano_Atual_Sem_Conselho_Deve_Lancar_NegocioException()
        {
            var conselhoDto = new ConselhoClasseSinteseDto(1, 1, "12345", "98765", 0);
            var turma = new Turma { AnoLetivo = DateTime.Now.Year };
            var fechamentoTurma = new FechamentoTurma { Turma = turma };
            SetupMocksIniciais(fechamentoTurma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExisteConselhoClasseUltimoBimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(conselhoDto));
        }

        [Fact]
        public async Task Executar_Quando_Bimestre_Nao_Final_Deve_Mapear_Sintese_Corretamente()
        {
            var conselhoDto = new ConselhoClasseSinteseDto(1, 1, "12345", "98765", 1);
            var turma = new Turma { AnoLetivo = DateTime.Now.Year, CodigoTurma = "turma1", ModalidadeCodigo = Modalidade.Fundamental };
            var fechamentoTurma = new FechamentoTurma { Turma = turma, PeriodoEscolar = new PeriodoEscolar() };
            var disciplinas = new List<DisciplinaResposta> {
             new DisciplinaResposta { CodigoComponenteCurricular = 123, LancaNota = false, GrupoMatriz = new GrupoMatriz { Id = 1, Nome = "Grupo 1" } } };

            var frequencia = new List<FrequenciaAluno>
             {
                 new FrequenciaAluno
                 {
                     DisciplinaId = "123",
                     TotalAulas = 20,
                     TotalAusencias = 1,
                     TotalCompensacoes = 0
                 }
             };

            SetupMocksIniciais(fechamentoTurma);
            SetupMocksFluxoPrincipal(turma, disciplinas, frequencia);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExisteConselhoClasseUltimoBimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAulasNaoLancamNotaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TotalAulasNaoLancamNotaDto>());

            var resultado = await _useCase.Executar(conselhoDto);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Grupo 1", resultado.First().Titulo);
            Assert.Equal("95,00%", resultado.First().ComponenteSinteses.First().PercentualFrequencia);
        }

        [Fact]
        public async Task Executar_Quando_Bimestre_Final_E_Componente_Permite_Frequencia_Deve_Mapear_Sintese()
        {
            var conselhoDto = new ConselhoClasseSinteseDto(1, 1, "12345", "98765", 0);
            var turma = new Turma { AnoLetivo = 2025, CodigoTurma = "turma1", ModalidadeCodigo = Modalidade.Fundamental };
            var fechamentoTurma = new FechamentoTurma { Turma = turma };
            var disciplinas = new List<DisciplinaResposta> {
              new DisciplinaResposta { CodigoComponenteCurricular = 123, LancaNota = false, GrupoMatriz = new GrupoMatriz { Id = 1, Nome = "Grupo 1" } } };

            var totalAulas = new List<TotalAulasNaoLancamNotaDto>
            {
                new TotalAulasNaoLancamNotaDto { DisciplinaId = 123, TotalAulas = "10" }
            };
           
            SetupMocksIniciais(fechamentoTurma);
            SetupMocksFluxoPrincipal(turma, disciplinas, new List<FrequenciaAluno>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExisteConselhoClasseUltimoBimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteRegistraFrequenciaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(totalAulas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterSinteseAlunoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SinteseDto { Valor = "Aprovado" });

            var resultado = await _useCase.Executar(conselhoDto);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Aprovado", resultado.First().ComponenteSinteses.First().ParecerFinal);
        }

        [Fact]
        public async Task Executar_Quando_Bimestre_Final_E_Componente_Nao_Permite_Frequencia_Deve_Mapear_Sintese()
        {
            var conselhoDto = new ConselhoClasseSinteseDto(1, 1, "12345", "98765", 0);
            var turma = new Turma { AnoLetivo = DateTime.Now.Year, CodigoTurma = "98765", ModalidadeCodigo = Modalidade.Fundamental };
            var fechamentoTurma = new FechamentoTurma { Turma = turma };
            var disciplinas = new List<DisciplinaResposta> {
                new DisciplinaResposta { CodigoComponenteCurricular = 123, LancaNota = false, GrupoMatriz = new SME.SGP.Aplicacao.Integracoes.Respostas.GrupoMatriz { Id = 1, Nome = "Grupo 1" } }
            };

            SetupMocksIniciais(fechamentoTurma);
            SetupMocksFluxoPrincipal(turma, disciplinas, new List<FrequenciaAluno>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExisteConselhoClasseUltimoBimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteRegistraFrequenciaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAulasSemFrequenciaPorTurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<TotalAulasPorAlunoTurmaDto>());

            var resultado = await _useCase.Executar(conselhoDto);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        private void SetupMocksIniciais(FechamentoTurma fechamentoTurma)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalCompensacoesComponenteNaoLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TotalCompensacoesComponenteNaoLancaNotaDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaPorIdAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(fechamentoTurma);
        }

        private void SetupMocksFluxoPrincipal(Turma turma, IEnumerable<DisciplinaResposta> disciplinas, IEnumerable<FrequenciaAluno> frequencias)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDisciplinasPorCodigoTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciasAlunoComponentePorTurmasQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(frequencias);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new PeriodoEscolar());
        }
    }
}
