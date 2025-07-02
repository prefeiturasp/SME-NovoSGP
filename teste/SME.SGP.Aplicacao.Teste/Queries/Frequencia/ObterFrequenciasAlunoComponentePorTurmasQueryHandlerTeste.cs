using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciasAlunoComponentePorTurmasQueryHandlerTeste
    {
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterFrequenciasAlunoComponentePorTurmasQueryHandler _handler;
        private readonly Faker _faker;

        public ObterFrequenciasAlunoComponentePorTurmasQueryHandlerTeste()
        {
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new ObterFrequenciasAlunoComponentePorTurmasQueryHandler(_repositorioFrequenciaMock.Object, _mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve consolidar frequências e complementar com aulas sem registro")]
        public async Task Handle_QuandoExecutado_DeveConsolidarEComplementarFrequencias()
        {
            // Arrange
            var alunoCodigo = "123";
            var turmasCodigos = new[] { "T1" };
            var tipoCalendarioId = 1L;
            var disciplinaIdExistente = "D1";
            var disciplinaIdNova = "D2";

            var query = new ObterFrequenciasAlunoComponentePorTurmasQuery(alunoCodigo, turmasCodigos, tipoCalendarioId, null, 0);

            var frequenciaExistente = new List<FrequenciaAluno>
            {
                new FrequenciaAluno { DisciplinaId = disciplinaIdExistente, TotalAulas = 10, TotalAusencias = 2, Bimestre = 1 }
            };

            var aulasComponentes = new List<TurmaComponenteQntAulasDto>
            {
                // Componente que já tem frequência
                new TurmaComponenteQntAulasDto { TurmaCodigo = "T1", ComponenteCurricularCodigo = disciplinaIdExistente, Bimestre = 1 },
                // Novo componente que não tem frequência registrada, mas tem aulas
                new TurmaComponenteQntAulasDto { TurmaCodigo = "T1", ComponenteCurricularCodigo = disciplinaIdNova, Bimestre = 2, AulasQuantidade = 5 }
            };

            _repositorioFrequenciaMock.Setup(r => r.ObterFrequenciaComponentesAlunoPorTurmas(alunoCodigo, turmasCodigos, tipoCalendarioId, 0))
                .ReturnsAsync(frequenciaExistente);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new int[] { 1, 2 });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulasComponentes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, resultado.Count()); // Deve retornar os 2 componentes (o existente e o novo)

            var freqExistenteConsolidada = resultado.FirstOrDefault(f => f.DisciplinaId == disciplinaIdExistente);
            Assert.NotNull(freqExistenteConsolidada);
            Assert.Equal(10, freqExistenteConsolidada.TotalAulas);
            Assert.Equal(2, freqExistenteConsolidada.TotalAusencias);

            var freqNovaConsolidada = resultado.FirstOrDefault(f => f.DisciplinaId == disciplinaIdNova);
            Assert.NotNull(freqNovaConsolidada);
            Assert.Equal(0, freqNovaConsolidada.TotalAulas); // A soma do total de aulas vem do objeto FrequenciaAluno, que para o novo é 0.
        }

        [Fact(DisplayName = "Deve agrupar e somar as frequências por componente")]
        public async Task Handle_QuandoComponenteTemMultiplosBimestres_DeveAgruparESomar()
        {
            // Arrange
            var alunoCodigo = "123";
            var turmasCodigos = new[] { "T1" };
            var tipoCalendarioId = 1L;
            var disciplinaId = "D1";

            var query = new ObterFrequenciasAlunoComponentePorTurmasQuery(alunoCodigo, turmasCodigos, tipoCalendarioId, null, 0);

            var frequenciasBimestrais = new List<FrequenciaAluno>
            {
                new FrequenciaAluno { DisciplinaId = disciplinaId, TotalAulas = 10, TotalAusencias = 2, TotalCompensacoes = 1, Bimestre = 1 },
                new FrequenciaAluno { DisciplinaId = disciplinaId, TotalAulas = 12, TotalAusencias = 3, TotalCompensacoes = 0, Bimestre = 2 }
            };

            var aulasComponentes = new List<TurmaComponenteQntAulasDto>
            {
                new TurmaComponenteQntAulasDto { TurmaCodigo = "T1", ComponenteCurricularCodigo = disciplinaId, Bimestre = 1 },
                new TurmaComponenteQntAulasDto { TurmaCodigo = "T1", ComponenteCurricularCodigo = disciplinaId, Bimestre = 2 }
            };

            _repositorioFrequenciaMock.Setup(r => r.ObterFrequenciaComponentesAlunoPorTurmas(alunoCodigo, turmasCodigos, tipoCalendarioId, 0))
                .ReturnsAsync(frequenciasBimestrais);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new int[] { 1, 2 });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulasComponentes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var freqConsolidada = Assert.Single(resultado);
            Assert.Equal(disciplinaId, freqConsolidada.DisciplinaId);
            Assert.Equal(22, freqConsolidada.TotalAulas); // 10 + 12
            Assert.Equal(5, freqConsolidada.TotalAusencias); // 2 + 3
            Assert.Equal(1, freqConsolidada.TotalCompensacoes); // 1 + 0
        }
    }
}
