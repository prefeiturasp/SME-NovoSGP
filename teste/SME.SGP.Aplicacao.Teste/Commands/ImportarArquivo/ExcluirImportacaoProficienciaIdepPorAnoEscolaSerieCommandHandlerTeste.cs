using Moq;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.ImportarArquivo.ProficienciaIdep
{
    public class ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandlerTeste
    {
        private readonly Mock<IRepositorioProficienciaIdep> _repositorioProficienciaIdepMock;
        private readonly ExcluirImportacaoProficienciaIdepCommandHandler _handler;

        public ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandlerTeste()
        {
            _repositorioProficienciaIdepMock = new Mock<IRepositorioProficienciaIdep>();
            _handler = new ExcluirImportacaoProficienciaIdepCommandHandler(_repositorioProficienciaIdepMock.Object);
        }

        [Fact]
        public void Construtor_DeveInstanciarCorretamente_QuandoRepositorioEhValido()
        {
            var handler = new ExcluirImportacaoProficienciaIdepCommandHandler(_repositorioProficienciaIdepMock.Object);
            Assert.NotNull(handler);
        }

        [Fact]
        public void Construtor_DeveLancarArgumentNullException_QuandoRepositorioEhNulo()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ExcluirImportacaoProficienciaIdepCommandHandler(null));
            Assert.Equal("repositorioProficienciaIdep", exception.ParamName);
        }
    }
}