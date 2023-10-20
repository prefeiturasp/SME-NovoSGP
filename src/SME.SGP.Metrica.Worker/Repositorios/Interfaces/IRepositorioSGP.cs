using SME.SGP.Metrica.Worker.Entidade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios.Interfaces
{
    public interface IRepositorioSGP
    {
        Task<int> ObterQuantidadeAcessosDia(DateTime data);
        Task<IEnumerable<ConselhoClasseDuplicado>> ObterConselhosClasseDuplicados();
    }
}
