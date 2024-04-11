using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.ServicosFake
{
    public class ObterAlunosAtivosPorTurmaCodigoEvasaoFrequenciaQueryHandlerFake : IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string ALUNO_CODIGO_2 = "2";
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";
        private readonly string ALUNO_CODIGO_5 = "5";
        private readonly string ALUNO_CODIGO_6 = "6";
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosAtivosPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_1,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_1}",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_2,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_2}",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_3,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_3}",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_4,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_4}",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_5,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_5}",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_6,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_6}",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                },
            };
            return await Task.FromResult(retorno);
        }
    }
}
