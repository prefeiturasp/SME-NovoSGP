using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes
{
    public class ObterAlunosEolPorCodigosEAnoQueryHandlerFake: IRequestHandler<ObterAlunosEolPorCodigosEAnoQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterAlunosEolPorCodigosEAnoQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<TurmasDoAlunoDto>()
            {
                new TurmasDoAlunoDto()
                {
                    CodigoAluno = 1111111,
                    NomeAluno = "Nome aluno 1111111",
                    CodigoTurma = 1
                },
                new TurmasDoAlunoDto()
                {
                    CodigoAluno = 2222222,
                    NomeAluno = "Nome aluno 2222222",
                    CodigoTurma = 1
                },
                new TurmasDoAlunoDto()
                {
                    CodigoAluno = 3333333,
                    NomeAluno = "Nome aluno 3333333",
                    CodigoTurma = 1
                },
                new TurmasDoAlunoDto()
                {
                    CodigoAluno = 4444444,
                    NomeAluno = "Nome aluno 4444444",
                    CodigoTurma = 1
                },
                new TurmasDoAlunoDto()
                {
                    CodigoAluno = 14,
                    NomeAluno = "Nome aluno 14",
                    CodigoTurma = 1
                }
            };

            return await Task.FromResult(alunos);
        }
    }
}