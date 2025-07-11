using Bogus;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterAlunosAusentesPorTurmaNoPeriodoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioFrequenciaConsulta> _repositorioFrequenciaMock;
        private readonly ObterAlunosAusentesPorTurmaNoPeriodoQueryHandler _handler;
        private readonly Faker _faker;

        public ObterAlunosAusentesPorTurmaNoPeriodoQueryHandlerTeste()
        {
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaConsulta>();
            _handler = new ObterAlunosAusentesPorTurmaNoPeriodoQueryHandler(_repositorioFrequenciaMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando o repositório for nulo")]
        public void DeveLancarExcecao_QuandoRepositorioNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObterAlunosAusentesPorTurmaNoPeriodoQueryHandler(null));
        }

        [Fact(DisplayName = "Deve chamar o repositório para obter alunos ausentes e retornar os dados")]
        public async Task DeveChamarRepositorio_E_RetornarAlunosAusentes()
        {
            // Arrange
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var dataInicio = DateTime.Now.Date;
            var dataFim = dataInicio.AddDays(15);
            var componenteId = _faker.Random.Long(1, 1000).ToString();

            var query = new ObterAlunosAusentesPorTurmaNoPeriodoQuery(turmaCodigo, dataInicio, dataFim, componenteId);

            var retornoRepositorio = new List<AlunoComponenteCurricularDto>
            {
                new AlunoComponenteCurricularDto
                {
                    AlunoCodigo = _faker.Random.AlphaNumeric(8),
                    ComponenteCurricularId = componenteId
                }
            };

            _repositorioFrequenciaMock
                .Setup(r => r.ObterAlunosAusentesPorTurmaEPeriodo(turmaCodigo, dataInicio, dataFim, componenteId))
                .ReturnsAsync(retornoRepositorio);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositorioFrequenciaMock.Verify(r => r.ObterAlunosAusentesPorTurmaEPeriodo(turmaCodigo, dataInicio, dataFim, componenteId), Times.Once);
            Assert.Same(retornoRepositorio, resultado);
        }
    }
}
