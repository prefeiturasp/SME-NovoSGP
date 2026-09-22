using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciaGeralIndexadaPorAlunosQueryHandlerTeste
    {
        private const string CODIGO_ALUNO_1 = "1000";

        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterFrequenciaGeralIndexadaPorAlunosQueryHandler handler;

        public ObterFrequenciaGeralIndexadaPorAlunosQueryHandlerTeste()
        {
            mediatorMock = new Mock<IMediator>();
            handler = new ObterFrequenciaGeralIndexadaPorAlunosQueryHandler(mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Sem_Codigo_Valido_Nao_Deve_Consultar_A_Base()
        {
            var query = new ObterFrequenciaGeralIndexadaPorAlunosQuery(new[] { null, string.Empty, "   " }, "TURMA-1", "138");

            var frequenciaPorAluno = await handler.Handle(query, CancellationToken.None);

            frequenciaPorAluno.Should().BeEmpty();
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Consultar_Uma_Unica_Vez_Com_Os_Codigos_Distintos()
        {
            var frequenciaAluno1 = new FrequenciaAluno { Id = 1, CodigoAluno = CODIGO_ALUNO_1 };

            mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery>(q => q.AlunosCodigos.Length == 1 &&
                                                                                                         q.AlunosCodigos[0] == CODIGO_ALUNO_1 &&
                                                                                                         q.TurmaCodigo == "TURMA-1" &&
                                                                                                         q.ComponenteCurricularCodigo == "138"),
                                           It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno> { frequenciaAluno1 });

            var query = new ObterFrequenciaGeralIndexadaPorAlunosQuery(new[] { CODIGO_ALUNO_1, CODIGO_ALUNO_1, null }, "TURMA-1", "138");

            var frequenciaPorAluno = await handler.Handle(query, CancellationToken.None);

            frequenciaPorAluno.Should().HaveCount(1);
            frequenciaPorAluno[CODIGO_ALUNO_1].Should().BeSameAs(frequenciaAluno1);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
