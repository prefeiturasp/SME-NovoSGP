using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.SupervisoresEscolaDre
{
    public class RemoverAtribuicoesResponsaveisCommandHandlerTeste
    {
        private readonly Mock<IRepositorioSupervisorEscolaDre> repositorioMock;
        private readonly RemoverAtribuicoesResponsaveisCommandHandler handler;

        public RemoverAtribuicoesResponsaveisCommandHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioSupervisorEscolaDre>();
            handler = new RemoverAtribuicoesResponsaveisCommandHandler(repositorioMock.Object);
        }

        [Fact]
        public async Task Deve_Chamar_Repositorio_Para_Remocao_Em_Lote_Quando_Lista_De_Ids_Valida()
        {
            // Arrange
            var idsParaRemover = new List<long> { 10, 25, 33 };
            var command = new RemoverAtribuicoesResponsaveisCommand(idsParaRemover);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            // Verifica se o método de remoção em lote do repositório foi chamado exatamente uma vez
            // e se foi chamado com a lista de IDs correta.
            repositorioMock.Verify(r =>
                r.RemoverAtribuicoesEmLote(It.Is<IEnumerable<long>>(ids => ids.SequenceEqual(idsParaRemover))),
                Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Chamar_Repositorio_Quando_Lista_De_Ids_Vazia()
        {
            // Arrange
            var idsParaRemover = new List<long>(); // Lista vazia
            var command = new RemoverAtribuicoesResponsaveisCommand(idsParaRemover);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            // Verifica se o método de remoção do repositório NUNCA foi chamado,
            // confirmando que a cláusula de guarda para listas vazias funcionou.
            repositorioMock.Verify(r =>
                r.RemoverAtribuicoesEmLote(It.IsAny<IEnumerable<long>>()),
                Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Chamar_Repositorio_Quando_Lista_De_Ids_Nula()
        {
            // Arrange
            var command = new RemoverAtribuicoesResponsaveisCommand(null); // Lista nula

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            // Verifica se o método de remoção do repositório NUNCA foi chamado,
            // confirmando que a cláusula de guarda para listas nulas funcionou.
            repositorioMock.Verify(r =>
                r.RemoverAtribuicoesEmLote(It.IsAny<IEnumerable<long>>()),
                Times.Never);
        }
    }
}
