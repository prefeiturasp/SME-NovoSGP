using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{ 
    public class ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQuery : IRequest<IEnumerable<AlunoSituacaoConselhoDto>>
    {
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }

        public ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQuery(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }
    }
}
