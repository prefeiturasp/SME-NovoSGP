using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ConselhoClasse
{
    public class ObterNotasConselhoEmAprovacaoPorIdsQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_delegar_todos_os_ids_ao_repositorio_uma_unica_vez()
        {
            var ids = new long[] { 1001, 1002, 1003 };
            var esperado = new List<ConselhoClasseNotaAprovacaoDto>
            {
                new ConselhoClasseNotaAprovacaoDto { Id = 1001, NotaEmAprovacao = 8.5 },
                new ConselhoClasseNotaAprovacaoDto { Id = 1002, NotaEmAprovacao = 3 },
                new ConselhoClasseNotaAprovacaoDto { Id = 1003, NotaEmAprovacao = 0 }
            };
            var repositorio = new Mock<IRepositorioConselhoClasseNotaConsulta>(MockBehavior.Strict);
            repositorio.Setup(r => r.ObterNotasConselhoEmAprovacaoPorIds(It.Is<IEnumerable<long>>(valores => valores.SequenceEqual(ids))))
                .ReturnsAsync(esperado);
            var handler = new ObterNotasConselhoEmAprovacaoPorIdsQueryHandler(repositorio.Object);

            var retorno = await handler.Handle(new ObterNotasConselhoEmAprovacaoPorIdsQuery(ids), CancellationToken.None);

            Assert.Same(esperado, retorno);
            repositorio.Verify(r => r.ObterNotasConselhoEmAprovacaoPorIds(It.Is<IEnumerable<long>>(valores => valores.SequenceEqual(ids))), Times.Once);
            repositorio.VerifyNoOtherCalls();
        }
    }
}
