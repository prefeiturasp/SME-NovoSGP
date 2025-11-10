using MediatR;
using SME.SGP.Infra.Dtos.Aluno;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap
{
    public class ObterAlunosTurmaPapQuery : IRequest<IEnumerable<DadosMatriculaAlunoTipoPapDto>>
    {
        public int AnoLetivo { get; set; }

        public ObterAlunosTurmaPapQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }
}