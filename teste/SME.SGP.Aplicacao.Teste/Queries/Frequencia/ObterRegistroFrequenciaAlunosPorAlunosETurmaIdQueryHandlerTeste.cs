using Bogus;
using Moq;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioRegistroFrequenciaAlunoConsulta> _repositorioFrequenciaAlunoMock;
        private readonly Mock<IRepositorioCache> _repositorioCacheMock;
        private readonly ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandler _handler;

        public ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandlerTeste()
        {
            _repositorioFrequenciaAlunoMock = new Mock<IRepositorioRegistroFrequenciaAlunoConsulta>();
            _repositorioCacheMock = new Mock<IRepositorioCache>();
            _handler = new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandler(
                _repositorioFrequenciaAlunoMock.Object,
                _repositorioCacheMock.Object);
        }

        [Fact(DisplayName = "Deve lançar exceção quando as dependências forem nulas")]
        public void DeveLancarExcecao_QuandoDependenciasForemNulas()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandler(null, _repositorioCacheMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandler(_repositorioFrequenciaAlunoMock.Object, null));
        }

        [Fact(DisplayName = "Deve chamar repositório com Ids desconsiderados vazios quando não houver dados no cache")]
        public async Task DeveChamarRepositorio_ComIdsDesconsideradosVazios_QuandoCacheVazio()
        {
            // Arrange
            var turmasId = new[] { "100", "200" };
            var query = new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery(DateTime.Now, new[] { "ALUNO1" }, "PROF1", turmasId);
            var retornoEsperado = new List<RegistroFrequenciaPorDisciplinaAlunoDto>();

            _repositorioCacheMock.Setup(r => r.ObterAsync(It.IsAny<string>(), false)).ReturnsAsync((string)null);

            _repositorioFrequenciaAlunoMock
                .Setup(r => r.ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(
                    query.DataAula,
                    query.TurmasId,
                    query.Alunos,
                    query.Professor,
                    It.Is<long[]>(ids => !ids.Any()))) // Verifica se o array está vazio
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Same(retornoEsperado, resultado);
            _repositorioCacheMock.Verify(r => r.ObterAsync(It.IsAny<string>(), false), Times.Exactly(turmasId.Length));
            _repositorioFrequenciaAlunoMock.Verify(r => r.ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(
                query.DataAula, query.TurmasId, query.Alunos, query.Professor, It.Is<long[]>(ids => !ids.Any())), Times.Once);
        }

        [Fact(DisplayName = "Deve chamar repositório com Ids desconsiderados do cache quando houver dados")]
        public async Task DeveChamarRepositorio_ComIdsDesconsideradosDoCache_QuandoCacheContemDados()
        {
            // Arrange
            var turmasId = new[] { "100", "200" };
            var query = new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery(DateTime.Now, new[] { "ALUNO1" }, "PROF1", turmasId);
            var retornoEsperado = new List<RegistroFrequenciaPorDisciplinaAlunoDto>();

            var idsDesconsideradosTurma1 = new long[] { 1, 2, 3 };
            var idsDesconsideradosTurma2 = new long[] { 4, 5 };
            var idsAgregadosEsperados = new long[] { 1, 2, 3, 4, 5 };

            var chaveCacheTurma1 = string.Format(NomeChaveCache.REGISTROS_FREQUENCIA_ALUNO_EXCLUIDOS_TURMA, turmasId[0]);
            var chaveCacheTurma2 = string.Format(NomeChaveCache.REGISTROS_FREQUENCIA_ALUNO_EXCLUIDOS_TURMA, turmasId[1]);

            _repositorioCacheMock.Setup(r => r.ObterAsync(chaveCacheTurma1, false)).ReturnsAsync(JsonConvert.SerializeObject(idsDesconsideradosTurma1));
            _repositorioCacheMock.Setup(r => r.ObterAsync(chaveCacheTurma2, false)).ReturnsAsync(JsonConvert.SerializeObject(idsDesconsideradosTurma2));

            _repositorioFrequenciaAlunoMock
                .Setup(r => r.ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(
                    query.DataAula,
                    query.TurmasId,
                    query.Alunos,
                    query.Professor,
                    It.Is<long[]>(ids => ids.SequenceEqual(idsAgregadosEsperados))))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Same(retornoEsperado, resultado);
            _repositorioCacheMock.Verify(r => r.ObterAsync(It.IsAny<string>(), false), Times.Exactly(turmasId.Length));
            _repositorioFrequenciaAlunoMock.Verify(r => r.ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(
                query.DataAula, query.TurmasId, query.Alunos, query.Professor, It.Is<long[]>(ids => ids.SequenceEqual(idsAgregadosEsperados))), Times.Once);
        }
    }
}
