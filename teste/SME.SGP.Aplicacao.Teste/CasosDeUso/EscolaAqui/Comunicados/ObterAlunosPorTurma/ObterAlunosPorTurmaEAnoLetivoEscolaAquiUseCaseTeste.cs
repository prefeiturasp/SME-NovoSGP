using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.ObterAlunosPorTurma
{
    public class ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase useCase;

        public ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Somente_Alunos_Com_Situacoes_Ativas()
        {
            var codigoTurma = "TURMA01";
            var anoLetivo = 2025;

            var alunosMock = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "1", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta { CodigoAluno = "2", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Rematriculado },
                new AlunoPorTurmaResposta { CodigoAluno = "3", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente }, // inativo
                new AlunoPorTurmaResposta { CodigoAluno = "4", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido },
                new AlunoPorTurmaResposta { CodigoAluno = "5", CodigoSituacaoMatricula = SituacaoMatriculaAluno.DispensadoEdFisica } // inativo
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosMock);

            var resultado = (await useCase.Executar(codigoTurma, anoLetivo)).ToList();

            Assert.Equal(3, resultado.Count);
            Assert.DoesNotContain(resultado, x => x.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Desistente);
            Assert.DoesNotContain(resultado, x => x.CodigoSituacaoMatricula == SituacaoMatriculaAluno.DispensadoEdFisica);
        }      
    }
}
