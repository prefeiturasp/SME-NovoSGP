using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.ServicosFake
{
    public class ObterAlunosEolPorTurmaEvasaoFrequenciaQueryHandlerFake : IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string ALUNO_CODIGO_2 = "2";
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";
        private readonly string ALUNO_CODIGO_5 = "5";
        private readonly string ALUNO_CODIGO_6 = "6";
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosEolPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_1,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_1}",
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_2,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_2}",
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_3,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_3}",
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_4,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_4}",
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_5,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_5}",
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = ALUNO_CODIGO_6,
                    NomeAluno = $"Aluno {ALUNO_CODIGO_6}",
                },
            };
            return await Task.FromResult(retorno);
        }
    }
}
