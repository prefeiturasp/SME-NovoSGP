using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery : IRequest<List<ConselhoClasseAlunoDto>>
    {
        public ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery(long turmaId, int bimestre, int situacaoConselhoClasse)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            SituacaoConselhoClasse = situacaoConselhoClasse;
        }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public int SituacaoConselhoClasse { get; set; }
    }
}
