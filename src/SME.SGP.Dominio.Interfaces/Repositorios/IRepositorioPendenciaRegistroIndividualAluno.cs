﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaRegistroIndividualAluno
    {
        Task<long> SalvarAsync(PendenciaRegistroIndividualAluno entidade);
    }
}