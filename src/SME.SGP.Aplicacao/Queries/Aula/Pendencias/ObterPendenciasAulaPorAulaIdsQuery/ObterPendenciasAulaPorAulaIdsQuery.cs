using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdsQuery : IRequest<bool>
    {
        public ObterPendenciasAulaPorAulaIdsQuery(long[] aulasId, Modalidade modalidade)
        {
            AulasId = aulasId;
            EhModalidadeInfantil = modalidade == Modalidade.Infantil;
        }
        public long[] AulasId { get; set; }

        public bool EhModalidadeInfantil { get; internal set; }


    }
}
