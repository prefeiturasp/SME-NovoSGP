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
    public class ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioFrequenciaConsulta> _repositorioFrequenciaMock;
        private readonly ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryHandler _handler;
        private readonly Faker _faker;

        public ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryHandlerTeste()
        {
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaConsulta>();
            _handler = new ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryHandler(_repositorioFrequenciaMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando o repositório for nulo")]
        public void DeveLancarExcecao_QuandoRepositorioNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryHandler(null));
        }

        [Fact(DisplayName = "Deve chamar o repositório para obter os motivos de ausência e retornar os dados")]
        public async Task DeveChamarRepositorio_E_RetornarMotivosDeAusencia()
        {
            // Arrange
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            short bimestre = 1;
            short anoLetivo = (short)DateTime.Now.Year;

            var query = new ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQuery(alunoCodigo, turmaCodigo, bimestre, anoLetivo);

            var retornoRepositorio = new List<AusenciaMotivoDto>
            {
                new AusenciaMotivoDto
                {
                    DataAusencia = DateTime.Now.Date,
                    MotivoAusencia = "Atestado Médico",
                    RegistradoPor = "PROFESSOR TESTE"
                }
            };

            _repositorioFrequenciaMock
                .Setup(r => r.ObterAusenciaMotivoPorAlunoTurmaBimestreAno(alunoCodigo, turmaCodigo, bimestre, anoLetivo))
                .ReturnsAsync(retornoRepositorio);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositorioFrequenciaMock.Verify(r => r.ObterAusenciaMotivoPorAlunoTurmaBimestreAno(alunoCodigo, turmaCodigo, bimestre, anoLetivo), Times.Once);
            Assert.Same(retornoRepositorio, resultado);
        }
    }
}
