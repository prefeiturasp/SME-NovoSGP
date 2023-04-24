using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

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
                }
            };

            return alunos;
        }
    }
}