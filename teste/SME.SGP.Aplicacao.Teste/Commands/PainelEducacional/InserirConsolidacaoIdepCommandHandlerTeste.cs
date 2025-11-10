using Moq;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class InserirConsolidacaoIdepCommandHandlerTeste
    {
        private readonly Mock<IRepositorioIdepPainelEducacionalConsolidacao> _repositorioIdepConsolidacao;
        private readonly InserirConsolidacaoIdepCommandHandler _handler;

        public InserirConsolidacaoIdepCommandHandlerTeste()
        {
            _repositorioIdepConsolidacao = new Mock<IRepositorioIdepPainelEducacionalConsolidacao>();
            _handler = new InserirConsolidacaoIdepCommandHandler(_repositorioIdepConsolidacao.Object);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Se_Repositorio_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new InserirConsolidacaoIdepCommandHandler(null));
        }

        [Fact]
        public async Task Handle_Deve_Retornar_False_Se_DadosIdep_For_Nulo()
        {
            var command = new InserirConsolidacaoIdepCommand(null);
            var cancellationToken = CancellationToken.None;

            var resultado = await _handler.Handle(command, cancellationToken);

            Assert.False(resultado);
            _repositorioIdepConsolidacao.Verify(x => x.BulkInsertAsync(It.IsAny<IEnumerable<PainelEducacionalConsolidacaoIdep>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Chamar_Repositorio_Inserir_Se_DadosIdep_For_Valido()
        {
            var dadosIdep = new List<PainelEducacionalConsolidacaoIdep> { new PainelEducacionalConsolidacaoIdep() };
            var command = new InserirConsolidacaoIdepCommand(dadosIdep);
            var cancellationToken = CancellationToken.None;

            var resultado = await _handler.Handle(command, cancellationToken);

            Assert.True(resultado);
            _repositorioIdepConsolidacao.Verify(x => x.BulkInsertAsync(dadosIdep), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Repassar_CancellationToken_Corretamente()
        {
            var dadosIdep = new List<PainelEducacionalConsolidacaoIdep> { new PainelEducacionalConsolidacaoIdep() };
            var command = new InserirConsolidacaoIdepCommand(dadosIdep);
            var cancellationToken = new CancellationToken(true);

            var resultado = await _handler.Handle(command, cancellationToken);

            Assert.True(resultado);
            _repositorioIdepConsolidacao.Verify(x => x.BulkInsertAsync(dadosIdep), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Lidar_Com_Excecoes_Do_Repositorio()
        {
            var dadosIdep = new List<PainelEducacionalConsolidacaoIdep> { new PainelEducacionalConsolidacaoIdep() };
            var command = new InserirConsolidacaoIdepCommand(dadosIdep);
            var cancellationToken = CancellationToken.None;

            _repositorioIdepConsolidacao
                .Setup(x => x.BulkInsertAsync(It.IsAny<IEnumerable<PainelEducacionalConsolidacaoIdep>>()))
                .ThrowsAsync(new Exception("Erro no repositório"));

            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, cancellationToken));
        }

        [Fact]
        public async Task Handle_Deve_Retornar_True_Se_DadosIdep_For_Lista_Vazia()
        {
            var dadosIdep = new List<PainelEducacionalConsolidacaoIdep>();
            var command = new InserirConsolidacaoIdepCommand(dadosIdep);
            var cancellationToken = CancellationToken.None;

            var resultado = await _handler.Handle(command, cancellationToken);

            Assert.True(resultado);
            _repositorioIdepConsolidacao.Verify(x => x.BulkInsertAsync(dadosIdep), Times.Once);
        }
    }
}