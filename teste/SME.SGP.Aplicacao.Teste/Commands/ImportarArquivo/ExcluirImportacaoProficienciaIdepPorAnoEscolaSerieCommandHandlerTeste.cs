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
        private readonly ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandler _handler;

        public ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandlerTeste()
        {
            _repositorioProficienciaIdepMock = new Mock<IRepositorioProficienciaIdep>();
            _handler = new ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandler(_repositorioProficienciaIdepMock.Object);
        }

        [Fact]
        public void Construtor_DeveInstanciarCorretamente_QuandoRepositorioEhValido()
        {
            var handler = new ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandler(_repositorioProficienciaIdepMock.Object);
            Assert.NotNull(handler);
        }

        [Fact]
        public void Construtor_DeveLancarArgumentNullException_QuandoRepositorioEhNulo()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandler(null));
            Assert.Equal("repositorioProficienciaIdep", exception.ParamName);
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            var anoLetivo = 2023;
            var codigoEolEscola = "123456";
            var serieAno = 5;
            var command = new ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand(anoLetivo, codigoEolEscola, serieAno);

            _repositorioProficienciaIdepMock
                .Setup(x => x.ExcluirPorAnoEscolaSerie(anoLetivo, codigoEolEscola, serieAno))
                .ReturnsAsync(true);

            var resultado = await _handler.Handle(command, CancellationToken.None);

            _repositorioProficienciaIdepMock.Verify(
                x => x.ExcluirPorAnoEscolaSerie(anoLetivo, codigoEolEscola, serieAno),
                Times.Once);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoExclusaoFalhar()
        {
            var command = new ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand(2023, "123456", 5);

            _repositorioProficienciaIdepMock
                .Setup(x => x.ExcluirPorAnoEscolaSerie(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            var resultado = await _handler.Handle(command, CancellationToken.None);
            Assert.False(resultado);
        }

        [Theory]
        [InlineData(2023, "123456", 5)]
        [InlineData(2024, "789012", 9)]
        [InlineData(2022, "345678", 3)]
        public async Task Handle_DeveFuncionarComDiferentesParametros(int anoLetivo, string codigoEolEscola, int serieAno)
        {
            var command = new ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand(anoLetivo, codigoEolEscola, serieAno);

            _repositorioProficienciaIdepMock
                .Setup(x => x.ExcluirPorAnoEscolaSerie(anoLetivo, codigoEolEscola, serieAno))
                .ReturnsAsync(true);

            var resultado = await _handler.Handle(command, CancellationToken.None);

            _repositorioProficienciaIdepMock.Verify(
                x => x.ExcluirPorAnoEscolaSerie(anoLetivo, codigoEolEscola, serieAno),
                Times.Once);

            Assert.True(resultado);
        }
    }
}