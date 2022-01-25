﻿using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioRegistroFrequenciaAluno : IRepositorioBase<RegistroFrequenciaAluno>
    {
        Task RemoverPorRegistroFrequenciaIdENumeroAula(long registroFrequenciaId, int numeroAula, string codigoAluno);
        Task RemoverPorRegistroFrequenciaId(long registroFrequenciaId);
        Task<bool> InserirVarios(IEnumerable<RegistroFrequenciaAluno> registros);
        Task ExcluirVarios(List<long> idsParaExcluir);
    }
}
