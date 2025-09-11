using Bogus;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterAulasComFrequenciaPorTurmaCodigoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioFrequenciaConsulta> _repositorioFrequenciaMock;
        private readonly ObterAulasComFrequenciaPorTurmaCodigoQueryHandler _handler;
        private readonly Faker _faker;

        public ObterAulasComFrequenciaPorTurmaCodigoQueryHandlerTeste()
        {
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaConsulta>();
            _handler = new ObterAulasComFrequenciaPorTurmaCodigoQueryHandler(_repositorioFrequenciaMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando o repositório for nulo")]
        public void DeveLancarExcecao_QuandoRepositorioNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObterAulasComFrequenciaPorTurmaCodigoQueryHandler(null));
        }

        [Fact(DisplayName = "Deve chamar o repositório para obter aulas com frequência e retornar os dados")]
        public async Task DeveChamarRepositorio_E_RetornarAulasComFrequencia()
        {
            // Arrange
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var query = new ObterAulasComFrequenciaPorTurmaCodigoQuery(turmaCodigo);

            var retornoRepositorio = new List<AulaComFrequenciaNaDataDto>
            {
                new AulaComFrequenciaNaDataDto
                {
                    AulaId = _faker.Random.Long(1, 1000),
                    DataAula = DateTime.Now.Date,
                    RegistroFrequenciaId = _faker.Random.Long(1001, 2000)
                }
            };

            _repositorioFrequenciaMock
                .Setup(r => r.ObterAulasComRegistroFrequenciaPorTurma(turmaCodigo))
                .ReturnsAsync(retornoRepositorio);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositorioFrequenciaMock.Verify(r => r.ObterAulasComRegistroFrequenciaPorTurma(turmaCodigo), Times.Once);
            Assert.Same(retornoRepositorio, resultado);
        }
    }
}
