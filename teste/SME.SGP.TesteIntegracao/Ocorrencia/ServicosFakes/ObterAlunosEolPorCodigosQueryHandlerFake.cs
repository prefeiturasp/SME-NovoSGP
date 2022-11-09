using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class ObterAlunosEolPorCodigosQueryHandlerFake : IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        private const int ALUNO_1 = 1;
        private const int ALUNO_2 = 2;
        private const int ALUNO_3 = 3;


        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterAlunosEolPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return new List<TurmasDoAlunoDto>()
            {
                new TurmasDoAlunoDto()
                {
                    CodigoAluno = ALUNO_1,
                    NomeAluno = "Nome Aluno 1",
                    NomeSocialAluno = "Nome Social Aluno 1"
                },
                new TurmasDoAlunoDto()
                {
                    CodigoAluno = ALUNO_2,
                    NomeAluno = "Nome Aluno 2",
                    NomeSocialAluno = "Nome Social Aluno 2"
                },
                new TurmasDoAlunoDto()
                {
                    CodigoAluno = ALUNO_3,
                    NomeAluno = "Nome Aluno 3",
                    NomeSocialAluno = "Nome Social Aluno 3"
                }
            };
        }
    }
}
