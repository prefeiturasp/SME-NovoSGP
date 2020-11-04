﻿using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdsQuery : IRequest<long[]>
    {
        public ObterPendenciasAulaPorAulaIdsQuery(long[] aulasId)
        {
            AulasId = aulasId;
        }
        public long[] AulasId { get; set; }


    }
}
