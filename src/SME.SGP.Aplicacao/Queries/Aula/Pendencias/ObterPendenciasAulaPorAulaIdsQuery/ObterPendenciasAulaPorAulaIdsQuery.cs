using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdsQuery : IRequest<bool>
    {
        public long[] AulasId { get; set; }

        public bool EhModalidadeInfantil { get; internal set; }
        public int EtapaEJA { get; set; }
        public long[] ComponentesId { get; set; }

        public ObterPendenciasAulaPorAulaIdsQuery(long[] aulasId, Modalidade modalidade, int etapaEja, long [] componentesId)
        {
            AulasId = aulasId;
            EhModalidadeInfantil = modalidade == Modalidade.EducacaoInfantil;
            EtapaEJA = etapaEja;
            ComponentesId = componentesId;
        }
    }
}
