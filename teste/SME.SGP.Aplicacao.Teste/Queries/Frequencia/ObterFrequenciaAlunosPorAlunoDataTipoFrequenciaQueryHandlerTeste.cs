using Bogus;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaAlunoDisciplinaPeriodoMock;
        private readonly ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQueryHandler _handler;

        public ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQueryHandler(_repositorioFrequenciaAlunoDisciplinaPeriodoMock.Object);
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            // Arrange
            var codigoAluno = _faker.Random.Int(1, 1000).ToString();
            var dataReferencia = _faker.Date.Past();
            var tipoFrequencia = _faker.PickRandom<TipoFrequenciaAluno>();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                Tipo = tipoFrequencia
            };
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock
                .Setup(r => r.ObterPorAlunoDataAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<TipoFrequenciaAluno>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(frequenciaAluno);

            var query = new ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQuery(codigoAluno, dataReferencia, tipoFrequencia);
            // Act
            var response = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(response);
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock.Verify(r => r.ObterPorAlunoDataAsync(codigoAluno, dataReferencia, tipoFrequencia, "", ""), Times.Once);
        }
    }
}
