using MediatR;
using Moq;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class ConsolidacaoNotaAlunoCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioConselhoClasseNota> repositorioConselhoClasseNota;
        private readonly ConsolidacaoNotaAlunoCommandHandler consolidacaoNotaAlunoCommandHandler;
        public ConsolidacaoNotaAlunoCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioConselhoClasseNota = new Mock<IRepositorioConselhoClasseNota>();
            consolidacaoNotaAlunoCommandHandler = new ConsolidacaoNotaAlunoCommandHandler(mediator.Object, repositorioConselhoClasseNota.Object);
        }

        [Fact( DisplayName = "ConsolidacaoNotaAlunoCommandHandler - Não deve publicar na fila quando o componente não lança nota")]
        public async Task NaoDevePublicarNaFilaQuandoComponenteNaoLancaNota()
        {
            // Arrange
            var componenteCurricularId = 123;
            var alunoCodigo = "1";
            var turmaId = 1;
            var consolidacaoNotaAluno = new ConsolidacaoNotaAlunoDto()
            {
                AlunoCodigo = alunoCodigo,
                ComponenteCurricularId = componenteCurricularId,
                Bimestre = (int)Bimestre.Primeiro,
                Inativo = false,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TurmaId = turmaId,
                ConceitoId = null,
                Nota = null,
                ConselhoClasse = true
            };

            var request = new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAluno);
            var obterComponenteLancaNotaQueryResult = false;

            mediator.Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(obterComponenteLancaNotaQueryResult);

            var result = await consolidacaoNotaAlunoCommandHandler.Handle(request, CancellationToken.None);

            mediator.Verify(m => m.Send(It.Is<ObterComponenteLancaNotaQuery>(q => q.ComponenteCurricularId == componenteCurricularId), CancellationToken.None), Times.Once);

            Assert.False(result);
        }
    }
}
