using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Aluno.ObterAlunosPorTurma
{
    public class ObterAlunosPorTurmaQueryHandlerTeste
    {
        private readonly Mock<IHttpClientFactory> httpClientFactory;
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly ObterAlunosPorTurmaQueryHandler queryHandler;

        public ObterAlunosPorTurmaQueryHandlerTeste()
        {
            httpClientFactory = new Mock<IHttpClientFactory>();
            repositorioCache = new Mock<IRepositorioCache>();
            queryHandler = new ObterAlunosPorTurmaQueryHandler(httpClientFactory.Object, repositorioCache.Object);
        }

        [Fact(DisplayName = "ObterAlunosPorTurmaQuery - Deve obter os alunos por turma desconsiderando matrícula com vínculo indevido")]
        public async Task DeveRetornarAlunosDesconsiderandoMatriculasComVinculoIndevido()
        {
            var respostaCache = new List<AlunoPorTurmaResposta>()
            {
                new()
                {
                    CodigoAluno = "1",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    DataSituacao = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 11, 10)
                },
                new()
                {
                    CodigoAluno = "2",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido,
                    DataSituacao = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 12, 01)
                },
                new()
                {
                    CodigoAluno = "3",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    DataSituacao = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 20)
                }
            };

            var chaveCache = string.Format(NomeChaveCache.ALUNOS_TURMA_INATIVOS, "1", false);

            repositorioCache.Setup(x => x.Obter(chaveCache, It.IsAny<bool>()))
               .Returns(JsonConvert.SerializeObject(respostaCache));

            var resultado = await queryHandler.Handle(new ObterAlunosPorTurmaQuery("1"), It.IsAny<CancellationToken>());

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(respostaCache.Count(r => r.CodigoSituacaoMatricula != SituacaoMatriculaAluno.VinculoIndevido), resultado.Count());

            foreach (var item in respostaCache)
            {
                if (item.CodigoSituacaoMatricula == SituacaoMatriculaAluno.VinculoIndevido)
                {
                    Assert.DoesNotContain(resultado, r => r.CodigoAluno == item.CodigoAluno);
                    continue;
                }

                Assert.Contains(resultado, r => r.CodigoAluno == item.CodigoAluno);
            }
        }
    }
}
