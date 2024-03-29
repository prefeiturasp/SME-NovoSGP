﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAulaPrevistaBimestre : IRepositorioBase<AulaPrevistaBimestre>
    {
        Task<IEnumerable<AulaPrevistaBimestre>> ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestre(long aulaPrevistaId, int[] bimestre);
    }
}
