using Moq;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.ImportarArquivo.ProficienciaIdep
{
    public class SalvarImportacaoProficienciaIdepCommandHandlerTeste
    {
        private readonly Mock<IRepositorioProficienciaIdep> _repositorioProficienciaIdepMock;
        private readonly SalvarImportacaoProficienciaIdepCommandHandler _handler;

        public SalvarImportacaoProficienciaIdepCommandHandlerTeste()
        {
            _repositorioProficienciaIdepMock = new Mock<IRepositorioProficienciaIdep>();
            _handler = new SalvarImportacaoProficienciaIdepCommandHandler(_repositorioProficienciaIdepMock.Object);
        }

        [Fact]
        public void Construtor_DeveInstanciarCorretamente_QuandoRepositorioEhValido()
        {
            var handler = new SalvarImportacaoProficienciaIdepCommandHandler(_repositorioProficienciaIdepMock.Object);
            Assert.NotNull(handler);
        }

        [Fact]
        public void Construtor_DeveLancarArgumentNullException_QuandoRepositorioEhNulo()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SalvarImportacaoProficienciaIdepCommandHandler(null));
            Assert.Equal("repositorioProficienciaIdep", exception.ParamName);
        }

        [Fact]
        public async Task Handle_DevePropagarExcecao_QuandoRepositorioLancarExcecao()
        {
            var dto = new ProficienciaIdepDto(5, "123456", 2023, "123", 7.5m);
            var command = new SalvarImportacaoProficienciaIdepCommand(dto);
            var exceptionMessage = "Erro no banco de dados";

            _repositorioProficienciaIdepMock
                .Setup(x => x.SalvarAsync(It.IsAny<Dominio.Entidades.ProficienciaIdep>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(command, CancellationToken.None));

            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}