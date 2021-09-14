using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdsQuery : IRequest<bool>
    {
        public long[] AulasId { get; set; }

        public bool EhModalidadeInfantil { get; internal set; }

        public ObterPendenciasAulaPorAulaIdsQuery(long[] aulasId, Modalidade modalidade)
        {
            AulasId = aulasId;
            EhModalidadeInfantil = modalidade == Modalidade.EducacaoInfantil;
        }
    }
}
