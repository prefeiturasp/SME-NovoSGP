using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosAtivosPorAnoLetivo
{
    public class ObterAlunosAtivosPorAnoLetivoQuery : IRequest<IList<DadosMatriculaAlunoTipoPapDto>>
    {
        public ObterAlunosAtivosPorAnoLetivoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
        public int AnoLetivo { get; set; }
    }
}
