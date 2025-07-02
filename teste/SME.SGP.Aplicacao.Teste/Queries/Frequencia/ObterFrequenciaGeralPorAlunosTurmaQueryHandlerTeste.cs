using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciaGeralPorAlunosTurmaQueryHandlerTeste
    {
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaMock;
        private readonly ObterFrequenciaGeralPorAlunosTurmaQueryHandler _handler;

        public ObterFrequenciaGeralPorAlunosTurmaQueryHandlerTeste()
        {
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ObterFrequenciaGeralPorAlunosTurmaQueryHandler(_repositorioFrequenciaMock.Object);
        }

        [Fact(DisplayName = "Deve agrupar e somar as frequências por aluno")]
        public async Task Handle_QuandoExecutado_DeveAgruparESomarFrequenciasPorAluno()
        {
            // Arrange
            var aluno1Codigo = "ALUNO1";
            var aluno2Codigo = "ALUNO2";
            var turmaCodigo = "TURMA1";
            var query = new ObterFrequenciaGeralPorAlunosTurmaQuery(new[] { aluno1Codigo, aluno2Codigo }, turmaCodigo);

            var frequenciasDoRepositorio = new List<FrequenciaAluno>
            {
                // Frequências Aluno 1
                new FrequenciaAluno { CodigoAluno = aluno1Codigo, TotalAulas = 20, TotalAusencias = 2, TotalCompensacoes = 1 },
                new FrequenciaAluno { CodigoAluno = aluno1Codigo, TotalAulas = 15, TotalAusencias = 1, TotalCompensacoes = 0 },
                // Frequências Aluno 2
                new FrequenciaAluno { CodigoAluno = aluno2Codigo, TotalAulas = 20, TotalAusencias = 5, TotalCompensacoes = 2 }
            };

            _repositorioFrequenciaMock.Setup(r => r.ObterFrequenciaGeralPorAlunosETurmas(query.CodigosAlunos, query.CodigoTurma))
                .ReturnsAsync(frequenciasDoRepositorio);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count()); // Deve retornar 2 registros, um para cada aluno.

            var freqAluno1 = resultado.FirstOrDefault(f => f.CodigoAluno == aluno1Codigo);
            Assert.NotNull(freqAluno1);
            Assert.Equal(35, freqAluno1.TotalAulas);         // 20 + 15
            Assert.Equal(3, freqAluno1.TotalAusencias);        // 2 + 1
            Assert.Equal(1, freqAluno1.TotalCompensacoes);      // 1 + 0

            var freqAluno2 = resultado.FirstOrDefault(f => f.CodigoAluno == aluno2Codigo);
            Assert.NotNull(freqAluno2);
            Assert.Equal(20, freqAluno2.TotalAulas);
            Assert.Equal(5, freqAluno2.TotalAusencias);
            Assert.Equal(2, freqAluno2.TotalCompensacoes);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando o repositório não encontrar frequências")]
        public async Task Handle_QuandoRepositorioRetornaVazio_DeveRetornarVazio()
        {
            // Arrange
            var query = new ObterFrequenciaGeralPorAlunosTurmaQuery(new[] { "ALUNO_INEXISTENTE" }, "TURMA_X");

            _repositorioFrequenciaMock.Setup(r => r.ObterFrequenciaGeralPorAlunosETurmas(query.CodigosAlunos, query.CodigoTurma))
                .ReturnsAsync(new List<FrequenciaAluno>());

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
