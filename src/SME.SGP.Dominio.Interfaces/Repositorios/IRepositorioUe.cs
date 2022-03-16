using System;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUe
    {
        Task<IEnumerable<Ue>> SincronizarAsync(IEnumerable<Ue> entidades, IEnumerable<Dre> dres);
        Task<long> IncluirAsync(Ue ueParaIncluir);
        Task AtualizarAsync(Ue ueParaAtualizar);
    }
}