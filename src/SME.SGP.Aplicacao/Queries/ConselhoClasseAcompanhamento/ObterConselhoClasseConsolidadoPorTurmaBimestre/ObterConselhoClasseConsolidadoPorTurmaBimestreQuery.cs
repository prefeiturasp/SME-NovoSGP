using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
   public class ObterConselhoClasseConsolidadoPorTurmaBimestreQuery : IRequest<IEnumerable<ConselhoClasseConsolidadoTurmaAluno>>
    {
        public ObterConselhoClasseConsolidadoPorTurmaBimestreQuery(long turmaId, int bimestre, int situacaoConselhoClasse)
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
