using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosTurmaProgramaPapEolQuery : IRequest<IEnumerable<AlunosTurmaProgramaPapDto>>
    {
        public ObterAlunosAtivosTurmaProgramaPapEolQuery(int anoLetivo, string[] alunosCodigos)
        {
            AnoLetivo = anoLetivo;
            AlunosCodigos = alunosCodigos;
        }

        public int AnoLetivo { get; set; }
        public string[] AlunosCodigos { get; set; }
    }
}